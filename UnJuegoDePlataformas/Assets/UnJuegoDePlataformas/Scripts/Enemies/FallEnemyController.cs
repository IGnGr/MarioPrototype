using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnemyController : MonoBehaviour
{

    /// <summary>
    /// Variable para indicar la dirección, se calcula aleatoriamente en la instanciación
    /// </summary>
    private float directionX;

    /// <summary>
    /// Velocidad de movimiento de los Goombas
    /// </summary>
    private float speed = 3f;


    /// <summary>
    /// Instancia del RigidBody
    /// </summary>
    private Rigidbody2D rigidbody;


    /// <summary>
    /// Instancia del BoxCollider2D
    /// </summary>
    private BoxCollider2D boxCollider;

    /// <summary>
    /// Valor de anchura del sprite. Se usa en el Raycast
    /// </summary>
    private float width = 0.45f;

    /// </summary>
    /// ContactFilter2D para filtrar el contacto con el suelo
    /// </summary>
    private ContactFilter2D groundContactFilter;

    /// </summary>
    /// Indica si está en el suelo
    /// </summary>
    private bool isGrounded => rigidbody.IsTouching(groundContactFilter);

    private bool isInitialized = false;

    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        //Inicialización de variables
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        groundContactFilter.SetLayerMask(1 << LayerMask.NameToLayer("Ground"));
}

// Update is called once per frame
void FixedUpdate()
    {


        if (isGrounded && !isInitialized)
        {
            //Elección de dirección aleatoria
            directionX = Random.Range(0, 2) == 0 ? -1 : 1;
            isInitialized = true;
            animator.SetBool("isGrounded", true);
        }

        //Checkeo de collisiones
        CheckForWalls();

        //Update de la velocidad con la dirección y velocidad correspondientes
        rigidbody.velocity = new Vector2(directionX * speed, rigidbody.velocity.y);



    }

    /// <summary>
    /// Checkeo de paredes para cambiar de sentido si es necesario
    /// </summary>
    void CheckForWalls()
    {
        RaycastHit2D rightHit, leftHit;


        // Se desactiva y activa el collider para evitar que el Raycast detecte el propio collider en vez del de la pared.
        boxCollider.enabled = false;
        leftHit = Physics2D.Raycast(transform.position, -transform.right, width);
        rightHit = Physics2D.Raycast(transform.position, transform.right, width);
        boxCollider.enabled = true;

        if (leftHit || rightHit)
            directionX = -directionX;
    }


}
