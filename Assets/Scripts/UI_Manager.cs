using System.Collections;
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
    [SerializeField]
    private Text _mineText;
    [SerializeField]
    private Slider _bossHealthBar;
    [SerializeField]
    private Text _youAreTheWinnerText;
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
        _bossHealthBar = GameObject.Find("Boss_Health").GetComponent<Slider>();
        _bossHealthBar.gameObject.SetActive(false);
        _youAreTheWinnerText.gameObject.SetActive(false);
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL.");
        }
    }

   
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
        if (_waveNumber == 4)
        {
            _waveText.gameObject.SetActive(true);
            _waveText.text = "Final Boss";
            yield return _waveTextTimer;
            _waveText.gameObject.SetActive(false);
        }
        else
        {
            _waveText.gameObject.SetActive(true);
            _waveText.text = "Wave " + _waveNumber;
            yield return _waveTextTimer;
            _waveText.gameObject.SetActive(false);
        }
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

    public void UpdateMineCount(int minesLeft)
    {
        _mineText.text = "Mines: " + minesLeft;
    }
    
    public void UpdateBossHealth(int health)
    {
        if (_bossHealthBar != null)
        {
            _bossHealthBar.value = health;
        }
    }

    public void UpdateThrusterBar(float fuel)
    {
        _thrusterBar.value = fuel;
    }

    public void YouAreTheWinnerSequence()
    {
        _gameManager.GameOver();
        _youAreTheWinnerText.gameObject.SetActive(true);
        _restartLevelText.gameObject.SetActive(true);
        StartCoroutine(YouAreTheWinnerFlicker());
    }

    private IEnumerator YouAreTheWinnerFlicker()
    {
        while (true)
        {
            _youAreTheWinnerText.text = "You Are The Winner";
            yield return new WaitForSeconds(0.25f);
            _gameOverText.text = " ";
            yield return new WaitForSeconds(0.25f);
        }
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
