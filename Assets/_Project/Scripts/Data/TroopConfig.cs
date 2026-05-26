using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Data
{
    public enum TroopRole
    {
        Infantry,
        Archer,
        Cavalry,
        Siege
    }

    [CreateAssetMenu(menuName = "SLG Learn/Troop Config", fileName = "TroopConfig")]
    public sealed class TroopConfig : ScriptableObject
    {
        [SerializeField] private string id = "infantry";
        [SerializeField] private string displayName = "Infantry";
        [SerializeField] private TroopRole role = TroopRole.Infantry;
        [SerializeField] private TroopRole counterRole = TroopRole.Cavalry;
        [SerializeField, Min(1f)] private float counterMultiplier = 1.25f;
        [SerializeField, Min(1)] private int power = 10;
        [SerializeField, Min(0f)] private float trainingDurationSeconds = 5f;
        [SerializeField] private List<ResourceAmount> trainingCosts = new();

        public string Id => id;
        public string DisplayName => displayName;
        public TroopRole Role => role;
        public TroopRole CounterRole => counterRole;
        public float CounterMultiplier => counterMultiplier;
        public int Power => power;
        public float TrainingDurationSeconds => trainingDurationSeconds;
        public IReadOnlyList<ResourceAmount> TrainingCosts => trainingCosts;
    }

    public enum TechnologyEffectType
    {
        ResourceProductionMultiplier,
        TrainingSpeedMultiplier,
        TroopPowerMultiplier
    }

    [System.Serializable]
    public sealed class TechnologyEffect
    {
        [SerializeField] private TechnologyEffectType type;
        [SerializeField] private string targetId;
        [SerializeField] private float value;

        public TechnologyEffectType Type => type;
        public string TargetId => targetId;
        public float Value => value;
    }

    [System.Serializable]
    public sealed class TechnologyLevelConfig
    {
        [SerializeField, Min(0f)] private float researchDurationSeconds;
        [SerializeField] private List<ResourceAmount> researchCosts = new();
        [SerializeField] private List<TechnologyPrerequisite> technologyPrerequisites = new();
        [SerializeField] private List<BuildingLevelRequirement> buildingRequirements = new();
        [SerializeField] private List<TechnologyEffect> effects = new();

        public float ResearchDurationSeconds => researchDurationSeconds;
        public IReadOnlyList<ResourceAmount> ResearchCosts => researchCosts;
        public IReadOnlyList<TechnologyPrerequisite> TechnologyPrerequisites => technologyPrerequisites;
        public IReadOnlyList<BuildingLevelRequirement> BuildingRequirements => buildingRequirements;
        public IReadOnlyList<TechnologyEffect> Effects => effects;
    }

    [System.Serializable]
    public sealed class TechnologyPrerequisite
    {
        [SerializeField] private string technologyId;
        [SerializeField, Min(1)] private int requiredLevel = 1;

        public string TechnologyId => technologyId;
        public int RequiredLevel => requiredLevel;
    }

    [System.Serializable]
    public sealed class BuildingLevelRequirement
    {
        [SerializeField] private string buildingId;
        [SerializeField, Min(1)] private int requiredLevel = 1;

        public string BuildingId => buildingId;
        public int RequiredLevel => requiredLevel;
    }

    [CreateAssetMenu(menuName = "SLG Learn/Technology Config", fileName = "TechnologyConfig")]
    public sealed class TechnologyConfig : ScriptableObject
    {
        [SerializeField] private string id = "technology";
        [SerializeField] private string displayName = "Technology";
        [SerializeField] private List<TechnologyLevelConfig> levels = new();

        public string Id => id;
        public string DisplayName => displayName;
        public int MaxLevel => levels.Count;
        public IReadOnlyList<TechnologyLevelConfig> Levels => levels;

        public TechnologyLevelConfig GetLevel(int level)
        {
            if (level <= 0 || level > levels.Count)
            {
                return null;
            }

            return levels[level - 1];
        }
    }
}
