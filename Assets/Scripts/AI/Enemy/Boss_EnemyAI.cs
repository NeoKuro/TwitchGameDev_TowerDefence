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

    private void OnDisable()
    {
        AIManager.BossDefeated();
    }
}