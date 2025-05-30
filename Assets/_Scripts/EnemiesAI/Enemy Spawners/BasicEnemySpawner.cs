using System.Collections.Generic;
using UnityEngine;
using VFolders.Libs;
using static EnemiesEnums;

public class BasicEnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject playerRanged;
    [SerializeField] Collider pathArea;
    [SerializeField] Enemy enemyToSpawn;
    [SerializeField][Range(3,10)] int numberOfEnemies;
    [SerializeField] int minToSpawn;
    [SerializeField] int maxToSpawn;
    [SerializeField] bool randomized;
    [SerializeField] Transform[] transformsPAths;
    [SerializeField] Transform[] SpawnSpot;
    private List<GameObject> enemies;
    bool playerIn;
    private void Start()
    {
        enemies = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !playerIn)
        {
            playerIn = true;
            SpawnEnemies();
        }
    }

    private void OnTriggerExit(Collider other) //====================================NEED T CHECK THIS FUNCTIUON TO UNDERSTAND IT ===============
    {
        if (other.CompareTag("Player"))
        {
            for (int i = enemies.Count - 1; i >= 0; i--) // Iterate backwards
            {
                Destroy(enemies[i]); // Destroy the enemy
                enemies.RemoveAt(i); // Remove it from the list
            }
        }
        playerIn = false;
    }

    private void SpawnEnemies()
    {
        if(randomized)
        {
            //Instantiate(enemy, spawnPos[Random.Range(0, spawnPos.Length)].position, enemy.transform.rotation);            
            int spawnAmount = Random.Range(minToSpawn, maxToSpawn);
            Debug.Log(minToSpawn + " " + spawnAmount + " " + maxToSpawn);
            for(int i = 0; i < spawnAmount; i++)
            {

                GameObject enemyInstance = EnemiesManager.instance.enemies[(int)enemyToSpawn];
                GameObject enemy = Instantiate(enemyInstance, SpawnSpot[Random.Range(0, SpawnSpot.Length)].position, enemyInstance.transform.rotation);
                FatherSpawner q = enemy.GetComponent<FatherSpawner>();
                for (int e = 0; e < transformsPAths.Length; e++)
                {
                    q.transforms[e] = transformsPAths[e];
                }
                enemies.Add(enemy);
            }
        }
        else
        {
            // There is an error with SerializeField and the arrays that I need fix ---------------------------------------<       >-------------------
            for (int i = 0; i < numberOfEnemies; i++)
            {
                GameObject enemyInstance = EnemiesManager.instance.enemies[(int)enemyToSpawn];
                GameObject enemy = Instantiate(enemyInstance, SpawnSpot[Random.Range(0, SpawnSpot.Length)].position, enemyInstance.transform.rotation);
                enemy.GetComponent<FatherSpawner>();
                for (int e = 0; e < transformsPAths.Length; e++)
                {
                    enemy.GetComponent<FatherSpawner>().transforms[e] = transformsPAths[e];
                }

                enemies.Add(enemy);
            }
        }
    }

}
