//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CombatTypeBase : ScriptableObject
{
    [SerializeField]
    private string _combatTypeName;
    [SerializeField]
    private Color _representativeTypeColour;

    [Header("Affinities")]
    [SerializeField]
    private List<CombatTypeBase> _weaknessAffinity;
    [SerializeField]
    private List<CombatTypeBase> _strengthAffinity;


            


    public string CombatTypeName
    {
        get { return _combatTypeName; }
    }

    public Color RepresentativeTypeColour
    {
        get { return _representativeTypeColour; }
    }

    public List<CombatTypeBase> WeaknessAffinities
    {
        get { return _weaknessAffinity; }
    }

    public List<CombatTypeBase> StrengthAffinities
    {
        get { return _strengthAffinity; }
    }

    public int GetModifiedDamage(int baseDamage, CombatTypeBase targetCombatType)
    {
        if (targetCombatType == null)
        {
            Debug.LogErrorFormat("Target Combat Type is null! Has the AI been correctly assigned a combat type on spawn?");
            return baseDamage;
        }

        float damageModifier = 1f;
        damageModifier = _strengthAffinity.Where(x => { return x.name == targetCombatType.name; }).FirstOrDefault() != null ? 1.25f : 
                            _weaknessAffinity.Where(x => { return x.name == targetCombatType.name; }).FirstOrDefault() != null ? 0.75f : 1f;

        return Mathf.FloorToInt(baseDamage * damageModifier);
    }
}