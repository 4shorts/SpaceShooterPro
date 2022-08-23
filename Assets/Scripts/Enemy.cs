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
    private GameObject _laserPrefab;
    private Player _player;

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
                    break;
                }
            case 1:
                {
                    _distanceY = _speed * Mathf.Sin(_frequency * Time.time - _spawnTime + _phase) * Time.deltaTime;
                    transform.Translate(Vector3.right * _distanceY);
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    break;
                }
            case 2:
                {
                    Vector2 direction = (Vector2)target.position - _rb.position;
                    direction.Normalize();
                    float rotateAmount = Vector3.Cross(direction, transform.up).z;
                    _rb.angularVelocity = -rotateAmount * _rotateSpeed;
                    Vector3.Cross(direction, transform.up);
                    _rb.velocity = transform.up * _speed;
                    break;
                }

            case 3:
                {
                    transform.Translate(Vector3.down * _speed * Time.deltaTime);
                    break;
                }

            case 4:
                {                   
                    transform.Translate(Vector3.right * _horizontalSpeed * Time.deltaTime);
                    break;
                }
            case 5:
                {
                    transform.Translate(Vector3.left * _horizontalSpeed * Time.deltaTime);
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
      
    }
   
}
