using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapons : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8f;
    [SerializeField]
    private float _beamSpeed = 2f;
    public Transform target;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _rotationSpeed = 200f;
    [SerializeField]
    private bool _lasersON;
    [SerializeField]
    private bool _missileOn;
    [SerializeField]
    private bool _beamOn;
    

    // Start is called before the first frame update
    void Start()
    {
            target = GameObject.FindWithTag("Player").transform;
            _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        if (_lasersON == true)
        {
            BossFrontLaser();
        }
        else if (_missileOn == true)
        {
            BossMissiles();
        }
        else
        {
            BossBeam();
        }
    }
    
    void BossFrontLaser()
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

    void BossMissiles()
    {
        if (target != null)
        {
            _rb.transform.position = Vector2.MoveTowards(_rb.transform.position, target.position, _speed * Time.deltaTime);
            transform.up = target.position - _rb.transform.position;
        }
    }

    void BossBeam()
    {
        transform.Translate(Vector3.down * _beamSpeed * Time.deltaTime);
        if (transform.position.y < -100f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
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
    }
}
