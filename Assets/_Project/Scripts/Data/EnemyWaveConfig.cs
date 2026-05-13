using UnityEngine;

namespace SLGLearn.Data
{
    [System.Serializable]
    public sealed class EnemyWaveConfig
    {
        [SerializeField] private float z;
        [SerializeField, Min(1)] private int count = 3;
        [SerializeField, Min(1f)] private float health = 3f;
        [SerializeField, Min(0f)] private float moveSpeed = 2f;
        [SerializeField, Min(0.1f)] private float attackRange = 1.15f;
        [SerializeField, Min(0.1f)] private float attacksPerSecond = 0.6f;
        [SerializeField, Min(1)] private int damageMembers = 1;

        public float Z => z;
        public int Count => count;
        public float Health => health;
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;
        public float AttacksPerSecond => attacksPerSecond;
        public int DamageMembers => damageMembers;
    }
}
