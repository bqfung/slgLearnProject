using System.Collections.Generic;
using SLGLearn.Base;
using SLGLearn.Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace SLGLearn.UI
{
    public static class RuntimeBaseUiBuilder
    {
        private static readonly Vector2 StrongholdCardSize = new(292f, 92f);

        public static void Build(SlgBaseRuntime runtime)
        {
            var canvasObject = new GameObject("BaseCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasObject.AddComponent<GraphicRaycaster>();

            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            CreateTitle(canvasObject.transform, font);

            var resourceTexts = CreateResourceTexts(canvasObject.transform, font, runtime);
            var buildingRows = CreateBuildingRows(canvasObject.transform, font, runtime);
            var powerText = CreatePowerText(canvasObject.transform, font);
            var formationPowerText = CreateFormationPowerText(canvasObject.transform, font);
            var troopRows = CreateTroopRows(canvasObject.transform, font, runtime);
            var technologyRows = CreateTechnologyRows(canvasObject.transform, font, runtime);
            var battleReportText = CreateBattleReportText(canvasObject.transform, font);
            var chapterText = CreateChapterText(canvasObject.transform, font);
            var strongholdRows = CreateStrongholdRows(canvasObject.transform, font, runtime);

            canvasObject.AddComponent<BaseHudController>().Configure(
                runtime,
                resourceTexts,
                buildingRows,
                troopRows,
                technologyRows,
                powerText,
                formationPowerText,
                chapterText,
                strongholdRows,
                battleReportText);
            CreateEventSystem();
        }

        private static void CreateTitle(Transform parent, Font font)
        {
            var title = CreateText(parent, "Title", font, "SLG Base", 34, TextAnchor.UpperLeft);
            var rect = title.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(32f, -24f);
            rect.sizeDelta = new Vector2(420f, 48f);
        }

        private static List<Text> CreateResourceTexts(Transform parent, Font font, SlgBaseRuntime runtime)
        {
            var texts = new List<Text>();
            var index = 0;
            foreach (var _ in runtime.Inventory.ConfigsById)
            {
                var text = CreateText(parent, $"Resource_{index}", font, string.Empty, 24, TextAnchor.UpperLeft);
                var rect = text.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0f, 1f);
                rect.anchorMax = new Vector2(0f, 1f);
                rect.anchoredPosition = new Vector2(32f, -82f - index * 34f);
                rect.sizeDelta = new Vector2(420f, 32f);
                texts.Add(text);
                index++;
            }

            return texts;
        }

        private static List<BuildingRow> CreateBuildingRows(Transform parent, Font font, SlgBaseRuntime runtime)
        {
            var rows = new List<BuildingRow>();
            for (var i = 0; i < runtime.BuildingStates.Count; i++)
            {
                rows.Add(CreateBuildingRow(parent, font, runtime, i));
            }

            return rows;
        }

        private static Text CreatePowerText(Transform parent, Font font)
        {
            var text = CreateText(parent, "TotalPower", font, string.Empty, 24, TextAnchor.UpperLeft);
            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(700f, -82f);
            rect.sizeDelta = new Vector2(420f, 32f);
            return text;
        }

        private static Text CreateFormationPowerText(Transform parent, Font font)
        {
            var text = CreateText(parent, "MarchPower", font, string.Empty, 24, TextAnchor.UpperLeft);
            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(700f, -116f);
            rect.sizeDelta = new Vector2(420f, 32f);
            return text;
        }

        private static List<TroopRow> CreateTroopRows(Transform parent, Font font, SlgBaseRuntime runtime)
        {
            var rows = new List<TroopRow>();
            var index = 0;
            foreach (var pair in runtime.TroopInventory.ConfigsById)
            {
                rows.Add(CreateTroopRow(parent, font, runtime, pair.Value, index));
                index++;
            }

            return rows;
        }

        private static Text CreateBattleReportText(Transform parent, Font font)
        {
            var text = CreateText(parent, "BattleReport", font, string.Empty, 20, TextAnchor.UpperLeft);
            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(700f, -430f);
            rect.sizeDelta = new Vector2(580f, 48f);
            return text;
        }

        private static Text CreateChapterText(Transform parent, Font font)
        {
            var text = CreateText(parent, "ChapterStatus", font, string.Empty, 20, TextAnchor.UpperLeft);
            var rect = text.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(32f, -500f);
            rect.sizeDelta = new Vector2(620f, 44f);
            return text;
        }

        private static List<TechnologyRow> CreateTechnologyRows(Transform parent, Font font, SlgBaseRuntime runtime)
        {
            var rows = new List<TechnologyRow>();
            for (var i = 0; i < runtime.TechnologyStates.Count; i++)
            {
                rows.Add(CreateTechnologyRow(parent, font, runtime, i));
            }

            return rows;
        }

        private static List<StrongholdRow> CreateStrongholdRows(Transform parent, Font font, SlgBaseRuntime runtime)
        {
            var rows = new List<StrongholdRow>();
            var positionsById = CollectStrongholdMapPositions(runtime);
            for (var i = 0; i < runtime.Strongholds.Count; i++)
            {
                if (runtime.Strongholds[i] != null)
                {
                    rows.Add(CreateStrongholdRow(parent, font, runtime, runtime.Strongholds[i], i, positionsById));
                }
            }

            return rows;
        }

        private static StrongholdRow CreateStrongholdRow(
            Transform parent,
            Font font,
            SlgBaseRuntime runtime,
            StrongholdConfig config,
            int index,
            IReadOnlyDictionary<string, Vector2> positionsById)
        {
            var position = ResolveStrongholdMapPosition(config, index);
            var prerequisiteLineImages = CreatePrerequisiteLines(parent, config, position, positionsById);

            var panel = new GameObject($"Stronghold_{index}");
            panel.transform.SetParent(parent);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = position;
            rect.sizeDelta = StrongholdCardSize;

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.1f, 0.11f, 0.14f, 0.82f);

            var node = new GameObject("MapNode");
            node.transform.SetParent(panel.transform);
            var nodeRect = node.AddComponent<RectTransform>();
            SetChildRect(nodeRect, new Vector2(16f, -16f), new Vector2(26f, 26f));
            var nodeImage = node.AddComponent<Image>();
            nodeImage.color = new Color(0.35f, 0.38f, 0.42f, 1f);

            var nameText = CreateText(panel.transform, "Name", font, string.Empty, 22, TextAnchor.UpperLeft);
            SetChildRect(nameText.rectTransform, new Vector2(52f, -10f), new Vector2(220f, 28f));

            var powerText = CreateText(panel.transform, "Power", font, string.Empty, 15, TextAnchor.UpperLeft);
            SetChildRect(powerText.rectTransform, new Vector2(52f, -40f), new Vector2(216f, 34f));

            var rewardText = CreateText(panel.transform, "Reward", font, string.Empty, 16, TextAnchor.UpperLeft);
            SetChildRect(rewardText.rectTransform, new Vector2(52f, -70f), new Vector2(206f, 34f));

            var button = CreateButton(panel.transform, font, "Attack");
            SetChildRect(button.GetComponent<RectTransform>(), new Vector2(176f, -14f), new Vector2(96f, 32f));
            button.onClick.AddListener(() => runtime.TryAttackStronghold(config));

            return new StrongholdRow(config, image, nodeImage, prerequisiteLineImages, nameText, powerText, rewardText, button);
        }

        private static Dictionary<string, Vector2> CollectStrongholdMapPositions(SlgBaseRuntime runtime)
        {
            var positions = new Dictionary<string, Vector2>();
            for (var i = 0; i < runtime.Strongholds.Count; i++)
            {
                var stronghold = runtime.Strongholds[i];
                if (stronghold != null && !string.IsNullOrWhiteSpace(stronghold.Id))
                {
                    positions[stronghold.Id] = ResolveStrongholdMapPosition(stronghold, i);
                }
            }

            return positions;
        }

        private static Vector2 ResolveStrongholdMapPosition(StrongholdConfig config, int index)
        {
            if (config != null && config.MapPosition != Vector2.zero)
            {
                return config.MapPosition;
            }

            return new Vector2(32f + index * 316f, -548f);
        }

        private static List<Image> CreatePrerequisiteLines(
            Transform parent,
            StrongholdConfig config,
            Vector2 currentPosition,
            IReadOnlyDictionary<string, Vector2> positionsById)
        {
            var lines = new List<Image>();
            if (config == null || config.PrerequisiteStrongholdIds.Count == 0)
            {
                return lines;
            }

            var currentCenter = GetStrongholdCardCenter(currentPosition);
            for (var i = 0; i < config.PrerequisiteStrongholdIds.Count; i++)
            {
                var prerequisiteId = config.PrerequisiteStrongholdIds[i];
                if (!positionsById.TryGetValue(prerequisiteId, out var prerequisitePosition))
                {
                    continue;
                }

                lines.Add(CreateMapLine(parent, GetStrongholdCardCenter(prerequisitePosition), currentCenter));
            }

            return lines;
        }

        private static Image CreateMapLine(Transform parent, Vector2 start, Vector2 end)
        {
            var lineObject = new GameObject("StrongholdLink");
            lineObject.transform.SetParent(parent);
            var rect = lineObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);

            var direction = end - start;
            rect.anchoredPosition = start + direction * 0.5f;
            rect.sizeDelta = new Vector2(direction.magnitude, 5f);
            rect.localRotation = Quaternion.Euler(0f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);

            var image = lineObject.AddComponent<Image>();
            image.color = new Color(0.26f, 0.28f, 0.32f, 0.95f);
            return image;
        }

        private static Vector2 GetStrongholdCardCenter(Vector2 position)
        {
            return position + new Vector2(StrongholdCardSize.x * 0.5f, -StrongholdCardSize.y * 0.5f);
        }

        private static TroopRow CreateTroopRow(Transform parent, Font font, SlgBaseRuntime runtime, TroopConfig config, int index)
        {
            var panel = new GameObject($"Troop_{index}");
            panel.transform.SetParent(parent);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(700f, -170f - index * 126f);
            rect.sizeDelta = new Vector2(580f, 106f);

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.09f, 0.12f, 0.15f, 0.82f);

            var nameText = CreateText(panel.transform, "Name", font, string.Empty, 24, TextAnchor.UpperLeft);
            SetChildRect(nameText.rectTransform, new Vector2(18f, -12f), new Vector2(340f, 30f));

            var powerText = CreateText(panel.transform, "Power", font, string.Empty, 18, TextAnchor.UpperLeft);
            SetChildRect(powerText.rectTransform, new Vector2(18f, -44f), new Vector2(220f, 24f));

            var statusText = CreateText(panel.transform, "Status", font, string.Empty, 18, TextAnchor.UpperLeft);
            SetChildRect(statusText.rectTransform, new Vector2(230f, -42f), new Vector2(190f, 44f));

            var button = CreateButton(panel.transform, font, "Train");
            SetChildRect(button.GetComponent<RectTransform>(), new Vector2(448f, -12f), new Vector2(112f, 32f));
            button.onClick.AddListener(() => runtime.TryTrain(config));

            var addButton = CreateButton(panel.transform, font, "+");
            SetChildRect(addButton.GetComponent<RectTransform>(), new Vector2(448f, -48f), new Vector2(52f, 32f));
            addButton.onClick.AddListener(() => runtime.TryAddToFormation(config));

            var removeButton = CreateButton(panel.transform, font, "-");
            SetChildRect(removeButton.GetComponent<RectTransform>(), new Vector2(508f, -48f), new Vector2(52f, 32f));
            removeButton.onClick.AddListener(() => runtime.TryRemoveFromFormation(config));

            return new TroopRow(config, nameText, powerText, statusText, button, addButton, removeButton);
        }

        private static TechnologyRow CreateTechnologyRow(Transform parent, Font font, SlgBaseRuntime runtime, int index)
        {
            var panel = new GameObject($"Technology_{index}");
            panel.transform.SetParent(parent);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(700f, -500f - index * 106f);
            rect.sizeDelta = new Vector2(580f, 88f);

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.12f, 0.11f, 0.16f, 0.82f);

            var nameText = CreateText(panel.transform, "Name", font, string.Empty, 22, TextAnchor.UpperLeft);
            SetChildRect(nameText.rectTransform, new Vector2(18f, -10f), new Vector2(240f, 28f));

            var statusText = CreateText(panel.transform, "Status", font, string.Empty, 18, TextAnchor.UpperLeft);
            SetChildRect(statusText.rectTransform, new Vector2(18f, -42f), new Vector2(240f, 24f));

            var effectText = CreateText(panel.transform, "Effect", font, string.Empty, 17, TextAnchor.UpperLeft);
            SetChildRect(effectText.rectTransform, new Vector2(260f, -14f), new Vector2(178f, 54f));

            var button = CreateButton(panel.transform, font, "Research");
            SetChildRect(button.GetComponent<RectTransform>(), new Vector2(448f, -24f), new Vector2(112f, 40f));
            var state = runtime.TechnologyStates[index];
            button.onClick.AddListener(() => runtime.TryResearch(state));

            return new TechnologyRow(state, nameText, statusText, effectText, button);
        }

        private static BuildingRow CreateBuildingRow(Transform parent, Font font, SlgBaseRuntime runtime, int index)
        {
            var panel = new GameObject($"Building_{index}");
            panel.transform.SetParent(parent);

            var rect = panel.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = new Vector2(32f, -180f - index * 116f);
            rect.sizeDelta = new Vector2(620f, 96f);

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.08f, 0.1f, 0.13f, 0.82f);

            var nameText = CreateText(panel.transform, "Name", font, string.Empty, 24, TextAnchor.UpperLeft);
            SetChildRect(nameText.rectTransform, new Vector2(18f, -12f), new Vector2(260f, 30f));

            var productionText = CreateText(panel.transform, "Production", font, string.Empty, 18, TextAnchor.UpperLeft);
            SetChildRect(productionText.rectTransform, new Vector2(18f, -44f), new Vector2(300f, 24f));

            var statusText = CreateText(panel.transform, "Status", font, string.Empty, 18, TextAnchor.UpperLeft);
            SetChildRect(statusText.rectTransform, new Vector2(310f, -16f), new Vector2(180f, 58f));

            var button = CreateButton(panel.transform, font, "Upgrade");
            SetChildRect(button.GetComponent<RectTransform>(), new Vector2(488f, -24f), new Vector2(112f, 44f));
            var state = runtime.BuildingStates[index];
            button.onClick.AddListener(() => runtime.TryUpgrade(state));

            return new BuildingRow(nameText, productionText, statusText, button);
        }

        private static Button CreateButton(Transform parent, Font font, string label)
        {
            var buttonObject = new GameObject(label + "Button");
            buttonObject.transform.SetParent(parent);
            var rect = buttonObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);

            var image = buttonObject.AddComponent<Image>();
            image.color = new Color(0.15f, 0.38f, 0.78f, 1f);

            var button = buttonObject.AddComponent<Button>();
            var text = CreateText(buttonObject.transform, "Label", font, label, 18, TextAnchor.MiddleCenter);
            text.color = Color.white;
            text.rectTransform.anchorMin = Vector2.zero;
            text.rectTransform.anchorMax = Vector2.one;
            text.rectTransform.offsetMin = Vector2.zero;
            text.rectTransform.offsetMax = Vector2.zero;
            return button;
        }

        private static Text CreateText(Transform parent, string name, Font font, string value, int fontSize, TextAnchor anchor)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent);
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);

            var text = textObject.AddComponent<Text>();
            text.text = value;
            text.font = font;
            text.fontSize = fontSize;
            text.alignment = anchor;
            text.color = Color.white;
            return text;
        }

        private static void SetChildRect(RectTransform rect, Vector2 anchoredPosition, Vector2 size)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
        }

        private static void CreateEventSystem()
        {
            if (Object.FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

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
