using SLGLearn.Core;
using SLGLearn.Data;
using SLGLearn.Level;
using SLGLearn.Player;
using UnityEngine;

namespace SLGLearn.Enemy
{
    public sealed class EnemyPool : ComponentPool<PooledEnemy>
    {
        public static EnemyPool Shared { get; private set; }

        protected override void Awake()
        {
            if (Shared != null && Shared != this)
            {
                Destroy(gameObject);
                return;
            }

            Shared = this;
            base.Awake();
        }

        public static EnemyPool GetOrCreate()
        {
            if (Shared != null)
            {
                return Shared;
            }

            var poolObject = new GameObject("EnemyPool");
            return poolObject.AddComponent<EnemyPool>();
        }

        public EnemyHealth Spawn(Transform parent, SquadManager squad, Vector3 position, EnemyWaveConfig wave)
        {
            var pooledEnemy = Get();
            var enemyTransform = pooledEnemy.transform;

            enemyTransform.SetParent(parent);
            enemyTransform.position = position;
            enemyTransform.rotation = Quaternion.identity;
            enemyTransform.localScale = new Vector3(0.7f, 1f, 0.7f);

            pooledEnemy.Configure(this);
            pooledEnemy.gameObject.SetActive(true);

            var health = pooledEnemy.GetComponent<EnemyHealth>();
            health.Configure(wave.Health);

            pooledEnemy.GetComponent<EnemyMeleeAttacker>().Configure(
                squad,
                wave.MoveSpeed,
                wave.AttackRange,
                wave.AttacksPerSecond,
                wave.DamageMembers);

            return health;
        }

        protected override PooledEnemy CreateItem()
        {
            var enemyObject = RuntimePrimitiveFactory.InstantiatePrefabOrPrimitive(
                RuntimePrimitiveFactory.EnemyPrefab,
                PrimitiveType.Capsule,
                "Enemy",
                transform);

            var renderer = enemyObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = RuntimePrimitiveFactory.CreateMaterial(RuntimePrimitiveFactory.EnemyColor);
            }

            RuntimePrimitiveFactory.GetOrAdd<EnemyHealth>(enemyObject);
            RuntimePrimitiveFactory.GetOrAdd<EnemyMeleeAttacker>(enemyObject);
            return RuntimePrimitiveFactory.GetOrAdd<PooledEnemy>(enemyObject);
        }
    }
}
