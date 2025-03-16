using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using FoodMatch.Scripts.Game.Factory;
using UnityEngine;
using Zenject;

namespace FoodMatch.Scripts.Game
{
    public class ItemManager: IInitializable, IDisposable
    {
        private List<Transform> _containers = new List<Transform>();
        private List<Item> _items;
        private HashSet<Item> _containedItems = new HashSet<Item>();
        [Inject] IItemFactory _itemFactory;
        private int _containerSize = 7;

        public void Initialize()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EventBus.Subscribe<MatchFoundEvent>(OnMatchFound);
            EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
            EventBus.Subscribe<LevelFailedEvent>(OnLevelFailed);
            EventBus.Subscribe<SetContainersEvent>(SetContainers);
        }
        public void Dispose()
        {
            EventBus.Unsubscribe<MatchFoundEvent>(OnMatchFound);
            EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
            EventBus.Unsubscribe<LevelFailedEvent>(OnLevelFailed);
            EventBus.Unsubscribe<SetContainersEvent>(SetContainers);
        }

        private void SetContainers(SetContainersEvent eventData)
        {
            _containers = eventData.Containers;
        }

        private async void OnLevelFailed(LevelFailedEvent eventData)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                RemoveAll();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private async void OnLevelCompleted(LevelCompletedEvent eventData)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                RemoveAll();
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
        }

        private void RemoveAll()
        {
            foreach (var item in _items.ToList())
            {
                if (item == null)
                {
                    continue;
                }
                _items.Remove(item);
                _itemFactory.Remove(item);
            }

            foreach (var item in _containedItems)
            {
                if (item == null)
                {
                    continue;
                }
                _containedItems.Remove(item);
                _itemFactory.Remove(item);
            }
            _items.Clear();
            _containedItems.Clear();
        }
        
        private async void OnMatchFound(MatchFoundEvent matchFoundEvent)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(.5f));
            Debug.Log($"MatchFoundEvent: {matchFoundEvent.MatchedItems.Count}");

            foreach (var item in matchFoundEvent.MatchedItems)
            {
                if (item == null)
                {
                    return;
                }
                _containedItems.Remove(item);
                _itemFactory.Remove(item);
            }
        }

        public async void InitializeItems(LevelData levelData)
        {
            Debug.Log($"Initializing items {levelData.LevelId}");
            _items = new List<Item>();
            var goals = levelData.Goals;

            for (int i = 0; i < levelData.TotalItemCount; i++)
            {
                var chooseGoal =  goals[i % 3];
                Item item = await _itemFactory.CreateItem(chooseGoal.ItemSet,chooseGoal.itemType);
                _items.Add(item);
            }
        }

        public List<Item> GetContainedItems()
        {
            return _containedItems.ToList();
        }

        public async UniTask AddItem(Item selectedItem)
        {
            if (_containedItems.Count >= _containerSize)
            {
                Debug.LogError("Container full");
                EventBus.Publish(new LevelFailedEvent());
                return;
            }

            _containedItems.Add(selectedItem);

            if (_containedItems.Count >= 3)
            {
                EventBus.Publish(new CheckMatchEvent(selectedItem));
            }
            await AssignItemToContainer(selectedItem);

        }
        private async UniTask AssignItemToContainer(Item item)
        {
            Transform targetContainer = GetContainerForItem(item);

            if (targetContainer == null)
            {
                Debug.LogError("No available container found!");
                return;
            }

            item.transform.SetParent(targetContainer);
            await AnimateItemToPosition(item, targetContainer);
        }

        private Transform GetContainerForItem(Item item)
        {
            foreach (var container in _containers)
            {
                if (container.childCount == 0)
                {
                    return container;
                }
            }

            return null;
        }

        private async UniTask AnimateItemToPosition(Item item, Transform targetContainer)
        {
            item.transform.SetParent(targetContainer);
            Vector3 targetPosition = targetContainer.position;
            var sequence = DOTween.Sequence();
            sequence.SetId(this);

            sequence.Join(item.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutBack));
            sequence.Join(item.transform.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBack));
            sequence.Join(item.transform.DOScale(Vector3.one * 80f, 0.5f).SetEase(Ease.OutBack));
            sequence.Play();
            await sequence.AsyncWaitForCompletion();
        }
    }
}