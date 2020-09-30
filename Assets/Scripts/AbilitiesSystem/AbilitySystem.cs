//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Targeting))]
public class AbilitySystem : AIComponent
{
    [Header("Ability System Setup")]
    [SerializeField]
    private AbilityBase _ability;


    private float _nextExecutionTime = 0f;
    private ITarget _targetingBehaviour;
    private ITarget TargetingBehaviour
    {
        get
        {
            if (_targetingBehaviour == null)
            {
                _targetingBehaviour = transform.root.GetComponentInChildren<ITarget>();
                if (_targetingBehaviour == null)
                {
                    Debug.LogErrorFormat(this, "Was unable to find reference to ITarget on object!");
                }
            }

            return _targetingBehaviour;
        }
    }
    private IAbilitiesBehaviour _abilitiesBehaviour;
    private IAbilitiesBehaviour AbilitiesBehaviour
    {
        get
        {
            if (_abilitiesBehaviour == null)
            {
                _abilitiesBehaviour = transform.root.GetComponentInChildren<IAbilitiesBehaviour>();
                if (_abilitiesBehaviour == null)
                {
                    Debug.LogErrorFormat(this, "Was unable to find reference to IAbilitiesBehaviour on object!");
                }
            }

            return _abilitiesBehaviour;
        }
    }


    protected override void Awake()
    {
        base.Awake();
        _nextExecutionTime = _ability.AbilityCooldown + Time.time;
    }

    private void OnEnable()
    {
        AbilitiesBehaviour.RegisterComponentUpdate(ExecuteAbility);
    }

    private void OnDisable()
    {
        AbilitiesBehaviour.DeregisterComponentUpdate(ExecuteAbility);
    }



    private void ExecuteAbility()
    {
        if (Time.time < _nextExecutionTime)
        {
            return;
        }

        if(TargetingBehaviour.TargetingComponent.CurrentTarget == null)
        {
            return;
        }

        _nextExecutionTime = _ability.AbilityCooldown + Time.time;
        _ability.ExecuteAbility(transform.position, TargetingBehaviour.TargetingComponent.CurrentTarget);

        //_currentTarget.GetComponent<AIHealth_Health>().ChangeHealth((int)-damageAmount);

    }

}