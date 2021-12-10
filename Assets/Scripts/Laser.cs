using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _speed = 10f;
    private bool _isEnemy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isEnemy == false)
            MoveUp();
        else
            MoveDown();
    }

    void MoveUp()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);

        if (transform.position.y >= 8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (transform.position.y < -8f)
        {
            if (transform.parent != null)
                Destroy(transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }

    public void AssignLaserToEnemy()
    {
        _isEnemy = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && _isEnemy == true)
        {
            Player player = collision.GetComponent<Player>();

            if (player != null)
                player.Damage();
        }
    }

    public bool EnemyLaser()
    {
        return _isEnemy;
    }
}
