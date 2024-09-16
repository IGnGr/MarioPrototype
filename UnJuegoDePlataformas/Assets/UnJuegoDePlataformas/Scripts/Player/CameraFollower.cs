using UnityEngine;

/// <summary>
/// Clase encargada de gestionar el Scroll de la camara
/// </summary>
public class CameraFollower : MonoBehaviour
{

    /// <summary>
    /// Transform del jugador
    /// </summary>
    public PlayerController Player;

    /// <summary>
    /// Offset entre el jugador y la camara
    /// </summary>
    private Vector3 offset;

    /// <summary>
    /// Valor fijo de la camara en el eje Y
    /// </summary>
    private float yValue = 4f;




    private void Start()
    {

        Player = GameObject.Find("Player").GetComponent<PlayerController>();
        //Calculo del offset
        offset = Player.transform.position - transform.position;
    }

    private void Update()
    {
        //Calculo del movimiento
        Vector3 newLocation = Player.GetCurrentTransform().position - offset;
        newLocation.y = yValue;
        newLocation.x = Mathf.Max(transform.position.x, Player.GetCurrentTransform().position.x);
        transform.position = newLocation;

    }

    public void MoveCameraToPlayer()
    {
        Vector3 newLocation = Player.GetCurrentTransform().position - offset;
        transform.position = newLocation;
    }
}
