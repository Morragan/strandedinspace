using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState { Spawning, Waiting, Counting }

    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Transform[] spawnPoints;
    public Wave[] waves;
    public float timeBetweenWaves = 5f;
    public int NextWave { get; private set; } = 0;
    public float WaveCountdown { get; private set; } = 0f;
    public SpawnState State { get; private set; } = SpawnState.Counting;

    private float searchCountdown = 2f;

    private void Start()
    {
        WaveCountdown = timeBetweenWaves;

        if (spawnPoints.Length < 1)
        {
            Debug.LogError("Enemies have nowhere to spawn");
        }
    }

    private void Update()
    {
        if(State == SpawnState.Waiting)
        {
            // are there any enemies left?
            if (!isEnemyAlive())
            {
                // End current round and begin a new one
                OnWaveCompleted();
            }
            else
            {
                return;
            }
        }

        if(WaveCountdown <= 0)
        {
            if(State != SpawnState.Spawning)
            {
                StartCoroutine(SpawnWave(waves[NextWave]));
            }
        }
        else
        {
            WaveCountdown -= Time.deltaTime;
        }
    }

    bool isEnemyAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) return false;
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        State = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        State = SpawnState.Waiting;
        yield break;
    }

    void OnWaveCompleted()
    {
        State = SpawnState.Counting;
        WaveCountdown = timeBetweenWaves;

        if (NextWave + 1 >= waves.Length)
        {
            // all waves completed
            NextWave = 0;
        }
        else
        {
            NextWave++;
        }
    }

    void SpawnEnemy(Transform _enemy)
    {
        // Spawn enemy
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }
}

