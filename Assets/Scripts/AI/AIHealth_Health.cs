//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth_Health : Health
{
    private AI _aiReference;

    protected void Start()
    {
        _aiReference = GetComponent<AI>();
    }

    protected override void EvaluateHealth()
    {
        //Debug.Log("Current Health Is: " + currentHealth);
        if (currentHealth <= 0 && !_isDead)
        {
            //Debug.Log("KILLED OBJECT!");
            _isDead = true;
            _aiReference.AIKilled();
        }
    }
}