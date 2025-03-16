using System;
using System.Collections.Generic;

namespace FoodMatch.Scripts.Data
{
    [Serializable]
    public class LevelData
    {
        public int LevelId;
        public int TotalItemCount;
        public float TotalTimeFromSeconds;
        public List<Goal> Goals;
    }
    
    [Serializable]
    public class Goal
    {
        public int ItemSet;
        public int itemType;
        public int requiredCount;
    }
}