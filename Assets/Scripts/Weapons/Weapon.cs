//-----------------------------\\
//     Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private uint _damageToAI = 10;
    [SerializeField]
    private Vector3 _positionOffset = new Vector3(0, 0, -1f);
    [SerializeField]
    private GameObject _detonateEffects;
    

    protected AI _currentTarget;
    protected CombatTypeBase _weaponType;
    protected ParticleSystem _pSystem;
    protected Renderer[] _renderers;

    protected bool _hasDetonated = false;

    protected virtual void Awake()
    {
        _renderers = GetComponentsInChildren<Renderer>();
        _pSystem = GetComponentInChildren<ParticleSystem>();
    }

    protected virtual void Start()
    {
        // Stuff In here ran on object creation
    }

    protected virtual void Update()
    {
        if (GameManager.Instance.hasGameOvered)
        {
            return;
        }

    }

    public virtual void Initialise(Vector3 startPos, AI target, CombatTypeBase weaponType)
    {
        if(weaponType == null)
        {
            Debug.LogErrorFormat(this, "Weapon type passed in is NULL!");
            return;
        }

        _pSystem.Clear();
        _pSystem.Play();
        _hasDetonated = false;
        transform.position = startPos + _positionOffset;
        _currentTarget = target;
        _weaponType = weaponType;
        _currentTarget.RegisterForceDetonateAction(ForceDetonateWeapon);
        gameObject.SetActive(true);
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].enabled = true;
        }
    }

    protected virtual void DetonateWeapon()
    {
        if (_currentTarget != null)
        {
            _currentTarget.DamageAI((int)-_damageToAI, _weaponType);
            _currentTarget.DeregisterForceDetonateAction(ForceDetonateWeapon);
        }

        _currentTarget = null;
        PlayDetonateEffects();
        for (int i = 0; i < _renderers.Length; i++)
        {
            _renderers[i].enabled = false;
        }

        _hasDetonated = true;

        if(!gameObject.activeInHierarchy)
        {
            return;
        }

        StartCoroutine(DisableWeaponAfterTime());
    }

    protected virtual void ForceDetonateWeapon()
    {
        _currentTarget = null;
        DetonateWeapon();
    }

    protected virtual void PlayDetonateEffects()
    {
        if (_hasDetonated)
        {
            return;
        }

        GameObject explosion = ObjectPool.GetObjectOfType(_detonateEffects);
        explosion.transform.position = transform.position;
        explosion.SetActive(true);
    }

    protected IEnumerator DisableWeaponAfterTime()
    {
        _pSystem.Stop();
        float particleLifeTime = (_pSystem.main.startLifetime.constantMax) * 2f;
        float endTime = Time.time + particleLifeTime;
        while (Time.time < endTime)
        {
            yield return new WaitForEndOfFrame();
        }

        gameObject.SetActive(false);
    }
}