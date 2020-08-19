//-----------------------------\\
//     Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mobile_Weapon : Weapon
{
    [SerializeField]
    private float _weaponSpeed = 20f;
    [SerializeField]
    private float _detonationRadius = 0.1f;



    protected override void Update()
    {
        if (GameManager.Instance.hasGameOvered)
        {
            return;
        }

        base.Update();
        MoveWeapon();
    }

    protected virtual void MoveWeapon()
    {
        if (_currentTarget == null)
        {
            DetonateWeapon();
            return;
        }

        Vector3 myPosition = new Vector3(transform.position.x, transform.position.y, 0f);
        Vector3 targetPosition = new Vector3(_currentTarget.transform.position.x, _currentTarget.transform.position.y, 0f);

        Vector3 vectorToTarget = targetPosition - myPosition;
        float angleToTarget = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = q;
        transform.position += transform.right * Time.deltaTime * _weaponSpeed;

        float sqDistanceToWaypoint = UsefulMethods.CheapDistanceSquared(myPosition, targetPosition);
        if (sqDistanceToWaypoint <= (_detonationRadius * _detonationRadius))
        {
            DetonateWeapon();
        }
    }
}