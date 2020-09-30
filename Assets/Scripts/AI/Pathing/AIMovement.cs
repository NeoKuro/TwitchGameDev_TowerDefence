//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : AIComponent
{
    public float aiSpeed = 1;
    public float snapDistance = 0.1f;

    private float _previousDistance = Mathf.Infinity;
    private bool hasArrivedAtWaypoint = false;
    private AIPathingWaypoint currentWaypoint;

    private IMovement _aiMovement;
    private IMovement aiMovement
    {
        get
        {
            if(_aiMovement == null)
            {
                _aiMovement = _aiRef.GetComponentInChildren<IMovement>();
            }

            return _aiMovement;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        SetNewWaypoint();
    }

    private void OnEnable()
    {
        aiMovement.RegisterComponentUpdate(MoveAI);
    }

    private void OnDisable()
    {
        aiMovement.DeregisterComponentUpdate(MoveAI);
    }

    public override void SetEnhancedAI(float _enhancedMultiplier)
    {
        base.SetEnhancedAI(_enhancedMultiplier);
        aiSpeed *= _enhancedMultiplier;
        snapDistance *= _enhancedMultiplier;
    }


    private void MoveAI()
    {
        if (!aiMovement.CanMove())
        {
            return;
        }

        if (hasArrivedAtWaypoint)
        {
            if(currentWaypoint.IsFinalDestination)
            {
                ArrivedAtFinalDestination();
            }

            SetNewWaypoint();
            return;
        }
        UpdatePosition();
    }

    private void ArrivedAtFinalDestination()
    {
        aiMovement.ArrivedAtFinalDestination();
        transform.position = AISpawnManager.Instance._spawnLocation.position;
    }

    private void SetNewWaypoint()
    {
        currentWaypoint = AIPathingManager.GetNextWaypoint(currentWaypoint);
        Vector3 vectorToTarget = currentWaypoint.transform.position - transform.position;
        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = q;
        _previousDistance = Mathf.Infinity;
        hasArrivedAtWaypoint = false;
    }

    private void UpdatePosition()
    {
        transform.position += transform.right * Time.deltaTime * aiSpeed;
        float sqDistanceToWaypoint = UsefulMethods.CheapDistanceSquared(transform.position, currentWaypoint.transform.position);
        if (sqDistanceToWaypoint <= (snapDistance * snapDistance) || sqDistanceToWaypoint > _previousDistance)
        {
            hasArrivedAtWaypoint = true;
            transform.position = currentWaypoint.transform.position;
        }
        _previousDistance = sqDistanceToWaypoint;
    }
}