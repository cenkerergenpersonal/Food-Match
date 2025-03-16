using FoodMatch.Scripts.Common;
using FoodMatch.Scripts.Data;

namespace FoodMatch.Scripts.Events
{
    public class UpdateGoalEvent : IEvent
    {
        public Goal Goal;
        public int CurrentCount;

        public UpdateGoalEvent(Goal goal, int currentCount)
        {
            Goal = goal;
            CurrentCount = currentCount;
        }
    }
}