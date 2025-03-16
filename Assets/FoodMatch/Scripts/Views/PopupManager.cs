using System.Collections.Generic;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;
using FoodMatch.Scripts.Events;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace FoodMatch.Scripts.Views
{
    public class PopupManager : MonoBehaviour
    {
        [Inject] private PopupController popupController;
        [Inject] private PopupData popupLibrary;

        private Dictionary<PopupType, Popup> loadedPopups = new Dictionary<PopupType, Popup>();

        private void OnEnable()
        {
            EventBus.Subscribe<ShowPopupEvent>(OnShowPopup);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe<ShowPopupEvent>(OnShowPopup);
        }

        private void OnShowPopup(ShowPopupEvent evt)
        {
            if (loadedPopups.ContainsKey(evt.PopupType))
            {
                popupController.Show(loadedPopups[evt.PopupType]);
            }
            else
            {
                LoadPopupFromAddressables(evt.PopupType);
            }
        }

        private void LoadPopupFromAddressables(PopupType popupType)
        {
            AssetReferenceGameObject popupPrefabRef = popupLibrary.GetPopupPrefab(popupType);
            if (popupPrefabRef == null)
            {
                return;
            }

            popupPrefabRef.InstantiateAsync().Completed += (AsyncOperationHandle<GameObject> handle) =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    GameObject popupInstance = handle.Result;
                    Popup popup = popupInstance.GetComponent<Popup>();

                    if (popup == null)
                    {
                        Destroy(popupInstance);
                        return;
                    }

                    popupInstance.transform.SetParent(transform, false);
                    popupInstance.SetActive(false);
                    loadedPopups[popupType] = popup;

                    popupController.Show(popup);
                }
            };
        }
    }
}