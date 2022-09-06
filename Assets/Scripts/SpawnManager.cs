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
       
        
    }

    int GenerateEnemyIndex(int random)
    {
        if (random >= 0 && random < 30)
        {
            return 0;
        }
        else if (random >= 30 && random < 55)
        {
            return 1;
        }
        else if (random >= 55 && random < 65)
        {
            return 2;
        }
        else if (random >= 65 && random < 85)
        {
            return 3;
        }
        else if (random >= 85 && random < 90)
        {
            return 4;
        }
        else if (random >= 90 && random < 95)
        {
            return 5;
        }
        else
        {
            return 6;
        }
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while(_stopSpawning == false)
        {

            int randomEnemy = GenerateEnemyIndex(Random.Range(0, 101));
     
            if (_enemies[4])
            {
                Vector3 posToSpawn = new Vector3(-10f, Random.Range(3f, 4f), 0);
                GameObject newEnemy = Instantiate(_enemies[randomEnemy], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else if (_enemies[5])
            {
                Vector3 posToSpawn = new Vector3(11f, Random.Range(2.5f, 4.5f), 0);
                GameObject newEnemy = Instantiate(_enemies[randomEnemy], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            else
            {
                Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
                GameObject newEnemy = Instantiate(_enemies[randomEnemy], posToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            
            yield return new WaitForSeconds(5.0f);
        }
            
    }
    int GeneratePowerupIndex(int random)
    {
        if (random >= 30 && random < 50)
        {
            return 0;
        }
        else if (random >= 50 && random < 60)
        {
            return 1;
        }
        else if (random >= 60 && random < 75)
        {
            return 2;
        }
        else if (random >= 0 && random < 30)
        {
            return 3;
        }
        else if (random >= 75 && random < 80)
        {
            return 4;
        }
        else if (random >= 80 && random < 90)
        {
            return 5;
        }
        else
        {
            return 6;
        }
    }


    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 7, 0);
            int randomPowerUp = GeneratePowerupIndex(Random.Range(0, 101));
            float wait_time = Random.Range(3, 8);
            GameObject newPowerup = Instantiate(_powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(wait_time);
        }
    }
    


    public void OnPlayerDeath()
    {

        _stopSpawning = true;

    }
}
