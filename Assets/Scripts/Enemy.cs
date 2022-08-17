using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
    private float _spawnTime;
    [SerializeField]
    private float _frequency;
    private float _phase;
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
  
    

    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
        _spawnTime = Time.time;
        _speed *= Random.Range(0.75f, 1.25f);

        if (Random.Range(0, 4) == 3)
        {
            _enemyID = 1;
            _frequency = Mathf.PI * Random.Range(0.16f, 0.64f);
            _phase = Random.Range(0f, 2f);
        }
        else
        {
            _enemyID = 0;
        }
       
        
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
       
        if (Time.time > _canFire && _enemyID == 0)
        {
            _fireRate = Random.Range(3f, 7f);

            _canFire = Time.time + _fireRate;

            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

       
    }

    void CalculateMovement()
    {
        if (_enemyID == 1)
        {
            _distanceY = _speed * Mathf.Sin(_frequency * Time.time - _spawnTime + _phase) * Time.deltaTime;
        }
        else
        {
            _distanceY = 0f;
        }
        transform.Translate(Vector3.right * _distanceY);
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            
            Player player = other.transform.GetComponent<Player>();
             
            if (player != null)
            {
                player.Damage();
            }
           
            _anim.SetTrigger("OnEnemyDeath");
            _speed = 0f;
            _audioSource.Play();
            Destroy(this.gameObject, 2.8f);

        }
       
        if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);

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

        if (other.tag == "HeatSeekMissile")
        {
            Destroy (other.gameObject);
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
