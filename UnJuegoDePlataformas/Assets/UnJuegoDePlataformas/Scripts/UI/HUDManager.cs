using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Clase para gestionar el HUD del juego
/// </summary>
public class HUDManager : MonoBehaviour
{

    /// <summary>
    /// Referencia al GameObject con el texto del tiempo
    /// </summary>
    [SerializeField]
    private Text TimeText;

    /// <summary>
    /// Referencia al GameObject con el texto de los puntos
    /// </summary>
    [SerializeField]
    private Text PointsText;

    /// <summary>
    /// Referencia al GameObject con el texto de las monedas
    /// </summary>
    [SerializeField]
    private Text CoinsText;


    /// <summary>
    /// Variable con el valor de los puntos
    /// </summary>
    private float pointsCount = 0f;

    /// <summary>
    /// Variable con el valor de las monedas
    /// </summary>
    private float coinsCount = 0f;



    // Update is called once per frame
    void Update()
    {

        //Actualizamos los valores en cada frame
        PointsText.text = "Puntos: " + pointsCount.ToString();
        CoinsText.text = coinsCount.ToString();
        TimeText.text = System.TimeSpan.FromSeconds(Time.realtimeSinceStartup).ToString("hh':'mm':'ss");
    }


    /// <summary>
    /// Añade puntos al HUD
    /// </summary>
    public void AddPoints(int points)
    {
        pointsCount += points;
    }

    /// <summary>
    /// Añade monedas al HUD
    /// </summary>
    public void AddCoins(int coins)
    {
        coinsCount += coins;
    }
}
