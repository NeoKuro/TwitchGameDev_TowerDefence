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
    public float attackRadius = 3f;
    public float shootCooldown = 1;

    [SerializeField]
    private GameObject _weaponReference;

    private Enemy_AI _currentTarget;


    private float _nextShootTime = 0f;
    private float _attackRadiusSq = 9f;

    private void Start()
    {
        _attackRadiusSq = attackRadius * attackRadius;
    }

    private void Update()
    {
        if (GameManager.Instance.hasGameOvered)
        {
            return;
        }


        Targetting();
    }


    private void Targetting()
    {
        if (Time.time < _nextShootTime)
        {
            return;
        }

        // Stuff in here ran each frame
        if (!TargetIsValid(_currentTarget))
        {
            _currentTarget = null;
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
            //float distSq = UsefulMethods.CheapDistanceSquared(transform.position, enemies[i].transform.position);
            if(TargetIsValid(enemies[i]))
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

    private bool TargetIsValid(Enemy_AI enemy)
    {
        if(enemy == null)
        {
            return false;
        }

        if (enemy.IsDead)
        {
            return false;
        }

        float distSq = UsefulMethods.CheapDistanceSquared(transform.position, enemy.transform.position);
        if (distSq >= _attackRadiusSq)
        {
            return false;
        }

        return true;
    }

    private void ShootTarget()
    {
        _nextShootTime = Time.time + shootCooldown;
        GameObject bullet = ObjectPool.GetObjectOfType(_weaponReference);
        Weapon w = bullet.GetComponentInChildren<Weapon>();

        if(w == null)
        {
            w = bullet.AddComponent<Weapon>();
        }

        w.Initialise(transform.position, _currentTarget);


        //_currentTarget.GetComponent<AIHealth_Health>().ChangeHealth((int)-damageAmount);

    }
}