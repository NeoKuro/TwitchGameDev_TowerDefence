//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AIManager;

public class AISpawnManager : Singleton<AISpawnManager>
{
    public struct WaveSizeContainer
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
    [SerializeField]
    private int _waveCooldown = 15;
    [SerializeField]
    private RangeInt _waveSize = new RangeInt(4, 8);


    [SerializeField]
    public Transform _spawnLocation;

    private bool _shouldSpawn;
    private float _nextSpawnTime = 0;

    protected override void Awake()
    {
        base.Awake(); 
        Initialise();
    }

    private void Update()
    {
        SpawnWave();
    }

    public override void Initialise()
    {
        BeginSpawnWave();
    }

    public override void OnRetryExecuted()
    {
        _waveSize = new RangeInt(4, 8);
        Initialise();
    }

    private void IncreaseWaveDifficulty_Internal(int waveCountIncrease)
    {
        _waveSize += waveCountIncrease;
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

        SpawnMinionWave();
        SpawnBossWave();

        _shouldSpawn = false;
    }

    private void SpawnMinionWave()
    {
        //GameObject objectToSpawn = _availableEnemyPrefabs[UnityEngine.Random.Range(0, _availableEnemyPrefabs.Count)];
        WaveManager.WavePredictionAI waveData = WaveManager.GetCurrentWave();
        GameObject objectToSpawn = waveData._aitype._aiObjectReference;
        bool isEnhanced = WaveManager.GetChanceOfEnhancedAI();

        int toSpawnCount = _waveSize.Random;
        for (int i = 0; i < toSpawnCount; i++)
        {
            Vector3 spawnPos = _spawnLocation.position + (Vector3.up * objectToSpawn.transform.localScale.y * 1.1f) * i;
            GameObject waveObject = Instantiate(objectToSpawn, spawnPos, _spawnLocation.rotation);
            Enemy_AI aiRef = waveObject.GetComponent<Enemy_AI>();
            if (aiRef == null)
            {
                Debug.LogErrorFormat(waveObject, "Could not find reference to 'AI' script on object {0}", waveObject.name);
                continue;
            }

            aiRef.InitialiseAI(new object[] { spawnPos }, waveData._combatType);

            if (isEnhanced)
            {
                Debug.Log("I am enhancing DA AI");
                aiRef.EnhanceAI();
            }
        }
    }

    private void SpawnBossWave()
    {
        WaveManager.WavePredictionAI? bossType = WaveManager.GetCurrentBossWave();
        if (bossType == null)
        {
            return;
        }

        GameObject bossToSpawn = bossType.Value._aitype._aiObjectReference;
        Vector3 spawnPos = _spawnLocation.position + (Vector3.up * bossToSpawn.transform.localScale.y * 1.1f);
        GameObject bossObj = Instantiate(bossToSpawn, spawnPos, _spawnLocation.rotation);
        Enemy_AI aiRef = bossObj.GetComponent<Enemy_AI>();
        if (aiRef == null)
        {
            Debug.LogErrorFormat(bossObj, "Could not find reference to 'AI' script on object {0}", bossObj.name);
        }
        else
        {
            // Passing in NULL for Combat Type because its already assigned within the Boss specifically
            aiRef.InitialiseAI(new object[] { spawnPos }, null);
        }
    }


    public static void IncreaseWaveDifficulty(int waveCountIncrease)
    {
        Instance?.IncreaseWaveDifficulty_Internal(waveCountIncrease);
    }
}