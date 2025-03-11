using Cysharp.Threading.Tasks;
using FoodMatch.Scripts.Common;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace FoodMatch.Scripts.Game.Factory
{
    public class ItemFactory : IItemFactory
    {
        private readonly DiContainer _container;
        private readonly ObjectPool<ItemBase> _pool;
        private readonly string _address = "Item";
        
        
        public ItemFactory(DiContainer container, ObjectPool<ItemBase> pool)
        {
            _container = container;
            _pool = pool;
        }
        
        public async UniTask<ItemBase> CreateItem()
        {
            ItemBase item = _pool.Get();
            if (item == null)
            {
                GameObject prefab = await LoadPrefabAsync(_address);
                if (prefab == null) return null;

                item = _container.InstantiatePrefab(prefab).GetComponent<ItemBase>();
                _pool.Return(item);
            }

            item.gameObject.SetActive(true);
            return item;
        }

        private async UniTask<GameObject> LoadPrefabAsync(string address)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
            await handle.Task;
            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }
    }
}