﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private float _spawnRate = 5f;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        Player player = gameObject.GetComponent<Player>();

        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            newEnemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnRate);
        }
    }

    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            Instantiate(powerups[Random.Range(0, 3)], posToSpawn, Quaternion.identity);

            yield return new WaitForSeconds(Random.Range(3f, 8f));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
