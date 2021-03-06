//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy_AI : AI, IMovement
{
    [Header("Ai Core Settings")]
    [SerializeField, Range(1f, 10f)]
    private float _enhancedMultiplier = 1.25f;

    [Header("AI Death Settings")]
    [SerializeField]
    private int _damageToPlayerHealth = 5;
    [SerializeField]
    private int _moneyToRewardOnDeath = 8;
    [SerializeField]
    private Color _deathColour = Color.red;
    [SerializeField]
    private Recolouriser _aiRecolouriser;



    private AIHealth_Health _healthComponent;
    private AIMovement _aiMovement;


    public override bool IsValidTarget
    {
        get
        {
            if(!base.IsValidTarget)
            {
                return false;
            }

            return _healthComponent != null ? !_healthComponent._isDead : true;
        }
    }


    private void Awake()
    {
        AIManager.AddNewEnemyAI(this);
        _healthComponent = transform.root.GetComponentInChildren<AIHealth_Health>();

        if (_aiRecolouriser == null)
        {
            _aiRecolouriser = transform.root.GetComponentInChildren<Recolouriser>();
        }

        if (_aiRecolouriser == null)
        {
            _aiRecolouriser = transform.root.gameObject.AddComponent<Recolouriser>();
        }

        _aiMovement = transform.root.GetComponentInChildren<AIMovement>();
    }

    public bool CanMove()
    {
        return !GameManager.Instance.hasGameOvered && !_healthComponent._isDead;
    }

    public void ArrivedAtFinalDestination()
    {
        a_forceDetonateIncomingWeapons?.Invoke();
        PlayerManager.DecreaseHealth(_damageToPlayerHealth);
    }


    public override void AIKilled()
    {
        AIManager.RemoveEnemyAI(this);
        PlayerManager.IncrementCurrency(_moneyToRewardOnDeath);
        _aiRecolouriser.BeginRecolourise(_deathColour, true);
    }

    public override void DamageAI(int damageAmount, CombatTypeBase weaponType)
    {
        if (_healthComponent == null)
        {
            Debug.LogError("No Health Component Found on Object", this);
            return;
        }

        damageAmount = weaponType.GetModifiedDamage(damageAmount, _aiCombatType);
        _healthComponent.ChangeHealth(damageAmount);
    }

    public void EnhanceAI()
    {
        _damageToPlayerHealth *= Mathf.RoundToInt(_damageToPlayerHealth * _enhancedMultiplier);
        _moneyToRewardOnDeath = Mathf.RoundToInt(_moneyToRewardOnDeath * _enhancedMultiplier);
        _aiMovement.SetEnhancedAI(_enhancedMultiplier);
    }
}