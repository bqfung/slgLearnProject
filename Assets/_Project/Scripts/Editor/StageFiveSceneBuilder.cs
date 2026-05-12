using SLGLearn.Core;
using SLGLearn.Enemy;
using SLGLearn.Level;
using SLGLearn.Player;
using SLGLearn.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace SLGLearn.EditorTools
{
    public static class StageFiveSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/Stage05_UIFlow.unity";

        [MenuItem("SLG Learn/Build Stage 05 UI Flow Scene")]
        public static void BuildScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateLighting();
            CreateRoad();
            var squad = CreatePlayerSquad();
            CreateCamera(squad.transform);

            CreateGatePair(14f, GateOperation.Add, 5, GateOperation.Multiply, 2);
            CreateEnemyWave(squad.GetComponent<SquadManager>(), 28f, 3, 3f, 2f);
            CreateGatePair(40f, GateOperation.Add, 8, GateOperation.Subtract, 3);
            CreateEnemyWave(squad.GetComponent<SquadManager>(), 54f, 5, 5f, 2.4f);
            CreateGatePair(66f, GateOperation.Multiply, 2, GateOperation.Add, 6);
            CreateEnemyWave(squad.GetComponent<SquadManager>(), 78f, 7, 7f, 2.8f);

            var boss = CreateBoss(squad.GetComponent<SquadManager>(), new Vector3(0f, 1.4f, 96f));
            var outcome = CreateOutcomeController(squad, boss);
            CreateBattleUi(squad.GetComponent<SquadManager>(), boss, outcome);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath);

            Debug.Log($"Stage 05 UI flow scene created: {ScenePath}");
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
            road.transform.position = new Vector3(0f, -0.05f, 60f);
            road.transform.localScale = new Vector3(10f, 0.1f, 130f);

            CreateBoundary("Left Boundary", -5.25f);
            CreateBoundary("Right Boundary", 5.25f);
        }

        private static void CreateBoundary(string name, float x)
        {
            var boundary = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boundary.name = name;
            boundary.transform.position = new Vector3(x, 0.25f, 60f);
            boundary.transform.localScale = new Vector3(0.2f, 0.5f, 130f);
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

            panel.GetComponent<Renderer>().sharedMaterial = CreateMaterial(operation == GateOperation.Subtract
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

        private static void CreateEnemyWave(SquadManager squad, float z, int count, float health, float speed)
        {
            var parent = new GameObject($"EnemyWave_{z:00}");
            var spacing = 1.2f;
            var width = (count - 1) * spacing;

            for (var i = 0; i < count; i++)
            {
                var x = i * spacing - width * 0.5f;
                CreateEnemy(parent.transform, squad, new Vector3(x, 0.6f, z), health, speed);
            }
        }

        private static void CreateEnemy(Transform parent, SquadManager squad, Vector3 position, float health, float speed)
        {
            var enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemy.name = "Enemy";
            enemy.transform.SetParent(parent);
            enemy.transform.position = position;
            enemy.transform.localScale = new Vector3(0.7f, 1f, 0.7f);

            enemy.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.85f, 0.25f, 0.2f));
            enemy.AddComponent<EnemyHealth>().Configure(health);
            enemy.AddComponent<EnemyMeleeAttacker>().Configure(squad, speed, 1.15f, 0.6f, 1);
        }

        private static EnemyHealth CreateBoss(SquadManager squad, Vector3 position)
        {
            var boss = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            boss.name = "Boss";
            boss.transform.position = position;
            boss.transform.localScale = new Vector3(2.2f, 2.8f, 2.2f);
            boss.GetComponent<Renderer>().sharedMaterial = CreateMaterial(new Color(0.45f, 0.15f, 0.85f));

            var health = boss.AddComponent<EnemyHealth>();
            health.Configure(60f);

            boss.AddComponent<EnemyMeleeAttacker>().Configure(squad, 1.1f, 2.2f, 0.8f, 2);
            return health;
        }

        private static GameOutcomeController CreateOutcomeController(GameObject squad, EnemyHealth boss)
        {
            var outcomeObject = new GameObject("GameOutcomeController");
            var controller = outcomeObject.AddComponent<GameOutcomeController>();
            controller.Configure(squad.GetComponent<SquadManager>(), squad.GetComponent<SquadController>(), boss, null);
            return controller;
        }

        private static void CreateBattleUi(SquadManager squad, EnemyHealth boss, GameOutcomeController outcome)
        {
            var canvasObject = new GameObject("BattleCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();

            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var squadText = CreateText(canvasObject.transform, "SquadText", font, "Squad: 0", new Vector2(24f, -24f), TextAnchor.UpperLeft);
            var bossText = CreateText(canvasObject.transform, "BossText", font, "Boss: 0 / 0", new Vector2(24f, -64f), TextAnchor.UpperLeft);
            var resultPanel = CreateResultPanel(canvasObject.transform);
            var resultText = CreateText(resultPanel.transform, "ResultText", font, string.Empty, new Vector2(0f, 52f), TextAnchor.MiddleCenter);
            var restartButton = CreateRestartButton(resultPanel.transform, font);

            var hud = canvasObject.AddComponent<BattleHudController>();
            hud.Configure(squad, boss, outcome, squadText, bossText, resultPanel, resultText, restartButton);

            CreateEventSystem();
        }

        private static GameObject CreateResultPanel(Transform parent)
        {
            var panel = new GameObject("ResultPanel");
            panel.transform.SetParent(parent);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(360f, 220f);
            rect.anchoredPosition = Vector2.zero;

            var image = panel.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.72f);

            return panel;
        }

        private static Button CreateRestartButton(Transform parent, Font font)
        {
            var buttonObject = new GameObject("RestartButton");
            buttonObject.transform.SetParent(parent);

            var rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(180f, 48f);
            rect.anchoredPosition = new Vector2(0f, -42f);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.15f, 0.45f, 0.95f);

            var button = buttonObject.AddComponent<Button>();
            CreateText(buttonObject.transform, "Label", font, "Restart", Vector2.zero, TextAnchor.MiddleCenter);
            return button;
        }

        private static Text CreateText(Transform parent, string name, Font font, string value, Vector2 position, TextAnchor anchor)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent);

            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = anchor == TextAnchor.UpperLeft ? new Vector2(0f, 1f) : new Vector2(0.5f, 0.5f);
            rect.anchorMax = rect.anchorMin;
            rect.sizeDelta = anchor == TextAnchor.UpperLeft ? new Vector2(280f, 36f) : new Vector2(280f, 56f);
            rect.anchoredPosition = position;

            var text = textObject.AddComponent<Text>();
            text.text = value;
            text.font = font;
            text.fontSize = anchor == TextAnchor.UpperLeft ? 24 : 42;
            text.alignment = anchor;
            text.color = Color.white;
            return text;
        }

        private static void CreateEventSystem()
        {
            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
#else
            eventSystemObject.AddComponent<StandaloneInputModule>();
#endif
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
