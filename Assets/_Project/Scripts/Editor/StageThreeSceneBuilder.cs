using SLGLearn.Core;
using SLGLearn.Enemy;
using SLGLearn.Level;
using SLGLearn.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SLGLearn.EditorTools
{
    public static class StageThreeSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/Stage03_Combat.unity";

        [MenuItem("SLG Learn/Build Stage 03 Combat Scene")]
        public static void BuildScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateLighting();
            CreateRoad();
            var squad = CreatePlayerSquad();
            CreateCamera(squad.transform);

            CreateGatePair(16f, GateOperation.Add, 5, GateOperation.Multiply, 2);
            CreateEnemyLine(28f, 3, 3f);
            CreateGatePair(42f, GateOperation.Subtract, 3, GateOperation.Add, 8);
            CreateEnemyLine(56f, 5, 5f);
            CreateEnemyLine(72f, 7, 7f);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath);

            Debug.Log($"Stage 03 combat scene created: {ScenePath}");
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Directional Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void CreateRoad()
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "Road";
            road.transform.position = new Vector3(0f, -0.05f, 55f);
            road.transform.localScale = new Vector3(10f, 0.1f, 120f);

            CreateBoundary("Left Boundary", -5.25f);
            CreateBoundary("Right Boundary", 5.25f);
        }

        private static void CreateBoundary(string name, float x)
        {
            var boundary = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boundary.name = name;
            boundary.transform.position = new Vector3(x, 0.25f, 55f);
            boundary.transform.localScale = new Vector3(0.2f, 0.5f, 120f);
        }

        private static GameObject CreatePlayerSquad()
        {
            var squad = new GameObject("PlayerSquad");
            squad.transform.position = new Vector3(0f, 0.5f, 0f);

            squad.AddComponent<SquadController>();
            squad.AddComponent<SquadManager>();
            squad.AddComponent<SquadDebugInput>();

            return squad;
        }

        private static void CreateCamera(Transform target)
        {
            var cameraObject = new GameObject("Main Camera");
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 7f, -8f);
            cameraObject.transform.rotation = Quaternion.Euler(42f, 0f, 0f);

            var camera = cameraObject.AddComponent<Camera>();
            camera.fieldOfView = 55f;
            camera.nearClipPlane = 0.1f;
            camera.farClipPlane = 300f;

            cameraObject.AddComponent<AudioListener>();
            cameraObject.AddComponent<CameraFollow>().SetTarget(target);
        }

        private static void CreateGatePair(
            float z,
            GateOperation leftOperation,
            int leftValue,
            GateOperation rightOperation,
            int rightValue)
        {
            CreateGate(new Vector3(-2.2f, 1f, z), leftOperation, leftValue);
            CreateGate(new Vector3(2.2f, 1f, z), rightOperation, rightValue);
        }

        private static void CreateGate(Vector3 position, GateOperation operation, int value)
        {
            var gate = new GameObject($"Gate_{operation}_{value}_{position.z:00}");
            gate.transform.position = position;

            var body = gate.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;

            var trigger = gate.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(2.4f, 2.4f, 0.5f);

            CreateGatePanel(gate.transform, operation);
            var label = CreateGateLabel(gate.transform, operation, value);

            gate.AddComponent<SquadGate>().Configure(operation, value, label);
        }

        private static void CreateGatePanel(Transform parent, GateOperation operation)
        {
            var panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.name = "Panel";
            panel.transform.SetParent(parent);
            panel.transform.localPosition = Vector3.zero;
            panel.transform.localRotation = Quaternion.identity;
            panel.transform.localScale = new Vector3(2.2f, 2.2f, 0.15f);

            var renderer = panel.GetComponent<Renderer>();
            renderer.sharedMaterial = CreateMaterial(operation == GateOperation.Subtract
                ? new Color(0.85f, 0.2f, 0.2f)
                : new Color(0.1f, 0.55f, 0.95f));

            Object.DestroyImmediate(panel.GetComponent<Collider>());
        }

        private static TextMesh CreateGateLabel(Transform parent, GateOperation operation, int value)
        {
            var labelObject = new GameObject("Label");
            labelObject.transform.SetParent(parent);
            labelObject.transform.localPosition = new Vector3(0f, 0f, -0.11f);
            labelObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            var label = labelObject.AddComponent<TextMesh>();
            label.text = GetDisplayText(operation, value);
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = 0.5f;
            label.fontSize = 96;
            label.color = Color.white;
            return label;
        }

        private static void CreateEnemyLine(float z, int count, float health)
        {
            var parent = new GameObject($"EnemyLine_{z:00}");
            var spacing = 1.2f;
            var width = (count - 1) * spacing;

            for (var i = 0; i < count; i++)
            {
                var x = i * spacing - width * 0.5f;
                CreateEnemy(parent.transform, new Vector3(x, 0.6f, z), health);
            }
        }

        private static void CreateEnemy(Transform parent, Vector3 position, float health)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.transform.SetParent(parent);
            enemy.transform.position = position;
            enemy.transform.localScale = new Vector3(0.7f, 1f, 0.7f);

            var renderer = enemy.GetComponent<Renderer>();
            renderer.sharedMaterial = CreateMaterial(new Color(0.85f, 0.25f, 0.2f));

            var enemyHealth = enemy.AddComponent<EnemyHealth>();
            var serializedObject = new SerializedObject(enemyHealth);
            serializedObject.FindProperty("maxHealth").floatValue = health;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }

        private static Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            var material = new Material(shader);
            material.color = color;
            return material;
        }

        private static string GetDisplayText(GateOperation operation, int value)
        {
            return operation switch
            {
                GateOperation.Add => $"+{value}",
                GateOperation.Subtract => $"-{value}",
                GateOperation.Multiply => $"x{value}",
                _ => value.ToString()
            };
        }
    }
}
