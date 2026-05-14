using SLGLearn.Data;
using UnityEngine;

namespace SLGLearn.Level
{
    public static class RuntimePrimitiveFactory
    {
        private static VisualConfig visuals;

        public static Color BulletColor => visuals != null ? visuals.BulletColor : new Color(1f, 0.85f, 0.2f);
        public static Color DamageNumberColor => visuals != null ? visuals.DamageNumberColor : new Color(1f, 0.25f, 0.15f);
        public static Color HitEffectColor => visuals != null ? visuals.HitEffectColor : new Color(1f, 0.45f, 0.1f);
        public static float BulletSize => visuals != null ? visuals.BulletSize : 0.22f;
        public static float HitEffectStartSize => visuals != null ? visuals.HitEffectStartSize : 0.25f;
        public static float DamageNumberCharacterSize => visuals != null ? visuals.DamageNumberCharacterSize : 0.28f;
        public static int DamageNumberFontSize => visuals != null ? visuals.DamageNumberFontSize : 72;
        public static Color EnemyColor => visuals != null ? visuals.EnemyColor : new Color(0.85f, 0.25f, 0.2f);
        public static Color BossColor => visuals != null ? visuals.BossColor : new Color(0.45f, 0.15f, 0.85f);
        public static Color SoldierColor => visuals != null ? visuals.SoldierColor : new Color(0.2f, 0.65f, 0.95f);
        public static Color PositiveGateColor => visuals != null ? visuals.PositiveGateColor : new Color(0.1f, 0.55f, 0.95f);
        public static Color NegativeGateColor => visuals != null ? visuals.NegativeGateColor : new Color(0.85f, 0.2f, 0.2f);
        public static float GateLabelCharacterSize => visuals != null ? visuals.GateLabelCharacterSize : 0.5f;
        public static int GateLabelFontSize => visuals != null ? visuals.GateLabelFontSize : 96;
        public static Color ResultPanelColor => visuals != null ? visuals.ResultPanelColor : new Color(0f, 0f, 0f, 0.72f);
        public static Color RestartButtonColor => visuals != null ? visuals.RestartButtonColor : new Color(0.15f, 0.45f, 0.95f);
        public static Color HudTextColor => visuals != null ? visuals.HudTextColor : Color.white;
        public static GameObject BulletPrefab => visuals != null ? visuals.BulletPrefab : null;
        public static GameObject HitEffectPrefab => visuals != null ? visuals.HitEffectPrefab : null;
        public static GameObject EnemyPrefab => visuals != null ? visuals.EnemyPrefab : null;
        public static GameObject BossPrefab => visuals != null ? visuals.BossPrefab : null;
        public static GameObject SoldierPrefab => visuals != null ? visuals.SoldierPrefab : null;

        public static void Configure(VisualConfig visualConfig)
        {
            visuals = visualConfig;
        }

        public static Material CreateMaterial(Color color)
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

        public static GameObject InstantiatePrefabOrPrimitive(GameObject prefab, PrimitiveType fallbackPrimitive, string objectName, Transform parent)
        {
            GameObject instance;
            if (prefab != null)
            {
                instance = Object.Instantiate(prefab, parent);
                instance.name = objectName;
                return instance;
            }

            instance = GameObject.CreatePrimitive(fallbackPrimitive);
            instance.name = objectName;
            instance.transform.SetParent(parent);
            return instance;
        }

        public static T GetOrAdd<T>(GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>();
            return component != null ? component : gameObject.AddComponent<T>();
        }

        public static void DisableAndDestroyCollider(GameObject gameObject)
        {
            var collider = gameObject.GetComponent<Collider>();
            if (collider == null)
            {
                return;
            }

            collider.enabled = false;
            Object.Destroy(collider);
        }
    }
}
