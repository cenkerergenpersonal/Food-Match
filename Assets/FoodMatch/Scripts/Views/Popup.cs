using FoodMatch.Scripts.Data;
using UnityEngine;

namespace FoodMatch.Scripts.Views
{
    public class Popup : MonoBehaviour
    {
        public PopupType PopupType;

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}