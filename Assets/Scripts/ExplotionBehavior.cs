using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplotionBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioClip _explostionClip;
    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if( _audioSource == null )
        {
            Debug.LogError("AudioSource in ExplotionBehavior is NULL");
        }
        else
        {
            _audioSource.clip = _explostionClip;
        }
        _audioSource.Play();
        Destroy(this.gameObject, 3.0f);
        
    }

  
}
