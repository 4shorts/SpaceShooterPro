﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    [SerializeField]
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
       
        if (Time.time > _canFire)
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
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -6f)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 8f, 0);
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
      
    }
   
}
