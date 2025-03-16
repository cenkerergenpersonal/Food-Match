using System.Collections.Generic;
using UnityEngine;

namespace FoodMatch.Scripts.Data
{
    [CreateAssetMenu(fileName = "NewLevelConfig", menuName = "Tools/Level Config")]
    public class LevelConfig: ScriptableObject
    {
        public ItemSet levelItemSet;
        public List<int> items;
    }
}