using SLGLearn.Enemy;
using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class SoldierWeapon : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float attackRange = 8f;
        [SerializeField, Min(0.1f)] private float attacksPerSecond = 1f;
        [SerializeField, Min(0.1f)] private float damage = 1f;
        [SerializeField, Min(0.1f)] private float bulletSpeed = 18f;
        [SerializeField, Min(0.1f)] private float bulletLifetime = 1.5f;
        [SerializeField] private Vector3 muzzleOffset = new(0f, 0.6f, 0.35f);
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

            FireAt(target);
            cooldown = 1f / attacksPerSecond;
        }

        private void FireAt(EnemyHealth target)
        {
            var toTarget = target.transform.position - transform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude > 0f)
            {
                transform.rotation = Quaternion.LookRotation(toTarget.normalized, Vector3.up);
            }

            var bulletPool = BulletPool.GetOrCreate();
            var bullet = bulletPool.Spawn(transform.TransformPoint(muzzleOffset));
            bullet.Launch(bulletPool, bullet.transform.position, target, damage, bulletSpeed, bulletLifetime);
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
