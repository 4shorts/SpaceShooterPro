using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4f;
    private Player _player;

    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
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

            Destroy(this.gameObject);

        }
       
        if (other.tag == "Laser")
        {
            Player player = other.transform.GetComponent<Player>();
            Destroy(other.gameObject);
            if (player != null)
            {
                player.AddScore(10);
            }
            //Add 10 to score


            Destroy(this.gameObject);
            

        }
      
    }
}
