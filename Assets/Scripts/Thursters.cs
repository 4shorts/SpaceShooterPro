using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thursters : MonoBehaviour
{
    Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Debug.LogError("Player script not found within Thrusters script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        ThrusterBoost();
    }

    void ThrusterBoost()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            _player.ThrusterBoostActivated();
        }
        else
        {
            _player.ThrusterBoostDeactivated();
        }
    }
}
