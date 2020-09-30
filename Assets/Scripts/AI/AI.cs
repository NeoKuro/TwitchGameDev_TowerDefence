//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AI : MonoBehaviour, IComponent
{
    protected Action a_forceDetonateIncomingWeapons;
    protected Action a_UpdateComponents;

    [Header("Basic AI Setup")]
    [SerializeField]
    private bool _validTargetByDefault = true;

    public virtual bool IsValidTarget
    {
        get
        {
            return _validTargetByDefault;
        }
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.hasGameOvered)
        {
            return;
        }

        a_UpdateComponents?.Invoke();
    }

    public virtual void InitialiseAI(object[] data)
    {

    }

    public virtual void AIKilled()
    {
        // IHEALTH
    }

    public virtual void DamageAI(int damageAmount)
    {
        // IHealth
    }

    public virtual void RegisterForceDetonateAction(Action a_forceDetonate)
    {
        a_forceDetonateIncomingWeapons += a_forceDetonate;
    }

    public virtual void DeregisterForceDetonateAction(Action a_forceDetonate)
    {
        a_forceDetonateIncomingWeapons -= a_forceDetonate;
    }

    public void RegisterComponentUpdate(Action a)
    {
        a_UpdateComponents += a;
    }

    public void DeregisterComponentUpdate(Action a)
    {
        a_UpdateComponents -= a;
    }
}