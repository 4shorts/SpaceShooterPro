using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSeekMissile : MonoBehaviour
{
    private bool _closestEnemyNull = false;
    private Rigidbody2D rigidBody;
    [SerializeField]
    private float _moveSpeed = 10f;
    [SerializeField]
    private float _angleChangeSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        
        FindClosestEnemy();
    }

    public void FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;
        Enemy closestEnemy = null;
        Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

        foreach (Enemy currentEnemy in allEnemies)
        {
            float _distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (_distanceToEnemy < distanceToClosestEnemy)
            {
                distanceToClosestEnemy = _distanceToEnemy;
                closestEnemy = currentEnemy;
            }
        }

        if (closestEnemy != null)
        {
            _closestEnemyNull = false;
            Vector2 targetDirection = (Vector2)closestEnemy.transform.position - rigidBody.position;
            targetDirection.Normalize();
            float rotateAmount = Vector3.Cross(targetDirection, transform.up).z;
            rigidBody.angularVelocity = -_angleChangeSpeed * rotateAmount;
            rigidBody.velocity = transform.up * _moveSpeed;

        }
        else if (closestEnemy == null)
        {
            _closestEnemyNull = true;
            FixedUpdate();
        }


    }
    void FixedUpdate()
    {
        if (_closestEnemyNull == true)
        {
            rigidBody.velocity = transform.up * _moveSpeed;
            transform.rotation = Quaternion.identity;
        }
    }

    
}