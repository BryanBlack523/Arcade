using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public bool isCoop = false;

    private bool _isGameOver = false;

    [SerializeField]
    private GameObject _pauseMenu;

    private Animator _pauseAnimator;

    public void Start()
    {
        _pauseAnimator = GameObject.Find("Pause_Menu").GetComponent<Animator>();
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetKeyDown(KeyCode.P))
        {
            _pauseMenu.SetActive(true);
            _pauseAnimator.SetBool("isPaused", true);
            Time.timeScale = 1;
        }
    }

    public void ResumeGame()
    {
        _pauseMenu.SetActive(false);
        _pauseAnimator.SetBool("isPaused", false);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        _isGameOver = true;
    }
}
