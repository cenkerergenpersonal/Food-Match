using System;
using Cysharp.Threading.Tasks;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using TMPro;
using UnityEngine;

namespace FoodMatch.Scripts.Game
{
    public class GameUiController : MonoBehaviour
    {
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private TMP_Text timerText;
        
        private int _timeInSec = 0;
        private bool _timerEnabled = false;
        private void OnEnable()
        {
            EventBus.Subscribe<LevelCompletedEvent>(OnGameCompleted);
            EventBus.Subscribe<LevelFailedEvent>(OnGameFailed);
        }

        private void OnGameFailed(LevelFailedEvent eventData)
        {
            _timerEnabled = false;
        }

        private void OnGameCompleted(LevelCompletedEvent eventData)
        {
            _timerEnabled = false;
        }

        public void Initialize(LevelData levelData)
        {
            levelText.text = $"Level {levelData.LevelId+1}";
            _timeInSec = (int)levelData.TotalTimeFromSeconds;
            StartTimer();
        }
        
        public void StartTimer()
        {
            if (_timerEnabled)
                return;

            _timerEnabled = true;
            RunTimer().Forget();
        }
        private async UniTaskVoid RunTimer()
        {
            while (_timerEnabled && _timeInSec > 0)
            {
                UpdateTimerText();
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                _timeInSec--;
            }

            if (_timeInSec <= 0)
            {
                _timerEnabled = false;
                TimerFinished();
            }
        }

        private void UpdateTimerText()
        {

                int minutes = _timeInSec / 60;
                int seconds = _timeInSec % 60;
                timerText.text = $"{minutes:D2}:{seconds:D2}";
        }

        private void TimerFinished()
        {
            timerText.text = "00:00";
            EventBus.Publish(new LevelFailedEvent());
            EventBus.Publish(new ShowPopupEvent(PopupType.LevelFailed));
        }
    }
}