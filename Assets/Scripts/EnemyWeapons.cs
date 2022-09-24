using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapons : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    public Transform target;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _rotateSpeed = 200f;
    [SerializeField]
    private bool _isMissile = false;
    [SerializeField]
    private GameObject _explosionPrefab;
    


    void Start()
    {
        
            target = GameObject.Find("Player").GetComponent<Player>().transform;
            _rb = GetComponent<Rigidbody2D>();
        
    }




    // Update is called once per frame
    void Update()
    {
        if (_isMissile == true)
        {
            EnemyMissile();
        }
        else
        {
            EnemyLaser();
        }
        
       
    }

    void EnemyLaser()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
       

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    void EnemyMissile()
    {
        if (target != null)
        {
            _rb.transform.position = Vector2.MoveTowards(_rb.transform.position, target.position, _speed * Time.deltaTime);
            transform.up = target.position - _rb.transform.position;
              
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
                Destroy(this.gameObject);
            }
        }

        if (other.tag == "Powerup")
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject, .1f);
            Destroy(this.gameObject);
        }
        
    }
}
