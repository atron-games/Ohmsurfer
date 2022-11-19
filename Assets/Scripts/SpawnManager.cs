using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject stdGround;
    public GameObject chargedGround;
    public GameObject enemyPrefab;
    
    public Vector2 spawnPosition;
    public Vector2 enemyOffset;
    
    private PlayerController playerControllerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TestGround());

        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //TEST ENVIRONMENT
    IEnumerator TestGround()
    { 
        while(true)
        {
            Instantiate(chargedGround, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2);
            Instantiate(stdGround, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2);
            Instantiate(chargedGround, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2);
            Instantiate(stdGround, spawnPosition, Quaternion.identity);
            Instantiate(enemyPrefab, spawnPosition + enemyOffset, Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
    }
}
