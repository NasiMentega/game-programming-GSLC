using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    
    public GameObject enemyPrefab; 
    public int enemyCount = 2; 
    public float spawnRadius = 5f; 
    public float minDistance = 2f; 

    public float spawnInterval = 3f; 

    private Transform player;

    void Start()
    {
        StartCoroutine(CheckForPlayer());
        StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator CheckForPlayer()
    {
        while (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                Debug.Log("Player ditemukan: " + player.name);
            }
            else
            {
                Debug.LogWarning("Player belum ditemukan, mencoba lagi...");
            }
            yield return new WaitForSeconds(2f);
        }
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (true) 
        {
            if (player != null)
            {
                SpawnEnemies();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 spawnPosition = GetValidSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    private Vector2 GetValidSpawnPosition()
    {
        Vector2 spawnPosition;
        int attempts = 10; 

        do
        {
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
            spawnPosition = (Vector2)transform.position + randomOffset;
            attempts--;
        }
        while (attempts > 0 && player != null && Vector2.Distance(spawnPosition, player.position) < minDistance);

        return spawnPosition;
    }
}
