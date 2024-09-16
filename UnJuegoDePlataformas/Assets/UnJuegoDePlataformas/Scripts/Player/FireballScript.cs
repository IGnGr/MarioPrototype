using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{

    PlayerController playerController;

    public Vector3 currentDirection;

    float speed = 4.0f;

    Rigidbody2D rb;

    private int lifespan = 3;

    /// <summary>
    /// Referencia al AudioManager 
    /// </summary>
    AudioManager audioManager;

    /// <summary>
    /// Instancia del BoxCollider2D
    /// </summary>
    private BoxCollider2D boxCollider;

    /// <summary>
    /// Referencia al HUDManager 
    /// </summary>
    private HUDManager hudManager;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        boxCollider = GetComponent<BoxCollider2D>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        hudManager = GameObject.Find("HUDManager").GetComponent<HUDManager>();

        currentDirection =  !playerController.isFacingLeft ? new Vector3(1,-1,0) : new Vector3(-1,-1,0);
        StartCoroutine( DestroyOnSecs(lifespan));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        rb.velocity = currentDirection * speed;

        CheckForWalls();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            
            currentDirection = Vector3.ProjectOnPlane(currentDirection, Vector3.down);
            Debug.Log(currentDirection);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemies"))
        {

            Destroy(collision.gameObject);
            Destroy(this.gameObject);
            hudManager.AddPoints(100);

        }
    }

    IEnumerator DestroyOnSecs(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }


    /// <summary>
    /// Checkeo de parede
    /// </summary>
    void CheckForWalls()
    {
        RaycastHit2D rightHit, leftHit;


        // Se desactiva y activa el collider para evitar que el Raycast detecte el propio collider en vez del de la pared.
        boxCollider.enabled = false;
        leftHit = Physics2D.Raycast(transform.position, -transform.right, boxCollider.size.x, 1 << LayerMask.NameToLayer("Ground"));
        rightHit = Physics2D.Raycast(transform.position, transform.right, boxCollider.size.x, 1 << LayerMask.NameToLayer("Ground"));
        boxCollider.enabled = true;

        if (leftHit || rightHit)
        {
            Destroy(this.gameObject);
        }
            
    }
}
