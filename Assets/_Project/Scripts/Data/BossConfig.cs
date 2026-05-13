using UnityEngine;

namespace SLGLearn.Data
{
    [System.Serializable]
    public sealed class BossConfig
    {
        [SerializeField] private Vector3 position = new(0f, 1.4f, 96f);
        [SerializeField, Min(1f)] private float health = 60f;
        [SerializeField, Min(0f)] private float moveSpeed = 1.1f;
        [SerializeField, Min(0.1f)] private float attackRange = 2.2f;
        [SerializeField, Min(0.1f)] private float attacksPerSecond = 0.8f;
        [SerializeField, Min(1)] private int damageMembers = 2;

        public Vector3 Position => position;
        public float Health => health;
        public float MoveSpeed => moveSpeed;
        public float AttackRange => attackRange;
        public float AttacksPerSecond => attacksPerSecond;
        public int DamageMembers => damageMembers;
    }
}
