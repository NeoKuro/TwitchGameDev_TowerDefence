//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour
{
    protected Action a_forceDetonateIncomingWeapons;

    public virtual void InitialiseAI(object[] data)
    {

    }

    public virtual void AIKilled()
    {

    }

    public virtual void DamageAI(int damageAmount)
    {

    }

    public virtual void RegisterForceDetonateAction(Action a_forceDetonate)
    {
        a_forceDetonateIncomingWeapons += a_forceDetonate;
    }

    public virtual void DeregisterForceDetonateAction(Action a_forceDetonate)
    {
        a_forceDetonateIncomingWeapons -= a_forceDetonate;
    }
}