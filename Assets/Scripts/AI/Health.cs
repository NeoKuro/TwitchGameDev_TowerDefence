//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField]
    protected int currentHealth = 100;

    public void ChangeHealth(int deltaHealthInput)
    {
        currentHealth = currentHealth + deltaHealthInput;
        EvaluateHealth();
    }

    protected virtual void EvaluateHealth()
    {
        Debug.Log("Current Health Is: " + currentHealth);
        if (currentHealth <= 0)
        {
            Debug.Log("KILLED OBJECT!");
            Destroy(transform.root.gameObject);
        }
    }
}