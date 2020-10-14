using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    private static string DebugPrefix = "<color=yellow>[Singleton]</color>";

    //private static object _lock = new object();

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarningFormat("{0} {1} already destroyed on application quit. Won't create again - returning null.", DebugPrefix, typeof(T).ToString());
                return null;
            }

            if (_instance == null)
            {
                //Debug.LogErrorFormat("{0} {1} does not exist!", DebugPrefix, typeof(T).ToString());
                Singleton<T> newSingleton = FindObjectOfType<Singleton<T>>();
                if(newSingleton == null)
                { 
                    Debug.LogErrorFormat("{0} {1} does not exist!", DebugPrefix, typeof(T).ToString());
                    return _instance;
                }

                _instance = newSingleton as T;
            }

            return _instance;
        }
    }

    public abstract void Initialise();
    public abstract void OnRetryExecuted();

    private static bool applicationIsQuitting = false;
    /// <summary>
    /// When Unity quits, it destroys objects in a random order.
    /// In principle, a Singleton is only destroyed when application quits.
    /// If any script calls Instance after it have been destroyed, 
    ///   it will create a buggy ghost object that will stay on the Editor scene
    ///   even after stopping playing the Application. Really bad!
    /// So, this was made to be sure we're not creating that buggy ghost object.
    /// </summary>
    public void OnDestroy()
    {
        //applicationIsQuitting = true;
        Application.quitting -= ApplicationQuitting;
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }

        Application.quitting += ApplicationQuitting;
        applicationIsQuitting = false;
        _instance = this as T;
        //DontDestroyOnLoad(gameObject);
    }

    private void ApplicationQuitting()
    {
        applicationIsQuitting = true;
    }
}