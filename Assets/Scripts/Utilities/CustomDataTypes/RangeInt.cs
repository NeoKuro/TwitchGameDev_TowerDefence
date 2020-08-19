//-----------------------------\\
//    Project TowerDefence
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class RangeInt
{
    public int min = 0;
    public int max = 1;

    public RangeInt(int _min, int _max)
    {
        min = _min;
        max = _max;
    }

    public int Random
    {
        get
        {
            return UnityEngine.Random.Range(min, max);
        }
    }

    public static RangeInt operator +(RangeInt lhs, int rhs)
    {
        return new RangeInt(lhs.min + rhs, lhs.max + rhs);
    }
}