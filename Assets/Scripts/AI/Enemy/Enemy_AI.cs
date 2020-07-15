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
    [SerializeField]
    private int _damageToPlayerHealth = 5;

    [SerializeField]
    private int _moneyToRewardOnDeath = 8;


    public Vector3 endPosition;
    public float aiSpeed = 1;
    public float snapDistance = 0.1f;


    protected Vector3 _spawnPoint;
    private Vector3 startPosition;
    private float positionLerp = 0;

    private bool moveForward = false;
    private AIHealth_Health _healthComponent;
    private AIPathingWaypoint currentWaypoint;

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
    public override void InitialiseAI(object[] data)
    {
        _spawnPoint = (Vector3)data[0];
    }


    public override void AIKilled()
    {
        AIManager.RemoveEnemyAI(this);
        PlayerManager.IncrementCurrency(_moneyToRewardOnDeath);
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
        if (_healthComponent == null)
        {
            Debug.LogError("No Health Component Found on Object", this);
            return;
        }

        _healthComponent.ChangeHealth(damageAmount);
    }

    private void MoveAI()
    {
        if (!moveForward)
        {
            currentWaypoint = AIPathingManager.GetNextWaypoint(currentWaypoint);

            if(currentWaypoint == null)
            {
                PlayerManager.DecreaseHealth(_damageToPlayerHealth);
                transform.position = _spawnPoint;
                currentWaypoint = AIPathingManager.GetNextWaypoint(currentWaypoint);
            }

            Vector3 vectorToTarget = currentWaypoint.transform.position - transform.position;
            float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            transform.rotation = q;

            moveForward = true;
            return;
        }

        // TODO: Stop running  these calculations when we've reached our destination!
        //          (Or, teleport to the beginning)
        transform.position += transform.right * Time.deltaTime * aiSpeed;
        float sqDistanceToWaypoint = UsefulMethods.CheapDistanceSquared(transform.position, currentWaypoint.transform.position);
        if (sqDistanceToWaypoint <= (snapDistance * snapDistance))
        {
            moveForward = false;
            transform.position = currentWaypoint.transform.position;
        }
    }
}