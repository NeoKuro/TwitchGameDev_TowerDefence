//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IComponent
{
    void RegisterComponentUpdate(Action a);
    void DeregisterComponentUpdate(Action a);
}