using System;
using SLGLearn.Data;

namespace SLGLearn.Building
{
    public sealed class BuildingRuntimeState
    {
        public BuildingRuntimeState(BuildingConfig config)
        {
            Config = config;
            Level = config != null && config.MaxLevel > 0 ? 1 : 0;
        }

        public BuildingConfig Config { get; }
        public int Level { get; private set; }
        public float UpgradeRemainingSeconds { get; private set; }
        public bool IsUpgrading => UpgradeRemainingSeconds > 0f;
        public bool IsMaxLevel => Config == null || Level >= Config.MaxLevel;

        public BuildingLevelConfig CurrentLevel => Config?.GetLevel(Level);
        public BuildingLevelConfig NextLevel => Config?.GetLevel(Level + 1);

        public void StartUpgrade(float durationSeconds)
        {
            UpgradeRemainingSeconds = MathF.Max(0f, durationSeconds);
            if (UpgradeRemainingSeconds <= 0f)
            {
                CompleteUpgrade();
            }
        }

        public void Restore(int level, float upgradeRemainingSeconds)
        {
            Level = Math.Clamp(level, Config != null && Config.MaxLevel > 0 ? 1 : 0, Config?.MaxLevel ?? 0);
            UpgradeRemainingSeconds = MathF.Max(0f, upgradeRemainingSeconds);
        }

        public void Tick(float deltaTime)
        {
            if (!IsUpgrading)
            {
                return;
            }

            UpgradeRemainingSeconds = MathF.Max(0f, UpgradeRemainingSeconds - deltaTime);
            if (UpgradeRemainingSeconds <= 0f)
            {
                CompleteUpgrade();
            }
        }

        private void CompleteUpgrade()
        {
            if (!IsMaxLevel)
            {
                Level++;
            }

            UpgradeRemainingSeconds = 0f;
        }
    }
}
