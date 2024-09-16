using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase para gestionar el trigger de caida fuera del mapa
/// </summary>
public class FallTrigger : MonoBehaviour
{


    /// <summary>
    /// ContactFilter2D para filtrar el contacto con Mario
    /// </summary>
    private ContactFilter2D marioContactFilter;

    /// <summary>
    /// Instancia del RigidBody
    /// </summary>
    private Rigidbody2D rigidBody;

    /// <summary>
    /// Instancia de la clase para gestionar el final de partida
    /// </summary>
    private EndGameTrigger endGameTrigger;


    /// <summary>
    /// Instancia del GameObject del gameplayManager
    /// </summary>    
    public GameObject gameplayManager;


    // Start is called before the first frame update
    void Start()
    {
        //Inicialización de variables

        rigidBody = GetComponent<Rigidbody2D>();
        endGameTrigger = gameplayManager.GetComponent<EndGameTrigger>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Colisión con Mario
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            PlayerController playerScript = collision.gameObject.GetComponent<PlayerController>();
            playerScript.HandleDamage(true);
        }
        else
        {
            //En caso de collision con Goombas, se desactivan dichos GameObjects
            collision.gameObject.SetActive(false);
        }

    }

}
