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
        public GameObject _aiObjectReference;
        public AI _aiTypeReference;
        public Sprite _aiTypeIcon;
        public bool _unlocked;

        public void SetUnlocked()
        {
            _unlocked = true;
        }
    }

    [SerializeField]
    private List<AIType> _aiTypeReferences = new List<AIType>();
    [SerializeField]
    private List<AIType> _bossTypeReferences = new List<AIType>();
    [SerializeField]
    private List<CombatTypeBase> _availableCombatTypes = new List<CombatTypeBase>();

    private int _bossesDefeated = 0;

    private List<Enemy_AI> _enemyAI = new List<Enemy_AI>();
    private List<AIType> _unlockedAI = new List<AIType>();
    private List<AIType> _unlockedBosses = new List<AIType>();

    private Coroutine _spawnCoroutine = null;

    public List<CombatTypeBase> AvailableCombatTypes => _availableCombatTypes;


    protected override void Awake()
    {
        base.Awake();
        Initialise();
    }

    public override void Initialise()
    {
        _unlockedAI.AddRange(_aiTypeReferences.Where(x => { return x._unlocked; }));
        _unlockedBosses.AddRange(_bossTypeReferences.Where(x => { return x._unlocked; }));
    }

    public override void OnRetryExecuted()
    {
        _unlockedAI.Clear();
        _unlockedBosses.Clear();
        _enemyAI.Clear();
        Initialise();
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
55        EvaluateEnemies();
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
        //UnlockNewAIType();
        //UnlockNewBossType();
    }

    private void BossDefeatPredicted_Internal(int bossDefeatPredictionCount)
    {
        UnlockNewAIType(bossDefeatPredictionCount);
        UnlockNewBossType(bossDefeatPredictionCount);
    }

    private void UnlockNewAIType(int bossDefeatPredictionCount)
    {
        if (_unlockedAI.Count >= _aiTypeReferences.Count)
        {
            return;
        }

        List<AIType> availableAITypes = _aiTypeReferences.Where(x => { return x._minBossesBeforeUnlock <= bossDefeatPredictionCount && !x._unlocked; }).ToList();
        for (int i = 0; i < _aiTypeReferences.Count; i++)
        {
            for (int j = 0; j < availableAITypes.Count; j++)
            {
                if (_aiTypeReferences[i].Name != availableAITypes[j].Name)
                {
                    continue;
                }
                AIType tmp = _aiTypeReferences[i];
                tmp._unlocked = true;
                _aiTypeReferences[i] = tmp;
                _unlockedAI.Add(tmp);
                break;
            }
        }
    }

    private void UnlockNewBossType(int bossDefeatPredictionCount)
    {
        if (_unlockedBosses.Count >= _bossTypeReferences.Count)
        {
            return;
        }

        List<AIType> availableAITypes = _bossTypeReferences.Where(x => { return x._minBossesBeforeUnlock <= bossDefeatPredictionCount && !x._unlocked; }).ToList();
        for (int i = 0; i < _bossTypeReferences.Count; i++)
        {
            for (int j = 0; j < availableAITypes.Count; j++)
            {
                if(_bossTypeReferences[i].Name != availableAITypes[j].Name)
                {
                    continue;
                }
                AIType tmp = _bossTypeReferences[i];
                tmp._unlocked = true;
                _bossTypeReferences[i] = tmp;
                _unlockedBosses.Add(tmp);
                break;
            }
        }
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

    public static void BossDefeatPredicted(int bossDefeatPredictionCount)
    {
        Instance.BossDefeatPredicted_Internal(bossDefeatPredictionCount);
    }

}