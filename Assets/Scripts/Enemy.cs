using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _points = 10;

    private Player _player;
    private Animator _animator;

    [SerializeField]
    private AudioSource _audioSource;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _fireRate = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();

        if (_player == null)
            Debug.LogError("Player is null");

        _animator = GetComponent<Animator>();

        if (_animator == null)
            Debug.LogError("Animator is null");
        if (_audioSource == null)
            Debug.LogError("Audio source is null");
        StartCoroutine(enemyFireRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -6f)
        {
            transform.position = new Vector3(Random.Range(-8f, 8f), 7, 0);
        }
    }

    IEnumerator enemyFireRoutine()
    {
        GameObject enemyLaser = Instantiate(_laserPrefab, transform.position + new Vector3(0, -2, 0), Quaternion.identity);
        Laser laser = enemyLaser.GetComponent<Laser>();
        laser.AssignLaserToEnemy();
        yield return new WaitForSeconds(_fireRate);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject , 2.8f);

            Player player = other.transform.GetComponent<Player>();

            if (player != null)
                player.Damage();
        }

        if (other.tag == "Laser")
        {
            if (_player != null)
                _player.AddToScore(_points);

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;

            _audioSource.Play();

            Destroy(GetComponent<Collider2D>());
            Destroy(other.gameObject);
            Destroy(this.gameObject, 2.8f);
        }

    }
}
