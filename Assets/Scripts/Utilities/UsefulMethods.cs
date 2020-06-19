using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsefulMethods : MonoBehaviour
{

    public static double Deg2RadD
    {
        get
        {
            return Math.PI / 180d;
        }
    }

    public static float CheapDistance(Vector3 v1, Vector3 v2)
    {
        return Mathf.Sqrt(CheapDistanceSquared(v1, v2));
    }

    public static float CheapDistanceSquared(Vector3 v1 , Vector3 v2)
    {
        Vector3 deltaPos = v2 - v1;
        float dist = deltaPos.x.Square() + deltaPos.y.Square() + deltaPos.z.Square();
        return dist;
    }

    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        //return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }

    public static int IntToBit(int layer)
    {
        return 1 << layer;
    }

    #region - Get Largest Element In Vector -
    public static float GetLargestElementInVector(Vector2 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        return maxVal;
    }

    public static float GetLargestElementInVector(Vector3 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        if (vector.z > maxVal)
            maxVal = vector.z;

        return maxVal;
    }

    public static float GetLargestElementInVector(Vector4 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        if (vector.z > maxVal)
            maxVal = vector.z;

        if (vector.w > maxVal)
            maxVal = vector.w;

        return maxVal;
    }
    #endregion

    public static float Map(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }
}
