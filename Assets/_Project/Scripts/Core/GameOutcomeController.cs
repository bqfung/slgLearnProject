using System;
using SLGLearn.Enemy;
using SLGLearn.Player;
using UnityEngine;

namespace SLGLearn.Core
{
    public sealed class GameOutcomeController : MonoBehaviour
    {
        [SerializeField] private SquadManager squad;
        [SerializeField] private SquadController squadController;
        [SerializeField] private EnemyHealth boss;
        [SerializeField] private TextMesh resultLabel;

        private bool hasFinished;

        public event Action<bool> Finished;
        public bool HasFinished => hasFinished;
        public bool IsVictory { get; private set; }

        private void Update()
        {
            if (hasFinished || squad == null || boss == null)
            {
                return;
            }

            if (squad.Count <= 0)
            {
                Finish(false);
                return;
            }

            if (boss.IsDead)
            {
                Finish(true);
            }
        }

        public void Configure(SquadManager playerSquad, SquadController playerController, EnemyHealth bossHealth, TextMesh label)
        {
            squad = playerSquad;
            squadController = playerController;
            boss = bossHealth;
            resultLabel = label;

            if (resultLabel != null)
            {
                resultLabel.text = string.Empty;
            }
        }

        private void Finish(bool isVictory)
        {
            hasFinished = true;
            IsVictory = isVictory;

            if (squadController != null)
            {
                squadController.enabled = false;
            }

            if (resultLabel != null)
            {
                resultLabel.text = isVictory ? "VICTORY" : "DEFEAT";
                resultLabel.color = isVictory ? Color.green : Color.red;
            }

            Debug.Log(isVictory ? "Victory: Boss defeated." : "Defeat: Squad eliminated.");
            Finished?.Invoke(isVictory);
        }
    }
}
