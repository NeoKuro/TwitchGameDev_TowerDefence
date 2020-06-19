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

public class Enemy_AI : AI
{

    public Vector3 endPosition;
    public float aiSpeed = 1;
    public float snapDistance = 0.1f;

    private Vector3 startPosition;
    private float positionLerp = 0;

    private bool moveForward = true;
    private AIHealth_Health _healthComponent;

    private void Awake()
    {
        AIManager.AddNewEnemyAI(this);
        _healthComponent = transform.root.GetComponentInChildren<AIHealth_Health>();
    }

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        MoveAI();
    }

    public override void AIKilled()
    {
        AIManager.RemoveEnemyAI(this);
        Destroy(transform.root.gameObject);
    }

    public void ChangeDestination(Vector3 newDestination)
    {
        endPosition = newDestination;
        startPosition = transform.position;
        positionLerp = 0;
    }

    public void DamageAI(int damageAmount)
    {
        if(_healthComponent == null)
        {
            Debug.LogError("No Health Component Found on Object", this);
            return;
        }

        _healthComponent.ChangeHealth(damageAmount);
    }

    private void MoveAI()
    {
        if (!moveForward)
            return;
        // TODO: Stop running  these calculations when we've reached our destination!
        //          (Or, teleport to the beginning)
        transform.position += Vector3.down * Time.deltaTime * aiSpeed;
        float sqDistanceToWaypoint = UsefulMethods.CheapDistanceSquared(transform.position, endPosition);
        if (sqDistanceToWaypoint <= (snapDistance * snapDistance))
        {
            moveForward = false;
            transform.position = endPosition;
        }

        //positionLerp = Mathf.Clamp01(positionLerp + (Time.deltaTime * aiSpeed));
        //Vector3 newPos = Vector3.Lerp(startPosition, endPosition, positionLerp);
        //transform.position = newPos;
    }
}