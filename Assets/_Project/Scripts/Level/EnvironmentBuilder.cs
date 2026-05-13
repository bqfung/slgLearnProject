using SLGLearn.Data;
using UnityEngine;

namespace SLGLearn.Level
{
    public sealed class EnvironmentBuilder
    {
        public void Build(LevelConfig config)
        {
            CreateLighting();
            CreateRoad(config);
        }

        private static void CreateLighting()
        {
            var lightObject = new GameObject("Directional Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void CreateRoad(LevelConfig config)
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "Road";
            road.transform.position = new Vector3(0f, -0.05f, config.RoadLength * 0.5f - 5f);
            road.transform.localScale = new Vector3(config.RoadWidth, 0.1f, config.RoadLength);

            var boundaryX = config.RoadWidth * 0.5f + 0.25f;
            CreateBoundary("Left Boundary", -boundaryX, config.RoadLength);
            CreateBoundary("Right Boundary", boundaryX, config.RoadLength);
        }

        private static void CreateBoundary(string name, float x, float roadLength)
        {
            var boundary = GameObject.CreatePrimitive(PrimitiveType.Cube);
            boundary.name = name;
            boundary.transform.position = new Vector3(x, 0.25f, roadLength * 0.5f - 5f);
            boundary.transform.localScale = new Vector3(0.2f, 0.5f, roadLength);
        }
    }
}
