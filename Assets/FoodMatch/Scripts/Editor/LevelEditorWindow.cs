using System.Collections.Generic;
using System.IO;
using FoodMatch.Scripts.Data;
using UnityEditor;
using UnityEngine;

namespace FoodMatch.Scripts.Editor
{
    public class LevelEditorWindow: EditorWindow
    {
        private LevelData currentLevel = new LevelData { TotalItemCount = 3 };
        private List<Goal> _goals = new List<Goal>();
        private LevelConfig selectedLevelConfig;
        private string levelDirectory;
        private string _fileName = "Level1";

        [MenuItem("Tools/Level Editor")]
        public static void ShowWindow()
        {
            GetWindow<LevelEditorWindow>("Level Editor");
        }

        private void OnEnable()
        {
            levelDirectory = Path.Combine(Application.persistentDataPath, "FoodMatch/Levels");
            if (!Directory.Exists(levelDirectory))
            {
                Directory.CreateDirectory(levelDirectory);
            }
            CheckLists();
        }
        private void CheckLists()
        {
            _goals ??= new List<Goal>();
        }
        private void OnGUI()
        {
            GUILayout.Label("Level Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            _fileName = EditorGUILayout.TextField("File Name", _fileName);

            currentLevel.LevelId = EditorGUILayout.IntField("Level ID", currentLevel.LevelId);
            currentLevel.LevelId = EditorGUILayout.IntField("Level ID", currentLevel.LevelId);

            selectedLevelConfig = (LevelConfig)EditorGUILayout.ObjectField("Level Config", selectedLevelConfig, typeof(LevelConfig), false);

            int newTotalCount = EditorGUILayout.IntField("Total Item Count", currentLevel.TotalItemCount);
            if (newTotalCount % 3 == 0 && newTotalCount > 0)
            {
                currentLevel.TotalItemCount = newTotalCount;
            }
            else
            {
                EditorGUILayout.HelpBox("Total Item Count must be a multiple of 3!", MessageType.Warning);
            }
            
            DrawGoals();

            EditorGUILayout.Space();
            GUILayout.Label("JSON", EditorStyles.boldLabel);

            if (GUILayout.Button("Load Level from JSON"))
            {
                LoadLevel();
            }

            if (GUILayout.Button("Save Level to JSON"))
            {
                SaveLevel();
            }

            if (GUILayout.Button("Open JSON Folder"))
            {
                Application.OpenURL(levelDirectory);
            }
        }
        
        private void DrawGoals()
        {
            GUILayout.Label("Goals", EditorStyles.boldLabel);
            int goalCount =_goals.Count;

            for (int i = 0; i < goalCount; i++)
            {
                if (i >= goalCount)
                {
                    continue;
                }

                GUILayout.BeginHorizontal();


                _goals[i].itemType = Mathf.Clamp(EditorGUILayout.IntField("Type", _goals[i].itemType), 0, 3);
                _goals[i].requiredCount = EditorGUILayout.IntField("Count", _goals[i].requiredCount);
                _goals[i].ItemSet = EditorGUILayout.IntField( "ItemSet", _goals[i].ItemSet);

                if (GUILayout.Button("X"))
                {
                    _goals.RemoveAt(i);
                    break;
                }

                GUILayout.EndHorizontal();
            }

            if (GUILayout.Button("Add Goal"))
            {
                _goals.Add(new Goal { itemType = 0, requiredCount = 1 });
            }
        }

        private void LoadLevel()
        {
            string path = $"Assets/FoodMatch/Levels/{_fileName}.json";
            if (!File.Exists(path))
            {
                Debug.LogError("Level file not found: " + _fileName);
                return;
            }

            string json = File.ReadAllText(path);
            currentLevel = JsonUtility.FromJson<LevelData>(json);
            _goals = currentLevel.Goals;
            selectedLevelConfig = Resources.Load<LevelConfig>($"Configs/{currentLevel.LevelId}");
            if (selectedLevelConfig == null)
            {
                Debug.LogError($"LevelConfig {currentLevel.LevelId} not found!");
            }
            else
            {
                Debug.Log($"Level {currentLevel.LevelId} loaded!");
            }

            Debug.Log("Level Loaded: " + _fileName);
        }

        private void SaveLevel()
        {
            currentLevel.Goals = _goals;
            if (selectedLevelConfig == null)
            {
                Debug.LogError("You must select a LevelConfig!");
                return;
            }
            if (currentLevel.TotalItemCount % 3 != 0)
            {
                Debug.LogError("Total item count must be a multiple of 3!");
                return;
            }

            string json = JsonUtility.ToJson(currentLevel, true);
            File.WriteAllText($"Assets/FoodMatch/Levels/{_fileName}.json", json);
            Debug.Log("Level Saved: " + _fileName);
        }
    }

}