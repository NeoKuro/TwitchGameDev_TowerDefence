//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawnManager : Singleton<AISpawnManager>
{
    private struct WaveSizeContainer
    {
        public int minWaveSize;
        public int maxWaveSize;

        public WaveSizeContainer(int min, int max)
        {
            minWaveSize = min;
            maxWaveSize = max;
        }

        public int Random
        {
            get
            {
                return UnityEngine.Random.Range(minWaveSize, maxWaveSize);
            }
        }
    }

    private const string _enemyPrefabFilePath = "\\EnemyPrefabs\\";

    private int _waveCooldown = 15;
    private WaveSizeContainer _waveSize = new WaveSizeContainer(3, 10);

    [SerializeField]
    public Transform _spawnLocation;
    [SerializeField]
    private List<GameObject> _availableEnemyPrefabs;

    private bool _shouldSpawn;
    private float _nextSpawnTime = 0;

    protected override void Awake()
    {
        base.Awake();
        if (_availableEnemyPrefabs == null || _availableEnemyPrefabs.Count == 0)
        {
            Debug.LogError("We don't have any prefabs assinged!!", this);
            enabled = false;
        }
        BeginSpawnWave();
    }

    private void Update()
    {
        SpawnWave();
    }

    public void BeginSpawnWave()
    {
        _shouldSpawn = true;
        _nextSpawnTime = Time.time + _waveCooldown;
    }

    private void SpawnWave()
    {
        if (!_shouldSpawn)
        {
            return;
        }

        if (Time.time < _nextSpawnTime)
        {
            return;
        }

        GameObject objectToSpawn = _availableEnemyPrefabs[UnityEngine.Random.Range(0, _availableEnemyPrefabs.Count)];

        int toSpawnCount = _waveSize.Random;
        for (int i = 0; i < toSpawnCount; i++)
        {
            Vector3 spawnPos = _spawnLocation.position + (Vector3.up * objectToSpawn.transform.localScale.y * 1.1f) * i;
            Instantiate(objectToSpawn, spawnPos, _spawnLocation.rotation);
        }
        _shouldSpawn = false;
    }
}