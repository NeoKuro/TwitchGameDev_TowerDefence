//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_EnemyAI : Enemy_AI
{
    [SerializeField]
    private CombatTypeBase _forcedBossCombatType;

    public override CombatTypeBase AICombatType => _forcedBossCombatType;

    public override void InitialiseAI(object[] data, CombatTypeBase assignedCombatType)
    {
        base.InitialiseAI(data, _forcedBossCombatType);
        _aiCombatType = _forcedBossCombatType;
    }

    private void OnDisable()
    {
        AIManager.BossDefeated();
    }
}