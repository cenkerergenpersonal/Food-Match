using UnityEngine;
using Zenject;

namespace FoodMatch.Scripts.Game
{
    public class ItemInputHandler: MonoBehaviour
    {
        [SerializeField] private Item _item;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Collider _collider;
        [Inject] private ItemManager _itemsController; 
        
        private void OnMouseDown()
        {
            _rigidbody.isKinematic = true;
            _collider.isTrigger = true;
            _itemsController.AddItem(_item);
        }
    }
}