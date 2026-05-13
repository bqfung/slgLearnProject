using SLGLearn.Player;
using UnityEngine;

namespace SLGLearn.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public sealed class EnemyMeleeAttacker : MonoBehaviour
    {
        [SerializeField] private SquadManager targetSquad;
        [SerializeField, Min(0f)] private float moveSpeed = 2f;
        [SerializeField, Min(0.1f)] private float attackRange = 1.2f;
        [SerializeField, Min(0.1f)] private float attacksPerSecond = 0.5f;
        [SerializeField, Min(1)] private int damageMembers = 1;

        private EnemyHealth health;
        private float cooldown;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
        }

        private void Update()
        {
            if (targetSquad == null || health.IsDead || targetSquad.Count <= 0)
            {
                return;
            }

            var toTarget = targetSquad.transform.position - transform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude > attackRange * attackRange)
            {
                MoveToward(toTarget);
                return;
            }

            Attack();
        }

        public void Configure(SquadManager squad, float speed, float range, float rate, int damage)
        {
            targetSquad = squad;
            moveSpeed = Mathf.Max(0f, speed);
            attackRange = Mathf.Max(0.1f, range);
            attacksPerSecond = Mathf.Max(0.1f, rate);
            damageMembers = Mathf.Max(1, damage);
            cooldown = 0f;
        }

        private void MoveToward(Vector3 toTarget)
        {
            var direction = toTarget.normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            if (direction.sqrMagnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        private void Attack()
        {
            cooldown -= Time.deltaTime;
            if (cooldown > 0f)
            {
                return;
            }

            targetSquad.RemoveMembers(damageMembers);
            cooldown = 1f / attacksPerSecond;
        }
    }
}
