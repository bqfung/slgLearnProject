using System.Collections.Generic;
using SLGLearn.Data;
using UnityEditor;
using UnityEngine;

namespace SLGLearn.EditorTools
{
    [CustomEditor(typeof(LevelConfig))]
    public sealed class LevelConfigEditor : Editor
    {
        private IReadOnlyList<LevelConfigIssue> lastIssues = new List<LevelConfigIssue>();
        private LevelValidationSettings validationSettings;

        private void OnEnable()
        {
            validationSettings = LevelValidationSettingsProvider.LoadOrCreateDefault();
        }

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            EditorGUILayout.Space(12f);
            EditorGUILayout.LabelField("Level Design Tools", EditorStyles.boldLabel);
            validationSettings = (LevelValidationSettings)EditorGUILayout.ObjectField(
                "Validation Settings",
                validationSettings,
                typeof(LevelValidationSettings),
                false);

            if (GUILayout.Button("Validate Config"))
            {
                Validate();
            }

            if (GUILayout.Button("Generate Preview Scene For This Config"))
            {
                GeneratePreviewScene();
            }

            DrawValidationResult();
        }

        private void Validate()
        {
            var config = (LevelConfig)target;
            lastIssues = LevelConfigValidator.Validate(config, validationSettings);
        }

        private void GeneratePreviewScene()
        {
            var config = (LevelConfig)target;
            lastIssues = LevelConfigValidator.Validate(config, validationSettings);

            if (LevelConfigValidator.HasErrors(lastIssues))
            {
                var shouldContinue = EditorUtility.DisplayDialog(
                    "Validation Errors",
                    "This config has validation errors:\n\n" + FormatIssues(lastIssues, LevelConfigIssueSeverity.Error) + "\n\nGenerate the preview scene anyway?",
                    "Generate Anyway",
                    "Cancel");

                if (!shouldContinue)
                {
                    return;
                }
            }

            StageSixSceneBuilder.BuildScene(config);
        }

        private void DrawValidationResult()
        {
            if (lastIssues == null || lastIssues.Count == 0)
            {
                EditorGUILayout.HelpBox("No validation issues.", MessageType.Info);
                return;
            }

            foreach (var issue in lastIssues)
            {
                var messageType = issue.Severity == LevelConfigIssueSeverity.Error
                    ? MessageType.Error
                    : MessageType.Warning;

                EditorGUILayout.HelpBox(FormatIssue(issue), messageType);
            }
        }

        private static string FormatIssues(IReadOnlyList<LevelConfigIssue> issues, LevelConfigIssueSeverity severity)
        {
            var messages = new List<string>();
            for (var i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    messages.Add("- " + FormatIssue(issues[i]));
                }
            }

            return string.Join("\n", messages);
        }

        private static string FormatIssue(LevelConfigIssue issue)
        {
            return string.IsNullOrEmpty(issue.PropertyPath)
                ? issue.Message
                : $"{issue.PropertyPath}: {issue.Message}";
        }
    }
}
