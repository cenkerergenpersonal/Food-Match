using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Events;

namespace FoodMatch.Scripts.Views
{
    public class StartPopup: Popup
    {
        public void OnPlayButtonClicked()
        {
            EventBus.Publish(new StartGameEvent("Level1"));
            Hide();
        }
    }
}