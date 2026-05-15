using SLGLearn.Data;
using SLGLearn.Level;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SLGLearn.EditorTools
{
    public static class StageSixSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/Stage06_DataDriven.unity";
        private const string ConfigPath = "Assets/_Project/ScriptableObjects/Stage06_LevelConfig.asset";
        private const string VisualConfigPath = "Assets/_Project/ScriptableObjects/Stage06_VisualConfig.asset";

        [MenuItem("SLG Learn/Build Stage 06 Data Driven Scene")]
        public static void BuildScene()
        {
            var config = LoadOrCreateConfig();
            BuildScene(config, ScenePath);
        }

        public static void BuildScene(LevelConfig config)
        {
            if (config == null)
            {
                Debug.LogError("Cannot build data driven scene because LevelConfig is missing.");
                return;
            }

            var configPath = AssetDatabase.GetAssetPath(config);
            var configName = string.IsNullOrEmpty(configPath)
                ? config.name
                : System.IO.Path.GetFileNameWithoutExtension(configPath);
            var previewPath = $"Assets/_Project/Scenes/Preview_{configName}.unity";
            BuildScene(config, previewPath);
        }

        private static void BuildScene(LevelConfig config, string scenePath)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var builderObject = new GameObject("LevelBuilder");
            builderObject.AddComponent<LevelBuilder>().Configure(config, true);

            EditorSceneManager.SaveScene(scene, scenePath);
            EditorSceneManager.OpenScene(scenePath);

            Debug.Log($"Data driven scene created: {scenePath}");
        }

        private static LevelConfig LoadOrCreateConfig()
        {
            var visualConfig = LoadOrCreateVisualConfig();
            var config = AssetDatabase.LoadAssetAtPath<LevelConfig>(ConfigPath);
            if (config != null)
            {
                EnsureVisualConfig(config, visualConfig);
                return config;
            }

            config = ScriptableObject.CreateInstance<LevelConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
            WriteDefaultConfig(config, visualConfig);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return config;
        }

        private static VisualConfig LoadOrCreateVisualConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<VisualConfig>(VisualConfigPath);
            if (config != null)
            {
                return config;
            }

            config = ScriptableObject.CreateInstance<VisualConfig>();
            AssetDatabase.CreateAsset(config, VisualConfigPath);
            EditorUtility.SetDirty(config);
            return config;
        }

        private static void EnsureVisualConfig(LevelConfig config, VisualConfig visualConfig)
        {
            var serializedConfig = new SerializedObject(config);
            var visualProperty = serializedConfig.FindProperty("visualConfig");
            if (visualProperty.objectReferenceValue == null)
            {
                visualProperty.objectReferenceValue = visualConfig;
                serializedConfig.ApplyModifiedPropertiesWithoutUndo();
                EditorUtility.SetDirty(config);
                AssetDatabase.SaveAssets();
            }
        }

        private static void WriteDefaultConfig(LevelConfig config, VisualConfig visualConfig)
        {
            var serializedConfig = new SerializedObject(config);

            serializedConfig.FindProperty("roadLength").floatValue = 130f;
            serializedConfig.FindProperty("roadWidth").floatValue = 10f;

            var gates = serializedConfig.FindProperty("gates");
            gates.arraySize = 3;
            WriteGate(gates.GetArrayElementAtIndex(0), 14f, GateOperation.Add, 5, GateOperation.Multiply, 2);
            WriteGate(gates.GetArrayElementAtIndex(1), 40f, GateOperation.Add, 8, GateOperation.Subtract, 3);
            WriteGate(gates.GetArrayElementAtIndex(2), 66f, GateOperation.Multiply, 2, GateOperation.Add, 6);

            var waves = serializedConfig.FindProperty("enemyWaves");
            waves.arraySize = 3;
            WriteWave(waves.GetArrayElementAtIndex(0), 28f, 3, 3f, 2f, 1.15f, 0.6f, 1);
            WriteWave(waves.GetArrayElementAtIndex(1), 54f, 5, 5f, 2.4f, 1.15f, 0.6f, 1);
            WriteWave(waves.GetArrayElementAtIndex(2), 78f, 7, 7f, 2.8f, 1.15f, 0.6f, 1);

            var boss = serializedConfig.FindProperty("boss");
            boss.FindPropertyRelative("position").vector3Value = new Vector3(0f, 1.4f, 96f);
            boss.FindPropertyRelative("health").floatValue = 60f;
            boss.FindPropertyRelative("moveSpeed").floatValue = 1.1f;
            boss.FindPropertyRelative("attackRange").floatValue = 2.2f;
            boss.FindPropertyRelative("attacksPerSecond").floatValue = 0.8f;
            boss.FindPropertyRelative("damageMembers").intValue = 2;

            serializedConfig.FindProperty("visualConfig").objectReferenceValue = visualConfig;

            serializedConfig.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
        }

        private static void WriteGate(
            SerializedProperty gate,
            float z,
            GateOperation leftOperation,
            int leftValue,
            GateOperation rightOperation,
            int rightValue)
        {
            gate.FindPropertyRelative("z").floatValue = z;
            gate.FindPropertyRelative("leftOperation").enumValueIndex = (int)leftOperation;
            gate.FindPropertyRelative("leftValue").intValue = leftValue;
            gate.FindPropertyRelative("rightOperation").enumValueIndex = (int)rightOperation;
            gate.FindPropertyRelative("rightValue").intValue = rightValue;
        }

        private static void WriteWave(
            SerializedProperty wave,
            float z,
            int count,
            float health,
            float moveSpeed,
            float attackRange,
            float attacksPerSecond,
            int damageMembers)
        {
            wave.FindPropertyRelative("z").floatValue = z;
            wave.FindPropertyRelative("count").intValue = count;
            wave.FindPropertyRelative("health").floatValue = health;
            wave.FindPropertyRelative("moveSpeed").floatValue = moveSpeed;
            wave.FindPropertyRelative("attackRange").floatValue = attackRange;
            wave.FindPropertyRelative("attacksPerSecond").floatValue = attacksPerSecond;
            wave.FindPropertyRelative("damageMembers").intValue = damageMembers;
        }
    }
}
