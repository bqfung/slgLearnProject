using System.Collections.Generic;
using SLGLearn.Data;

namespace SLGLearn.EditorTools
{
    public enum LevelConfigIssueSeverity
    {
        Warning,
        Error
    }

    public sealed class LevelConfigIssue
    {
        public LevelConfigIssue(LevelConfigIssueSeverity severity, string message, string propertyPath = "")
        {
            Severity = severity;
            Message = message;
            PropertyPath = propertyPath;
        }

        public LevelConfigIssueSeverity Severity { get; }
        public string Message { get; }
        public string PropertyPath { get; }
    }

    public static class LevelConfigValidator
    {
        public static IReadOnlyList<LevelConfigIssue> Validate(LevelConfig config)
        {
            return Validate(config, null);
        }

        public static IReadOnlyList<LevelConfigIssue> Validate(LevelConfig config, LevelValidationSettings settings)
        {
            var issues = new List<LevelConfigIssue>();

            if (config == null)
            {
                AddError(issues, "LevelConfig is missing.");
                return issues;
            }

            if (config.RoadLength <= 0f)
            {
                AddError(issues, "Road length must be greater than 0.", "roadLength");
            }

            if (config.RoadWidth <= 0f)
            {
                AddError(issues, "Road width must be greater than 0.", "roadWidth");
            }

            ValidateGates(config, issues);
            ValidateWaves(config, issues);
            ValidateBoss(config, issues);
            ValidateSpacing(config, settings, issues);

            if (config.Visuals == null)
            {
                AddWarning(issues, "VisualConfig is not assigned. Runtime visuals will use code fallbacks.", "visualConfig");
            }

            return issues;
        }

        public static bool HasErrors(IReadOnlyList<LevelConfigIssue> issues)
        {
            for (var i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == LevelConfigIssueSeverity.Error)
                {
                    return true;
                }
            }

            return false;
        }

        private static void ValidateGates(LevelConfig config, ICollection<LevelConfigIssue> issues)
        {
            for (var i = 0; i < config.Gates.Count; i++)
            {
                var gate = config.Gates[i];
                if (gate == null)
                {
                    AddError(issues, $"Gate {i} is null.", GatePath(i));
                    continue;
                }

                if (gate.Z < 0f)
                {
                    AddError(issues, $"Gate {i} has negative Z position.", GatePath(i, "z"));
                }

                if (gate.Z > config.RoadLength)
                {
                    AddError(issues, $"Gate {i} is beyond road length.", GatePath(i, "z"));
                }

                if (gate.LeftValue <= 0 || gate.RightValue <= 0)
                {
                    AddError(issues, $"Gate {i} values must be greater than 0.", GatePath(i));
                }
            }
        }

        private static void ValidateWaves(LevelConfig config, ICollection<LevelConfigIssue> issues)
        {
            var previousZ = -1f;
            for (var i = 0; i < config.EnemyWaves.Count; i++)
            {
                var wave = config.EnemyWaves[i];
                if (wave == null)
                {
                    AddError(issues, $"Enemy wave {i} is null.", WavePath(i));
                    continue;
                }

                if (wave.Z < 0f)
                {
                    AddError(issues, $"Enemy wave {i} has negative Z position.", WavePath(i, "z"));
                }

                if (wave.Z > config.RoadLength)
                {
                    AddError(issues, $"Enemy wave {i} is beyond road length.", WavePath(i, "z"));
                }

                if (wave.Z < previousZ)
                {
                    AddError(issues, $"Enemy wave {i} appears before the previous wave. Keep waves ordered by Z.", WavePath(i, "z"));
                }

                if (wave.Count <= 0)
                {
                    AddError(issues, $"Enemy wave {i} count must be greater than 0.", WavePath(i, "count"));
                }

                if (wave.Health <= 0f)
                {
                    AddError(issues, $"Enemy wave {i} health must be greater than 0.", WavePath(i, "health"));
                }

                previousZ = wave.Z;
            }
        }

        private static void ValidateBoss(LevelConfig config, ICollection<LevelConfigIssue> issues)
        {
            if (config.Boss == null)
            {
                AddError(issues, "Boss config is missing.", "boss");
                return;
            }

            var bossZ = config.Boss.Position.z;
            if (bossZ < 0f)
            {
                AddError(issues, "Boss Z position must be greater than or equal to 0.", "boss.position");
            }

            if (bossZ > config.RoadLength)
            {
                AddError(issues, "Boss is beyond road length.", "boss.position");
            }

            if (config.Boss.Health <= 0f)
            {
                AddError(issues, "Boss health must be greater than 0.", "boss.health");
            }

            if (config.EnemyWaves.Count > 0)
            {
                var lastWave = config.EnemyWaves[^1];
                if (lastWave != null && bossZ <= lastWave.Z)
                {
                    AddError(issues, "Boss should be placed after the last enemy wave.", "boss.position");
                }
            }
        }

        private static void ValidateSpacing(LevelConfig config, LevelValidationSettings settings, ICollection<LevelConfigIssue> issues)
        {
            ValidateGateSpacing(config, settings, issues);
            ValidateWaveSpacing(config, settings, issues);
            ValidateGateWaveSpacing(config, settings, issues);
            ValidateBossSpacing(config, settings, issues);
        }

        private static void ValidateGateSpacing(LevelConfig config, LevelValidationSettings settings, ICollection<LevelConfigIssue> issues)
        {
            var minSpacing = settings == null ? 8f : settings.MinGateSpacing;
            for (var i = 1; i < config.Gates.Count; i++)
            {
                var previous = config.Gates[i - 1];
                var current = config.Gates[i];
                if (previous == null || current == null)
                {
                    continue;
                }

                var distance = current.Z - previous.Z;
                if (distance < 0f)
                {
                    AddWarning(issues, $"Gate {i} appears before gate {i - 1}. Keep gates ordered by Z.", GatePath(i, "z"));
                    continue;
                }

                if (distance < minSpacing)
                {
                    AddWarning(issues, $"Gate {i} is only {distance:0.#} units after gate {i - 1}. Recommended gate spacing is at least {minSpacing:0.#}.", GatePath(i, "z"));
                }
            }
        }

        private static void ValidateWaveSpacing(LevelConfig config, LevelValidationSettings settings, ICollection<LevelConfigIssue> issues)
        {
            var minSpacing = settings == null ? 10f : settings.MinWaveSpacing;
            for (var i = 1; i < config.EnemyWaves.Count; i++)
            {
                var previous = config.EnemyWaves[i - 1];
                var current = config.EnemyWaves[i];
                if (previous == null || current == null)
                {
                    continue;
                }

                var distance = current.Z - previous.Z;
                if (distance >= 0f && distance < minSpacing)
                {
                    AddWarning(issues, $"Enemy wave {i} is only {distance:0.#} units after wave {i - 1}. Recommended wave spacing is at least {minSpacing:0.#}.", WavePath(i, "z"));
                }
            }
        }

        private static void ValidateGateWaveSpacing(LevelConfig config, LevelValidationSettings settings, ICollection<LevelConfigIssue> issues)
        {
            var minSpacing = settings == null ? 6f : settings.MinGateWaveSpacing;
            for (var gateIndex = 0; gateIndex < config.Gates.Count; gateIndex++)
            {
                var gate = config.Gates[gateIndex];
                if (gate == null)
                {
                    continue;
                }

                for (var waveIndex = 0; waveIndex < config.EnemyWaves.Count; waveIndex++)
                {
                    var wave = config.EnemyWaves[waveIndex];
                    if (wave == null)
                    {
                        continue;
                    }

                    var distance = System.Math.Abs(gate.Z - wave.Z);
                    if (distance < minSpacing)
                    {
                        AddWarning(
                            issues,
                            $"Gate {gateIndex} and enemy wave {waveIndex} are only {distance:0.#} units apart. Recommended gate-wave spacing is at least {minSpacing:0.#}.",
                            GatePath(gateIndex, "z"));
                    }
                }
            }
        }

        private static void ValidateBossSpacing(LevelConfig config, LevelValidationSettings settings, ICollection<LevelConfigIssue> issues)
        {
            if (config.Boss == null || config.EnemyWaves.Count == 0)
            {
                return;
            }

            var minSpacing = settings == null ? 12f : settings.MinBossAfterLastWaveSpacing;
            var lastWaveIndex = config.EnemyWaves.Count - 1;
            var lastWave = config.EnemyWaves[lastWaveIndex];
            if (lastWave == null)
            {
                return;
            }

            var distance = config.Boss.Position.z - lastWave.Z;
            if (distance > 0f && distance < minSpacing)
            {
                AddWarning(issues, $"Boss is only {distance:0.#} units after the last enemy wave. Recommended boss spacing is at least {minSpacing:0.#}.", "boss.position");
            }
        }

        private static void AddError(ICollection<LevelConfigIssue> issues, string message, string propertyPath = "")
        {
            issues.Add(new LevelConfigIssue(LevelConfigIssueSeverity.Error, message, propertyPath));
        }

        private static void AddWarning(ICollection<LevelConfigIssue> issues, string message, string propertyPath = "")
        {
            issues.Add(new LevelConfigIssue(LevelConfigIssueSeverity.Warning, message, propertyPath));
        }

        private static string GatePath(int index, string childPath = "")
        {
            return ElementPath("gates", index, childPath);
        }

        private static string WavePath(int index, string childPath = "")
        {
            return ElementPath("enemyWaves", index, childPath);
        }

        private static string ElementPath(string collectionName, int index, string childPath)
        {
            var path = $"{collectionName}.Array.data[{index}]";
            return string.IsNullOrEmpty(childPath) ? path : $"{path}.{childPath}";
        }
    }
}
