using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Level Validation Settings", fileName = "LevelValidationSettings")]
    public sealed class LevelValidationSettings : ScriptableObject
    {
        [Header("Spacing Warnings")]
        [SerializeField, Min(0f)] private float minGateSpacing = 8f;
        [SerializeField, Min(0f)] private float minWaveSpacing = 10f;
        [SerializeField, Min(0f)] private float minGateWaveSpacing = 6f;
        [SerializeField, Min(0f)] private float minBossAfterLastWaveSpacing = 12f;

        public float MinGateSpacing => minGateSpacing;
        public float MinWaveSpacing => minWaveSpacing;
        public float MinGateWaveSpacing => minGateWaveSpacing;
        public float MinBossAfterLastWaveSpacing => minBossAfterLastWaveSpacing;
    }
}
