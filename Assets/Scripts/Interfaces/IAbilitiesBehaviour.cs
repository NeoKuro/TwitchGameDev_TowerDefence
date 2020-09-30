//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAbilitiesBehaviour : IComponent
{
    AbilitySystem AbilitySystemComponent { get; }
}