using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Clase para gestionar el fin de partida
/// </summary>
public class EndGameTrigger : MonoBehaviour
{

    /// <summary>
    /// Referencia al canvas donde se encuentra el díalogo de reinicio de partida
    /// </summary>
    [SerializeField]
    private GameObject endGameScreenInCanvas;

    /// <summary>
    /// Referencia al canvas donde se encuentra la puntuación
    /// </summary>
    [SerializeField]
    private GameObject scoreInCanvas;

    /// <summary>
    /// Instancia del AudioManager
    /// </summary>
    AudioManager audioManager;


    /// <summary>
    /// Referencia al texto del diálogo de fin de partida
    /// </summary>
    public TMP_Text textInScreen;

    /// <summary>
    /// Función para parar el tiempo y mostrar el diálogo de fin de partida
    /// </summary>
    public void PopUpRestartScreen()
    {
        Time.timeScale = 0;
        endGameScreenInCanvas.gameObject.SetActive(true);
        scoreInCanvas.gameObject.SetActive(false);
        audioManager.PlayDeathSFX();

    }

    public void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }


    /// <summary>
    /// Función para mostrar el diálogo de final de partida cambiando el texto por defecto (YOU LOST).
    /// </summary>
    public void PopUpRestartScreen(string text)
    {
        textInScreen.text = text;
        PopUpRestartScreen();
    }


    /// <summary>
    /// Función reiniciar la partida recargando la escena
    /// </summary>
    public static void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
