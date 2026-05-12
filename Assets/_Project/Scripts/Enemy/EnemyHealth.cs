using UnityEngine;

namespace SLGLearn.Enemy
{
    public sealed class EnemyHealth : MonoBehaviour
    {
        [SerializeField, Min(1f)] private float maxHealth = 5f;

        private float currentHealth;
        private bool isDead;

        public bool IsDead => isDead;
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void Configure(float health)
        {
            maxHealth = Mathf.Max(1f, health);
            currentHealth = maxHealth;
            isDead = false;
            gameObject.SetActive(true);
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
