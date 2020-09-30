//-----------------------------\\
//      Project Tower Defence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovement : IComponent
{
    bool CanMove();
    void ArrivedAtFinalDestination();

}