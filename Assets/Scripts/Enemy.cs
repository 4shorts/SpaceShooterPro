using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _horizontalSpeed = 2f;
    [SerializeField]
    private float _spawnTime;
    [SerializeField]
    private float _frequency;
    private float _phase;
    [SerializeField]
    private int _enemyID = 0;
    private float _distanceY;
    [SerializeField]
    private GameObject _enemyLaserPrefab;
    private Player _player;
    [SerializeField]
    private GameObject _enemyMissilePrefab;
    private Animator _anim;
    [SerializeField]
    private AudioClip _explostionClip;
    private AudioSource _audioSource;
    private float _fireRate = 3.0f;
    private float _canFire = -1;

    public Transform target;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _rotateSpeed = 200f;

    private GameObject _enemyShield;
    
    [SerializeField]
    private float LaserCastRadius = .5f;
    [SerializeField]
    private float LaserCastDistance = 8.0f;

     private float DodgeRate = 2.0f;
    

    

    

    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnTime = Time.time;
        _speed *= Random.Range(0.75f, 1.25f);
        target = GameObject.FindWithTag("Player").transform;
        _rb = GetComponent<Rigidbody2D>();
        _enemyShield = GameObject.Find("Enemy_Shield");

        _frequency = Mathf.PI * Random.Range(0.16f, 0.64f);
        _phase = Random.Range(0f, 2f);
       
       

       
       
        
        if (_player == null)
        {
            Debug.LogError("The Player is NULL.");

        }
        _anim = GetComponent<Animator>();
        
        if (_anim == null)
        {
            Debug.LogError("The Animator is NULL");
        }
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource on enemey is NULL");
        }
        else
        {
            _audioSource.clip = _explostionClip;
        }
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
        CalculateMovement();
        

       
    }

    void CalculateMovement()
    {
        switch(_enemyID)
        {
            case 0:
                {

                    _distanceY = 0;
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    AvoidLaser();
                    FireLaser();
                    break;
                }
            case 1:
                {
                    _distanceY = _speed * Mathf.Sin(_frequency * Time.time - _spawnTime + _phase) * Time.deltaTime;
                    transform.Translate(Vector3.right * _distanceY);
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    FireLaser();
                    break;
                }
            case 2:
                {
                    if (_player != null)
                    {
                        Vector2 direction = (Vector2)target.position - _rb.position;
                        direction.Normalize();
                        float rotateAmount = Vector3.Cross(direction, transform.up).z;
                        _rb.angularVelocity = -rotateAmount * _rotateSpeed;
                        Vector3.Cross(direction, transform.up);
                        _rb.velocity = transform.up * _speed;
                        FireEnemyMissile();
                    }
                    break;
                }

            case 3:
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    FireLaser();
                    break;
                }

            case 4:
                {                   
                    transform.Translate(Vector3.right * _horizontalSpeed * Time.deltaTime);
                    FireLaser();
                    break;
                }
            case 5:
                {
                    transform.Translate(Vector3.left * _horizontalSpeed * Time.deltaTime);
                    FireEnemyMissile();
                    break;
                }
            case 6:
                {
                    if (_player != null)
                    { 
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);


                    Vector3 targetDir = target.position - transform.position;
                    float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
                    Quaternion q = Quaternion.AngleAxis(angle + 90, Vector3.forward);


                    transform.GetChild(0).transform.rotation = Quaternion.RotateTowards(transform.rotation, q, _rotateSpeed);
                    FireSmartLaser(q);
                }
                    break;
                }
                           
        }
        

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8f, 0);
        }

        if (transform.position.x > 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x < -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }




    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            
        }
    }

    
    void FireEnemyMissile()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyMissilePrefab, transform.position, Quaternion.identity);
        }
    }

    void FireSmartLaser(Quaternion fireAngle)
    {
        if (Time.time > _canFire)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up);
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_enemyLaserPrefab, transform.position, fireAngle);

        }
    }
    void AvoidLaser()
    {
        RaycastHit2D LaserHit = Physics2D.CircleCast(transform.position, LaserCastRadius, Vector2.down, LaserCastDistance, LayerMask.GetMask("laser"));

        if (LaserHit.collider != null)
        {
            if (LaserHit.collider.CompareTag("Laser"))
            {
                transform.position = new Vector3(transform.position.x - DodgeRate, transform.position.y, transform.position.z);
                DodgeRate -= .3f;
                if (DodgeRate <= 0f)
                {
                    DodgeRate = .05f;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Player")
        {

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
            }

            if (_enemyShield == true)
            {
                _enemyShield.SetActive(false);
                Destroy(_enemyShield);
            }
            else
            { 

            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);
            }

        }

        if (other.tag == "Laser")
        {

            Destroy(other.gameObject);
            if (_enemyShield == true)
            {
                _enemyShield.SetActive(false);
                Destroy(_enemyShield);
            }
            else
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0f;
                _audioSource.Play();

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }
        
        if (other.tag == "HeatSeekMissile")
        {
            Destroy (other.gameObject);
            if (_enemyShield == true)
            {
                _enemyShield.SetActive(false);
                Destroy(_enemyShield);
            }
            else
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0f;
                _audioSource.Play();

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }

        if (other.tag == "Mine")
        {

            Destroy(other.gameObject);
            if (_enemyShield == true)
            {
                _enemyShield.SetActive(false);
                Destroy(_enemyShield);
            }
            else
            {
                if (_player != null)
                {
                    _player.AddScore(10);
                }
                _anim.SetTrigger("OnEnemyDeath");
                _speed = 0f;
                _audioSource.Play();

                Destroy(GetComponent<Collider2D>());
                Destroy(this.gameObject, 2.8f);
            }
        }

    }
   
}
