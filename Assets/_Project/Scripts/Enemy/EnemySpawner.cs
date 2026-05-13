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
            var boss = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            boss.name = "Boss";
            boss.transform.position = bossConfig.Position;
            boss.transform.localScale = new Vector3(2.2f, 2.8f, 2.2f);
            boss.GetComponent<Renderer>().sharedMaterial =
                RuntimePrimitiveFactory.CreateMaterial(new Color(0.45f, 0.15f, 0.85f));

            var health = boss.AddComponent<EnemyHealth>();
            health.Configure(bossConfig.Health);
            boss.AddComponent<EnemyMeleeAttacker>().Configure(
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
