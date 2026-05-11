using UnityEngine;

namespace SLGLearn.Enemy
{
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float maxHealth = 5f;

        private float currentHealth;
        private bool isDead;

        public bool IsDead => isDead;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(float damage)
        {
            if (isDead || damage <= 0f)
            {
                return;
            }

            currentHealth = Mathf.Max(0f, currentHealth - damage);
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;
            gameObject.SetActive(false);
        }
    }
}
