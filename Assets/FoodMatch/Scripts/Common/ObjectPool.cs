using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FoodMatch.Scripts.Common{
    public class ObjectPool<T> where T : MonoBehaviour
    {
        private readonly Queue<T> _pool = new Queue<T>();
        private readonly DiContainer _container;
        private readonly string _address;
        private readonly Transform _parent;
        private readonly GameObject _prefab;
        private readonly int _initialSize;

        public ObjectPool(DiContainer container, GameObject prefab, int initialSize, Transform parent)
        {
            _container = container;
            _prefab = prefab;
            _initialSize = initialSize;
            _parent = parent;

            for (int i = 0; i < _initialSize; i++)
            {
                CreateNewInstance();
            }
        }

        private void CreateNewInstance()
        {
            T obj = _container.InstantiatePrefab(_prefab, _parent).GetComponent<T>();
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                T obj = _pool.Dequeue();
                obj.gameObject.SetActive(true);
                return obj;
            }
            else
            {
                T obj = _container.InstantiatePrefab(_prefab, _parent).GetComponent<T>();
                return obj;
            }
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _pool.Enqueue(obj);
        }
    }
}