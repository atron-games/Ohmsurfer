using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private PlayerController playerControllerScript;
    
    public GameObject[] buildings;
  
    public GameObject bossPrefab;
    public GameObject bossPlatform;
    private bool bossSpawned = false;

    public float bossSpawnTime;
    private float spawnOffset;
    private float realSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        StartCoroutine(StandardSpawning());
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControllerScript.gameStarted == false)
        {
            spawnOffset = Time.timeSinceLevelLoad;
        }

        realSpawnTime = bossSpawnTime + spawnOffset;

        if (playerControllerScript.gameOver == false && Time.timeSinceLevelLoad >= realSpawnTime && bossSpawned == false && playerControllerScript.gameStarted == true)
        {
            bossSpawned = true;
            Instantiate(bossPrefab, new Vector2(12, 0), Quaternion.identity);
            Instantiate(bossPlatform, new Vector2(-5, -9.5f), Quaternion.identity);
        }
    }

    //TEST ENVIRONMENT
    IEnumerator StandardSpawning()
    {
        //STANDARD RANDOMIZED SPAWNING
        while(true)
        {
            while (playerControllerScript.gameStarted == true && playerControllerScript.gameOver == false && Time.timeSinceLevelLoad <= realSpawnTime - 2)
            {
                int buildingIndex = Random.Range(0, 3);
                Instantiate(buildings[buildingIndex], new Vector2(20, Random.Range(-7, -3)), Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(2, 3));
            }

            yield return null;
        }
    }
}
