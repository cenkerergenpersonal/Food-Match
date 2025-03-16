using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using UnityEngine;
using Zenject;

namespace FoodMatch.Scripts.Game
{
    public class GoalManager : IInitializable, IDisposable
    {
        private Dictionary<int, int> _currentMatches;
        private List<Goal> _goals;

        private ObjectPool<GoalValidator> _goalValidatorPool;
        private List<GoalValidator> _goalValidators;

        private Transform _goalParent;
        
        [Inject]
        public void Construct(DiContainer container, [Inject(Id = "GoalCheckerPrefab")] GameObject goalCheckerPrefab, [Inject(Id = "GoalParent")] Transform goalParent)
        {
            Initialize();
            InstantiateGoals(container, goalCheckerPrefab, goalParent);
            SubscribeEvents();
        }

        public void Initialize()
        {
            _currentMatches = new Dictionary<int, int>();
            _goals = new List<Goal>();
            _goalValidators = new List<GoalValidator>();
        }
        private void SubscribeEvents()
        {
            EventBus.Subscribe<UpdateGoalEvent>(OnGoalUpdated);
            EventBus.Subscribe<MatchFoundEvent>(OnMatchFound);
            EventBus.Subscribe<LevelCompletedEvent>(OnLevelCompleted);
            EventBus.Subscribe<LevelFailedEvent>(OnLevelFailed);
        }

        private void UnsubscribeEvents()
        {
            EventBus.Unsubscribe<UpdateGoalEvent>(OnGoalUpdated);
            EventBus.Unsubscribe<MatchFoundEvent>(OnMatchFound);
            EventBus.Unsubscribe<LevelCompletedEvent>(OnLevelCompleted);
            EventBus.Unsubscribe<LevelFailedEvent>(OnLevelFailed);
        }
        
        public void Dispose()
        {
            UnsubscribeEvents();
        }
        
        
        private void InstantiateGoals(DiContainer container, GameObject goalCheckerPrefab, Transform goalParent)
        {
            var instantiatedPrefab = GameObject.Instantiate(goalCheckerPrefab);
            _goalValidatorPool = new ObjectPool<GoalValidator>(container, instantiatedPrefab, 5, goalParent);
            _goalParent = goalParent;
        }

        private async void OnLevelFailed(LevelFailedEvent eventData)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.1f));
                ClearGoals();
            }
            catch (Exception e)
            {
                Debug.Log($"when level failed exception: {e.GetType()}");
            }
        }
        private async void OnLevelCompleted(LevelCompletedEvent eventData)
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1.1f));
                ClearGoals();
            }
            catch (Exception e)
            {
                Debug.Log($"when level failed exception: {e.GetType()}");
            }
        }
        private void ClearGoals()
        {
            ClearGoalValidators();
            _currentMatches = new Dictionary<int, int>();
            _goals = new List<Goal>();
        }

        private void ClearGoalValidators()
        {
            if (_goalValidators == null)
            {
                return;
            }
            foreach (var checker in _goalValidators)
            {
                _goalValidatorPool.Return(checker);
            }
            _goalValidators.Clear();
        }

        public void InitializeGoals(List<Goal> levelGoals)
        {
            _goals = levelGoals;

            ClearGoalValidators();

            foreach (var goal in _goals)
            {
                GetGoalValidator(goal);
            }
        }

        private void GetGoalValidator(Goal goal)
        {
            var checker = _goalValidatorPool.Get();
            checker.transform.SetParent(_goalParent, false);
            checker.Initialize(goal);
            _goalValidators.Add(checker);
        }

        private void OnMatchFound(MatchFoundEvent eventData)
        {
            foreach (var item in eventData.MatchedItems)
            {
                AddMatch(item);
            }
        }

        private void AddMatch(Item item)
        {
            var key = item.ItemId;

            if (!_currentMatches.ContainsKey(key))
            {
                _currentMatches[key] = 0;
            }
            _currentMatches[key]++;
            
            EventBus.Publish(new UpdateGoalEvent(new Goal
            {
                itemType = (int)item.ItemId,
                requiredCount = _currentMatches[key]
            }, _currentMatches[key]));

            CheckGoals();
            if (IsGoalComplete())
            {
                EventBus.Publish(new LevelCompletedEvent());
                EventBus.Publish(new ShowPopupEvent(PopupType.LevelCompleted));
            }
        }

        private void OnGoalUpdated(UpdateGoalEvent eventData)
        {
            var goal = eventData.Goal;
            var key = goal.itemType;

            if (!_currentMatches.ContainsKey(key))
            {
                _currentMatches[key] = 0;
            }

            _currentMatches[key] = eventData.CurrentCount;
        }

        private bool IsGoalComplete()
        {
            foreach (var goal in _goals)
            {
                var key = goal.itemType;
                if (_currentMatches.ContainsKey(key) && _currentMatches[key] >= goal.requiredCount)
                {
                    continue;
                }
                return false;
            }
            return true;
        }

        private void CheckGoals()
        {
            foreach (var goal in _goals)
            {
                var key = goal.itemType;
                int currentCount = _currentMatches.ContainsKey(key) ? _currentMatches[key] : 0;

                if (goal.requiredCount == currentCount) continue;

                EventBus.Publish(new UpdateGoalEvent(goal, currentCount));
            }
        }
    }
}