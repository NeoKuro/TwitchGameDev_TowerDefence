//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Offensive_Ability : AbilityBase
{
    [Header("Offensive Ability Settings")]
    [SerializeField]
    private Weapon _abilityWeapon;
    [SerializeField]
    private RangeFloat _attackRange = new RangeFloat(0f, 20f);

    public Weapon AbilityWeapon
    {
        get
        {
            if(_abilityWeapon == null)
            {
                Debug.LogErrorFormat(this, "Offesnive Ability weapon is NULL! Ability Name: {0}", _abilityName);
            }
            return _abilityWeapon;
        }
    }

    public override void ExecuteAbility(Vector3 initialPosition, AI target)
    {
        if(AbilityWeapon == null)
        {
            Debug.LogErrorFormat(this, "Ability Weapon is NULL!");
            return;
        }

        GameObject bullet = ObjectPool.GetObjectOfType(_abilityWeapon.gameObject);
        Weapon w = bullet.GetComponentInChildren<Weapon>();

        // For "Artillery" weapons, could pass in an array of AI (Targets) and the WEAPON SCRIPT determines the median target location rather than it being
        //      pre-calcualted.
        w.Initialise(initialPosition, target, _abilityCombatType);

    }
}