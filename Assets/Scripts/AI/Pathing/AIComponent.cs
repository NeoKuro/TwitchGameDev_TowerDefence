//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIComponent : MonoBehaviour
{
    protected AI _aiRef;


    protected virtual void Awake()
    {
        _aiRef = transform.root.GetComponentInChildren<AI>();
    }

    public virtual void SetEnhancedAI(float _enhancedMultiplier)
    {

    }
}