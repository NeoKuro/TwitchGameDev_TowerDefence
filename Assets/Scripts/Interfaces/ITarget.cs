//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITarget : IComponent
{
    Targeting TargetingComponent { get; }

}