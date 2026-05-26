using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Resource Config", fileName = "ResourceConfig")]
    public sealed class ResourceConfig : ScriptableObject
    {
        [SerializeField] private string id = "food";
        [SerializeField] private string displayName = "Food";
        [SerializeField, Min(0f)] private float startingAmount = 100f;
        [SerializeField, Min(1f)] private float capacity = 1000f;

        public string Id => id;
        public string DisplayName => displayName;
        public float StartingAmount => startingAmount;
        public float Capacity => capacity;
    }
}
