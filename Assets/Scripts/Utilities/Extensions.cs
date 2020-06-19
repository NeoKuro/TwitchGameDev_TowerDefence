using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{

    public static Vector3 NicePosition(this MonoBehaviour obj)
    {
        return obj.transform.position;
    }

    public static Vector3 LocalPosition(this MonoBehaviour obj)
    {
        return obj.transform.localPosition;
    }

    public static float Square(this float val)
    {
        return val * val;

    }
    public static int LayerToLayerMask(this GameObject obj)
    {
        return 1 << obj.layer;
    }

    public static Vector3 Flatten(this Vector3 pos)
    {
        return new Vector3(pos.x, 0f, pos.z);
    }

    //private static System.Random rng = new System.Random();
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            //int k = rng.Next(n + 1);
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    #region - Get Largest Element In Vector -
    public static float GetLargestElementInVector(this Vector2 vector)
    {
        float maxVal = 0f;
        if (vector.x > maxVal)
            maxVal = vector.x;

        if (vector.y > maxVal)
            maxVal = vector.y;

        return maxVal;
    }

    public static float GetLargestElementInVector(this Vector3 vector)
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

    public static float GetLargestElementInVector(this Vector4 vector)
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

}
