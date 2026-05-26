using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Stronghold Config", fileName = "StrongholdConfig")]
    public sealed class StrongholdConfig : ScriptableObject
    {
        [SerializeField] private string id = "stronghold_01";
        [SerializeField] private string displayName = "Outpost";
        [SerializeField, Min(1)] private int recommendedPower = 30;
        [SerializeField, Range(0f, 1f)] private float victoryLossRate = 0.2f;
        [SerializeField, Range(0f, 1f)] private float defeatLossRate = 0.5f;
        [SerializeField] private List<StrongholdGarrisonUnit> garrison = new();
        [SerializeField] private List<string> prerequisiteStrongholdIds = new();
        [SerializeField] private Vector2 mapPosition;
        [SerializeField] private List<ResourceAmount> rewards = new();
        [SerializeField] private List<ResourceAmount> repeatRewards = new();

        public string Id => id;
        public string DisplayName => displayName;
        public int RecommendedPower => recommendedPower;
        public float VictoryLossRate => victoryLossRate;
        public float DefeatLossRate => defeatLossRate;
        public IReadOnlyList<StrongholdGarrisonUnit> Garrison => garrison;
        public IReadOnlyList<string> PrerequisiteStrongholdIds => prerequisiteStrongholdIds;
        public Vector2 MapPosition => mapPosition;
        public IReadOnlyList<ResourceAmount> Rewards => rewards;
        public IReadOnlyList<ResourceAmount> FirstClearRewards => rewards;
        public IReadOnlyList<ResourceAmount> RepeatRewards => repeatRewards;
    }

    [System.Serializable]
    public sealed class StrongholdGarrisonUnit
    {
        [SerializeField] private string troopId;
        [SerializeField, Min(1)] private int count = 1;

        public string TroopId => troopId;
        public int Count => count;
    }
}
