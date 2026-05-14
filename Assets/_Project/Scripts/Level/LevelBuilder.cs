using SLGLearn.Core;
using SLGLearn.Data;
using SLGLearn.Enemy;
using SLGLearn.Player;
using SLGLearn.UI;
using UnityEngine;

namespace SLGLearn.Level
{
    public sealed class LevelBuilder : MonoBehaviour
    {
        [SerializeField] private LevelConfig levelConfig;
        [SerializeField] private bool buildOnStart = true;

        private bool hasBuilt;

        private void Start()
        {
            if (buildOnStart)
            {
                Build();
            }
        }

        public void Configure(LevelConfig config, bool shouldBuildOnStart)
        {
            levelConfig = config;
            buildOnStart = shouldBuildOnStart;
        }

        public void Build()
        {
            if (hasBuilt)
            {
                return;
            }

            if (levelConfig == null)
            {
                Debug.LogError("LevelBuilder requires a LevelConfig.");
                return;
            }

            hasBuilt = true;

            RuntimePrimitiveFactory.Configure(levelConfig.Visuals);
            new EnvironmentBuilder().Build(levelConfig);
            var squad = new PlayerSquadFactory().Create();
            new RuntimeCameraFactory().Create(squad.transform);

            var gateBuilder = new GateBuilder();
            foreach (var gate in levelConfig.Gates)
            {
                gateBuilder.BuildGatePair(gate);
            }

            var squadManager = squad.GetComponent<SquadManager>();
            var enemySpawner = new EnemySpawner();
            foreach (var wave in levelConfig.EnemyWaves)
            {
                enemySpawner.SpawnWave(squadManager, wave);
            }

            var boss = enemySpawner.SpawnBoss(squadManager, levelConfig.Boss);
            var outcome = CreateOutcomeController(squad, boss);
            new RuntimeUiBuilder().BuildBattleUi(squadManager, boss, outcome);
        }

        private static GameOutcomeController CreateOutcomeController(GameObject squad, EnemyHealth boss)
        {
            var outcomeObject = new GameObject("GameOutcomeController");
            var controller = outcomeObject.AddComponent<GameOutcomeController>();
            controller.Configure(squad.GetComponent<SquadManager>(), squad.GetComponent<SquadController>(), boss, null);
            return controller;
        }
    }
}
