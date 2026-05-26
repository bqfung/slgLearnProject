using System;
using SLGLearn.Data;

namespace SLGLearn.Troop
{
    public sealed class TroopTrainingOrder
    {
        public TroopTrainingOrder(TroopConfig config, float remainingSeconds)
        {
            Config = config;
            RemainingSeconds = MathF.Max(0f, remainingSeconds);
        }

        public TroopConfig Config { get; }
        public float RemainingSeconds { get; private set; }
        public bool IsComplete => RemainingSeconds <= 0f;

        public void Tick(float deltaTime)
        {
            RemainingSeconds = MathF.Max(0f, RemainingSeconds - deltaTime);
        }
    }
}

namespace SLGLearn.Technology
{
    public sealed class TechnologyRuntimeState
    {
        public TechnologyRuntimeState(TechnologyConfig config)
        {
            Config = config;
            Level = 0;
        }

        public TechnologyConfig Config { get; }
        public int Level { get; private set; }
        public float ResearchRemainingSeconds { get; private set; }
        public bool IsResearching => ResearchRemainingSeconds > 0f;
        public bool IsMaxLevel => Config == null || Level >= Config.MaxLevel;
        public TechnologyLevelConfig CurrentLevel => Config?.GetLevel(Level);
        public TechnologyLevelConfig NextLevel => Config?.GetLevel(Level + 1);

        public void StartResearch(float durationSeconds)
        {
            ResearchRemainingSeconds = MathF.Max(0f, durationSeconds);
            if (ResearchRemainingSeconds <= 0f)
            {
                CompleteResearch();
            }
        }

        public void Restore(int level, float researchRemainingSeconds)
        {
            Level = Math.Clamp(level, 0, Config?.MaxLevel ?? 0);
            ResearchRemainingSeconds = MathF.Max(0f, researchRemainingSeconds);
        }

        public void Tick(float deltaTime)
        {
            if (!IsResearching)
            {
                return;
            }

            ResearchRemainingSeconds = MathF.Max(0f, ResearchRemainingSeconds - deltaTime);
            if (ResearchRemainingSeconds <= 0f)
            {
                CompleteResearch();
            }
        }

        private void CompleteResearch()
        {
            if (!IsMaxLevel)
            {
                Level++;
            }

            ResearchRemainingSeconds = 0f;
        }
    }
}
