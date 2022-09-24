using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Boss : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
    [SerializeField]
    private float _exitSpeed = 10f;
    [SerializeField]
    private int _bossID = 0;

    [SerializeField]
    private int _maxHealth = 100;
    [SerializeField]
    private int _health;

    private float _canFire = -1f;
    private float _fireRate = 3f;

    private bool _isHealthGone = false;
    private bool _isSideMovementOn = false;
    private bool _isMissileOn = false;
    private bool _isBeamOn = false;

    [SerializeField]
    private GameObject _bossFrontLaserPrefab;
    [SerializeField]
    private GameObject _bossFrontMissilesPrefab;
    [SerializeField]
    private GameObject _bossBeamPrefab;
    [SerializeField]
    private GameObject _finalBossLaserPrefab;
    
    private Animator _anim;
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _explostionClip;
    

    public Transform target;
   
    private float _dirX;
    private Rigidbody2D _rb;

    private SpawnManager _spawnManager;
    private Player _player;
    private UI_Manager _uiManager;
    WaitForSeconds _missileTimer = new WaitForSeconds(5.0f);

  


    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
       _audioSource = GetComponent<AudioSource>();
        target = GameObject.FindWithTag("Player").transform;
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _rb = GetComponent<Rigidbody2D>();
        _dirX = -1f;
        _anim = GetComponent<Animator>();
        _health = _maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        if (_isMissileOn == true)
        {
            FireMissile();
        }
        else if (_isBeamOn == true)
        {
            FireBeam();
        }
        
        else
        {
            FireLaser();
            
           
        }
        CalculateMovement();
        
        
    }



    void CalculateMovement()
    {
        switch (_bossID)
        {
            case 0: //first boss down
                
                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                if (transform.position.y <= 6.5f)
                {
                    
                    transform.position = new Vector3(transform.position.x, 6.5f, 0);
                }
                if (_isHealthGone == true)
                {
                    StartCoroutine(BossExit());
                    if (transform.position.y >= 20f)
                    {
                        BossLeave();
                    }
                }
                StartCoroutine(WeaponsFire());
           
                StartCoroutine(BossSideMovement());
                
                if (_isSideMovementOn == true)
                {
                    _rb.velocity = new Vector2(_dirX * _speed, _rb.velocity.y);

                }
                
                break;

            case 1: //boss comes from left "Left Side Boss"

                transform.Translate(transform.right * -1 * _speed * Time.deltaTime);
                if (transform.position.x >= 0)
                {
                    transform.position = new Vector3(0, transform.position.y, 0);
                }
                if (_isHealthGone == true)
                {
                    StartCoroutine(BossExit());
                    if (transform.position.x <=-20f)
                    {
                        BossLeave();
                    }
                }
                StartCoroutine(WeaponsFire());

                break;

            case 2: //boss comes from right "Right Side Boss"

                transform.Translate(transform.right * _speed * Time.deltaTime);
                if (transform.position.x <= 0)
                {
                    transform.position = new Vector3(0,transform.position.y, 0);
                }
                if (_isHealthGone == true)
                {
                    StartCoroutine(BossExit());
                    if (transform.position.x >=20f)
                    {
                        BossLeave();
                    }

                }
                StartCoroutine(WeaponsFire());
                break;

            case 3: //final boss going down "Final Boss"

                transform.Translate(Vector3.down * _speed * Time.deltaTime);
                if (transform.position.y <= 4f)
                {

                    transform.position = new Vector3(transform.position.x, 4f, 0);
                }
                if (_isHealthGone == true)
                {
                    _anim.SetTrigger("BlowUpEnemy");
                    _speed = 0;
                    _audioSource.Play();
                    Destroy(GetComponent<Collider2D>());
                    Destroy(this.gameObject, 2f);
                    _uiManager.YouAreTheWinnerSequence();

                }
                StartCoroutine(WeaponsFire2());

                StartCoroutine(BossSideMovement());

                if (_isSideMovementOn == true)
                {
                    _rb.velocity = new Vector2(_dirX * _speed, _rb.velocity.y);

                }

                break;
            default:
                break;
        }

    }
    void BossLeave()
    {
        _spawnManager.SpawnBosses();
        Destroy(this.gameObject);
    }

    IEnumerator BossSideMovement()
    {
        yield return new WaitForSeconds(8f);
        _isSideMovementOn = true;
    }

    void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(1f, 3f);
            _canFire = Time.time + _fireRate;
            Instantiate(_bossFrontLaserPrefab, transform.position, Quaternion.identity);
        }
    }

   

    void FireMissile()
    {
        if (Time.time >_canFire)
        {
            _fireRate = Random.Range(4f, 7f);
            _canFire = Time.time + _fireRate;
            Instantiate(_bossFrontMissilesPrefab, transform.position, Quaternion.identity);
            StartCoroutine(MissileTimer());
        }
    }

    IEnumerator MissileTimer()
    {
        yield return _missileTimer;
        Destroy(this.gameObject);
    }

    void FireBeam()
    {
        
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 6f);
            _canFire = Time.time + _fireRate;
            Instantiate(_bossBeamPrefab, transform.position, Quaternion.identity);
        }
    }

    IEnumerator WeaponsFire()
    {
        while (_player != null)
        {
            FireLaser();
            yield return new WaitForSeconds(10f);
            _isMissileOn = true;
            yield return new WaitForSeconds(10f);
            _isMissileOn = false;
           
        }
    }

    IEnumerator WeaponsFire2()
    {
        while (_player != null)
        {
            FireLaser();
            yield return new WaitForSeconds(10f);
            _isBeamOn = true;
            yield return new WaitForSeconds(10f);
            _isBeamOn = false;
            yield return new WaitForSeconds(10f);
            _isMissileOn = true;
            yield return new WaitForSeconds(10f);
            _isMissileOn = false;
        }
    }


    void TakeDamage(int damage)
    {
        _health -= damage;
        _uiManager.UpdateBossHealth(_health);
        if (_health <= 0)
        {
            _isHealthGone = true;
        }
        
    }

    IEnumerator BossExit()
    {
        yield return new WaitForEndOfFrame();
      
        transform.Translate(Vector3.up * _exitSpeed * Time.deltaTime);
        _health = _maxHealth;
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<wall>())
        {
            _dirX *= -1f;
        }

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                _player.AddScore(10);
            }
        }

        if (other.tag == "Laser")
        {
            
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
                TakeDamage(5);
            }
        }

        if (other.tag == "HeatSeekMissile")
        {
            
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore(10);
                TakeDamage(5);
            }

        }

        if (other.tag == "Mine")
        {
            _player.AddScore(10);
            Destroy(other.gameObject);
            if (_player != null)
            {
                TakeDamage(5);
            }
        }
    }

}
