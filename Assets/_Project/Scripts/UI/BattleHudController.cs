using SLGLearn.Core;
using SLGLearn.Enemy;
using SLGLearn.Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SLGLearn.UI
{
    public sealed class BattleHudController : MonoBehaviour
    {
        [SerializeField] private SquadManager squad;
        [SerializeField] private EnemyHealth boss;
        [SerializeField] private GameOutcomeController outcomeController;
        [SerializeField] private Text squadCountText;
        [SerializeField] private Text bossHealthText;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Text resultText;
        [SerializeField] private Button restartButton;

        private void Awake()
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }

            if (restartButton != null)
            {
                restartButton.onClick.AddListener(RestartScene);
            }
        }

        private void OnEnable()
        {
            if (outcomeController != null)
            {
                outcomeController.Finished += ShowResult;
            }
        }

        private void OnDisable()
        {
            if (outcomeController != null)
            {
                outcomeController.Finished -= ShowResult;
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveListener(RestartScene);
            }
        }

        private void Update()
        {
            if (squadCountText != null && squad != null)
            {
                squadCountText.text = $"Squad: {squad.Count}";
            }

            if (bossHealthText != null && boss != null)
            {
                bossHealthText.text = $"Boss: {Mathf.CeilToInt(boss.CurrentHealth)} / {Mathf.CeilToInt(boss.MaxHealth)}";
            }
        }

        public void Configure(
            SquadManager playerSquad,
            EnemyHealth bossHealth,
            GameOutcomeController controller,
            Text squadText,
            Text bossText,
            GameObject panel,
            Text result,
            Button restart)
        {
            squad = playerSquad;
            boss = bossHealth;
            outcomeController = controller;
            squadCountText = squadText;
            bossHealthText = bossText;
            resultPanel = panel;
            resultText = result;
            restartButton = restart;

            if (resultPanel != null)
            {
                resultPanel.SetActive(false);
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveListener(RestartScene);
                restartButton.onClick.AddListener(RestartScene);
            }

            if (outcomeController != null)
            {
                outcomeController.Finished -= ShowResult;
                outcomeController.Finished += ShowResult;
            }
        }

        private void ShowResult(bool isVictory)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultText != null)
            {
                resultText.text = isVictory ? "VICTORY" : "DEFEAT";
                resultText.color = isVictory ? Color.green : Color.red;
            }
        }

        private static void RestartScene()
        {
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(string.IsNullOrEmpty(activeScene.path) ? activeScene.name : activeScene.path);
        }
    }
}
