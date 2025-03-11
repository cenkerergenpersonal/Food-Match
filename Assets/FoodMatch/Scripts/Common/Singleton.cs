using System;
using UnityEngine;

namespace FoodMatch.Scripts.Common
{
    public abstract class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (Singleton<T>._instance == null)
                {
                    _instance = Activator.CreateInstance(typeof(T),true) as T;
                    if (_instance is IDisposable)
                    {
                        string message = $"An instance of {typeof(T).Name} shouldn't be disposable {typeof(T)} .";
                        throw new System.Exception(message);
                    }

                    Singleton<T>._instance = Instance;
                }
                return Singleton<T>._instance;
            }
        }
    }

    public abstract class BehaviourSingleton<T> : MonoBehaviour where T : Behaviour
    {
        private static T _instance;
        
        public static T Instance
        {
            get
            {
                return BehaviourSingleton<T>._instance;
            }
        }

        public virtual void Awake()
        {
            if (BehaviourSingleton<T>._instance != null)
            {
                DestroyImmediate(this);
                return;
            }

            BehaviourSingleton<T>._instance = this as T;
        }

        public virtual void OnDestroy()
        {
            ClearSubscriptions();
        }

        protected abstract void ClearSubscriptions();
        
    }
}