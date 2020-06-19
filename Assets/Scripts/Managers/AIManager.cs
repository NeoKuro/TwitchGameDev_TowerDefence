//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : Singleton<AIManager>
{
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

    private void EvaluateEnemies()
    {
        if(_enemyAI.Count != 0)
        {
            return;
        }

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


}