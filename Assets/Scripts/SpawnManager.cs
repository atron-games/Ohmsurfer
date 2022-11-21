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

    public float bossSpawnTime;
    public GameObject bossPrefab;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(TestGround());
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time == bossSpawnTime)
        {
            Instantiate(bossPrefab, new Vector2(12, 0), Quaternion.identity);
        }
    }

    //TEST ENVIRONMENT
    IEnumerator TestGround()
    { 
        while(playerControllerScript.gameOver == false && playerControllerScript.bossKilled == false)
        {
            Instantiate(chargedGround, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2);
            Instantiate(stdGround, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2);
            Instantiate(chargedGround, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(2);
            Instantiate(stdGround, spawnPosition, Quaternion.identity);
            //Instantiate(enemyPrefab, spawnPosition + enemyOffset, Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
    }
}
