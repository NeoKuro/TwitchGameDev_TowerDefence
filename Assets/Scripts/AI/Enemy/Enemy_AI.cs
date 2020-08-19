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

    [SerializeField, Range(1f, 10f)]
    private float _enhancedMultiplier = 1.25f;

    [SerializeField]
    private Color _materialColour = Color.white;
    [SerializeField]
    private Color _deathMaterialColour = Color.red;
    [SerializeField]
    private float _timeToDeath = 1f;


    public Vector3 endPosition;
    public float aiSpeed = 1;
    public float snapDistance = 0.1f;



    protected Vector3 _spawnPoint;
    private Vector3 startPosition;
    private float positionLerp = 0;

    private float _previousDistance = 9999f;
    private bool moveForward = false;
    private AIHealth_Health _healthComponent;
    private AIPathingWaypoint currentWaypoint;

    private Renderer _renderer;
    private Material _mainMaterial;

    public bool IsDead
    {
        get
        {
            return _healthComponent != null ? _healthComponent._isDead : true;
        }
    }


    private void Awake()
    {
        _previousDistance = Mathf.Infinity;
        AIManager.AddNewEnemyAI(this);
        _healthComponent = transform.root.GetComponentInChildren<AIHealth_Health>();
    }

    private void Start()
    {
        startPosition = transform.position;
        _renderer = GetComponentInChildren<Renderer>();

        if(_renderer == null)
        {
            Debug.LogErrorFormat(this, "Tried to find Renderer on object, but could not!");
            return;
        }

        _mainMaterial = _renderer.material;

        if(_mainMaterial == null)
        {
            Debug.LogErrorFormat(this, "Could not find Material or Renderer!");
            return;
        }

        _mainMaterial = new Material(_mainMaterial);
        _mainMaterial.color = _materialColour;
        _renderer.material = _mainMaterial;
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
        StartCoroutine(KillAI());
    }

    public void ChangeDestination(Vector3 newDestination)
    {
        endPosition = newDestination;
        startPosition = transform.position;
        positionLerp = 0;
    }

    public override void DamageAI(int damageAmount)
    {
        if (_healthComponent == null)
        {
            Debug.LogError("No Health Component Found on Object", this);
            return;
        }

        _healthComponent.ChangeHealth(damageAmount);
    }

    public void EnhanceAI()
    {
        _damageToPlayerHealth *= Mathf.RoundToInt(_damageToPlayerHealth * _enhancedMultiplier);
        _moneyToRewardOnDeath = Mathf.RoundToInt(_moneyToRewardOnDeath * _enhancedMultiplier);
        aiSpeed *= _enhancedMultiplier;
        snapDistance *= _enhancedMultiplier;
    }

    private void MoveAI()
    {
        if (GameManager.Instance.hasGameOvered)
        {
            return;
        }

        if(_healthComponent._isDead)
        {
            return;
        }

        if (!moveForward)
        {
            currentWaypoint = AIPathingManager.GetNextWaypoint(currentWaypoint);

            // Arrived at Final Destination!
            if (currentWaypoint == null)
            {
                a_forceDetonateIncomingWeapons?.Invoke();
                PlayerManager.DecreaseHealth(_damageToPlayerHealth);
                transform.position = AISpawnManager.Instance._spawnLocation.position;
                currentWaypoint = AIPathingManager.GetNextWaypoint(currentWaypoint);
            }

            Vector3 vectorToTarget = currentWaypoint.transform.position - transform.position;
            float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
            transform.rotation = q;
            _previousDistance = Mathf.Infinity;
            moveForward = true;
            return;
        }

        // TODO: Stop running  these calculations when we've reached our destination!
        //          (Or, teleport to the beginning)
        transform.position += transform.right * Time.deltaTime * aiSpeed;
        float sqDistanceToWaypoint = UsefulMethods.CheapDistanceSquared(transform.position, currentWaypoint.transform.position);
        if (sqDistanceToWaypoint <= (snapDistance * snapDistance) || sqDistanceToWaypoint > _previousDistance)
        {
            moveForward = false;
            transform.position = currentWaypoint.transform.position;
        }
        _previousDistance = sqDistanceToWaypoint;

    }

    private IEnumerator KillAI()
    {
        float endTime = Time.time + _timeToDeath;
        Color fadeCol = _deathMaterialColour;
        float initA = fadeCol.a;
        while (fadeCol.a > 0f)
        {
            fadeCol.a -= Mathf.Clamp((initA / _timeToDeath) * Time.deltaTime, 0f, 1f);
            _mainMaterial.color = fadeCol;
            yield return new WaitForEndOfFrame();
        }

        Destroy(transform.root.gameObject);
    }
}