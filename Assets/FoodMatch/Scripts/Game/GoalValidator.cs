using System.Linq;
using DG.Tweening;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FoodMatch.Scripts.Game
{
    public class GoalValidator : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private ItemSpriteSettings itemSpriteSettings;
        [SerializeField] private TMP_Text text;
        
        private Goal _goal;
        private int _remainingCount;

        public void Initialize(Goal goal)
        {
            _goal = goal;
            _remainingCount = goal.requiredCount;
            var sprites = itemSpriteSettings.ItemSprites.FindAll(item => (int)item.ItemType == goal.ItemSet);
            image.sprite = sprites.FindAll(itemSprite => itemSprite.ItemSprites[goal.ItemSet]).First().ItemSprites[goal.itemType];
            text.text = (_remainingCount/3).ToString();
            gameObject.SetActive(true);
        }

        private void OnEnable()
        {
            EventBus.Subscribe<UpdateGoalEvent>(OnGoalUpdated);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<UpdateGoalEvent>(OnGoalUpdated);
        }

        private void OnGoalUpdated(UpdateGoalEvent goalEvent)
        {
            if (_goal == null)
            {
                return;
            }
            if (_goal.itemType != goalEvent.Goal.itemType)
            {
                return;
            }
            var tempRemainingCount = _goal.requiredCount - goalEvent.CurrentCount;
            if (tempRemainingCount == _remainingCount)
            {
                return;
            }
            _remainingCount = tempRemainingCount;
            text.text = _remainingCount > 0 ? (_remainingCount/3).ToString() : "DONE";

            transform.DOScale(Vector3.one * 1.3f, .2f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                transform.DOScale(Vector3.one, .2f).SetEase(Ease.InBack);
            });
        }
    }
}