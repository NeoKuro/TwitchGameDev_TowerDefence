//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathingWaypoint : MonoBehaviour
{
    [SerializeField]
    private bool _isFinalDestination = false;

    public bool IsFinalDestination
    {
        get
        {
            return _isFinalDestination;
        }
    }
}