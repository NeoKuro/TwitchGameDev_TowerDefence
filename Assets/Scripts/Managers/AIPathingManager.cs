//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathingManager : Singleton<AIPathingManager>
{
    public List<AIPathingWaypoint> _allPathingWaypoints = new List<AIPathingWaypoint>();

    private AIPathingWaypoint GetNextWaypoint_Internal(int currentIndex)
    {

        if (currentIndex < _allPathingWaypoints.Count - 1)
        {
            return _allPathingWaypoints[currentIndex + 1];
        }

        return null;
    }

    private AIPathingWaypoint GetNextWaypoint_Internal(AIPathingWaypoint currentWaypoint)
    {
        int currentIndex = -1;

        if (currentWaypoint != null && !currentWaypoint.IsFinalDestination)
        {
            if (_allPathingWaypoints.Contains(currentWaypoint))
            {
                currentIndex = _allPathingWaypoints.IndexOf(currentWaypoint);
            }
        }

        return GetNextWaypoint_Internal(currentIndex);
    }

    public static AIPathingWaypoint GetNextWaypoint(int currentIndex)
    {
        if (Instance == null)
        {
            Debug.LogErrorFormat("AI Pathing Manager is null! Its not supposed to be!");
            return null;
        }

        return Instance.GetNextWaypoint_Internal(currentIndex);
    }

    public static AIPathingWaypoint GetNextWaypoint(AIPathingWaypoint currentWaypoint)
    {
        if (Instance == null)
        {
            Debug.LogErrorFormat("AI Pathing Manager is null! Its not supposed to be!");
            return null;
        }

        return Instance.GetNextWaypoint_Internal(currentWaypoint);
    }
}