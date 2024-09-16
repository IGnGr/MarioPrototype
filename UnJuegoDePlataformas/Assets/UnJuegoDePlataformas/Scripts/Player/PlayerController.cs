using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// Clase para gestionar el movimiento de Mario
/// </summary>
public class PlayerController : MonoBehaviour
{

    /// <summary>
    /// Instancia del GameObject de Mario
    /// </summary>
    [SerializeField]
    private GameObject player;
    /// <summary>
    /// Instancia del RigidBody de Mario
    /// </summary>
    private Rigidbody2D rigidbody;

    private CameraFollower cameraScript;

    /// <summary>
    /// Instancia del Collider de Mario
    /// </summary>
    private BoxCollider2D boxCollider;

    private Animator animator;

    /// <summary>
    /// Instancia de la clase para gestionar el final de partida
    /// </summary>
    private EndGameTrigger endGameTrigger;
    /// <summary>
    /// Instancia del GameObject del gameplayManager
    /// </summary>
    public GameObject gameplayManager;

    /// <summary>
    /// Instancia del AudioManager
    /// </summary>
    AudioManager audioManager;


    /// <summary>
    /// Instancia del HUDManager
    /// </summary>
    private HUDManager hudManager;

    /// <summary>
    /// ContactFilter2D para filtrar el contacto con el suelo
    /// </summary>
    private ContactFilter2D groundContactFilter;

    /// <summary>
    /// ContactFilter2D para filtrar el contacto con los bloques desde abajo
    /// </summary>
    private ContactFilter2D upperContactFilter;

    /// <summary>
    /// ContactFilter2D para filtrar el contacto con los Goombas desde arriba
    /// </summary>
    private ContactFilter2D enemyKilledContactFilter;


    private ContactFilter2D powerUPContactFilter;


    /// <summary>
    /// Variable para distinguir si el salto se ha producido este frame
    /// </summary>
    private bool triggeredJumpThisFrame = false;

    /// <summary>
    /// Variable que almacena el tiempo restante en el aire de Mario
    /// </summary>
    private float remainingjumpTime = 0.25f;
    /// <summary>
    /// Variable que indica si Mario está saltando
    /// </summary>
    private bool isJumping = false;


    /// <summary>
    /// El tiempo mínimo de salto, aunque se suelte el botón
    /// </summary>
    private float minimumJumpTime = 0.1f;

    /// <summary>
    /// La velocidad horizontal estándar
    /// </summary>
    private float standardSpeed = 9f;

    /// <summary>
    /// La velocidad horizontal corriendo
    /// </summary>
    private float runningSpeed = 13f;


    /// <summary>
    /// La aceleración hasta llegar al límite de velocidad horizontal
    /// </summary>
    private float horizontalAcceleration = 30f;


    /// <summary>
    /// La aceleración hasta llegar al límite de velocidad vertical
    /// </summary>
    private float VerticalAcceleration = 45f;

    /// <summary>
    /// La velocidad máxima al caer
    /// </summary>
    private float fallVelocity = 10f;


    /// <summary>
    /// La velocidad máxima al saltar
    /// </summary>
    private float jumpVelocity = 8f;

    /// <summary>
    /// El tiempo máximo de salto para Mario
    /// </summary>
    private float jumpTime = 0.8f;

    /// <summary>
    /// Variable para gestionar el salto posterior a derrotar a un Goomba
    /// </summary>
    private bool hasJumpedOnEnemy = false;


    public bool isBig = false;

    public bool hasFlower = false;

    private bool isFiring = false;

    private float fireCooldown = 0.5f;

    public GameObject fireballPrefab;

    public bool isFacingLeft = false;
    /// <summary>
    /// Indica si Mario está en el suelo
    /// </summary>
    private bool isGrounded => rigidbody.IsTouching(groundContactFilter) && !isJumping;


    /// <summary>
    /// Ancho del sprite de Mario, usado en los RayCast
    /// </summary>
    private float width => isBig ? 0.5f : 0.45f ;


    public int nLives = 3;

    private bool isInvulnerable;

    private float invulnerabilityTime = 1f;

    public GameObject RespawnsParent;


    #region Input System

    protected Vector2 inputMove;
    private void OnMove(InputValue value)
    {
        inputMove = value.Get<Vector2>();
    }


    protected bool inputJump;
    private void OnJump(InputValue value)
    {
        inputJump = value.isPressed;
    }

    protected bool inputRun;
    private void OnRun(InputValue value)
    {
        inputRun = value.isPressed;
    }

    protected bool inputShoot;
    private void OnShoot(InputValue value)
    {
        inputShoot = value.isPressed;
    }


    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        remainingjumpTime = jumpTime;
        groundContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Ground"));
        groundContactFilter.SetNormalAngle(85, 95);

        enemyKilledContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Enemies"));
        enemyKilledContactFilter.SetNormalAngle(90, 90);

        upperContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Ground"));
        upperContactFilter.SetNormalAngle(269,270);

        powerUPContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("PowerUP"));


        endGameTrigger = GameObject.Find("GameplayManager").GetComponent<EndGameTrigger>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        hudManager = GameObject.Find("HUDManager").GetComponent<HUDManager>();
        cameraScript = GameObject.Find("Main Camera").GetComponent<CameraFollower>();
    }

    private void Update()
    {
        HandleAnimation();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        HandleMovement();
        HandleFireShoot();
        CheckForGoombas();


    }

    public Transform GetCurrentTransform()
    {
        return rigidbody.transform;
    }


    void HandleFireShoot()
    {
        if (hasFlower && inputShoot && !isFiring)
        {
            StartCoroutine(FireShoot(fireCooldown));
        }
    }

    private IEnumerator FireShoot(float recoveryTime)
    {
        isFiring = true;


        Vector3 fireOrigin = transform.position;
        if (isFacingLeft)
        {
            fireOrigin.x -= boxCollider.size.x;
        }
        else
            fireOrigin.x += boxCollider.size.x;


        Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(recoveryTime);
        isFiring = false;
    }


    void HandleMovement()
    {

        triggeredJumpThisFrame = false;

        //left and right movement only
        Vector2 movementVector = inputMove;

        float currentSpeed = inputRun && isGrounded ? runningSpeed : standardSpeed;
        
        if(inputMove.x != 0)
             movementVector.x = Math.Clamp(rigidbody.velocity.x + inputMove.x * horizontalAcceleration * Time.fixedDeltaTime, -currentSpeed, currentSpeed);

        movementVector.y = 0;

        if (inputMove.x < 0) isFacingLeft = true;
        if (inputMove.x > 0) isFacingLeft = false;


        if (inputJump && isGrounded)
        {

            isJumping = true;
            audioManager.PlayJump();
            triggeredJumpThisFrame = true;
        }

        if (isJumping)
        {

            if (!triggeredJumpThisFrame)
            {
                remainingjumpTime -= Time.deltaTime;
            }

            if (remainingjumpTime <= 0.0f || (!inputJump && jumpTime - remainingjumpTime > minimumJumpTime) && !hasJumpedOnEnemy)
            {
                isJumping = false;
                remainingjumpTime = jumpTime;
                hasJumpedOnEnemy = false;
            }
            else
            {
                movementVector.y = jumpVelocity;
                //movementVector.y = Math.Clamp(rigidbody.velocity.y + VerticalAcceleration , 0, jumpVelocity);

            }

        }

        if((!isJumping && !isGrounded))
        {
            //movementVector.y -= fallVelocity;
            movementVector.y = Math.Clamp(rigidbody.velocity.y - VerticalAcceleration * 2, -fallVelocity, 0);

        }

        //Evitar movimiento y scroll hacia la izquierda del nivel
        if (rigidbody.transform.position.x <= Camera.main.transform.position.x - 10f && movementVector.x < 0)
        {
            movementVector.x = 0f;
        }


        rigidbody.velocity = movementVector;

    }

    private Vector3 DetermineProperRespawn()
    {
        Transform[] transforms = RespawnsParent.GetComponentsInChildren<Transform>();

        Vector3 resultPosition = transforms[1].position;

        //i = 0 es el parent; i = 1 es el default; 
        for (int i = 2; i< transforms.Length; i++)
        {
            if(transform.position.x >= transforms[i].position.x)
                resultPosition = transforms[i].position;
        }


        return resultPosition;
    }

    void CheckForGoombas()
    {
        RaycastHit2D rightHit, leftHit;
        
        boxCollider.enabled = false;

        float offsetBigMario = isBig ? 0.1f : 0.0f;

        Vector3 raycastOrigin = new Vector3(rigidbody.transform.position.x, rigidbody.transform.position.y - offsetBigMario, rigidbody.transform.position.z);

        leftHit = Physics2D.Raycast(raycastOrigin, -transform.right, width, 1 << LayerMask.NameToLayer("Enemies"));
        rightHit = Physics2D.Raycast(raycastOrigin, transform.right, width, 1 << LayerMask.NameToLayer("Enemies"));
        //Debug.DrawLine(raycastOrigin + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0), raycastOrigin + new Vector3(boxCollider.offset.x, boxCollider.offset.y, 0) + transform.right * width, Color.red);

        boxCollider.enabled = true;

        if((leftHit || rightHit) && !isInvulnerable)
        {
            HandleDamage(false);
        }

    }
    public void HandleDamage(bool fallDamage)
    {

        if(fallDamage)
        {
            isBig = false;
            hasFlower = false;

            //Golpe con Mario pequeño, tenemos que bajar una vida
            nLives--;

            if (nLives <= 0)
            {
                endGameTrigger.PopUpRestartScreen();
            }
            else
            {
                rigidbody.transform.position = DetermineProperRespawn();
                cameraScript.MoveCameraToPlayer();
            }
        }
        else
        {
            if (!isInvulnerable)
            {
                if (hasFlower)
                {
                    hasFlower = false;
                }
                else
                if (isBig)
                {
                    isBig = false;
                }
                else
                {

                    //Golpe con Mario pequeño, tenemos que bajar una vida
                    nLives--;

                    if (nLives <= 0)
                    {
                        endGameTrigger.PopUpRestartScreen();
                    }
                    else
                    {
                        rigidbody.transform.position = DetermineProperRespawn();
                        cameraScript.MoveCameraToPlayer();
                    }
                }
            }
            StartCoroutine(MakeInvulnerable(invulnerabilityTime));
        }







    }

    private IEnumerator MakeInvulnerable(float deltaTime)
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(deltaTime);
        isInvulnerable = false;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Matamos a un enemigo
        if (rigidbody.IsTouching(enemyKilledContactFilter))
        {
            ContactPoint2D[] contacts = new ContactPoint2D[5];

            collision.GetContacts(contacts);

            //Tenemos que mirar los contactos para evitar desactivar el propio escenario por error
            for (int i = 0; i < collision.GetContacts(contacts); i++) 
            {
                if (contacts[i].rigidbody.gameObject.layer == LayerMask.NameToLayer("Enemies"))
                {
                    if(contacts[i].rigidbody.gameObject.CompareTag("FallEnemy"))
                    {
                        HandleDamage(false);
                    }
                    else
                    {
                        contacts[i].rigidbody.gameObject.SetActive(false);

                        //Seteamos las variables de salto para dar el feedback tras matar al goomba 
                        hasJumpedOnEnemy = true;
                        remainingjumpTime = 0.15f;
                        isJumping = true;

                        //Reproducimos el sonido de Goomba aplastado
                        audioManager.PlayGoombaJumped();

                        //Añadimos los 50 puntos por el Goomba al HUD
                        hudManager.AddPoints(50);

                    }
                }
            }
        }


        //Si tocamos un bloque por debajo, cancelamos el salto
        if (rigidbody.IsTouching(upperContactFilter) && !isGrounded)
        {
            remainingjumpTime = 0;
        }


        if(rigidbody.IsTouching(powerUPContactFilter))
        {
            HandlePowerUP(collision);
        }

    }

    void HandlePowerUP(Collision2D collision)
    {
        ContactPoint2D[] contacts = new ContactPoint2D[5];

        collision.GetContacts(contacts);


        //Tenemos que mirar los contactos para evitar desactivar el propio escenario por error
        for (int i = 0; i < collision.GetContacts(contacts); i++)
        {
            if (contacts[i].rigidbody.gameObject.layer == LayerMask.NameToLayer("PowerUP"))
            {

                if (collision.gameObject.name.Contains("Mushroom"))
                {
                    isBig = true;
                }

                if (collision.gameObject.name.Contains("Flower"))
                {
                    if (!isBig)
                    {
                        isBig = true;

                    }
                    else
                    {
                        hasFlower = true;

                    }

                }
                contacts[i].rigidbody.gameObject.SetActive(false);
            }
        }
    }

    void HandleAnimation()
    {
        //Márgen en las velocidades para evitar problemas con los colliders de los bloques, que generan una pequeña velocidad en el personaje
        animator.SetBool("isWalking", isGrounded && (rigidbody.velocity.x > 0.1 || rigidbody.velocity.x <  -0.1));
        animator.SetBool("isJumping", !isGrounded);
        animator.SetBool("hasFlower", hasFlower);
        animator.SetBool("isBig", isBig);

        animator.SetBool("isFiring", isFiring);

        //Invertimos el sprite si cambiamos de lado
        if (rigidbody.velocity.x < 0 && rigidbody.transform.localScale.x > 0 || rigidbody.velocity.x > 0 && rigidbody.transform.localScale.x < 0)
        {
            Vector3 inverted = new Vector3(rigidbody.transform.localScale.x, rigidbody.transform.localScale.y, rigidbody.transform.localScale.z);
            inverted.x = -rigidbody.transform.localScale.x;
            rigidbody.transform.localScale = inverted;
        }
    }


}
