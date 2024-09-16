using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentLivesUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject life1,life2,life3;

    private PlayerController player;

    private int nLives;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        nLives = player.nLives;

        if(nLives == 2)
        {
            life3.SetActive(false);
        }
        if(nLives == 1) 
        {
            life2.SetActive(false);
        }
    }
}
