using SLGLearn.Enemy;
using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class Bullet : MonoBehaviour
    {
        [SerializeField, Min(0.01f)] private float hitRadius = 0.2f;

        private BulletPool owner;
        private EnemyHealth target;
        private float damage;
        private float speed;
        private float lifetime;

        private void Update()
        {
            if (target == null || target.IsDead || !target.gameObject.activeInHierarchy)
            {
                ReturnToPool();
                return;
            }

            lifetime -= Time.deltaTime;
            if (lifetime <= 0f)
            {
                ReturnToPool();
                return;
            }

            var targetPosition = target.transform.position + Vector3.up * 0.4f;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if ((targetPosition - transform.position).sqrMagnitude <= hitRadius * hitRadius)
            {
                var hitPosition = targetPosition;
                target.TakeDamage(damage);
                DamageNumberPool.GetOrCreate().Spawn(hitPosition + Vector3.up * 0.4f, damage);
                HitEffectPool.GetOrCreate().Spawn(hitPosition);
                ReturnToPool();
            }
        }

        public void Launch(BulletPool pool, Vector3 startPosition, EnemyHealth hitTarget, float hitDamage, float moveSpeed, float maxLifetime)
        {
            owner = pool;
            target = hitTarget;
            damage = hitDamage;
            speed = moveSpeed;
            lifetime = maxLifetime;
            transform.position = startPosition;
            gameObject.SetActive(true);
        }

        private void ReturnToPool()
        {
            target = null;

            if (owner != null)
            {
                owner.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
