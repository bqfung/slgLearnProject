using SLGLearn.Core;
using SLGLearn.Player;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SLGLearn.EditorTools
{
    public static class StageOneSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/Stage01_Movement.unity";

        [MenuItem("SLG Learn/Build Stage 01 Movement Scene")]
        public static void BuildScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            CreateLighting();
            CreateRoad();
            var squad = CreatePlayerSquad();
            CreateCamera(squad.transform);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath);

            Debug.Log($"Stage 01 movement scene created: {ScenePath}");
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
            road.transform.position = new Vector3(0f, -0.05f, 45f);
            road.transform.localScale = new Vector3(10f, 0.1f, 100f);

            var leftWall = CreateBoundary("Left Boundary", -5.25f);
            var rightWall = CreateBoundary("Right Boundary", 5.25f);

            leftWall.transform.position = new Vector3(-5.25f, 0.25f, 45f);
            rightWall.transform.position = new Vector3(5.25f, 0.25f, 45f);
        }

        private static GameObject CreateBoundary(string name, float x)
        {
            var boundary = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boundary.name = name;
            boundary.transform.position = new Vector3(x, 0.25f, 45f);
            boundary.transform.localScale = new Vector3(0.2f, 0.5f, 100f);
            return boundary;
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

            var listener = cameraObject.AddComponent<AudioListener>();
            listener.enabled = true;

            var follow = cameraObject.AddComponent<CameraFollow>();
            follow.SetTarget(target);
        }
    }
}
