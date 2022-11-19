using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject enemyProjectile;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Shoot", Random.Range(0, 1), Random.Range(1.5f, 3));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Shoot()
    {
        Instantiate(enemyProjectile, transform.position + new Vector3(-1, 0, 0), Quaternion.identity);
    }
}
