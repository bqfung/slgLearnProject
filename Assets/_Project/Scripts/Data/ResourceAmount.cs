using UnityEngine;

namespace SLGLearn.Data
{
    [System.Serializable]
    public sealed class ResourceAmount
    {
        [SerializeField] private string resourceId = "food";
        [SerializeField, Min(0f)] private float amount;

        public string ResourceId => resourceId;
        public float Amount => amount;
    }
}
