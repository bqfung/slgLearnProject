using SLGLearn.Core;
using SLGLearn.Enemy;
using SLGLearn.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace SLGLearn.UI
{
    public sealed class RuntimeUiBuilder
    {
        public void BuildBattleUi(SquadManager squad, EnemyHealth boss, GameOutcomeController outcome)
        {
            var canvasObject = new GameObject("BattleCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();

            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var squadText = CreateText(canvasObject.transform, "SquadText", font, "Squad: 0", new Vector2(24f, -24f), TextAnchor.UpperLeft);
            var bossText = CreateText(canvasObject.transform, "BossText", font, "Boss: 0 / 0", new Vector2(24f, -64f), TextAnchor.UpperLeft);
            var resultPanel = CreateResultPanel(canvasObject.transform);
            var resultText = CreateText(resultPanel.transform, "ResultText", font, string.Empty, new Vector2(0f, 52f), TextAnchor.MiddleCenter);
            var restartButton = CreateRestartButton(resultPanel.transform, font);

            var hud = canvasObject.AddComponent<BattleHudController>();
            hud.Configure(squad, boss, outcome, squadText, bossText, resultPanel, resultText, restartButton);

            CreateEventSystem();
        }

        private static GameObject CreateResultPanel(Transform parent)
        {
            var panel = new GameObject("ResultPanel");
            panel.transform.SetParent(parent);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(360f, 220f);
            rect.anchoredPosition = Vector2.zero;

            var image = panel.AddComponent<Image>();
            image.color = new Color(0f, 0f, 0f, 0.72f);
            return panel;
        }

        private static Button CreateRestartButton(Transform parent, Font font)
        {
            var buttonObject = new GameObject("RestartButton");
            buttonObject.transform.SetParent(parent);

            var rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(180f, 48f);
            rect.anchoredPosition = new Vector2(0f, -42f);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.15f, 0.45f, 0.95f);

            var button = buttonObject.AddComponent<Button>();
            CreateText(buttonObject.transform, "Label", font, "Restart", Vector2.zero, TextAnchor.MiddleCenter);
            return button;
        }

        private static Text CreateText(Transform parent, string name, Font font, string value, Vector2 position, TextAnchor anchor)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent);

            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = anchor == TextAnchor.UpperLeft ? new Vector2(0f, 1f) : new Vector2(0.5f, 0.5f);
            rect.anchorMax = rect.anchorMin;
            rect.sizeDelta = anchor == TextAnchor.UpperLeft ? new Vector2(280f, 36f) : new Vector2(280f, 56f);
            rect.anchoredPosition = position;

            var text = textObject.AddComponent<Text>();
            text.text = value;
            text.font = font;
            text.fontSize = anchor == TextAnchor.UpperLeft ? 24 : 42;
            text.alignment = anchor;
            text.color = Color.white;
            return text;
        }

        private static void CreateEventSystem()
        {
            var eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
#else
            eventSystemObject.AddComponent<StandaloneInputModule>();
#endif
        }
    }
}
