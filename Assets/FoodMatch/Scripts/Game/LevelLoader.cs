using System.IO;
using FoodMatch.Scripts.Data;
using UnityEngine;

namespace FoodMatch.Scripts.Game
{
    public class LevelLoader
    {
        private const string LevelPath = "Assets/FoodMatch/Levels/";

        public LevelData LoadLevel(string fileName)
        {
            string path = Path.Combine(LevelPath, $"{fileName}.json");
            if (!File.Exists(path))
            {
                Debug.LogError("Level file not found: " + fileName + $"from file {path}");
                return null;
            }

            string json = File.ReadAllText(path);
            LevelData level = JsonUtility.FromJson<LevelData>(json);

            Debug.Log($"Loaded Level: Moves {level.Goals.Count}, Grid {level.LevelId}x{level.TotalItemCount}");
            return level;
        }
    }
}