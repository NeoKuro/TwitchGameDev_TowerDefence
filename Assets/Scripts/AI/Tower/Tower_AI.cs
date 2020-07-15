//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class Tower_AI : AI
{
    public uint damageAmount = 10;
    public float attackRadius = 3f;
    public float shootCooldown = 1;
    private Enemy_AI _currentTarget;


    private float _nextShootTime = 0f;
    private float _attackRadiusSq = 9f;

    private void Start()
    {
        _attackRadiusSq = attackRadius * attackRadius;
    }

    private void Update()
    {
        Targetting();
    }


    private void Targetting()
    {
        if (Time.time < _nextShootTime)
        {
            return;
        }

        // Stuff in here ran each frame
        if (_currentTarget == null)
        {
            if (AcquireTarget())
            {
                ShootTarget();
            }
        }
        else
        {
            ShootTarget();
        }
    }

    private bool AcquireTarget()
    {
        List<Enemy_AI> enemies = AIManager.Instance.GetEnemyAI();
        if (enemies.Count == 0)
        {
            return false;
        }


        for (int i = 0; i < enemies.Count; i++)
        {
            float distSq = UsefulMethods.CheapDistanceSquared(transform.position, enemies[i].transform.position);
            if(distSq < _attackRadiusSq)
            {
                _currentTarget = enemies[i];
                break;
            }
        }

        if(_currentTarget == null)
        {
            return false;
        }

        //int randNo = Random.Range(0, enemies.Count);
        //_currentTarget = enemies[randNo];
        return true;
    }

    private void ShootTarget()
    {
        Debug.LogFormat("{0} has shooted target!", gameObject.name);
        _nextShootTime = Time.time + shootCooldown;
        _currentTarget.GetComponent<AIHealth_Health>().ChangeHealth((int)-damageAmount);

    }
}