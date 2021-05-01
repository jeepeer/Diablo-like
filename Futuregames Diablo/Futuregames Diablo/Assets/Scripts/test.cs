using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour
{
    public GameObject enemyPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) { Instantiate(enemyPrefab, transform.position, Quaternion.identity); }
    }

    private void SpawnEnemy()
    {
        if (Input.GetKeyDown(KeyCode.F)) { Instantiate(enemyPrefab, transform.position, Quaternion.identity); }

    }

}
