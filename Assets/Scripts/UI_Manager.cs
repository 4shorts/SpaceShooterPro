﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] 
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartLevelText;
    [SerializeField]
    private Text _ammoText;
    [SerializeField]
    private Slider _thrusterBar;
    [SerializeField]
    private Text _enemiesRemaining;
    [SerializeField]
    private Text _waveText;

    WaitForSeconds _waveTextTimer = new WaitForSeconds(2f);
   
    
   
    
    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
       
        _scoreText.text = "Score: 0";
        _ammoText.text = "Shots: 15/50";
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        _thrusterBar = GameObject.Find("Thruster_Bar").GetComponent<Slider>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

    // Update is called once per frame
   
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateEnemiesRemaining(int currentEnemies)
    {
        _enemiesRemaining.text = currentEnemies.ToString();
    }

    public IEnumerator WaveText(int _waveNumber)
    {
        _waveText.gameObject.SetActive(true);
        _waveText.text = "Wave " + _waveNumber;
        yield return _waveTextTimer;
        _waveText.gameObject.SetActive(false);
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
        
    }

    public void UpdateAmmoCount(int bulletsLeft)
    {
        _ammoText.text = "Shots: " + bulletsLeft + "/50";
    }


    public void UpdateThrusterBar(float fuel)
    {
        _thrusterBar.value = fuel;
    }


    
     
    
    void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(GameOverFlicker());
    }

    private IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.text = "GAME OVER";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = " ";
            yield return new WaitForSeconds(0.25f);
        }
    }

}
