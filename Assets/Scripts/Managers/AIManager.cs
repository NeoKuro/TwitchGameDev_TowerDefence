//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Random = UnityEngine.Random;

public class AIManager : Singleton<AIManager>
{
    [Serializable]
    public struct AIType
    {
        public string Name;
        public GameObject _aiTypeReference;
        public Sprite _aiTypeIcon;
    }

    [SerializeField]
    private List<AIType> _aiTypeReferences = new List<AIType>();

    private List<Enemy_AI> _enemyAI = new List<Enemy_AI>();

    private Coroutine _spawnCoroutine = null;

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
        int randNo = Random.Range(0, _aiTypeReferences.Count);
        return _aiTypeReferences[randNo];
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

}