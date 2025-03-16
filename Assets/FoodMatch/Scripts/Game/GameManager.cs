using System;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using Zenject;

namespace FoodMatch.Scripts.Game
{
    public class GameManager : IInitializable , IDisposable
    {
        private readonly LevelLoader _levelLoader;
        private readonly GoalManager _goalTracker;
        private readonly ItemManager _itemManager;
        private readonly GameUiController _gameUiController;

        public GameManager(LevelLoader levelLoader, GoalManager goalTracker, ItemManager itemManager, GameUiController gameUiController)
        {
            _levelLoader = levelLoader;
            _goalTracker = goalTracker;
            _itemManager = itemManager;
            _gameUiController = gameUiController;
        }
        
        public void Initialize()
        {
            EventBus.Subscribe<StartGameEvent>(OnGameStart);
        }
        public void Dispose()
        {
            EventBus.Unsubscribe<StartGameEvent>(OnGameStart);
        }
        
        private void OnGameStart(StartGameEvent eventData)
        {
            LevelData levelData = _levelLoader.LoadLevel(eventData.LevelFileName);
            if (levelData == null)
            {
                return;
            }
            
            _goalTracker.InitializeGoals(levelData.Goals);
            _itemManager.Initialize();
            _itemManager.InitializeItems(levelData);
            _gameUiController.Initialize(levelData);
        }
    }
}