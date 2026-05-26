using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Building Config", fileName = "BuildingConfig")]
    public sealed class BuildingConfig : ScriptableObject
    {
        [SerializeField] private string id = "farm";
        [SerializeField] private string displayName = "Farm";
        [SerializeField] private List<BuildingLevelConfig> levels = new();

        public string Id => id;
        public string DisplayName => displayName;
        public int MaxLevel => levels.Count;

        public BuildingLevelConfig GetLevel(int level)
        {
            if (level <= 0 || level > levels.Count)
            {
                return null;
            }

            return levels[level - 1];
        }
    }
}
