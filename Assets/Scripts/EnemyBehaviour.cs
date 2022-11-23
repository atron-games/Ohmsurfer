using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject enemyProjectile;
    private PlayerController playerControllerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        InvokeRepeating("Shoot", Random.Range(3, 4), Random.Range(1.5f, 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        if(playerControllerScript.gameOver == false)
        {
            Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
        }
    }
}
