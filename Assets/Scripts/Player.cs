using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
 
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedMultiplier = 2;
    [SerializeField]
    private float _minSpeed = 5f;
    [SerializeField]
    private float _maxSpeed = 10f;
    [SerializeField]
    private float _fuel = 100f;
    
   
    
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _heatSeekMissilePrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = -1f;
    [SerializeField]
    private int _ammoCount = 15;
    [SerializeField]
    private AudioClip _noAmmo;
    [SerializeField]
    private int _maxAmmoCount = 50;
    [SerializeField]
    private int _noAmmoCount = 0;
    [SerializeField]
    private int _mineCount = 0;
    
   

    [SerializeField]
    private int _lives = 3;
    private SpawnManager _spawnManager;
    


    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldsActive = false;
    private bool _isHeatSeekMissileActive = false;
    private bool _isCollectorCooldownRoutinePlaying = false;

    [SerializeField]
    private GameObject _minePrefab;

    [SerializeField]
    private int _shieldHits = 0;
    [SerializeField]
    private float _playerShieldAlpha = 1.0f;
    [SerializeField]
    private GameObject _shieldVisualizer;
    
   
    [SerializeField]
    private GameObject _rightEngine, _leftEngine;

    [SerializeField]
    private int _score;

   private UI_Manager _uiManager;

    [SerializeField]
    private AudioClip _laserClip;
   
    private AudioSource _audioSource;

    public CameraShake cameraShake;

    




    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        _audioSource = GetComponent<AudioSource>();
        
       
        
              

        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL.");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL.");
        }
        if (_audioSource == null)
        {
            Debug.LogError("The AudioSource on the player is NULL!");
        }
        else
        {
            _audioSource.clip = _laserClip;
        }
       

    }

    // Update is called once per frame
    void Update()
    {

        CalculateMovement();
        ThrusterBoost();
        CollectorActivated();
        
        

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (_ammoCount == 0)
            {
                AudioSource.PlayClipAtPoint(_noAmmo, transform.position);
                return;
            }
            FireLaser();
                     
        }  

        if (Input.GetKeyDown(KeyCode.M) && _mineCount > 0)
        {
            Instantiate(_minePrefab, transform.position, Quaternion.identity);
        }

        if (_ammoCount > 50)
        {
            _ammoCount = _maxAmmoCount;
        }

    }

    void CollectorActivated()
    {
        if (Input.GetKeyDown(KeyCode.C) && _isCollectorCooldownRoutinePlaying == false)
        {
            StartCollection();
        }
        else if (Input.GetKeyDown(KeyCode.C) && _isCollectorCooldownRoutinePlaying == true)
        {
            Debug.Log("Cool down in effect");
        }
    }

    void StartCollection()
    {
        GameObject[] _powerupList = GameObject.FindGameObjectsWithTag("Powerup");

        if (_powerupList != null)
        {
            for (int i = 0; i < _powerupList.Length; i++)
            {
                Powerup power = _powerupList[i].GetComponent<Powerup>();
                power.Collect();
                StartCoroutine(CollectorCoolDownRoutine(10f));
            }
        }

        if (_powerupList.Length == 0)
        {
            StartCoroutine(CollectorCoolDownRoutine(5f));
        }
    }

    IEnumerator CollectorCoolDownRoutine(float time)
    {
        _isCollectorCooldownRoutinePlaying = true;
        yield return new WaitForSeconds(time);
        _isCollectorCooldownRoutinePlaying = false;
    }

    void ThrusterBoost()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _speed = _maxSpeed;
            if (_fuel > 0)
            {
                _fuel -= 20f * Time.deltaTime;
                
            }
            else if (_fuel <= 0)
            {
                _speed = _minSpeed;
            }
        }
        else
        {
            _speed = _minSpeed;
            if(_fuel < 100)
            {
                _fuel += 5f * Time.deltaTime;
            }
        }
        _uiManager.UpdateThrusterBar(_fuel);
    }
    
    
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed  * Time.deltaTime);

        

        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if(transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 11)
        {
            transform.position = new Vector3(-11, transform.position.y, 0);
        }
        else if (transform.position.x <= -11)
        {
            transform.position = new Vector3(11, transform.position.y, 0);
        }

    }

   

    
    

    void FireLaser()
    {
        AmmoCount(-1);

        _canFire = Time.time + _fireRate;
       

        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);

        }
        else if (_isHeatSeekMissileActive == true)
        {
            Instantiate(_heatSeekMissilePrefab, transform.position, Quaternion.identity);
        }
       
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }       
      
        _audioSource.Play();  
    
    }

    public void AmmoCount(int bullets)
    {
        
        _ammoCount += bullets;
        _uiManager.UpdateAmmoCount(_ammoCount);
        
    }

    public void MineRefill()
    {
        _mineCount += 3;
        _uiManager.UpdateMineCount(_mineCount);
    }

    public void MineCount(int mines)
    {
        _mineCount += mines;
        _uiManager.UpdateMineCount(_mineCount);
    }

    public void AmmoRefill()
    {
       
        
        _ammoCount += 15;
        
        _uiManager.UpdateAmmoCount(_ammoCount);        
    }

    public void AmmoRemove()
    {
        
        _ammoCount = 0;
        _uiManager.UpdateAmmoCount(_ammoCount);
            
        
    }

    public void Health()
    {
        _lives = 3;
        _rightEngine.SetActive(false);
        _leftEngine.SetActive(false);
        _uiManager.UpdateLives(_lives);
    }

    
    public void Damage()
    {

        if (_isShieldsActive == true)
        {
            _shieldHits++;
            switch (_shieldHits)
            {
                case 1:
                    StartCoroutine(cameraShake.Shake(.5f, .05f));
                    _playerShieldAlpha = 0.75f;
                    _shieldVisualizer.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
                    break;
                case 2:
                    StartCoroutine(cameraShake.Shake(.5f, .15f));
                    _playerShieldAlpha = 0.40f;
                    _shieldVisualizer.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);
                    break;
                case 3:
                    StartCoroutine(cameraShake.Shake(.5f, .25f));
                    _isShieldsActive = false;
                    _shieldVisualizer.SetActive(false);
                    break;
            } 
            return;
        }



        _lives--;

        if (_lives == 2)
        {
            StartCoroutine(cameraShake.Shake(.5f, .5f));
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            StartCoroutine(cameraShake.Shake(.5f, .7f));
            _leftEngine.SetActive(true);
        }   
        

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
         
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);


        }

    }

    
    public void TripleShotActive()
    {
        
        _isTripleShotActive = true;
        
        StartCoroutine(TripleShotPowerDownRoutine());

    }

    IEnumerator TripleShotPowerDownRoutine()
    {

        yield return new WaitForSeconds(5.0f);
        
        _isTripleShotActive = false;


    }
    

    public void SpeedBoostActive()
    {
        if (_isSpeedBoostActive == true)
        {
            _speed *= _speedMultiplier;
            StartCoroutine(SpeedBoostPowerDownRoutine());
        }
    }


    
    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isSpeedBoostActive = false;
        _speed /= _speedMultiplier;
    }

    public void ShieldsActive()
    {
        _isShieldsActive = true;
        _shieldVisualizer.SetActive(true);
        _shieldHits = 0;
        _playerShieldAlpha = 1.0f;
        _shieldVisualizer.GetComponent<SpriteRenderer>().material.color = new Color(1f, 1f, 1f, _playerShieldAlpha);

     
       
    }

    public void HeatSeekMissileActive()
    {
        _isHeatSeekMissileActive = true;
        
        StartCoroutine(HeatSeekMissilePowerdownRoutine());
       
        
       
    }

    IEnumerator HeatSeekMissilePowerdownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _isHeatSeekMissileActive = false;
    }


    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);


    }

    



}
