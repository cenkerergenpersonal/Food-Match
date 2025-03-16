using System.Collections.Generic;
using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Game;

namespace FoodMatch.Scripts.Events
{
    public class MatchFoundEvent: IEvent
    {
        public HashSet<Item> MatchedItems;

        public MatchFoundEvent(HashSet<Item> matchedItems)
        {
            MatchedItems = matchedItems;
        }
    }
}