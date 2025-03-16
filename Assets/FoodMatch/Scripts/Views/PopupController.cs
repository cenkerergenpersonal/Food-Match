using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FoodMatch.Scripts.Views
{
    public class PopupController : MonoBehaviour
    {
        [SerializeField] private Image overlay;
        private Queue<Popup> _popupQueue = new Queue<Popup>();
        private bool _isPopupActive = false;

        public void Show(Popup popup)
        {
            _popupQueue.Enqueue(popup);
            if (!_isPopupActive)
            {
                ProcessQueue();
            }
        }

        private async void ProcessQueue()
        {
            while (_popupQueue.Count > 0)
            {
                _isPopupActive = true;
                Popup currentPopup = _popupQueue.Dequeue();
        
                await FadeOverlay(.9f);

                if (currentPopup == null || currentPopup.gameObject == null)
                {
                    Debug.LogWarning("Popup instance destroyed, skipping...");
                    continue;
                }

                currentPopup.Show();

                try
                {
                    using (var cancellation = new CancellationTokenSource())
                    {
                        cancellation.Token.Register(() => Debug.LogWarning("Popup cancelled due to scene change."));
                        await UniTask.WaitUntil(() => currentPopup == null || !currentPopup.gameObject.activeSelf, cancellationToken: cancellation.Token);
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.LogWarning("Popup wait cancelled!");
                }

                await FadeOverlay(0);
            }

            _isPopupActive = false;
        }
        
        private async UniTask FadeOverlay(float targetAlpha)
        {
            float duration = 0.3f;
            
            var sequence = DOTween.Sequence();
            sequence.SetId(this).Join(overlay.DOFade(targetAlpha, duration));
            sequence.OnComplete(() =>
            {
                overlay.raycastTarget = targetAlpha != 0;
            }).Play();
            await sequence.AsyncWaitForCompletion();


        }
    }
}