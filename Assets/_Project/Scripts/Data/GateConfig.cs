using SLGLearn.Level;
using UnityEngine;

namespace SLGLearn.Data
{
    [System.Serializable]
    public sealed class GateConfig
    {
        [SerializeField] private float z;
        [SerializeField] private GateOperation leftOperation = GateOperation.Add;
        [SerializeField, Min(1)] private int leftValue = 5;
        [SerializeField] private GateOperation rightOperation = GateOperation.Multiply;
        [SerializeField, Min(1)] private int rightValue = 2;

        public float Z => z;
        public GateOperation LeftOperation => leftOperation;
        public int LeftValue => leftValue;
        public GateOperation RightOperation => rightOperation;
        public int RightValue => rightValue;
    }
}
