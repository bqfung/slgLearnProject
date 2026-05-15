using SLGLearn.Data;
using UnityEditor;
using UnityEngine;

namespace SLGLearn.EditorTools
{
    public static class LevelValidationSettingsProvider
    {
        public const string DefaultSettingsPath = "Assets/_Project/ScriptableObjects/Stage07_LevelValidationSettings.asset";

        public static LevelValidationSettings LoadOrCreateDefault()
        {
            var settings = AssetDatabase.LoadAssetAtPath<LevelValidationSettings>(DefaultSettingsPath);
            if (settings != null)
            {
                return settings;
            }

            settings = ScriptableObject.CreateInstance<LevelValidationSettings>();
            AssetDatabase.CreateAsset(settings, DefaultSettingsPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return settings;
        }
    }
}
