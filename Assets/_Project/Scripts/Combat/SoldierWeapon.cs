using SLGLearn.Enemy;
using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class SoldierWeapon : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float attackRange = 8f;
        [SerializeField, Min(0.1f)] private float attacksPerSecond = 1f;
        [SerializeField, Min(0.1f)] private float damage = 1f;
        [SerializeField] private LayerMask targetLayers = ~0;

        private readonly Collider[] targetBuffer = new Collider[24];
        private float cooldown;

        private void Update()
        {
            cooldown -= Time.deltaTime;
            if (cooldown > 0f)
            {
                return;
            }

            var target = FindTarget();
            if (target == null)
            {
                return;
            }

            target.TakeDamage(damage);
            cooldown = 1f / attacksPerSecond;
        }

        private EnemyHealth FindTarget()
        {
            var count = Physics.OverlapSphereNonAlloc(
                transform.position,
                attackRange,
                targetBuffer,
                targetLayers,
                QueryTriggerInteraction.Ignore);

            EnemyHealth bestTarget = null;
            var bestDistance = float.MaxValue;
            var origin = transform.position;

            for (var i = 0; i < count; i++)
            {
                var target = targetBuffer[i].GetComponentInParent<EnemyHealth>();
                targetBuffer[i] = null;

                if (target == null || target.IsDead)
                {
                    continue;
                }

                var toTarget = target.transform.position - origin;
                if (Vector3.Dot(transform.forward, toTarget.normalized) < 0.1f)
                {
                    continue;
                }

                var distance = toTarget.sqrMagnitude;
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestTarget = target;
                }
            }

            return bestTarget;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
