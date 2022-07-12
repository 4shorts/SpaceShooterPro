using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    //handle to Text
   // [SerializeField]
    //private Text _scoreText;
    [SerializeField] private TMP_Text _scoreText;
    // Start is called before the first frame update
    void Start()
    {
       
        _scoreText.text = "Score: 0";

        
    }

    // Update is called once per frame
    void Update()
    {


        
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }
}
