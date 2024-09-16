using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Clase para gestionar el trigger de final de partida
/// </summary>
public class GoalScript : MonoBehaviour
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
    private GameObject gameplayManager;

    /// <summary>
    /// Instancia del AudioManager
    /// </summary>
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        //Inicialización de variables
        rigidBody = GetComponent<Rigidbody2D>();
        gameplayManager = GameObject.Find("GameplayManager");
        endGameTrigger = gameplayManager.GetComponent<EndGameTrigger>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Colisión con Mario
        if (collision.rigidbody.gameObject.CompareTag("Player"))
        {

            //Se acaba la partida con el mensaje de victoria
            endGameTrigger.PopUpRestartScreen("HAS GANADO");
            audioManager.PlayGoalSFX();
        }
    }


}
