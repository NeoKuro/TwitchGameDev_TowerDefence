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

public class Tower_AI : AI, ITarget, IAbilitiesBehaviour
{
    private Targeting _targetingComponent;
    public Targeting TargetingComponent
    {
        get
        {
            if(_targetingComponent == null)
            {
                _targetingComponent = transform.root.GetComponentInChildren<Targeting>();
                if(_targetingComponent == null)
                {
                    Debug.LogErrorFormat(this, "This Tower AI does not have a targeting script on it anywhere! I added one automatically with default settings!");
                    _targetingComponent = gameObject.AddComponent<Targeting>();
                }
            }

            return _targetingComponent;
        }
    }
    private AbilitySystem _abilitySystemComponent;
    public AbilitySystem AbilitySystemComponent
    {
        get
        {
            if (_abilitySystemComponent == null)
            {
                _abilitySystemComponent = transform.root.GetComponentInChildren<AbilitySystem>();
                if (_abilitySystemComponent == null)
                {
                    Debug.LogErrorFormat(this, "This Tower AI does not have an AbilitySystem script on it anywhere! I added one automatically with default settings!");
                    _abilitySystemComponent = gameObject.AddComponent<AbilitySystem>();
                }
            }

            return _abilitySystemComponent;
        }
    }


}