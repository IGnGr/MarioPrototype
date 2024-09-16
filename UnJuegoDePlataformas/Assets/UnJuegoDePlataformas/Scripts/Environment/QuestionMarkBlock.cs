using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// Clase del bloque "?"
/// </summary>
public class QuestionMarkBlock : MonoBehaviour
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
    /// Referencia al SpriteRenderer del GameObject 
    /// </summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>
    /// Sprite del bloque vacío
    /// </summary>
    [SerializeField]
    private Sprite emptyBlockSprite;

    /// <summary>
    /// Variable para guardar el estado del bloque
    /// </summary>
    private bool isEmpty = false;


    /// <summary>
    /// Variable para determinar la animación correcta al bloque
    /// </summary>
    [SerializeField]
    private bool hasCoin = true;

    private PlayerController playerController;


    [SerializeField]
    GameObject mushroomPrefab;

    [SerializeField]
    GameObject flowerPrefab;


    /// <summary>
    /// Referencia al AudioManager 
    /// </summary>
    AudioManager audioManager;

    /// <summary>
    /// Referencia al HUDManager 
    /// </summary>
    private HUDManager hudManager;


    // Start is called before the first frame update
    void Start()
    {

        //Inicialización de variables
        animator = GetComponent<Animator>();
        marioContactFilter.SetNormalAngle(90, 90);
        rigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetBool("hasCoin", hasCoin);
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        hudManager = GameObject.Find("HUDManager").GetComponent<HUDManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Detección de Mario
        if (!isEmpty && rigidBody.IsTouching(marioContactFilter) && collision.rigidbody.gameObject.CompareTag("Player"))
        {

            //Se ejecuta la animación y el audio
            animator.SetTrigger("MarioHit");

            //Se actualiza el bloque a uno vacío
            spriteRenderer.sprite = emptyBlockSprite;
            isEmpty = true;

            if (hasCoin)
            {
                //Se ejecuta la animación y el audio
                audioManager.PlayCoin();

                //Se actualiza el HUD
                hudManager.AddPoints(100);
                hudManager.AddCoins(1);
            }
            else
            {

                if (!playerController.isBig)
                    SpawnMushroom();
                else
                    SpawnFlower();

            }



        }
    }

    private void SpawnMushroom()
    {
        GameObject mushroom =  Instantiate<GameObject>(mushroomPrefab,transform.position, Quaternion.identity);
        mushroom.transform.position = new Vector3 (mushroom.transform.position.x, mushroom.transform.position.y + mushroom.transform.localScale.y, mushroom.transform.position.z);
    }

    private void SpawnFlower()
    {
        GameObject flower = Instantiate<GameObject>(flowerPrefab, transform.position, Quaternion.identity);
        flower.transform.position = new Vector3(flower.transform.position.x, flower.transform.position.y + flower.transform.localScale.y, flower.transform.position.z);
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
