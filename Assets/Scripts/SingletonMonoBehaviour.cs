using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance != null) return instance;
            else
            {
                Debug.LogError(typeof(T) + " is nothing");
                return null;
            }
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (instance == this) instance = null;
    }
}
