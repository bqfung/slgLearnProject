using SLGLearn.Data;
using SLGLearn.Level;
using SLGLearn.Player;
using UnityEngine;

namespace SLGLearn.Enemy
{
    public sealed class EnemySpawner
    {
        public void SpawnWave(SquadManager squad, EnemyWaveConfig wave)
        {
            var parent = new GameObject($"EnemyWave_{wave.Z:00}");
            var spacing = 1.2f;
            var width = (wave.Count - 1) * spacing;

            for (var i = 0; i < wave.Count; i++)
            {
                var x = i * spacing - width * 0.5f;
                SpawnEnemy(parent.transform, squad, new Vector3(x, 0.6f, wave.Z), wave);
            }
        }

        public EnemyHealth SpawnBoss(SquadManager squad, BossConfig bossConfig)
        {
            var boss = RuntimePrimitiveFactory.InstantiatePrefabOrPrimitive(
                RuntimePrimitiveFactory.BossPrefab,
                PrimitiveType.Capsule,
                "Boss",
                null);
            boss.transform.position = bossConfig.Position;
            boss.transform.localScale = new Vector3(2.2f, 2.8f, 2.2f);
            var renderer = boss.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = RuntimePrimitiveFactory.CreateMaterial(RuntimePrimitiveFactory.BossColor);
            }

            var health = RuntimePrimitiveFactory.GetOrAdd<EnemyHealth>(boss);
            health.Configure(bossConfig.Health);
            RuntimePrimitiveFactory.GetOrAdd<EnemyMeleeAttacker>(boss).Configure(
                squad,
                bossConfig.MoveSpeed,
                bossConfig.AttackRange,
                bossConfig.AttacksPerSecond,
                bossConfig.DamageMembers);
            return health;
        }

        private static void SpawnEnemy(Transform parent, SquadManager squad, Vector3 position, EnemyWaveConfig wave)
        {
            EnemyPool.GetOrCreate().Spawn(parent, squad, position, wave);
        }
    }
}
