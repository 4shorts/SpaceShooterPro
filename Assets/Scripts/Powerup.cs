using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;
 
    [SerializeField] //0 = Triple Shot 1 = Speed 2 = Shield 3 = AmmoRefill 4 = Health
    private int powerupID;
    [SerializeField]
    private AudioClip _clip;
    [SerializeField]
    private float _collectionSpeed = 5f;

    private bool _isCollecting = false;

    Vector3 playerPos;
   
    private GameObject _player;



    private void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }





    // Update is called once per frame
    void Update()
    {
        if (_isCollecting)
        {
            PowerupCollectionMove();
        }
        else
        {

            transform.Translate(Vector3.down * _speed * Time.deltaTime);
        }
        
        if (transform.position.y < -6f)
        {
            Destroy(this.gameObject);
        }
        
        
    }

    void PowerupCollectionMove()
    {
        playerPos = _player.transform.position;
        Vector3 direction = transform.position - playerPos;
        direction = -direction.normalized;
        transform.position += direction * _collectionSpeed * Time.deltaTime;
    }

    public void Collect()
    {
        _isCollecting = true;
    }

   
    private void OnTriggerEnter2D(Collider2D other)
    {


        if (other.tag == "Player")
        {


            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if (player != null)
            {

                switch (powerupID)
                {
                    case 0:

                        player.TripleShotActive();

                        break;
                    case 1:

                        player.SpeedBoostActive();

                        break;
                    case 2:

                        player.ShieldsActive();

                        break;
                    case 3:
                        player.AmmoRefill();

                        break;
                    case 4:
                        player.Health();

                        break;
                    case 5:
                        player.HeatSeekMissileActive();

                        break;
                    case 6:
                        player.AmmoRemove();

                        break;
                    default:
                        Debug.Log("default");
                        break;


                }





            }


            Destroy(this.gameObject);

        }

        

    }
}
