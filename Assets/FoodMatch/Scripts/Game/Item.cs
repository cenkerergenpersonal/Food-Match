using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace FoodMatch.Scripts.Game
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private ItemTransformData transformData;
        public int ItemId;
        private GameObject _gameObject;
        protected virtual void Awake()
        {
            _gameObject = gameObject;
        }

        private void OnEnable()
        {
            SetTransformData();
        }
        private void SetTransformData()
        {
            float randomX = Random.Range(transformData.minPosition.x, transformData.maxPosition.x);
            float randomY = Random.Range(transformData.minPosition.y, transformData.maxPosition.y);
            float randomZ = Random.Range(transformData.minPosition.z, transformData.maxPosition.z);
            transform.position = new Vector3(randomX, randomY, randomZ);

            float randomRotX = Random.Range(transformData.minRotation.x, transformData.maxRotation.x);
            float randomRotY = Random.Range(transformData.minRotation.y, transformData.maxRotation.y);
            float randomRotZ = Random.Range(transformData.minRotation.z, transformData.maxRotation.z);
            transform.rotation = Quaternion.Euler(randomRotX, randomRotY, randomRotZ);
        }

        public void DestroyGameObject()
        {
            Destroy(_gameObject);
        }
    }

    [Serializable]
    public struct ItemTransformData
    {
        public Vector3 minPosition;
        public Vector3 maxPosition;
        public Vector3 minRotation;
        public Vector3 maxRotation;
    }
}