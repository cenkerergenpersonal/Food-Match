using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;

namespace FoodMatch.Scripts.Events
{
    public class ShowPopupEvent: IEvent
    {
        public PopupType PopupType;

        public ShowPopupEvent(PopupType popupType)
        {
            PopupType = popupType;
        }
    }
}