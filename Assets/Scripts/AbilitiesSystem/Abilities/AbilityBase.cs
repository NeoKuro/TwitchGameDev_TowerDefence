//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : ScriptableObject
{
    [Header("Basic Ability Settings")]
    [SerializeField]
    protected string _abilityName;
    [SerializeField]
    protected float _abilityCooldown = 3f;

    public string AbilityName => _abilityName;
    public float AbilityCooldown => _abilityCooldown;

    public abstract void ExecuteAbility(Vector3 initialPosition, AI target);
}