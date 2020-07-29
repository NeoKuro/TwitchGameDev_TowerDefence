//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

public class AIManager : Singleton<AIManager>
{
    [Serializable]
    public struct AIType
    {
        public string Name;
        public int _minBossesBeforeUnlock;
        public GameObject _aiTypeReference;
        public Sprite _aiTypeIcon;
        public bool _unlocked;
    }

    [SerializeField]
    private List<AIType> _aiTypeReferences = new List<AIType>();
    [SerializeField]
    private List<AIType> _bossTypeReferences = new List<AIType>();

    private int _bossesDefeated = 0;

    private List<Enemy_AI> _enemyAI = new List<Enemy_AI>();
    private List<AIType> _unlockedAI = new List<AIType>();
    private List<AIType> _unlockedBosses = new List<AIType>();

    private Coroutine _spawnCoroutine = null;


    protected override void Awake()
    {
        base.Awake();
        _unlockedAI.AddRange(_aiTypeReferences.Where(x => { return x._unlocked; }));
        _unlockedBosses.AddRange(_bossTypeReferences.Where(x => { return x._unlocked; }));
    }


    public List<Enemy_AI> GetEnemyAI()
    {
        return new List<Enemy_AI>(_enemyAI);
    }

    private void AddNewEnemyAIInternal(Enemy_AI newAI)
    {
        if (_enemyAI.Contains(newAI))
        {
            return;
        }

        _enemyAI.Add(newAI);
    }

    private void RemoveEnemyAIInternal(Enemy_AI toRemove)
    {
        if (!_enemyAI.Contains(toRemove))
        {
            return;
        }

        _enemyAI.Remove(toRemove);
        EvaluateEnemies();
    }

    private AIType GetRandomAITypeInternal()
    {
        int randNo = Random.Range(0, _unlockedAI.Count);
        return _unlockedAI[randNo];
    }

    private AIType GetRandomBossTypeInternal()
    {
        int randNo = Random.Range(0, _unlockedBosses.Count);
        return _unlockedBosses[randNo];
    }

    private void BossDefeated_Internal()
    {
        _bossesDefeated++;
        UnlockNewAIType();
        UnlockNewBossType();
    }

    private void UnlockNewAIType()
    {
        if (_unlockedAI.Count >= _aiTypeReferences.Count)
        {
            return;
        }

        List<AIType> availableAITypes = _aiTypeReferences.Where(x => { return x._minBossesBeforeUnlock <= _bossesDefeated && !x._unlocked; }).ToList();
        _unlockedAI.Add(availableAITypes[Random.Range(0, availableAITypes.Count)]);
    }

    private void UnlockNewBossType()
    {
        if (_unlockedBosses.Count >= _bossTypeReferences.Count)
        {
            return;
        }

        List<AIType> availableAITypes = _bossTypeReferences.Where(x => { return x._minBossesBeforeUnlock <= _bossesDefeated && !x._unlocked; }).ToList();
        _unlockedAI.Add(availableAITypes[Random.Range(0, availableAITypes.Count)]);
    }

    private void EvaluateEnemies()
    {
        if(_enemyAI.Count != 0 || GameManager.Instance.hasGameOvered)
        {
            return;
        }

        WaveManager.IncrementWaveCount();
        AISpawnManager.Instance.BeginSpawnWave();
    }



    public static void AddNewEnemyAI(Enemy_AI newAI)
    {
        Instance?.AddNewEnemyAIInternal(newAI);
    }

    public static void RemoveEnemyAI(Enemy_AI newAI)
    {
        Instance?.RemoveEnemyAIInternal(newAI);
    }

    public static AIType GetRandomAIType()
    {
        return Instance.GetRandomAITypeInternal();
    }

    public static AIType GetRandomBossType()
    {
        return Instance.GetRandomBossTypeInternal();
    }

    public static void BossDefeated()
    {
        Instance.BossDefeated_Internal();
    }

}