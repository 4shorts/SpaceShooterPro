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
    


    void Start()
    {
        if (target != null)
        {
            target = GameObject.FindWithTag("Player").transform;
            _rb = GetComponent<Rigidbody2D>();
        }
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

            Vector2 direction = (Vector2)target.position - _rb.position;

            direction.Normalize();

            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            _rb.angularVelocity = -rotateAmount * _rotateSpeed;

            Vector3.Cross(direction, transform.up);

            _rb.velocity = transform.up * _speed;


            if (transform.position.y > 8f)
            {
                Destroy(this.gameObject);
            }
            if (transform.position.y < -8f)
            {
                Destroy(this.gameObject);
            }
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
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
        
    }
}
