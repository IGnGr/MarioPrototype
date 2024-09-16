using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudEnemyController : MonoBehaviour
{

    private GameObject player;

    private float speed = 8.0f;

    private Vector3 direction;

    private Rigidbody2D rb;

    private float offsetWithMario = 0.2f;

    private float cooldownTime = 4f;

    public GameObject fallEnemyPrefab;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        StartCoroutine(GenerateFallEnemy(cooldownTime));

    }

    // Update is called once per frame
    void Update()
    {
        
        direction = player.transform.position.x < transform.position.x ? new Vector3(-1,0,0) : new Vector3(1, 0, 0);
        direction = Mathf.Abs(player.transform.position.x - transform.position.x) <= offsetWithMario ? new Vector3(0, 0, 0) :direction;


        rb.velocity = direction * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            PlayerController playerScript = player.GetComponent<PlayerController>();
            playerScript.HandleDamage(false);

        }
    }

    private IEnumerator GenerateFallEnemy(float cooldownTime)
    {
        while(true)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) <= offsetWithMario)
            {
                GameObject fallEnemy = Instantiate<GameObject>(fallEnemyPrefab, transform.position, Quaternion.identity);
            }

            yield return new WaitForSeconds(cooldownTime);
        }
    }

}
