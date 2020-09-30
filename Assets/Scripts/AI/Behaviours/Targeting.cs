//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeting : AIComponent
{
    [Header("Targeting Setup")]
    [SerializeField]
    private float _targetingPeriod = 3f;
    [SerializeField]
    private bool _acquireNewTargetOnInvalid = true;
    [SerializeField]
    private float _targetingRadius = 10f;

    private float _targetingRadiusSq = Mathf.Infinity;
    private float _nextTargetingTime = 0f;
    private AI _currentTarget;

    private ITarget _targetComponent;
    public ITarget TargetComponent
    {
        get
        {
            if(_targetComponent == null)
            {
                _targetComponent= transform.root.GetComponentInChildren<ITarget>();
                if(_targetComponent == null)
                {
                    Debug.LogErrorFormat(this, "Was unable to find a ITarget component somewhere on the object!");
                }
            }
            return _targetComponent;
        }
    }

    public AI CurrentTarget
    {
        get
        {
            if(_acquireNewTargetOnInvalid)
            {
                if(!TargetIsValid(_currentTarget))
                {
                    AcquireTarget();
                }
            }

            return _currentTarget;
        }
    }
    public float TargetingRadiusSq
    {
        get
        {
            if(_targetingRadiusSq == Mathf.Infinity)
            {
                _targetingRadiusSq = _targetingRadius * _targetingRadius;
            }
            return _targetingRadiusSq;
        }
    }

    private void OnEnable()
    {
        TargetComponent.RegisterComponentUpdate(Targetting);
    }

    private void OnDisable()
    {
        TargetComponent.DeregisterComponentUpdate(Targetting);
    }


    private void Targetting()
    {
        if (Time.time < _nextTargetingTime)
        {
            return;
        }

        if (!TargetIsValid(CurrentTarget))
        {
            _currentTarget = null;
            if (AcquireTarget())
            {

            }
        }
    }

    private bool AcquireTarget()
    {
        List<Enemy_AI> enemies = AIManager.Instance.GetEnemyAI();
        if (enemies.Count == 0)
        {
            return false;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (TargetIsValid(enemies[i]))
            {
                _currentTarget = enemies[i];
                break;
            }
        }

        return _currentTarget != null;
    }

    private bool TargetIsValid(AI enemy)
    {
        if (enemy == null)
        {
            return false;
        }

        if (!enemy.IsValidTarget)
        {
            return false;
        }

        float distSq = UsefulMethods.CheapDistanceSquared(transform.position, enemy.transform.position);
        if (distSq >= TargetingRadiusSq)
        {
            return false;
        }

        return true;
    }

}