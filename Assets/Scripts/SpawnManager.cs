using System.Collections;
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
    [SerializeField]
    private GameObject[] _enemies;
    

   

   public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnAlienSaucerRoutine());
        StartCoroutine(SpawnAlienSaucer2Routine());
        StartCoroutine(SpawnSmartEnemyRoutine());
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int randomEnemy = Random.Range(0, 4);
            GameObject newEnemy = Instantiate(_enemies[randomEnemy], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
            
    }
    IEnumerator SpawnAlienSaucerRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(-10f, Random.Range(3f, 4f), 0);
            GameObject newEnemy = Instantiate(_enemies[4], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(20.0f, 40.0f));
        }
    }
    IEnumerator SpawnAlienSaucer2Routine()
    {
        yield return new WaitForSeconds(30.0f);
        while(_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(11f, Random.Range(2.5f, 4.5f), 0);
            GameObject newEnemy = Instantiate(_enemies[5], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(Random.Range(15.0f, 60.0f));
        }
    }

    IEnumerator SpawnSmartEnemyRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while(_stopSpawning==false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            GameObject newEnemy = Instantiate(_enemies[6], posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }


    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int randomPowerUp = Random.Range(0, 7);
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
