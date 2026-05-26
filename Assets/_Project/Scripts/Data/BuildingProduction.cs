using UnityEngine;

namespace SLGLearn.Data
{
    [System.Serializable]
    public sealed class BuildingProduction
    {
        [SerializeField] private string resourceId = "food";
        [SerializeField, Min(0f)] private float amountPerSecond;

        public string ResourceId => resourceId;
        public float AmountPerSecond => amountPerSecond;
    }
}
