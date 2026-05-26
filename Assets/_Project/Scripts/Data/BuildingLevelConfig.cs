using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Data
{
    [System.Serializable]
    public sealed class BuildingLevelConfig
    {
        [SerializeField, Min(0f)] private float upgradeDurationSeconds;
        [SerializeField] private List<ResourceAmount> upgradeCosts = new();
        [SerializeField] private List<BuildingProduction> production = new();

        public float UpgradeDurationSeconds => upgradeDurationSeconds;
        public IReadOnlyList<ResourceAmount> UpgradeCosts => upgradeCosts;
        public IReadOnlyList<BuildingProduction> Production => production;
    }
}
