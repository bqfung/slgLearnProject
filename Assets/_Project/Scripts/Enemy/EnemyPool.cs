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
            var enemyObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            enemyObject.name = "Enemy";
            enemyObject.transform.SetParent(transform);
            enemyObject.GetComponent<Renderer>().sharedMaterial =
                RuntimePrimitiveFactory.CreateMaterial(new Color(0.85f, 0.25f, 0.2f));

            enemyObject.AddComponent<EnemyHealth>();
            enemyObject.AddComponent<EnemyMeleeAttacker>();
            return enemyObject.AddComponent<PooledEnemy>();
        }
    }
}
