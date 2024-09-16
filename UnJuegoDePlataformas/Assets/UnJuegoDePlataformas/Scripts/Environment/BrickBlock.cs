using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase del bloque de ladrillo
/// </summary>
public class BrickBlock : MonoBehaviour
{

    /// <summary>
    /// Referencia al animator del GameObject
    /// </summary>
    private Animator animator;

    /// <summary>
    /// ContactFilter2D para filtrar el contacto con Mario
    /// </summary>
    private ContactFilter2D marioContactFilter;

    /// <summary>
    /// Referencia al Rigidbody2D del GameObject
    /// </summary>
    private Rigidbody2D rigidBody;


    /// <summary>
    /// Variable para determinar la animación correcta al bloque
    /// </summary>
    private bool hasCoin = false;

    /// <summary>
    /// Referencia al AudioManager 
    /// </summary>
    AudioManager audioManager;


    // Start is called before the first frame update
    void Start()
    {

        //Inicialización de variables
        animator = GetComponent<Animator>();
        marioContactFilter.SetNormalAngle(90, 90);
        rigidBody = GetComponent<Rigidbody2D>();
        animator.SetBool("hasCoin", hasCoin);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Detección de Mario
        if(rigidBody.IsTouching(marioContactFilter) && collision.rigidbody.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();

            if (playerScript.isBig || playerScript.hasFlower)
            {
                Destroy(this.gameObject);
            }
            else
            {
                //Se ejecuta la animación y el audio
                animator.SetTrigger("MarioHit");
                audioManager.PlayBlockHit();
            }




        }
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        //Cuando se deja de detectar a mario se resetea el Trigger de la animación
        if (collision.rigidbody.gameObject.CompareTag("Player"))
        {
                animator.ResetTrigger("MarioHit");
        }
    }
}
