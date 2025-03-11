using System.Collections.Generic;
using FoodMatch.Scripts.Game.Interfaces;
using UnityEngine;

namespace FoodMatch.Scripts.Game
{
    public abstract class ItemBase : MonoBehaviour, ITouchable, IMatchable
    {
        [SerializeField] private Vector3 minPosition;
        [SerializeField] private Vector3 maxPosition;

        [SerializeField] private Vector3 minRotation;
        [SerializeField] private Vector3 maxRotation;
        [SerializeField] private List<GameObject> _childs;
        
        private GameObject _gameObject;
        private bool _isTouched;
        private bool _isEnabledToTouch;
        private bool _isMatched;
        private bool _isEnabledToMatched;

        public virtual void Initialize(int childIndex)
        {
            _childs[childIndex].SetActive(true);
            MeshFilter childMeshFilter = _childs[childIndex].GetComponent<MeshFilter>();

            if (childMeshFilter != null)
            {
                Mesh mesh = childMeshFilter.sharedMesh;
                Bounds bounds = mesh.bounds;
                
                BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
                boxCollider.center = bounds.center;
                boxCollider.size = bounds.size;
            }
        }
        protected virtual void Awake()
        {
            _gameObject = gameObject;
        }

        protected virtual void Start()
        {
            
        }

        protected virtual void OnEnable()
        {
            float randomX = Random.Range(minPosition.x, maxPosition.x);
            float randomY = Random.Range(minPosition.y, maxPosition.y);
            float randomZ = Random.Range(minPosition.z, maxPosition.z);
            transform.position = new Vector3(randomX, randomY, randomZ);

            // Rastgele rotasyon ayarla
            float randomRotX = Random.Range(minRotation.x, maxRotation.x);
            float randomRotY = Random.Range(minRotation.y, maxRotation.y);
            float randomRotZ = Random.Range(minRotation.z, maxRotation.z);
            transform.rotation = Quaternion.Euler(randomRotX, randomRotY, randomRotZ);
        }

        protected virtual void OnDisable()
        {
            
        }

        public virtual void OnTouch()
        {
            if (_isEnabledToTouch)
            {
                _isTouched = true;
            }
        }

        public virtual void DisabledTouch()
        {
            _isEnabledToTouch = false;
        }

        public virtual void EnabledTouch()
        {
            _isEnabledToTouch = true;
        }

        public virtual void Match()
        {
            if (_isEnabledToMatched)
            {
                _isMatched = true;
            }
        }

        public virtual void Unmatch()
        {
            if (_isEnabledToMatched)
            {
                _isMatched = false;
            }
        }

        public virtual void DisableMatch()
        {
            _isEnabledToMatched = false;
        }

        public virtual void EnableMatch()
        {
            _isEnabledToMatched = true;
        }
    }
}