﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private bool _stopSpawning = false;
    [SerializeField]
    private GameObject[] _powerups;

    // Start is called before the first frame update
    void Start()

    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
            
    }

    IEnumerator SpawnPowerupRoutine()
    {

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int randomPowerUp = Random.Range(0, 2);
            float wait_time = Random.Range(3, 8);
            Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(wait_time);
        }
    }


    public void OnPlayerDeath()
    {

        _stopSpawning = true;

    }
}