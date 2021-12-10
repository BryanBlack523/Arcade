using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public bool isPlayerOne = false;
    [SerializeField]
    public bool isPlayerTwo = false;

    [SerializeField]
    private float _speed = 6f;
    private float _speedMultiplier = 2;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _fireCoolDown = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private float _powerupDownTimer = 5.0f;
    [SerializeField]
    private GameObject[] _damageVisualizer;

    [SerializeField]
    private int _score = 0;

    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _shield;

    private bool _isTripleShotEnabled = false;
    private bool _isSpeedBoostEnabled = false;
    private bool _isShieldEnabled = false;

    [SerializeField]
    private AudioClip _laserSound;
    private AudioSource _audioSource;

    void Start()
    {
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is null");
        }
        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is null");
        }
        if (_audioSource == null)
            Debug.LogError("Audio source is null");
        else
            _audioSource.clip = _laserSound;
        if (_gameManager == null)
        {
            Debug.LogError("Game Manager is null");
        }

        if (_gameManager.isCoop == false)
            transform.position = new Vector3(0, 0, 0);
    }

    void Update()
    {
        if (isPlayerOne)
        {
            Move("Horizontal", "Vertical");
            Fire(KeyCode.LeftShift);
        }
        else if (isPlayerTwo)
        {
            Move("HorizontalTwo", "VerticalTwo");
            Fire(KeyCode.Space);
        }
        
    }

    void Move(string horizontalAxis, string verticalAxis)
    {
        float horizontalInput = Input.GetAxis(horizontalAxis);
        float verticalInput = Input.GetAxis(verticalAxis);

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);


        transform.Translate(direction * _speed * Time.deltaTime);

        if (transform.position.y >= 6.2f)
            transform.position = new Vector3(transform.position.x, 6.2f, 0);
        else if (transform.position.y < -4.9f)
            transform.position = new Vector3(transform.position.x, -4.9f, 0);

        if (transform.position.x >= 12.4f)
            transform.position = new Vector3(-12.4f, transform.position.y, 0);
        else if (transform.position.x < -12.4f)
            transform.position = new Vector3(12.4f, transform.position.y, 0);
    }

    void Fire(KeyCode fireButton)
    {
        if (Input.GetKeyDown(fireButton) && Time.time > _fireCoolDown)
        {
            //Debug.Log("Space Key Pressed");

            _fireCoolDown = Time.time + _fireRate;

            if (_isTripleShotEnabled)
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            else
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.007f, 0), Quaternion.identity);

            _audioSource.Play();
        }
    }

    public void Damage()
    {
        if (_isShieldEnabled)
        {
            _isShieldEnabled = false;
            _shield.SetActive(false);
            return;
        }

        _lives--;

        if (_lives == 2)
        {
            _damageVisualizer[0].SetActive(true);
        }
        else if (_lives == 1)
        {
            _damageVisualizer[1].SetActive(true);
        }

        _uiManager.UpdateLives(_lives);

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _uiManager.CheckForBestScore(_score);
            Destroy(this.gameObject);
        }
    }

    public void TripleShotActive()
    {
        _isTripleShotEnabled = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostEnabled = true;
        _speed *= _speedMultiplier;
        StartCoroutine(SpeedPowerDownRoutine());
    }

    public void ShieldActive()
    {
        _isShieldEnabled = true;
        _shield.SetActive(true);
    }

    public void AddToScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerupDownTimer);
        _isTripleShotEnabled = false;
    }

    IEnumerator SpeedPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerupDownTimer);
        _speed /= _speedMultiplier;
        _isSpeedBoostEnabled = false;
    }
}
