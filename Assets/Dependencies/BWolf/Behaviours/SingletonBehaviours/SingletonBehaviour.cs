﻿// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.2
//----------------------------------

using UnityEngine;

namespace BWolf.Behaviours.SingletonBehaviours
{
    /// <summary>
    /// A basic unity implementation of a singleton. This can be used with components that have inspector field that need to be set beforehand
    /// </summary>
    public class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        private static bool appIsQuitting;
        protected bool isDuplicate;

        public static T Instance
        {
            get
            {
                if (appIsQuitting)
                {
                    //if this object is already destroyed during app closage return null to avoid MissingReferenceException
                    return null;
                }

                if (_instance == null)
                {
                    Debug.LogError($"Instance of {typeof(T).Name} is null :: make sure it is called after awake and the object is part of the scene");
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance != null && _instance != this)
            {
                isDuplicate = true;
                Destroy(this.gameObject);
            }
            else
            {
                _instance = GetComponent<T>();
                DontDestroyOnLoad(this.gameObject);
            }
        }

        //make sure no instance of this object can be made using new keyword by making the constructor protected
        protected SingletonBehaviour()
        {
        }

        protected virtual void OnDestroy()
        {
            if (!isDuplicate)
            {
                //only set appIsQuitting flag if this is not a duplicate being destroyed
                appIsQuitting = true;
            }
        }
    }
}