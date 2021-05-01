using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject enemyPrefab;

    public int enemyCount;
    private int maxEnemyCount = 4;
    [SerializeField] private float enemySpawnTimer;
 
	// Use this for initialization
	void Start ()
    {
        enemyCount = maxEnemyCount;
	}
	
	// Update is called once per frame
	void Update ()
    {
        enemySpawnTimer += Time.deltaTime;


        SpawnEnemies();
	}

    private void SpawnEnemies()
    {       
        if (enemyCount < maxEnemyCount && enemySpawnTimer >= 10)
        {
            Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemySpawnTimer = 0;
        }           
        else { return; }
    }
}
