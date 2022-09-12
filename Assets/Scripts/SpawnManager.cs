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

    private int _remainingEnemies;

    private int _currentWave;

    WaitForSeconds _enemySpawnTimer = new WaitForSeconds(5.0f);

    private UI_Manager _uiManager;

    Coroutine _co;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

        if(_uiManager == null )
        {
            Debug.LogError("UI_Manager is NULL in SpawnManager");
        }
        _currentWave = 0;

    }

    private void Update()
    {
        _uiManager.UpdateEnemiesRemaining(_remainingEnemies);
    }


    public void StartSpawning()
    {
        _currentWave++;
        NextWave();
       
    }

    void NextWave()
    {
        switch(_currentWave)
        {
            case 1:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(5));
                break;
            case 2:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(10));
                break;
            case 3:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(20));
                break;
            case 4:
                StartCoroutine(_uiManager.WaveText(_currentWave));
                StartCoroutine(SpawningEnemiesRoutine(1));
                break;

            default:
                Debug.Log("Invalid wave");
                break;
        }
    }

    IEnumerator SpawningEnemiesRoutine(int _remainingEnemiesToSpawn)
    {
        _remainingEnemies = _remainingEnemiesToSpawn;
        _co = StartCoroutine(SpawnPowerupRoutine());

        while (_stopSpawning == false)
        {
            yield return _enemySpawnTimer;
            SpawnRoutine();

            if (_remainingEnemies < 1)
            {
                _remainingEnemies = 0;
                StopCoroutine(_co);
                _currentWave++;
                NextWave();
                yield break;
            }
        }
    }

    void SpawnRoutine()
    {
        float _randomX = Random.Range(-9f, 9f);
        float _randomY = Random.Range(1f, 5f);
        int randomEnemy = GenerateEnemyIndex(Random.Range(0, 101));

        Vector3 _enemySpawn = new Vector3(_randomX, 7f, 0f);
        Vector3 _enemyNewMovementSpawn = new Vector3(_randomX, 7f, 0f);
        Vector3 _agressiveEnemySpawn = new Vector3(_randomX, 7f, 0f);
        Vector3 _shieldEnemySpawn = new Vector3(_randomX, 7f, 0f);
        Vector3 _alienSaucerSpawn = new Vector3(-11, _randomY, 0f);
        Vector3 _alienSaucer2Spawn = new Vector3(11, _randomY, 0f);
        Vector3 _smartEnemySpawn = new Vector3(_randomX, 7f, 0f);

        switch(randomEnemy)
        {
            case 0:
                GameObject _newEnemy = Instantiate(_enemies[0], _enemySpawn, Quaternion.identity);
                _newEnemy.transform.parent = _enemyContainer.transform;
                break;
            case 1:
                GameObject _newEnemyNewMovement = Instantiate(_enemies[1], _enemyNewMovementSpawn, Quaternion.identity);
                _newEnemyNewMovement.transform.parent = _enemyContainer.transform;
                break;
            case 2:
                GameObject _newAgressiveEnemy = Instantiate(_enemies[2], _agressiveEnemySpawn, Quaternion.identity);
                _newAgressiveEnemy.transform.parent = _enemyContainer.transform;
                break;
            case 3:
                GameObject _newShieldEnemy = Instantiate(_enemies[3], _shieldEnemySpawn, Quaternion.identity);
                _newShieldEnemy.transform.parent = _enemyContainer.transform;
                break;
            case 4:
                GameObject _newAlienSaucer = Instantiate(_enemies[4], _alienSaucerSpawn, Quaternion.identity);
                _newAlienSaucer.transform.parent = _enemyContainer.transform;
                break;
            case 5:
                GameObject _newAlienSaucer2 = Instantiate(_enemies[5], _alienSaucer2Spawn, Quaternion.identity);
                _newAlienSaucer2.transform.parent = _enemyContainer.transform;
                break;
            case 6:
                GameObject _newSmartEnemy = Instantiate(_enemies[6], _smartEnemySpawn, Quaternion.identity);
                _newSmartEnemy.transform.parent = _enemyContainer.transform;
                break;
            default:
                break;

        }
        _remainingEnemies--;

    }
    int GenerateEnemyIndex(int random)
    {
        if (random >= 0 && random < 30)
        {
            return 0; //enemy
        }
        else if (random >= 30 && random < 55)
        {
            return 1; //enemy new movement
        }
        else if (random >= 55 && random < 65)
        {
            return 2; //agressive enemy
        }
        else if (random >= 65 && random < 85)
        {
            return 3; //shield enemy
        }
        else if (random >= 85 && random < 90)
        {
            return 4; //alien saucer
        }
        else if (random >= 90 && random < 95)
        {
            return 5; //alien saucer 2
        }
        else
        {
            return 6; //smart enemy
        }
    }

   
    int GeneratePowerupIndex(int random)
    {
        if (random >= 30 && random < 50)
        {
            return 0; //triple_Shot
        }
        else if (random >= 50 && random < 60)
        {
            return 1; //Speed
        }
        else if (random >= 60 && random < 75)
        {
            return 2; //Shield
        }
        else if (random >= 0 && random < 30)
        {
            return 3; //ammo
        }
        else if (random >= 75 && random < 80)
        {
            return 4; //healthCollectable
        }
        else if (random >= 80 && random < 85)
        {
            return 5; //heatSeekMissile
        }
        else if (random >=85 && random < 90)
        {
            return 7; //minepowerup
        }
        else
        {
            return 6; //NegativeAmmo
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
