using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Events;
using Zenject;

namespace FoodMatch.Scripts.Game
{
    public class MatchSystem  : IInitializable, IDisposable
    {
        [Inject] ItemManager _itemsController;
        [Inject] GoalManager _goalManager;
        private List<Item> _items;
        
        public void Initialize()
        {
            EventBus.Subscribe<CheckMatchEvent>(StartCheckMatch);
            EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
            EventBus.Subscribe<LevelFailedEvent>(OnLevelFailed);
        }
        
        public void Dispose()
        {
            EventBus.Unsubscribe<CheckMatchEvent>(StartCheckMatch);
            EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
            EventBus.Unsubscribe<LevelFailedEvent>(OnLevelFailed);
        }
        
        private async void OnLevelFailed(LevelFailedEvent obj)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.1f));
        }

        private async void OnLevelCompleted(LevelCompletedEvent eventData)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1.1f));
        }

        private void StartCheckMatch(CheckMatchEvent eventData)
        {
            _items = _itemsController.GetContainedItems();

            HashSet<Item> matchedItems = new HashSet<Item>();

            var groupedItems = _items.GroupBy(item => item.ItemId)
                .Where(group => group.Count() >= 3); 
            
            foreach (var group in groupedItems)
            {
                foreach (var item in group)
                {
                    matchedItems.Add(item);
                }
            }
            
            if (matchedItems.Count > 0)
            {
                EventBus.Publish(new MatchFoundEvent(matchedItems));
            }
        }
    }
}