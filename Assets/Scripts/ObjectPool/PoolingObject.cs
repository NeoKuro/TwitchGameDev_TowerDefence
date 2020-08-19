//-----------------------------\\
//     Project Breedables
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingObject : MonoBehaviour
{
    [SerializeField]
    private GameObject _objRef;

    public void Initialise(GameObject objRef)
    {
        _objRef = objRef;
    }

    private void OnDisable()
    {
        if(_objRef == null)
        {
            Debug.LogErrorFormat(this, "ObjRef is not set on Pooling Object! I don't know what pool to add myself to!?");
            return;
        }

        ObjectPool.ReleaseGameObject(_objRef, gameObject);
    }
}