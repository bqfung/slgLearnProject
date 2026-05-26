using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Chapter Config", fileName = "ChapterConfig")]
    public sealed class ChapterConfig : ScriptableObject
    {
        [SerializeField] private string id = "chapter_01";
        [SerializeField] private string displayName = "Border Outskirts";
        [SerializeField] private List<string> strongholdIds = new();
        [SerializeField] private List<ResourceAmount> completionRewards = new();

        public string Id => id;
        public string DisplayName => displayName;
        public IReadOnlyList<string> StrongholdIds => strongholdIds;
        public IReadOnlyList<ResourceAmount> CompletionRewards => completionRewards;
    }
}
