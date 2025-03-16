using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace FoodMatch.Scripts.Data
{
    [CreateAssetMenu(fileName = "PopupData", menuName = "Game/Popup Data")]
    public class PopupData : ScriptableObject
    {
        [Serializable]
        public class PopupEntry
        {
            public PopupType popupType;
            public AssetReferenceGameObject popupPrefab;
        }

        public List<PopupEntry> popups = new List<PopupEntry>();

        public AssetReferenceGameObject GetPopupPrefab(PopupType popupType)
        {
            foreach (var entry in popups)
            {
                if (entry.popupType == popupType)
                    return entry.popupPrefab;
            }
            return null;
        }
    }
}