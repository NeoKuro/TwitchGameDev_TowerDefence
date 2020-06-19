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
    public float shootCooldown = 1;
    private Enemy_AI _currentTarget;


    private float _nextShootTime = 0f;

   private void Start()
   {
        AcquireTarget();
   }

   private void Update()
   {
       // Stuff in here ran each frame
       if(_currentTarget == null)
        {
            AcquireTarget();
        }
        else
        {
            ShootTarget();
        }
   }

    private void AcquireTarget()
    {
        List<Enemy_AI> enemies = AIManager.Instance.GetEnemyAI();
        if(enemies.Count == 0)
        {
            return;
        }

        int randNo = Random.Range(0, enemies.Count);
        _currentTarget = enemies[randNo];
    }

    private void ShootTarget()
    {
        if(Time.time < _nextShootTime)
        {
            return;
        }

        _nextShootTime = Time.time + shootCooldown;
        _currentTarget.GetComponent<AIHealth_Health>().ChangeHealth((int)-damageAmount);

    }
}