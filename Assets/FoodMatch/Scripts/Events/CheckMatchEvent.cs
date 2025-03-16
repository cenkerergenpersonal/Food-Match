using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Game;

namespace FoodMatch.Scripts.Events
{
    public class CheckMatchEvent : IEvent
    {
        public Item Item;
        public bool UseMove;

        public CheckMatchEvent(Item item)
        {
            Item = item;
        }
    }
}