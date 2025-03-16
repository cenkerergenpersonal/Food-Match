using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace FoodMatch.Scripts.Game.Factory
{
    public class ItemFactory : IItemFactory
    {
        private readonly DiContainer _container;
        private readonly string _address = "Item";
        
        public ItemFactory(DiContainer container)
        {
            _container = container;
        }
        
        public async UniTask<Item> CreateItem(int setId, int itemId)
        {
            var address = $"{_address}_{setId}_{itemId}";
            GameObject prefab = await LoadPrefabAsync(address);
            if (prefab == null) return null;

            var item = _container.InstantiatePrefab(prefab).GetComponent<Item>();

            item.gameObject.SetActive(true);
            return item;
        }

        private async UniTask<GameObject> LoadPrefabAsync(string address)
        {
            AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
            await handle.Task;
            return handle.Status == AsyncOperationStatus.Succeeded ? handle.Result : null;
        }

        public void Remove(Item item)
        {
            item.transform.DOScale(Vector3.zero, .3f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                if (item == null)
                {
                    return;
                }
                if (item.transform == null)
                {
                    return;
                }
                item.transform.SetParent(null);
                item.DestroyGameObject();
            });
        }
    }
}