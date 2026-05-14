using SLGLearn.Core;
using SLGLearn.Combat;
using SLGLearn.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace SLGLearn.UI
{
    public sealed class PoolDebugOverlay : MonoBehaviour
    {
        [SerializeField] private Text label;
        [SerializeField, Min(0.05f)] private float refreshInterval = 0.25f;
        [SerializeField] private KeyCode toggleKey = KeyCode.F3;

        private float refreshTimer;
        private bool isVisible = true;

        private void Awake()
        {
            if (label == null)
            {
                label = GetComponent<Text>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                isVisible = !isVisible;
                if (label != null)
                {
                    label.enabled = isVisible;
                }
            }

            if (!isVisible)
            {
                return;
            }

            refreshTimer -= Time.deltaTime;
            if (refreshTimer > 0f)
            {
                return;
            }

            refreshTimer = refreshInterval;
            Refresh();
        }

        public void Configure(Text debugLabel)
        {
            label = debugLabel;
            Refresh();
        }

        private void Refresh()
        {
            if (label == null)
            {
                return;
            }

            label.text =
                "Pools\n" +
                Format("Bullets", BulletPool.Shared) +
                Format("Damage", DamageNumberPool.Shared) +
                Format("Hit FX", HitEffectPool.Shared) +
                Format("Enemies", EnemyPool.Shared);
        }

        private static string Format<T>(string name, ComponentPool<T> pool) where T : Component
        {
            if (pool == null)
            {
                return $"{name}: not created\n";
            }

            return $"{name}: active {pool.ActiveCount} / free {pool.AvailableCount} / total {pool.TotalCreated} / peak {pool.PeakActiveCount} / grow {pool.ExpansionCount}\n";
        }
    }
}
