using System;
using System.Collections.Generic;
using SLGLearn.Base;
using SLGLearn.Building;
using SLGLearn.Data;
using SLGLearn.Technology;
using SLGLearn.Troop;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem.UI;
#endif

namespace SLGLearn.UI
{
    public static class WorldSlgVisualBuilder
    {
        public static void Build(SlgBaseRuntime runtime)
        {
            var root = new GameObject("WorldSlgView");
            var materials = new WorldSlgMaterials();

            CreateZone(root.transform, "Base Zone", new Vector3(-4f, 0.01f, 0f), new Vector3(7f, 0.04f, 7f), materials.BaseZone);
            CreateZone(root.transform, "Map Zone", new Vector3(4.5f, 0.01f, 0f), new Vector3(7f, 0.04f, 7f), materials.MapZone);

            var resourceText = CreateText(root.transform, "ResourcesText", new Vector3(-6.4f, 1.9f, -3.25f), 0.055f, "Resources");
            var powerText = CreateText(root.transform, "PowerText", new Vector3(-2.65f, 1.9f, -3.25f), 0.055f, "Power");
            var chapterText = CreateText(root.transform, "ChapterText", new Vector3(3.85f, 1.9f, -3.25f), 0.055f, "Chapter");
            var reportText = CreateText(root.transform, "BattleReportText", new Vector3(3.85f, 1.9f, 3.25f), 0.052f, "Report");
            var objectiveText = CreateText(root.transform, "ObjectiveText", new Vector3(-0.95f, 2.05f, 3.25f), 0.055f, "Next");

            var detailPanel = CreateDetailPanel();
            var taskPanel = CreateTaskPanel();
            var controller = root.AddComponent<WorldSlgVisualController>();
            var buildingViews = CreateBuildingViews(root.transform, runtime, materials, controller.SelectBuilding);
            var troopViews = CreateTroopViews(root.transform, runtime, materials, controller.SelectTroop);
            var technologyViews = CreateTechnologyViews(root.transform, runtime, materials, controller.SelectTechnology);
            var strongholdViews = CreateStrongholdViews(root.transform, runtime, materials, controller.SelectStronghold);
            var objectiveButton = CreateWorldButton(root.transform, "ObjectiveGo", "Go", new Vector3(1.25f, 1.72f, 3.25f), materials.GoodButton, controller.SelectCurrentObjective);

            controller.Configure(
                runtime,
                resourceText,
                powerText,
                chapterText,
                reportText,
                objectiveText,
                objectiveButton,
                detailPanel,
                taskPanel,
                buildingViews,
                troopViews,
                technologyViews,
                strongholdViews);

            CreateEventSystem();
        }

        private static List<WorldBuildingView> CreateBuildingViews(
            Transform parent,
            SlgBaseRuntime runtime,
            WorldSlgMaterials materials,
            Action<BuildingRuntimeState> onSelect)
        {
            var views = new List<WorldBuildingView>();
            var positions = new[]
            {
                new Vector3(-6.2f, 0.55f, -1.9f),
                new Vector3(-4.3f, 0.55f, -1.9f),
                new Vector3(-2.4f, 0.55f, -1.9f)
            };

            for (var i = 0; i < runtime.BuildingStates.Count; i++)
            {
                var state = runtime.BuildingStates[i];
                var position = positions[Mathf.Min(i, positions.Length - 1)] + new Vector3(0f, 0f, Mathf.Max(0, i - positions.Length + 1) * 1.6f);
                var body = CreatePrimitive(parent, $"Building_{state.Config.Id}", PrimitiveType.Cube, position, new Vector3(1.25f, 0.9f, 1.25f), materials.Building);
                body.AddComponent<WorldClickAction>().Configure(() => onSelect(state));
                var selectionMarker = CreateSelectionMarker(parent, $"Select_{state.Config.Id}", position, new Vector3(1.55f, 0.035f, 1.55f), materials.Selection);
                var label = CreateText(parent, $"Label_{state.Config.Id}", position + new Vector3(0f, 1.05f, 0f), 0.055f, string.Empty);
                var button = CreateWorldButton(parent, $"Upgrade_{state.Config.Id}", "U", position + new Vector3(0f, 0.12f, 1.05f), materials.Button, () => runtime.TryUpgrade(state));
                views.Add(new WorldBuildingView(state, body.transform, body.GetComponent<Renderer>(), label, selectionMarker, button));
            }

            return views;
        }

        private static List<WorldTroopView> CreateTroopViews(
            Transform parent,
            SlgBaseRuntime runtime,
            WorldSlgMaterials materials,
            Action<TroopConfig> onSelect)
        {
            var views = new List<WorldTroopView>();
            var index = 0;
            foreach (var pair in runtime.TroopInventory.ConfigsById)
            {
                var config = pair.Value;
                var position = new Vector3(-6.1f + index * 2.35f, 0.4f, 1.35f);
                var yard = CreatePrimitive(parent, $"TroopYard_{config.Id}", PrimitiveType.Cylinder, position, new Vector3(1.05f, 0.32f, 1.05f), materials.Troop);
                yard.AddComponent<WorldClickAction>().Configure(() => onSelect(config));
                var selectionMarker = CreateSelectionMarker(parent, $"Select_{config.Id}", position, new Vector3(1.35f, 0.035f, 1.35f), materials.Selection);
                var label = CreateText(parent, $"Label_{config.Id}", position + new Vector3(0f, 0.78f, 0f), 0.052f, string.Empty);
                var reserveMarkers = CreateCapsuleMarkers(parent, $"Reserve_{config.Id}", position + new Vector3(-0.5f, 0.42f, -0.35f), materials.ReserveMarker, 5);
                var marchMarkers = CreateCapsuleMarkers(parent, $"March_{config.Id}", position + new Vector3(-0.5f, 0.42f, 0.15f), materials.MarchMarker, 5);
                var train = CreateWorldButton(parent, $"Train_{config.Id}", "T", position + new Vector3(-0.55f, 0.1f, 1.05f), materials.Button, () => runtime.TryTrain(config));
                var add = CreateWorldButton(parent, $"Add_{config.Id}", "+", position + new Vector3(0.25f, 0.1f, 1.05f), materials.GoodButton, () => runtime.TryAddToFormation(config));
                var remove = CreateWorldButton(parent, $"Remove_{config.Id}", "-", position + new Vector3(0.85f, 0.1f, 1.05f), materials.BadButton, () => runtime.TryRemoveFromFormation(config));
                views.Add(new WorldTroopView(config, label, selectionMarker, reserveMarkers, marchMarkers, train, add, remove));
                index++;
            }

            return views;
        }

        private static List<WorldTechnologyView> CreateTechnologyViews(
            Transform parent,
            SlgBaseRuntime runtime,
            WorldSlgMaterials materials,
            Action<TechnologyRuntimeState> onSelect)
        {
            var views = new List<WorldTechnologyView>();
            var positions = new[]
            {
                new Vector3(-6.15f, 0.35f, 3.05f),
                new Vector3(-4.45f, 0.35f, 3.05f),
                new Vector3(-2.75f, 0.35f, 3.05f)
            };
            var positionsById = new Dictionary<string, Vector3>();
            for (var i = 0; i < runtime.TechnologyStates.Count; i++)
            {
                var state = runtime.TechnologyStates[i];
                var position = positions[Mathf.Min(i, positions.Length - 1)] + new Vector3(0f, 0f, Mathf.Max(0, i - positions.Length + 1) * 1.25f);
                positionsById[state.Config.Id] = position;
            }

            for (var i = 0; i < runtime.TechnologyStates.Count; i++)
            {
                var state = runtime.TechnologyStates[i];
                var position = positionsById[state.Config.Id];
                var prerequisiteLines = new List<LineRenderer>();
                var prerequisites = CollectTechnologyPrerequisites(state.Config);
                foreach (var prerequisiteId in prerequisites)
                {
                    if (positionsById.TryGetValue(prerequisiteId, out var start))
                    {
                        prerequisiteLines.Add(CreateLine(parent, $"TechLink_{prerequisiteId}_{state.Config.Id}", start, position, materials.TechLink));
                    }
                }

                var body = CreatePrimitive(parent, $"Technology_{state.Config.Id}", PrimitiveType.Sphere, position, new Vector3(0.72f, 0.72f, 0.72f), materials.Technology);
                body.AddComponent<WorldClickAction>().Configure(() => onSelect(state));
                var selectionMarker = CreateSelectionMarker(parent, $"Select_{state.Config.Id}", position, new Vector3(1.1f, 0.035f, 1.1f), materials.Selection);
                var label = CreateText(parent, $"Label_{state.Config.Id}", position + new Vector3(0f, 0.75f, 0f), 0.046f, string.Empty);
                var button = CreateWorldButton(parent, $"Research_{state.Config.Id}", "R", position + new Vector3(0f, -0.04f, 0.92f), materials.Button, () => runtime.TryResearch(state));
                views.Add(new WorldTechnologyView(state, body.GetComponent<Renderer>(), label, selectionMarker, prerequisiteLines, button));
            }

            return views;
        }

        private static HashSet<string> CollectTechnologyPrerequisites(TechnologyConfig config)
        {
            var prerequisiteIds = new HashSet<string>();
            if (config == null)
            {
                return prerequisiteIds;
            }

            for (var levelIndex = 0; levelIndex < config.Levels.Count; levelIndex++)
            {
                var level = config.Levels[levelIndex];
                if (level == null)
                {
                    continue;
                }

                for (var prerequisiteIndex = 0; prerequisiteIndex < level.TechnologyPrerequisites.Count; prerequisiteIndex++)
                {
                    var prerequisite = level.TechnologyPrerequisites[prerequisiteIndex];
                    if (prerequisite != null && !string.IsNullOrWhiteSpace(prerequisite.TechnologyId))
                    {
                        prerequisiteIds.Add(prerequisite.TechnologyId);
                    }
                }
            }

            return prerequisiteIds;
        }

        private static List<WorldStrongholdView> CreateStrongholdViews(
            Transform parent,
            SlgBaseRuntime runtime,
            WorldSlgMaterials materials,
            Action<StrongholdConfig> onSelect)
        {
            var views = new List<WorldStrongholdView>();
            var positions = new Dictionary<string, Vector3>
            {
                { "outpost", new Vector3(2.2f, 0.35f, 0f) },
                { "raider_camp", new Vector3(5.1f, 0.35f, -1.35f) },
                { "supply_depot", new Vector3(5.1f, 0.35f, 1.35f) }
            };

            for (var i = 0; i < runtime.Strongholds.Count; i++)
            {
                var config = runtime.Strongholds[i];
                var position = positions.TryGetValue(config.Id, out var mapped) ? mapped : new Vector3(2.2f + i * 1.8f, 0.35f, 0f);
                for (var prerequisiteIndex = 0; prerequisiteIndex < config.PrerequisiteStrongholdIds.Count; prerequisiteIndex++)
                {
                    if (positions.TryGetValue(config.PrerequisiteStrongholdIds[prerequisiteIndex], out var start))
                    {
                        CreateLine(parent, $"Link_{config.PrerequisiteStrongholdIds[prerequisiteIndex]}_{config.Id}", start, position, materials.Link);
                    }
                }

                var body = CreatePrimitive(parent, $"Stronghold_{config.Id}", PrimitiveType.Sphere, position, new Vector3(0.8f, 0.8f, 0.8f), materials.Locked);
                body.AddComponent<WorldClickAction>().Configure(() => onSelect(config));
                var selectionMarker = CreateSelectionMarker(parent, $"Select_{config.Id}", position, new Vector3(1.2f, 0.035f, 1.2f), materials.Selection);
                var label = CreateText(parent, $"Label_{config.Id}", position + new Vector3(0f, 0.82f, 0f), 0.05f, string.Empty);
                var garrisonMarkers = CreateCapsuleMarkers(parent, $"Garrison_{config.Id}", position + new Vector3(-0.45f, 0.42f, -0.55f), materials.GarrisonMarker, 5);
                var button = CreateWorldButton(parent, $"Attack_{config.Id}", "A", position + new Vector3(0f, -0.05f, 1.05f), materials.Button, () => runtime.TryAttackStronghold(config));
                views.Add(new WorldStrongholdView(config, body.GetComponent<Renderer>(), label, selectionMarker, garrisonMarkers, button));
            }

            return views;
        }

        private static GameObject CreateZone(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
        {
            return CreatePrimitive(parent, name, PrimitiveType.Cube, position, scale, material);
        }

        private static GameObject CreatePrimitive(Transform parent, string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material)
        {
            var obj = GameObject.CreatePrimitive(type);
            obj.name = name;
            obj.transform.SetParent(parent);
            obj.transform.position = position;
            obj.transform.localScale = scale;
            obj.GetComponent<Renderer>().material = material;
            return obj;
        }

        private static WorldButtonView CreateWorldButton(Transform parent, string name, string text, Vector3 position, Material material, Action onClick)
        {
            var button = CreatePrimitive(parent, name, PrimitiveType.Cube, position, new Vector3(0.72f, 0.18f, 0.34f), material);
            var clickAction = button.AddComponent<WorldClickAction>();
            clickAction.Configure(onClick);
            var label = CreateText(button.transform, "Label", new Vector3(0f, 0.22f, 0f), 0.035f, text);
            return new WorldButtonView(button.GetComponent<Renderer>(), label, clickAction);
        }

        private static GameObject CreateSelectionMarker(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
        {
            var marker = CreatePrimitive(parent, name, PrimitiveType.Cylinder, new Vector3(position.x, 0.035f, position.z), scale, material);
            marker.SetActive(false);
            return marker;
        }

        private static IReadOnlyList<GameObject> CreateCapsuleMarkers(Transform parent, string namePrefix, Vector3 startPosition, Material material, int count)
        {
            var markers = new List<GameObject>();
            for (var i = 0; i < count; i++)
            {
                var marker = CreatePrimitive(
                    parent,
                    $"{namePrefix}_{i + 1}",
                    PrimitiveType.Capsule,
                    startPosition + new Vector3(i * 0.24f, 0f, 0f),
                    new Vector3(0.13f, 0.22f, 0.13f),
                    material);
                marker.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                markers.Add(marker);
            }

            return markers;
        }

        private static TextMesh CreateText(Transform parent, string name, Vector3 localPosition, float size, string text)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            obj.transform.localPosition = localPosition;
            var mesh = obj.AddComponent<TextMesh>();
            mesh.text = text;
            mesh.fontSize = 32;
            mesh.characterSize = size;
            mesh.anchor = TextAnchor.MiddleCenter;
            mesh.alignment = TextAlignment.Center;
            mesh.color = Color.white;
            obj.AddComponent<WorldTextBillboard>();
            return mesh;
        }

        private static LineRenderer CreateLine(Transform parent, string name, Vector3 start, Vector3 end, Material material)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            var line = obj.AddComponent<LineRenderer>();
            line.positionCount = 2;
            line.SetPosition(0, start + Vector3.up * 0.08f);
            line.SetPosition(1, end + Vector3.up * 0.08f);
            line.startWidth = 0.08f;
            line.endWidth = 0.08f;
            line.material = material;
            return line;
        }

        private static WorldDetailPanel CreateDetailPanel()
        {
            var canvasObject = new GameObject("WorldDetailCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasObject.AddComponent<GraphicRaycaster>();

            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var panel = new GameObject("SelectedDetailPanel");
            panel.transform.SetParent(canvasObject.transform);
            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(1f, 0.5f);
            panelRect.anchorMax = new Vector2(1f, 0.5f);
            panelRect.pivot = new Vector2(1f, 0.5f);
            panelRect.anchoredPosition = new Vector2(-28f, 0f);
            panelRect.sizeDelta = new Vector2(420f, 580f);

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.05f, 0.07f, 0.09f, 0.84f);

            var title = CreateUiText(panel.transform, "Title", font, "Select Target", 28, TextAnchor.UpperLeft);
            title.color = new Color(0.92f, 0.96f, 1f, 1f);
            SetUiRect(title.rectTransform, new Vector2(24f, -20f), new Vector2(372f, 42f));

            var body = CreateUiText(panel.transform, "Body", font, "Click a building, troop yard, or stronghold.", 20, TextAnchor.UpperLeft);
            body.color = new Color(0.82f, 0.88f, 0.92f, 1f);
            SetUiRect(body.rectTransform, new Vector2(24f, -76f), new Vector2(372f, 340f));

            var primaryButton = CreateUiButton(panel.transform, font, "PrimaryAction", "Action");
            SetUiRect(primaryButton.RectTransform, new Vector2(24f, -438f), new Vector2(372f, 36f));

            var secondaryButton = CreateUiButton(panel.transform, font, "SecondaryAction", "Action");
            SetUiRect(secondaryButton.RectTransform, new Vector2(24f, -484f), new Vector2(178f, 36f));

            var tertiaryButton = CreateUiButton(panel.transform, font, "TertiaryAction", "Action");
            SetUiRect(tertiaryButton.RectTransform, new Vector2(218f, -484f), new Vector2(178f, 36f));

            return new WorldDetailPanel(title, body, primaryButton, secondaryButton, tertiaryButton);
        }

        private static WorldTaskPanel CreateTaskPanel()
        {
            var canvasObject = new GameObject("WorldTaskCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            canvasObject.AddComponent<GraphicRaycaster>();

            var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            var panel = new GameObject("TaskPanel");
            panel.transform.SetParent(canvasObject.transform);
            var panelRect = panel.AddComponent<RectTransform>();
            panelRect.anchorMin = new Vector2(0f, 1f);
            panelRect.anchorMax = new Vector2(0f, 1f);
            panelRect.pivot = new Vector2(0f, 1f);
            panelRect.anchoredPosition = new Vector2(28f, -250f);
            panelRect.sizeDelta = new Vector2(420f, 230f);

            var image = panel.AddComponent<Image>();
            image.color = new Color(0.05f, 0.07f, 0.09f, 0.74f);

            var title = CreateUiText(panel.transform, "Title", font, "Objectives", 24, TextAnchor.UpperLeft);
            title.color = new Color(0.92f, 0.96f, 1f, 1f);
            SetUiRect(title.rectTransform, new Vector2(20f, -16f), new Vector2(260f, 34f));

            var rows = new List<WorldTaskRow>();
            for (var i = 0; i < 3; i++)
            {
                var text = CreateUiText(panel.transform, $"Task_{i + 1}", font, string.Empty, 17, TextAnchor.MiddleLeft);
                text.color = new Color(0.82f, 0.88f, 0.92f, 1f);
                SetUiRect(text.rectTransform, new Vector2(20f, -62f - i * 52f), new Vector2(284f, 36f));

                var button = CreateUiButton(panel.transform, font, $"TaskGo_{i + 1}", "Go");
                SetUiRect(button.RectTransform, new Vector2(316f, -62f - i * 52f), new Vector2(78f, 34f));
                rows.Add(new WorldTaskRow(text, button));
            }

            return new WorldTaskPanel(rows);
        }

        private static Text CreateUiText(Transform parent, string name, Font font, string text, int fontSize, TextAnchor anchor)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            var value = obj.AddComponent<Text>();
            value.font = font;
            value.text = text;
            value.fontSize = fontSize;
            value.alignment = anchor;
            value.horizontalOverflow = HorizontalWrapMode.Wrap;
            value.verticalOverflow = VerticalWrapMode.Truncate;
            return value;
        }

        private static WorldUiButton CreateUiButton(Transform parent, Font font, string name, string text)
        {
            var obj = new GameObject(name);
            obj.transform.SetParent(parent);
            var rect = obj.AddComponent<RectTransform>();
            var image = obj.AddComponent<Image>();
            image.color = new Color(0.12f, 0.34f, 0.72f, 0.95f);
            var button = obj.AddComponent<Button>();

            var label = CreateUiText(obj.transform, "Label", font, text, 18, TextAnchor.MiddleCenter);
            label.color = Color.white;
            SetUiRect(label.rectTransform, Vector2.zero, Vector2.zero);
            label.rectTransform.anchorMin = Vector2.zero;
            label.rectTransform.anchorMax = Vector2.one;
            label.rectTransform.offsetMin = Vector2.zero;
            label.rectTransform.offsetMax = Vector2.zero;
            return new WorldUiButton(rect, image, button, label);
        }

        private static void SetUiRect(RectTransform rect, Vector2 anchoredPosition, Vector2 size)
        {
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = size;
        }

        private static void CreateEventSystem()
        {
            if (UnityEngine.Object.FindFirstObjectByType<EventSystem>() != null)
            {
                return;
            }

            var eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
#if ENABLE_INPUT_SYSTEM
            eventSystem.AddComponent<InputSystemUIInputModule>();
#else
            eventSystem.AddComponent<StandaloneInputModule>();
#endif
        }
    }

    public sealed class WorldSlgVisualController : MonoBehaviour
    {
        private IReadOnlyList<WorldBuildingView> buildingViews;
        private IReadOnlyList<WorldTroopView> troopViews;
        private IReadOnlyList<WorldTechnologyView> technologyViews;
        private IReadOnlyList<WorldStrongholdView> strongholdViews;
        private SlgBaseRuntime runtime;
        private TextMesh resourcesText;
        private TextMesh powerText;
        private TextMesh chapterText;
        private TextMesh reportText;
        private TextMesh objectiveText;
        private WorldButtonView objectiveButton;
        private WorldDetailPanel detailPanel;
        private WorldTaskPanel taskPanel;
        private BuildingRuntimeState selectedBuilding;
        private TroopConfig selectedTroop;
        private TechnologyRuntimeState selectedTechnology;
        private StrongholdConfig selectedStronghold;
        private BuildingRuntimeState objectiveBuilding;
        private TroopConfig objectiveTroop;
        private TechnologyRuntimeState objectiveTechnology;
        private StrongholdConfig objectiveStronghold;
        private WorldSelectionType objectiveSelectionType;
        private WorldSelectionType selectionType;

        public void Configure(
            SlgBaseRuntime baseRuntime,
            TextMesh resourceDisplay,
            TextMesh powerDisplay,
            TextMesh chapterDisplay,
            TextMesh reportDisplay,
            TextMesh objectiveDisplay,
            WorldButtonView nextObjectiveButton,
            WorldDetailPanel selectedDetailPanel,
            WorldTaskPanel objectivesPanel,
            IReadOnlyList<WorldBuildingView> buildings,
            IReadOnlyList<WorldTroopView> troops,
            IReadOnlyList<WorldTechnologyView> technologies,
            IReadOnlyList<WorldStrongholdView> strongholds)
        {
            runtime = baseRuntime;
            resourcesText = resourceDisplay;
            powerText = powerDisplay;
            chapterText = chapterDisplay;
            reportText = reportDisplay;
            objectiveText = objectiveDisplay;
            objectiveButton = nextObjectiveButton;
            detailPanel = selectedDetailPanel;
            taskPanel = objectivesPanel;
            buildingViews = buildings;
            troopViews = troops;
            technologyViews = technologies;
            strongholdViews = strongholds;
            runtime.Changed += Refresh;
            Refresh();
        }

        public void SelectBuilding(BuildingRuntimeState state)
        {
            selectedBuilding = state;
            selectedTroop = null;
            selectedTechnology = null;
            selectedStronghold = null;
            selectionType = WorldSelectionType.Building;
            RefreshDetail();
        }

        public void SelectCurrentObjective()
        {
            SelectObjectiveTarget(
                new WorldObjectiveTarget(
                    string.Empty,
                    objectiveSelectionType,
                    objectiveBuilding,
                    objectiveTroop,
                    objectiveTechnology,
                    objectiveStronghold));
        }

        public void SelectObjectiveTarget(WorldObjectiveTarget target)
        {
            switch (target.SelectionType)
            {
                case WorldSelectionType.Building when target.Building != null:
                    SelectBuilding(target.Building);
                    break;
                case WorldSelectionType.Troop when target.Troop != null:
                    SelectTroop(target.Troop);
                    break;
                case WorldSelectionType.Technology when target.Technology != null:
                    SelectTechnology(target.Technology);
                    break;
                case WorldSelectionType.Stronghold when target.Stronghold != null:
                    SelectStronghold(target.Stronghold);
                    break;
            }
        }

        public void SelectTroop(TroopConfig config)
        {
            selectedBuilding = null;
            selectedTroop = config;
            selectedTechnology = null;
            selectedStronghold = null;
            selectionType = WorldSelectionType.Troop;
            RefreshDetail();
        }

        public void SelectTechnology(TechnologyRuntimeState state)
        {
            selectedBuilding = null;
            selectedTroop = null;
            selectedTechnology = state;
            selectedStronghold = null;
            selectionType = WorldSelectionType.Technology;
            RefreshDetail();
        }

        public void SelectStronghold(StrongholdConfig config)
        {
            selectedBuilding = null;
            selectedTroop = null;
            selectedTechnology = null;
            selectedStronghold = config;
            selectionType = WorldSelectionType.Stronghold;
            RefreshDetail();
        }

        private void OnDestroy()
        {
            if (runtime != null)
            {
                runtime.Changed -= Refresh;
            }
        }

        private void Update()
        {
            if (runtime != null && runtime.MarchOrders.Count > 0)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            RefreshResources();
            RefreshBuildings();
            RefreshTroops();
            RefreshTechnologies();
            RefreshStrongholds();
            RefreshChapter();
            RefreshReport();
            RefreshObjective();
            RefreshDetail();
        }

        private void RefreshResources()
        {
            var parts = new List<string>();
            foreach (var pair in runtime.Inventory.ConfigsById)
            {
                parts.Add($"{pair.Value.DisplayName}: {runtime.Inventory.GetAmount(pair.Key):0}/{pair.Value.Capacity:0}");
            }

            resourcesText.text = string.Join("\n", parts);
            powerText.text = $"Reserve P {runtime.CalculateReservePower()}\nMarch P {runtime.CalculateFormationPower()}";
        }

        private void RefreshBuildings()
        {
            for (var i = 0; i < buildingViews.Count; i++)
            {
                var view = buildingViews[i];
                var height = 0.75f + view.State.Level * 0.22f;
                view.Body.localScale = new Vector3(1.25f, height, 1.25f);
                view.Body.position = new Vector3(view.Body.position.x, height * 0.5f, view.Body.position.z);
                view.Label.transform.position = new Vector3(view.Body.position.x, height + 0.22f, view.Body.position.z);
                view.Renderer.material.color = view.State.IsUpgrading
                    ? new Color(0.85f, 0.66f, 0.32f)
                    : view.State.IsMaxLevel ? new Color(0.38f, 0.68f, 0.46f) : new Color(0.55f, 0.52f, 0.44f);
                view.SelectionMarker.SetActive(selectionType == WorldSelectionType.Building && selectedBuilding == view.State);
                view.Label.text = $"{ShortName(view.State.Config.DisplayName)} Lv{view.State.Level}";
                view.UpgradeButton.SetEnabled(!view.State.IsMaxLevel && !view.State.IsUpgrading && view.State.NextLevel != null && runtime.Inventory.CanSpend(view.State.NextLevel.UpgradeCosts));
            }
        }

        private void RefreshTroops()
        {
            for (var i = 0; i < troopViews.Count; i++)
            {
                var view = troopViews[i];
                var reserve = runtime.TroopInventory.GetCount(view.Config.Id);
                var march = runtime.Formation.GetCount(view.Config.Id);
                view.Label.text = $"{ShortName(view.Config.DisplayName)}\nR{reserve} M{march}";
                view.SelectionMarker.SetActive(selectionType == WorldSelectionType.Troop && selectedTroop == view.Config);
                SetMarkerCount(view.ReserveMarkers, reserve);
                SetMarkerCount(view.MarchMarkers, march);
                view.TrainButton.SetEnabled(runtime.Inventory.CanSpend(view.Config.TrainingCosts));
                view.AddButton.SetEnabled(reserve > 0);
                view.RemoveButton.SetEnabled(march > 0);
            }
        }

        private void RefreshTechnologies()
        {
            for (var i = 0; i < technologyViews.Count; i++)
            {
                var view = technologyViews[i];
                var state = view.State;
                var nextLevel = state.NextLevel;
                var requirementsMet = nextLevel != null && runtime.AreTechnologyRequirementsMet(nextLevel);
                var canResearch = !state.IsMaxLevel && !state.IsResearching && requirementsMet && runtime.Inventory.CanSpend(nextLevel.ResearchCosts);
                view.Renderer.material.color = state.IsResearching
                    ? new Color(0.85f, 0.66f, 0.32f)
                    : state.IsMaxLevel ? new Color(0.38f, 0.68f, 0.46f)
                        : requirementsMet ? new Color(0.42f, 0.36f, 0.78f) : new Color(0.32f, 0.34f, 0.38f);
                var status = state.IsResearching ? $"{state.ResearchRemainingSeconds:0}s" : state.IsMaxLevel ? "Max" : requirementsMet ? "Ready" : "Lock";
                view.Label.text = $"{ShortName(state.Config.DisplayName)}\nLv{state.Level} {status}";
                view.SelectionMarker.SetActive(selectionType == WorldSelectionType.Technology && selectedTechnology == state);
                var linkColor = requirementsMet ? new Color(0.72f, 0.62f, 1f, 0.95f) : new Color(0.28f, 0.28f, 0.34f, 0.72f);
                for (var lineIndex = 0; lineIndex < view.PrerequisiteLines.Count; lineIndex++)
                {
                    view.PrerequisiteLines[lineIndex].startColor = linkColor;
                    view.PrerequisiteLines[lineIndex].endColor = linkColor;
                }

                view.ResearchButton.SetEnabled(canResearch);
            }
        }

        private void RefreshStrongholds()
        {
            for (var i = 0; i < strongholdViews.Count; i++)
            {
                var view = strongholdViews[i];
                var cleared = runtime.IsStrongholdCleared(view.Config);
                var unlocked = runtime.IsStrongholdUnlocked(view.Config);
                var marching = runtime.IsStrongholdMarching(view.Config);
                view.Renderer.material.color = cleared ? new Color(0.25f, 0.72f, 0.4f) : unlocked ? new Color(0.28f, 0.48f, 0.9f) : new Color(0.32f, 0.34f, 0.38f);
                var status = marching ? $"{runtime.GetStrongholdMarchRemainingSeconds(view.Config):0}s" : cleared ? "Clear" : unlocked ? "Ready" : "Lock";
                view.Label.text = $"{ShortName(view.Config.DisplayName)}\n{status}";
                view.SelectionMarker.SetActive(selectionType == WorldSelectionType.Stronghold && selectedStronghold == view.Config);
                SetMarkerCount(view.GarrisonMarkers, Mathf.CeilToInt(runtime.CalculateStrongholdEnemyPower(view.Config) / 20f));
                view.AttackButton.SetEnabled(unlocked && !marching && runtime.Formation.CalculateTotalCount() > 0);
            }
        }

        private void RefreshChapter()
        {
            if (runtime.Chapters.Count == 0)
            {
                chapterText.text = "Chapter None";
                return;
            }

            var chapter = runtime.Chapters[0];
            var cleared = 0;
            for (var i = 0; i < chapter.StrongholdIds.Count; i++)
            {
                if (FindStrongholdView(chapter.StrongholdIds[i]) is { } view && runtime.IsStrongholdCleared(view.Config))
                {
                    cleared++;
                }
            }

            var status = runtime.IsChapterComplete(chapter)
                ? runtime.IsChapterRewardClaimed(chapter) ? "Claimed" : "Complete"
                : "Progress";
            chapterText.text = $"{ShortName(chapter.DisplayName)}\n{status} {cleared}/{chapter.StrongholdIds.Count}";
        }

        private void RefreshReport()
        {
            if (runtime.LastBattleReport.HasValue)
            {
                var report = runtime.LastBattleReport.Value;
                var result = report.Victory ? report.FirstClearReward ? "Win First" : "Win" : "Lose";
                reportText.text = $"{result} {ShortName(report.Stronghold.DisplayName)}\nP {report.MarchPower}/{report.EnemyPower} L{report.Losses}";
                return;
            }

            reportText.text = runtime.MarchOrders.Count > 0 ? "Marching" : "Report None";
        }

        private void RefreshObjective()
        {
            if (objectiveText == null)
            {
                return;
            }

            var tasks = CollectObjectiveTargets();
            SetPrimaryObjective(tasks.Count > 0 ? tasks[0] : null);
            objectiveText.text = tasks.Count > 0 ? $"Next: {tasks[0].Text}" : "Next: Grow resources";
            objectiveButton?.SetEnabled(objectiveSelectionType != WorldSelectionType.None);
            taskPanel?.SetTasks(tasks, SelectObjectiveTarget);
        }

        private List<WorldObjectiveTarget> CollectObjectiveTargets()
        {
            var tasks = new List<WorldObjectiveTarget>();
            if (runtime.MarchOrders.Count > 0)
            {
                tasks.Add(WorldObjectiveTarget.None("Wait for march"));
                return tasks;
            }

            var chapterTarget = FindNextChapterStronghold();
            if (chapterTarget != null)
            {
                if (!runtime.IsStrongholdUnlocked(chapterTarget))
                {
                    var blockingTarget = FindBlockingStronghold(chapterTarget) ?? chapterTarget;
                    tasks.Add(WorldObjectiveTarget.ForStronghold($"Unlock {chapterTarget.DisplayName}", blockingTarget));
                    AddGrowthTasks(tasks);
                    return tasks;
                }

                if (runtime.Formation.CalculateTotalCount() > 0)
                {
                    tasks.Add(WorldObjectiveTarget.ForStronghold($"Attack {chapterTarget.DisplayName}", chapterTarget));
                    AddGrowthTasks(tasks);
                    return tasks;
                }

                var reserveTroop = FindReserveTroop();
                if (reserveTroop != null)
                {
                    tasks.Add(WorldObjectiveTarget.ForTroop("Add troops to march", reserveTroop));
                    AddGrowthTasks(tasks);
                    return tasks;
                }

                var recommendedTroop = runtime.CalculateStrongholdBattlePreview(chapterTarget).RecommendedTroop;
                tasks.Add(WorldObjectiveTarget.ForTroop(
                    recommendedTroop == null ? "Train troops" : $"Train {recommendedTroop.DisplayName}",
                    FindTrainableTroop(recommendedTroop) ?? recommendedTroop ?? FindTrainableTroop() ?? FindFirstTroop()));
                AddGrowthTasks(tasks);
                return tasks;
            }

            AddGrowthTasks(tasks);
            if (tasks.Count == 0)
            {
                tasks.Add(WorldObjectiveTarget.None("Grow resources"));
            }

            return tasks;
        }

        private void AddGrowthTasks(ICollection<WorldObjectiveTarget> tasks)
        {
            var researchTarget = FindResearchableTechnology();
            if (researchTarget != null)
            {
                tasks.Add(WorldObjectiveTarget.ForTechnology($"Research {researchTarget.Config.DisplayName}", researchTarget));
            }

            var upgradeTarget = FindUpgradeableBuilding();
            if (upgradeTarget != null)
            {
                tasks.Add(WorldObjectiveTarget.ForBuilding($"Upgrade {upgradeTarget.Config.DisplayName}", upgradeTarget));
            }
        }

        private void RefreshDetail()
        {
            if (detailPanel == null || runtime == null)
            {
                return;
            }

            switch (selectionType)
            {
                case WorldSelectionType.Building when selectedBuilding != null:
                    RefreshBuildingDetail(selectedBuilding);
                    break;
                case WorldSelectionType.Troop when selectedTroop != null:
                    RefreshTroopDetail(selectedTroop);
                    break;
                case WorldSelectionType.Technology when selectedTechnology != null:
                    RefreshTechnologyDetail(selectedTechnology);
                    break;
                case WorldSelectionType.Stronghold when selectedStronghold != null:
                    RefreshStrongholdDetail(selectedStronghold);
                    break;
                default:
                    detailPanel.Set("Selection", "Click a building, troop yard, technology, or stronghold to inspect its SLG data.");
                    detailPanel.SetActions();
                    break;
            }
        }

        private void RefreshBuildingDetail(BuildingRuntimeState state)
        {
            var lines = new List<string>
            {
                $"Level: {state.Level}/{state.Config.MaxLevel}",
                $"Production: {FormatProduction(state.CurrentLevel)}"
            };

            if (state.IsMaxLevel)
            {
                lines.Add("Status: Max level");
            }
            else if (state.IsUpgrading)
            {
                lines.Add($"Status: Upgrading {state.UpgradeRemainingSeconds:0.0}s");
            }
            else if (state.NextLevel != null)
            {
                lines.Add($"Next Cost: {FormatCosts(state.NextLevel.UpgradeCosts)}");
                lines.Add($"Upgrade Time: {state.NextLevel.UpgradeDurationSeconds:0.0}s");
                lines.Add(runtime.Inventory.CanSpend(state.NextLevel.UpgradeCosts) ? "Ready to upgrade" : "Need more resources");
            }

            detailPanel.Set(state.Config.DisplayName, string.Join("\n", lines));
            detailPanel.SetActions(
                "Upgrade",
                () => runtime.TryUpgrade(state),
                !state.IsMaxLevel && !state.IsUpgrading && state.NextLevel != null && runtime.Inventory.CanSpend(state.NextLevel.UpgradeCosts));
        }

        private void RefreshTroopDetail(TroopConfig config)
        {
            var reserve = runtime.TroopInventory.GetCount(config.Id);
            var march = runtime.Formation.GetCount(config.Id);
            var trainingCount = CountTrainingOrders(config.Id);
            var lines = new List<string>
            {
                $"Role: {config.Role}",
                $"Counters: {config.CounterRole} x{config.CounterMultiplier:0.##}",
                $"Reserve: {reserve}",
                $"March: {march}",
                $"Power Each: {runtime.GetEffectiveTroopPower(config)}",
                $"Train Time: {runtime.GetTrainingDuration(config):0.0}s",
                $"Train Cost: {FormatCosts(config.TrainingCosts)}"
            };

            if (trainingCount > 0)
            {
                lines.Add($"Training Orders: {trainingCount}");
            }

            lines.Add(runtime.Inventory.CanSpend(config.TrainingCosts) ? "Ready to train" : "Need more resources");
            detailPanel.Set(config.DisplayName, string.Join("\n", lines));
            detailPanel.SetActions(
                "Train",
                () => runtime.TryTrain(config),
                runtime.Inventory.CanSpend(config.TrainingCosts),
                "Add",
                () => runtime.TryAddToFormation(config),
                reserve > 0,
                "Remove",
                () => runtime.TryRemoveFromFormation(config),
                march > 0);
        }

        private void RefreshTechnologyDetail(TechnologyRuntimeState state)
        {
            var lines = new List<string>
            {
                $"Level: {state.Level}/{state.Config.MaxLevel}"
            };

            if (state.IsMaxLevel)
            {
                lines.Add("Status: Max level");
                lines.Add($"Current Effect: {FormatTechnologyEffects(state.CurrentLevel)}");
            }
            else if (state.IsResearching)
            {
                lines.Add($"Status: Researching {state.ResearchRemainingSeconds:0.0}s");
                lines.Add($"Next Effect: {FormatTechnologyEffects(state.NextLevel)}");
            }
            else if (state.NextLevel != null)
            {
                lines.Add($"Next Cost: {FormatCosts(state.NextLevel.ResearchCosts)}");
                lines.Add($"Research Time: {state.NextLevel.ResearchDurationSeconds:0.0}s");
                lines.Add($"Requires: {FormatTechnologyRequirements(state.NextLevel)}");
                lines.Add($"Next Effect: {FormatTechnologyEffects(state.NextLevel)}");
                lines.Add(runtime.AreTechnologyRequirementsMet(state.NextLevel)
                    ? runtime.Inventory.CanSpend(state.NextLevel.ResearchCosts) ? "Ready to research" : "Need more resources"
                    : "Requirements not met");
            }

            var canResearch = state.NextLevel != null
                && !state.IsMaxLevel
                && !state.IsResearching
                && runtime.AreTechnologyRequirementsMet(state.NextLevel)
                && runtime.Inventory.CanSpend(state.NextLevel.ResearchCosts);
            detailPanel.Set(state.Config.DisplayName, string.Join("\n", lines));
            detailPanel.SetActions("Research", () => runtime.TryResearch(state), canResearch);
        }

        private void RefreshStrongholdDetail(StrongholdConfig config)
        {
            var preview = runtime.CalculateStrongholdBattlePreview(config);
            var cleared = runtime.IsStrongholdCleared(config);
            var unlocked = runtime.IsStrongholdUnlocked(config);
            var marching = runtime.IsStrongholdMarching(config);
            var status = marching
                ? $"Marching {runtime.GetStrongholdMarchRemainingSeconds(config):0.0}s"
                : cleared ? "Cleared" : unlocked ? "Ready" : "Locked";
            var reward = cleared ? config.RepeatRewards : config.FirstClearRewards;
            var lines = new List<string>
            {
                $"Status: {status}",
                $"Enemy Main: {FormatRole(preview.EnemyMainRole)}",
                $"Recommended: {FormatRecommendedTroop(preview.RecommendedTroop)}",
                $"Enemy Power: {preview.EnemyPower}",
                $"Base March Power: {preview.BaseFormationPower}",
                $"Countered Power: {preview.CounteredFormationPower}",
                $"Preview: {FormatPreviewResult(preview.Result)}",
                $"Garrison: {FormatGarrison(config)}",
                $"Reward: {FormatCosts(reward)}",
                $"Requires: {FormatStrongholdRequirements(config)}",
                $"Victory Loss: {config.VictoryLossRate * 100f:0}%",
                $"Defeat Loss: {config.DefeatLossRate * 100f:0}%"
            };

            if (!unlocked)
            {
                lines.Add("Clear prerequisite strongholds first");
            }
            else if (runtime.Formation.CalculateTotalCount() <= 0)
            {
                lines.Add("Add troops to march before attack");
            }

            detailPanel.Set(config.DisplayName, string.Join("\n", lines));
            detailPanel.SetActions(
                "Attack",
                () => runtime.TryAttackStronghold(config),
                unlocked && !marching && runtime.Formation.CalculateTotalCount() > 0,
                "Recommend",
                () => runtime.TryRecommendFormation(config),
                unlocked && !marching && CalculateReserveTroopCount() > 0);
        }

        private WorldStrongholdView FindStrongholdView(string strongholdId)
        {
            for (var i = 0; i < strongholdViews.Count; i++)
            {
                if (strongholdViews[i].Config.Id == strongholdId)
                {
                    return strongholdViews[i];
                }
            }

            return null;
        }

        private static void SetMarkerCount(IReadOnlyList<GameObject> markers, int value)
        {
            var activeCount = Mathf.Clamp(value, 0, markers.Count);
            for (var i = 0; i < markers.Count; i++)
            {
                markers[i].SetActive(i < activeCount);
            }
        }

        private static string ShortName(string displayName)
        {
            return string.IsNullOrWhiteSpace(displayName) || displayName.Length <= 12
                ? displayName
                : displayName[..12];
        }

        private int CountTrainingOrders(string troopId)
        {
            var count = 0;
            for (var i = 0; i < runtime.TrainingOrders.Count; i++)
            {
                var order = runtime.TrainingOrders[i];
                if (order?.Config != null && order.Config.Id == troopId)
                {
                    count++;
                }
            }

            return count;
        }

        private int CalculateReserveTroopCount()
        {
            var total = 0;
            for (var i = 0; i < troopViews.Count; i++)
            {
                total += runtime.TroopInventory.GetCount(troopViews[i].Config.Id);
            }

            return total;
        }

        private TroopConfig FindReserveTroop()
        {
            for (var i = 0; i < troopViews.Count; i++)
            {
                var config = troopViews[i].Config;
                if (runtime.TroopInventory.GetCount(config.Id) > 0)
                {
                    return config;
                }
            }

            return null;
        }

        private TroopConfig FindTrainableTroop()
        {
            for (var i = 0; i < troopViews.Count; i++)
            {
                var config = troopViews[i].Config;
                if (runtime.Inventory.CanSpend(config.TrainingCosts))
                {
                    return config;
                }
            }

            return null;
        }

        private TroopConfig FindTrainableTroop(TroopConfig preferredTroop)
        {
            if (preferredTroop != null && runtime.Inventory.CanSpend(preferredTroop.TrainingCosts))
            {
                return preferredTroop;
            }

            return FindTrainableTroop();
        }

        private TroopConfig FindFirstTroop()
        {
            return troopViews.Count == 0 ? null : troopViews[0].Config;
        }

        private StrongholdConfig FindNextChapterStronghold()
        {
            if (runtime.Chapters.Count == 0)
            {
                return null;
            }

            var chapter = runtime.Chapters[0];
            for (var i = 0; i < chapter.StrongholdIds.Count; i++)
            {
                var view = FindStrongholdView(chapter.StrongholdIds[i]);
                if (view != null && !runtime.IsStrongholdCleared(view.Config))
                {
                    return view.Config;
                }
            }

            return null;
        }

        private StrongholdConfig FindBlockingStronghold(StrongholdConfig config)
        {
            if (config == null)
            {
                return null;
            }

            for (var i = 0; i < config.PrerequisiteStrongholdIds.Count; i++)
            {
                var view = FindStrongholdView(config.PrerequisiteStrongholdIds[i]);
                if (view != null && !runtime.IsStrongholdCleared(view.Config))
                {
                    return view.Config;
                }
            }

            return null;
        }

        private TechnologyRuntimeState FindResearchableTechnology()
        {
            for (var i = 0; i < technologyViews.Count; i++)
            {
                var state = technologyViews[i].State;
                if (state.NextLevel != null
                    && !state.IsMaxLevel
                    && !state.IsResearching
                    && runtime.AreTechnologyRequirementsMet(state.NextLevel)
                    && runtime.Inventory.CanSpend(state.NextLevel.ResearchCosts))
                {
                    return state;
                }
            }

            return null;
        }

        private void ClearObjectiveTarget()
        {
            objectiveSelectionType = WorldSelectionType.None;
            objectiveBuilding = null;
            objectiveTroop = null;
            objectiveTechnology = null;
            objectiveStronghold = null;
        }

        private void SetPrimaryObjective(WorldObjectiveTarget target)
        {
            ClearObjectiveTarget();
            if (target == null)
            {
                return;
            }

            objectiveSelectionType = target.SelectionType;
            objectiveBuilding = target.Building;
            objectiveTroop = target.Troop;
            objectiveTechnology = target.Technology;
            objectiveStronghold = target.Stronghold;
        }

        private void SetObjectiveBuilding(BuildingRuntimeState state)
        {
            objectiveSelectionType = WorldSelectionType.Building;
            objectiveBuilding = state;
        }

        private void SetObjectiveTroop(TroopConfig config)
        {
            objectiveSelectionType = config == null ? WorldSelectionType.None : WorldSelectionType.Troop;
            objectiveTroop = config;
        }

        private void SetObjectiveTechnology(TechnologyRuntimeState state)
        {
            objectiveSelectionType = WorldSelectionType.Technology;
            objectiveTechnology = state;
        }

        private void SetObjectiveStronghold(StrongholdConfig config)
        {
            objectiveSelectionType = config == null ? WorldSelectionType.None : WorldSelectionType.Stronghold;
            objectiveStronghold = config;
        }

        private BuildingRuntimeState FindUpgradeableBuilding()
        {
            for (var i = 0; i < buildingViews.Count; i++)
            {
                var state = buildingViews[i].State;
                if (state.NextLevel != null
                    && !state.IsMaxLevel
                    && !state.IsUpgrading
                    && runtime.Inventory.CanSpend(state.NextLevel.UpgradeCosts))
                {
                    return state;
                }
            }

            return null;
        }

        private static string FormatProduction(BuildingLevelConfig level)
        {
            if (level == null || level.Production.Count == 0)
            {
                return "None";
            }

            var parts = new List<string>();
            for (var i = 0; i < level.Production.Count; i++)
            {
                var production = level.Production[i];
                if (production != null && production.AmountPerSecond > 0f)
                {
                    parts.Add($"+{production.AmountPerSecond:0.#} {production.ResourceId}/s");
                }
            }

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        private static string FormatGarrison(StrongholdConfig config)
        {
            if (config == null || config.Garrison.Count == 0)
            {
                return "Fallback power";
            }

            var parts = new List<string>();
            for (var i = 0; i < config.Garrison.Count; i++)
            {
                var unit = config.Garrison[i];
                if (unit != null && unit.Count > 0 && !string.IsNullOrWhiteSpace(unit.TroopId))
                {
                    parts.Add($"{unit.TroopId} x{unit.Count}");
                }
            }

            return parts.Count == 0 ? "Fallback power" : string.Join(", ", parts);
        }

        private static string FormatRole(TroopRole? role)
        {
            return role.HasValue ? role.Value.ToString() : "Unknown";
        }

        private static string FormatRecommendedTroop(TroopConfig troop)
        {
            return troop == null ? "None" : $"{troop.DisplayName} vs {troop.CounterRole}";
        }

        private static string FormatPreviewResult(StrongholdBattlePreviewResult result)
        {
            return result switch
            {
                StrongholdBattlePreviewResult.ExpectedWin => "Expected Win",
                StrongholdBattlePreviewResult.Risky => "Risky",
                StrongholdBattlePreviewResult.NotEnoughPower => "Not Enough Power",
                _ => result.ToString()
            };
        }

        private static string FormatStrongholdRequirements(StrongholdConfig config)
        {
            if (config == null || config.PrerequisiteStrongholdIds.Count == 0)
            {
                return "None";
            }

            return string.Join(", ", config.PrerequisiteStrongholdIds);
        }

        private static string FormatTechnologyEffects(TechnologyLevelConfig level)
        {
            if (level == null || level.Effects.Count == 0)
            {
                return "None";
            }

            var parts = new List<string>();
            for (var i = 0; i < level.Effects.Count; i++)
            {
                var effect = level.Effects[i];
                if (effect == null || effect.Value == 0f)
                {
                    continue;
                }

                var target = string.IsNullOrWhiteSpace(effect.TargetId) ? "all" : effect.TargetId;
                parts.Add($"+{effect.Value * 100f:0}% {target} {FormatTechnologyEffectType(effect.Type)}");
            }

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        private static string FormatTechnologyRequirements(TechnologyLevelConfig level)
        {
            if (level == null)
            {
                return "None";
            }

            var parts = new List<string>();
            for (var i = 0; i < level.TechnologyPrerequisites.Count; i++)
            {
                var prerequisite = level.TechnologyPrerequisites[i];
                if (prerequisite != null && !string.IsNullOrWhiteSpace(prerequisite.TechnologyId))
                {
                    parts.Add($"{prerequisite.TechnologyId} Lv{prerequisite.RequiredLevel}");
                }
            }

            for (var i = 0; i < level.BuildingRequirements.Count; i++)
            {
                var requirement = level.BuildingRequirements[i];
                if (requirement != null && !string.IsNullOrWhiteSpace(requirement.BuildingId))
                {
                    parts.Add($"{requirement.BuildingId} Lv{requirement.RequiredLevel}");
                }
            }

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        private static string FormatTechnologyEffectType(TechnologyEffectType type)
        {
            return type switch
            {
                TechnologyEffectType.ResourceProductionMultiplier => "production",
                TechnologyEffectType.TrainingSpeedMultiplier => "training speed",
                TechnologyEffectType.TroopPowerMultiplier => "troop power",
                _ => type.ToString()
            };
        }

        private static string FormatCosts(IReadOnlyList<ResourceAmount> costs)
        {
            if (costs == null || costs.Count == 0)
            {
                return "Free";
            }

            var parts = new List<string>();
            for (var i = 0; i < costs.Count; i++)
            {
                if (costs[i] != null && costs[i].Amount > 0f)
                {
                    parts.Add($"{costs[i].Amount:0} {costs[i].ResourceId}");
                }
            }

            return parts.Count == 0 ? "Free" : string.Join(", ", parts);
        }
    }

    public sealed class WorldClickAction : MonoBehaviour
    {
        private bool interactable = true;
        private Action onClick;

        public void Configure(Action clickAction)
        {
            onClick = clickAction;
        }

        public void SetEnabled(bool enabled)
        {
            interactable = enabled;
        }

        private void OnMouseDown()
        {
            if (interactable)
            {
                onClick?.Invoke();
            }
        }
    }

    public enum WorldSelectionType
    {
        None,
        Building,
        Troop,
        Technology,
        Stronghold
    }

    public sealed class WorldDetailPanel
    {
        public WorldDetailPanel(Text title, Text body, WorldUiButton primaryButton, WorldUiButton secondaryButton, WorldUiButton tertiaryButton)
        {
            Title = title;
            Body = body;
            PrimaryButton = primaryButton;
            SecondaryButton = secondaryButton;
            TertiaryButton = tertiaryButton;
            SetActions();
        }

        public Text Title { get; }
        public Text Body { get; }
        public WorldUiButton PrimaryButton { get; }
        public WorldUiButton SecondaryButton { get; }
        public WorldUiButton TertiaryButton { get; }

        public void Set(string title, string body)
        {
            Title.text = title;
            Body.text = body;
        }

        public void SetActions(
            string primaryLabel = null,
            Action primaryAction = null,
            bool primaryEnabled = false,
            string secondaryLabel = null,
            Action secondaryAction = null,
            bool secondaryEnabled = false,
            string tertiaryLabel = null,
            Action tertiaryAction = null,
            bool tertiaryEnabled = false)
        {
            PrimaryButton.Configure(primaryLabel, primaryAction, primaryEnabled);
            SecondaryButton.Configure(secondaryLabel, secondaryAction, secondaryEnabled);
            TertiaryButton.Configure(tertiaryLabel, tertiaryAction, tertiaryEnabled);
        }
    }

    public sealed class WorldTaskPanel
    {
        private readonly IReadOnlyList<WorldTaskRow> rows;

        public WorldTaskPanel(IReadOnlyList<WorldTaskRow> taskRows)
        {
            rows = taskRows;
        }

        public void SetTasks(IReadOnlyList<WorldObjectiveTarget> tasks, Action<WorldObjectiveTarget> onSelect)
        {
            for (var i = 0; i < rows.Count; i++)
            {
                if (tasks != null && i < tasks.Count)
                {
                    rows[i].Set(tasks[i], onSelect);
                }
                else
                {
                    rows[i].Hide();
                }
            }
        }
    }

    public sealed class WorldTaskRow
    {
        public WorldTaskRow(Text label, WorldUiButton button)
        {
            Label = label;
            Button = button;
        }

        public Text Label { get; }
        public WorldUiButton Button { get; }

        public void Set(WorldObjectiveTarget target, Action<WorldObjectiveTarget> onSelect)
        {
            Label.gameObject.SetActive(true);
            Label.text = target.Text;
            var canSelect = target.SelectionType != WorldSelectionType.None;
            Button.Configure("Go", () => onSelect?.Invoke(target), canSelect);
        }

        public void Hide()
        {
            Label.gameObject.SetActive(false);
            Button.Configure(null, null, false);
        }
    }

    public sealed class WorldObjectiveTarget
    {
        public WorldObjectiveTarget(
            string text,
            WorldSelectionType selectionType,
            BuildingRuntimeState building,
            TroopConfig troop,
            TechnologyRuntimeState technology,
            StrongholdConfig stronghold)
        {
            Text = text;
            SelectionType = selectionType;
            Building = building;
            Troop = troop;
            Technology = technology;
            Stronghold = stronghold;
        }

        public string Text { get; }
        public WorldSelectionType SelectionType { get; }
        public BuildingRuntimeState Building { get; }
        public TroopConfig Troop { get; }
        public TechnologyRuntimeState Technology { get; }
        public StrongholdConfig Stronghold { get; }

        public static WorldObjectiveTarget None(string text)
        {
            return new WorldObjectiveTarget(text, WorldSelectionType.None, null, null, null, null);
        }

        public static WorldObjectiveTarget ForBuilding(string text, BuildingRuntimeState state)
        {
            return new WorldObjectiveTarget(text, state == null ? WorldSelectionType.None : WorldSelectionType.Building, state, null, null, null);
        }

        public static WorldObjectiveTarget ForTroop(string text, TroopConfig config)
        {
            return new WorldObjectiveTarget(text, config == null ? WorldSelectionType.None : WorldSelectionType.Troop, null, config, null, null);
        }

        public static WorldObjectiveTarget ForTechnology(string text, TechnologyRuntimeState state)
        {
            return new WorldObjectiveTarget(text, state == null ? WorldSelectionType.None : WorldSelectionType.Technology, null, null, state, null);
        }

        public static WorldObjectiveTarget ForStronghold(string text, StrongholdConfig config)
        {
            return new WorldObjectiveTarget(text, config == null ? WorldSelectionType.None : WorldSelectionType.Stronghold, null, null, null, config);
        }
    }

    public sealed class WorldUiButton
    {
        private readonly Color enabledColor = new(0.12f, 0.34f, 0.72f, 0.95f);
        private readonly Color disabledColor = new(0.18f, 0.2f, 0.23f, 0.9f);

        public WorldUiButton(RectTransform rectTransform, Image image, Button button, Text label)
        {
            RectTransform = rectTransform;
            Image = image;
            Button = button;
            Label = label;
        }

        public RectTransform RectTransform { get; }
        public Image Image { get; }
        public Button Button { get; }
        public Text Label { get; }

        public void Configure(string text, Action action, bool enabled)
        {
            var visible = !string.IsNullOrWhiteSpace(text);
            Button.gameObject.SetActive(visible);
            if (!visible)
            {
                return;
            }

            Label.text = text;
            Button.onClick.RemoveAllListeners();
            if (action != null)
            {
                Button.onClick.AddListener(() => action());
            }

            Button.interactable = enabled;
            Image.color = enabled ? enabledColor : disabledColor;
            Label.color = enabled ? Color.white : new Color(0.65f, 0.68f, 0.72f, 1f);
        }
    }

    public sealed class WorldTextBillboard : MonoBehaviour
    {
        private void LateUpdate()
        {
            var camera = Camera.main;
            if (camera != null)
            {
                transform.rotation = camera.transform.rotation;
            }
        }
    }

    public sealed class WorldBuildingView
    {
        public WorldBuildingView(BuildingRuntimeState state, Transform body, Renderer renderer, TextMesh label, GameObject selectionMarker, WorldButtonView upgradeButton)
        {
            State = state;
            Body = body;
            Renderer = renderer;
            Label = label;
            SelectionMarker = selectionMarker;
            UpgradeButton = upgradeButton;
        }

        public BuildingRuntimeState State { get; }
        public Transform Body { get; }
        public Renderer Renderer { get; }
        public TextMesh Label { get; }
        public GameObject SelectionMarker { get; }
        public WorldButtonView UpgradeButton { get; }
    }

    public sealed class WorldTroopView
    {
        public WorldTroopView(
            TroopConfig config,
            TextMesh label,
            GameObject selectionMarker,
            IReadOnlyList<GameObject> reserveMarkers,
            IReadOnlyList<GameObject> marchMarkers,
            WorldButtonView trainButton,
            WorldButtonView addButton,
            WorldButtonView removeButton)
        {
            Config = config;
            Label = label;
            SelectionMarker = selectionMarker;
            ReserveMarkers = reserveMarkers;
            MarchMarkers = marchMarkers;
            TrainButton = trainButton;
            AddButton = addButton;
            RemoveButton = removeButton;
        }

        public TroopConfig Config { get; }
        public TextMesh Label { get; }
        public GameObject SelectionMarker { get; }
        public IReadOnlyList<GameObject> ReserveMarkers { get; }
        public IReadOnlyList<GameObject> MarchMarkers { get; }
        public WorldButtonView TrainButton { get; }
        public WorldButtonView AddButton { get; }
        public WorldButtonView RemoveButton { get; }
    }

    public sealed class WorldStrongholdView
    {
        public WorldStrongholdView(StrongholdConfig config, Renderer renderer, TextMesh label, GameObject selectionMarker, IReadOnlyList<GameObject> garrisonMarkers, WorldButtonView attackButton)
        {
            Config = config;
            Renderer = renderer;
            Label = label;
            SelectionMarker = selectionMarker;
            GarrisonMarkers = garrisonMarkers;
            AttackButton = attackButton;
        }

        public StrongholdConfig Config { get; }
        public Renderer Renderer { get; }
        public TextMesh Label { get; }
        public GameObject SelectionMarker { get; }
        public IReadOnlyList<GameObject> GarrisonMarkers { get; }
        public WorldButtonView AttackButton { get; }
    }

    public sealed class WorldTechnologyView
    {
        public WorldTechnologyView(
            TechnologyRuntimeState state,
            Renderer renderer,
            TextMesh label,
            GameObject selectionMarker,
            IReadOnlyList<LineRenderer> prerequisiteLines,
            WorldButtonView researchButton)
        {
            State = state;
            Renderer = renderer;
            Label = label;
            SelectionMarker = selectionMarker;
            PrerequisiteLines = prerequisiteLines;
            ResearchButton = researchButton;
        }

        public TechnologyRuntimeState State { get; }
        public Renderer Renderer { get; }
        public TextMesh Label { get; }
        public GameObject SelectionMarker { get; }
        public IReadOnlyList<LineRenderer> PrerequisiteLines { get; }
        public WorldButtonView ResearchButton { get; }
    }

    public sealed class WorldButtonView
    {
        private readonly Color enabledColor;
        private readonly Color disabledColor = new(0.2f, 0.22f, 0.24f);

        private readonly WorldClickAction clickAction;

        public WorldButtonView(Renderer renderer, TextMesh label, WorldClickAction clickAction)
        {
            Renderer = renderer;
            Label = label;
            this.clickAction = clickAction;
            enabledColor = renderer.material.color;
        }

        public Renderer Renderer { get; }
        public TextMesh Label { get; }

        public void SetEnabled(bool enabled)
        {
            Renderer.material.color = enabled ? enabledColor : disabledColor;
            Label.color = enabled ? Color.white : new Color(0.65f, 0.65f, 0.65f);
            clickAction.SetEnabled(enabled);
        }
    }

    public sealed class WorldSlgMaterials
    {
        public WorldSlgMaterials()
        {
            BaseZone = Create(new Color(0.16f, 0.22f, 0.24f));
            MapZone = Create(new Color(0.19f, 0.19f, 0.24f));
            Building = Create(new Color(0.55f, 0.52f, 0.44f));
            Troop = Create(new Color(0.36f, 0.47f, 0.58f));
            Technology = Create(new Color(0.42f, 0.36f, 0.78f));
            Button = Create(new Color(0.15f, 0.35f, 0.78f));
            GoodButton = Create(new Color(0.18f, 0.55f, 0.32f));
            BadButton = Create(new Color(0.62f, 0.24f, 0.22f));
            Locked = Create(new Color(0.32f, 0.34f, 0.38f));
            Link = Create(new Color(0.68f, 0.7f, 0.72f));
            TechLink = Create(new Color(0.72f, 0.62f, 1f));
            ReserveMarker = Create(new Color(0.34f, 0.57f, 0.95f));
            MarchMarker = Create(new Color(0.28f, 0.78f, 0.42f));
            GarrisonMarker = Create(new Color(0.93f, 0.42f, 0.33f));
            Selection = Create(new Color(1f, 0.82f, 0.18f));
        }

        public Material BaseZone { get; }
        public Material MapZone { get; }
        public Material Building { get; }
        public Material Troop { get; }
        public Material Technology { get; }
        public Material Button { get; }
        public Material GoodButton { get; }
        public Material BadButton { get; }
        public Material Locked { get; }
        public Material Link { get; }
        public Material TechLink { get; }
        public Material ReserveMarker { get; }
        public Material MarchMarker { get; }
        public Material GarrisonMarker { get; }
        public Material Selection { get; }

        private static Material Create(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            if (shader == null)
            {
                shader = Shader.Find("Sprites/Default");
            }

            var material = new Material(shader);
            material.color = color;
            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", color);
            }

            return material;
        }
    }
}
