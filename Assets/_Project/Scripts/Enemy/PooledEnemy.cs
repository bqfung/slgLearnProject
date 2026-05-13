using UnityEngine;

namespace SLGLearn.Enemy
{
    [RequireComponent(typeof(EnemyHealth))]
    public sealed class PooledEnemy : MonoBehaviour
    {
        private EnemyPool owner;
        private EnemyHealth health;

        private void Awake()
        {
            health = GetComponent<EnemyHealth>();
            health.Died += OnDied;
        }

        public void Configure(EnemyPool pool)
        {
            owner = pool;
        }

        private void OnDied(EnemyHealth enemyHealth)
        {
            if (owner != null)
            {
                owner.Release(this);
            }
        }

        private void OnDestroy()
        {
            if (health != null)
            {
                health.Died -= OnDied;
            }
        }
    }
}
