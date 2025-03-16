using Cysharp.Threading.Tasks;
using DG.Tweening;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FoodMatch.Scripts.Views
{
    public class LevelCompletedPopup: Popup
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_Text _text;
        private void OnEnable()
        {
            SetCompletedTextImage();
            PlayShowAnimation();
        }

        private void SetCompletedTextImage()
        {
            if (PopupType == PopupType.LevelCompleted)
            {
                _text.text = "LEVEL COMPLETED!";
            }

            if (PopupType == PopupType.LevelFailed)
            {
                _text.text = "LEVEL FAILED!";
            }

            _text.DOFade(0, 0);
        }

        public void OnRestartButton()
        {
            Hide();
        }

        public async override void Hide()
        {
            await PlayHideAnimation();
            base.Hide();
            EventBus.Publish(new StartGameEvent("level1"));
        }

        private async UniTask PlayShowAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.SetId(this);
            var scale = _rectTransform.DOScale(Vector3.one, .3f).SetEase(Ease.OutBack);
            var fade = _canvasGroup.DOFade(1f, .3f);
            var fadeImage = _text.DOFade(1f, .3f);
            sequence.Join(scale).Join(fade).Append(fadeImage).Play();
            await sequence.AsyncWaitForCompletion();
        }
        private async UniTask PlayHideAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence.SetId(this);
            var scale = _rectTransform.DOScale(Vector3.zero, .3f).SetEase(Ease.OutBack);
            var fade = _canvasGroup.DOFade(0f, .3f);
            var fadeImage = _text.DOFade(0f, .3f);
            sequence.Join(scale).Join(fade).Append(fadeImage).Play();
            await sequence.AsyncWaitForCompletion();
        }
    }
}