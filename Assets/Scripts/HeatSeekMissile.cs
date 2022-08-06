﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class HeatSeekMissile : MonoBehaviour
{
    public Transform target;
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _rotateSpeed = 200f;

    // Update is called once per frame
    void Start()
    {
        target = GameObject.FindWithTag("Enemy").transform;
        _rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
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

}