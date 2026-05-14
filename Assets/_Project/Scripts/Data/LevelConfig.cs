using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Level Config", fileName = "LevelConfig")]
    public sealed class LevelConfig : ScriptableObject
    {
        [Header("Road")]
        [SerializeField, Min(10f)] private float roadLength = 130f;
        [SerializeField, Min(1f)] private float roadWidth = 10f;

        [Header("Content")]
        [SerializeField] private List<GateConfig> gates = new();
        [SerializeField] private List<EnemyWaveConfig> enemyWaves = new();
        [SerializeField] private BossConfig boss = new();

        [Header("Presentation")]
        [SerializeField] private VisualConfig visualConfig;

        public float RoadLength => roadLength;
        public float RoadWidth => roadWidth;
        public IReadOnlyList<GateConfig> Gates => gates;
        public IReadOnlyList<EnemyWaveConfig> EnemyWaves => enemyWaves;
        public BossConfig Boss => boss;
        public VisualConfig Visuals => visualConfig;
    }
}
