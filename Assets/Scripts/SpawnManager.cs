using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Array Variables
    public GameObject[] enemyPrefabs;

    // Float Variables
    private float spawnRangeX, spawnRangeY; // Range of starting position values
    private float startDelay, spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        spawnRangeX = 10;
        startDelay = 0.5f;
        spawnInterval = 1.5f;
    }

    // Begins spawning food
    public void StartSpawn()
    {
        InvokeRepeating("SpawnRandomEnemy", startDelay, spawnInterval);
    }

    // Randomly generate food index and spawn position
    void SpawnRandomEnemy()
    {
        // Determines spawn location
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        float yPos = 6;
        Vector3 spawn = new Vector3(xPos, yPos, 0);
        
        // Spawns random enemy
        int enemyIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject toSpawn = enemyPrefabs[enemyIndex];
        Instantiate(toSpawn, spawn, toSpawn.transform.rotation);
    }
}
