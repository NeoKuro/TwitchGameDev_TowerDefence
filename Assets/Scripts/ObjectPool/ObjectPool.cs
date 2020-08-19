//-----------------------------\\
//     Project Breedables
//       Twitch: NeoKuro
//    Author: Joshua Hughes
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [Serializable]
    public struct PoolSettings
    {
        public GameObject referenceToObject;
        public int noToSpawn;
    }

    [SerializeField]
    private List<PoolSettings> _poolSettings = new List<PoolSettings>();

    private Dictionary<GameObject, List<GameObject>> _objectPool = new Dictionary<GameObject, List<GameObject>>();

    protected override void Awake()
    {
        base.Awake();
        InitialiseObjectPool();
    }

    private void InitialiseObjectPool()
    {
        for (int i = 0; i < _poolSettings.Count; i++)
        {
            if (_objectPool.ContainsKey(_poolSettings[i].referenceToObject))
            {
                Debug.LogErrorFormat("You already added this Object to the pool!");
                continue;
            }

            _objectPool.Add(_poolSettings[i].referenceToObject, new List<GameObject>());

            bool _needsPooler = _poolSettings[i].referenceToObject.GetComponentInChildren<PoolingObject>() == null;
            for (int j = 0; j < _poolSettings[i].noToSpawn; j++)
            {
                _objectPool[_poolSettings[i].referenceToObject].Add(SpawnObject(_poolSettings[i].referenceToObject, _needsPooler));
            }
        }
    }

    private GameObject SpawnObject(GameObject objectToSPawn, bool needsPooler)
    {
        GameObject newObj = Instantiate(objectToSPawn, new Vector3(-1000, -1000, -100), Quaternion.identity, transform);
        if (needsPooler)
        {
            PoolingObject pooler = newObj.AddComponent<PoolingObject>();
            pooler.Initialise(objectToSPawn);
        }
        newObj.SetActive(false);
        return newObj;
    }

    private GameObject GetObjectOfType_Internal(GameObject objectReference)
    {
        if (!_objectPool.ContainsKey(objectReference))
        {
            _objectPool.Add(objectReference, new List<GameObject>());
        }

        if (_objectPool[objectReference].Count == 0)
        {
            bool needsPooler = objectReference.GetComponentInChildren<PoolingObject>() == null;
            GameObject tmpObj = SpawnObject(objectReference, needsPooler);
            return tmpObj;
        }

        GameObject newObj = _objectPool[objectReference][0];
        _objectPool[objectReference].RemoveAt(0);
        //newObj.transform.SetParent(null);
        return newObj;
    }

    private void ReleaseGameObject_Internal(GameObject objRef, GameObject objInstance)
    {
        // Cannot unpairent same frame gameobject is activated (due to error message spam)
        //objInstance.transform.SetParent(transform);
        _objectPool[objRef].Add(objInstance);
    }

    public static GameObject GetObjectOfType(GameObject objectReference)
    {
        return Instance?.GetObjectOfType_Internal(objectReference);
    }

    public static void ReleaseGameObject(GameObject objRef, GameObject objInstance)
    {
        Instance?.ReleaseGameObject_Internal(objRef, objInstance);
    }
}