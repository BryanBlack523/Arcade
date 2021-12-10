using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Sprite[] _livesSprites;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;

    public int _bestScore;
    [SerializeField]
    public Text bestText;

    // Start is called before the first frame update
    void Start()
    {
        

        _gameOverText.gameObject.SetActive(false);
        _scoreText.text = "Score: " + 0;
        bestText.text = "Best: " + PlayerPrefs.GetInt("BestScore", 0);
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_gameManager == null)
            Debug.LogError("Game manager is null");
    }

    public void ResumePlay()
    {
        GameManager gm = GameObject.Find("Game_Manager").GetComponent<GameManager>();
        gm.ResumeGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void CheckForBestScore(int currentScore)
    {
        if (currentScore > _bestScore)
        {
            _bestScore = currentScore;
            bestText.text = "Best: " + _bestScore;
            PlayerPrefs.SetInt("BestScore", _bestScore);
        }
            
    }

    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _livesSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        _gameOverText.gameObject.SetActive(true);
        _restartText.gameObject.SetActive(true);
        StartCoroutine(FlickerRoutine());
        _gameManager.GameOver();
    }

    IEnumerator FlickerRoutine()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

        }
    }
}
