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

        [MenuItem("SLG Learn/Build Stage 06 Data Driven Scene")]
        public static void BuildScene()
        {
            var config = LoadOrCreateConfig();
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            var builderObject = new GameObject("LevelBuilder");
            builderObject.AddComponent<LevelBuilder>().Configure(config, true);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath);

            Debug.Log($"Stage 06 data driven scene created: {ScenePath}");
        }

        private static LevelConfig LoadOrCreateConfig()
        {
            var config = AssetDatabase.LoadAssetAtPath<LevelConfig>(ConfigPath);
            if (config != null)
            {
                return config;
            }

            config = ScriptableObject.CreateInstance<LevelConfig>();
            AssetDatabase.CreateAsset(config, ConfigPath);
            WriteDefaultConfig(config);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return config;
        }

        private static void WriteDefaultConfig(LevelConfig config)
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
