using System.Collections.Generic;
using SLGLearn.Base;
using SLGLearn.Building;
using SLGLearn.Data;
using SLGLearn.Technology;
using SLGLearn.Troop;
using UnityEngine;
using UnityEngine.UI;

namespace SLGLearn.UI
{
    public sealed class BaseHudController : MonoBehaviour
    {
        private readonly List<Text> resourceTexts = new();
        private readonly List<BuildingRow> buildingRows = new();
        private readonly List<TroopRow> troopRows = new();
        private readonly List<TechnologyRow> technologyRows = new();
        private readonly List<StrongholdRow> strongholdRows = new();
        private Text totalPowerText;
        private Text formationPowerText;
        private Text chapterText;
        private Text battleReportText;

        private SlgBaseRuntime runtime;

        public void Configure(
            SlgBaseRuntime baseRuntime,
            IReadOnlyList<Text> resources,
            IReadOnlyList<BuildingRow> rows,
            IReadOnlyList<TroopRow> troopTrainingRows,
            IReadOnlyList<TechnologyRow> techRows,
            Text powerText,
            Text marchPowerText,
            Text chapterStatusText,
            IReadOnlyList<StrongholdRow> mapRows,
            Text reportText)
        {
            runtime = baseRuntime;
            resourceTexts.Clear();
            resourceTexts.AddRange(resources);
            buildingRows.Clear();
            buildingRows.AddRange(rows);
            troopRows.Clear();
            troopRows.AddRange(troopTrainingRows);
            technologyRows.Clear();
            technologyRows.AddRange(techRows);
            strongholdRows.Clear();
            strongholdRows.AddRange(mapRows);
            totalPowerText = powerText;
            formationPowerText = marchPowerText;
            chapterText = chapterStatusText;
            battleReportText = reportText;

            runtime.Changed += Refresh;
            Refresh();
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
            if (runtime == null)
            {
                return;
            }

            var index = 0;
            foreach (var pair in runtime.Inventory.ConfigsById)
            {
                if (index >= resourceTexts.Count)
                {
                    break;
                }

                var config = pair.Value;
                var amount = runtime.Inventory.GetAmount(config.Id);
                resourceTexts[index].text = $"{config.DisplayName}: {amount:0} / {config.Capacity:0}";
                index++;
            }

            for (var i = 0; i < buildingRows.Count; i++)
            {
                if (i < runtime.BuildingStates.Count)
                {
                    RefreshBuildingRow(buildingRows[i], runtime.BuildingStates[i]);
                }
            }

            for (var i = 0; i < troopRows.Count; i++)
            {
                RefreshTroopRow(troopRows[i]);
            }

            for (var i = 0; i < technologyRows.Count; i++)
            {
                RefreshTechnologyRow(technologyRows[i]);
            }

            if (totalPowerText != null)
            {
                totalPowerText.text = $"Reserve Power: {runtime.CalculateReservePower()}";
            }

            if (formationPowerText != null)
            {
                formationPowerText.text = $"March Power: {runtime.CalculateFormationPower()}";
            }

            for (var i = 0; i < strongholdRows.Count; i++)
            {
                RefreshStrongholdRow(strongholdRows[i]);
            }

            RefreshChapterStatus();
            RefreshBattleReport();
        }

        private void RefreshChapterStatus()
        {
            if (chapterText == null)
            {
                return;
            }

            if (runtime.Chapters.Count == 0)
            {
                chapterText.text = "Chapter: None";
                return;
            }

            var chapter = runtime.Chapters[0];
            var cleared = CountClearedStrongholds(chapter);
            var status = runtime.IsChapterComplete(chapter)
                ? runtime.IsChapterRewardClaimed(chapter) ? "Complete | Reward Claimed" : "Complete"
                : "In Progress";
            chapterText.text = $"Chapter: {chapter.DisplayName} [{status}] {cleared}/{chapter.StrongholdIds.Count}\nReward: {FormatCosts(chapter.CompletionRewards)}";
        }

        private void RefreshBuildingRow(BuildingRow row, BuildingRuntimeState state)
        {
            row.NameText.text = $"{state.Config.DisplayName} Lv.{state.Level}";
            row.ProductionText.text = FormatProduction(state.CurrentLevel);

            if (state.IsMaxLevel)
            {
                row.StatusText.text = "Max Level";
                row.UpgradeButton.interactable = false;
                return;
            }

            if (state.IsUpgrading)
            {
                row.StatusText.text = $"Upgrading: {state.UpgradeRemainingSeconds:0.0}s";
                row.UpgradeButton.interactable = false;
                return;
            }

            var nextLevel = state.NextLevel;
            row.StatusText.text = nextLevel == null ? "No next level" : $"Cost: {FormatCosts(nextLevel.UpgradeCosts)}";
            row.UpgradeButton.interactable = nextLevel != null && runtime.Inventory.CanSpend(nextLevel.UpgradeCosts);
        }

        private void RefreshTroopRow(TroopRow row)
        {
            var config = row.Config;
            var count = runtime.TroopInventory.GetCount(config.Id);
            var formationCount = runtime.Formation.GetCount(config.Id);
            var trainingCount = CountTrainingOrders(config.Id);

            row.NameText.text = $"{config.DisplayName} Reserve {count} | March {formationCount}";
            row.PowerText.text = $"Power: {runtime.GetEffectiveTroopPower(config)} each | Train: {runtime.GetTrainingDuration(config):0.0}s";
            row.StatusText.text = trainingCount > 0
                ? $"Training: {trainingCount} | Cost: {FormatCosts(config.TrainingCosts)}"
                : $"Cost: {FormatCosts(config.TrainingCosts)}";
            row.TrainButton.interactable = runtime.Inventory.CanSpend(config.TrainingCosts);
            row.AddButton.interactable = count > 0;
            row.RemoveButton.interactable = formationCount > 0;
        }

        private int CountTrainingOrders(string troopId)
        {
            var count = 0;
            for (var i = 0; i < runtime.TrainingOrders.Count; i++)
            {
                var order = runtime.TrainingOrders[i];
                if (order.Config != null && order.Config.Id == troopId)
                {
                    count++;
                }
            }

            return count;
        }

        private void RefreshTechnologyRow(TechnologyRow row)
        {
            var state = row.State;
            row.NameText.text = $"{state.Config.DisplayName} Lv.{state.Level}";

            if (state.IsMaxLevel)
            {
                row.StatusText.text = "Max Level";
                row.EffectText.text = FormatTechnologyEffects(state.CurrentLevel);
                row.ResearchButton.interactable = false;
                return;
            }

            if (state.IsResearching)
            {
                row.StatusText.text = $"Researching: {state.ResearchRemainingSeconds:0.0}s";
                row.EffectText.text = FormatTechnologyEffects(state.NextLevel);
                row.ResearchButton.interactable = false;
                return;
            }

            var nextLevel = state.NextLevel;
            if (nextLevel == null)
            {
                row.StatusText.text = "No next level";
                row.EffectText.text = "No effect";
                row.ResearchButton.interactable = false;
                return;
            }

            if (!runtime.AreTechnologyRequirementsMet(nextLevel))
            {
                row.StatusText.text = $"Locked: {FormatTechnologyRequirements(nextLevel)}";
                row.EffectText.text = FormatTechnologyEffects(nextLevel);
                row.ResearchButton.interactable = false;
                return;
            }

            row.StatusText.text = $"Cost: {FormatCosts(nextLevel.ResearchCosts)}";
            row.EffectText.text = FormatTechnologyEffects(nextLevel);
            row.ResearchButton.interactable = runtime.Inventory.CanSpend(nextLevel.ResearchCosts);
        }

        private void RefreshStrongholdRow(StrongholdRow row)
        {
            var isCleared = runtime.IsStrongholdCleared(row.Config);
            var isUnlocked = runtime.IsStrongholdUnlocked(row.Config);
            var isMarching = runtime.IsStrongholdMarching(row.Config);
            var status = isMarching
                ? $"Marching {runtime.GetStrongholdMarchRemainingSeconds(row.Config):0.0}s"
                : isCleared ? "Cleared" : isUnlocked ? "Unlocked" : "Locked";
            var panelColor = isCleared
                ? new Color(0.08f, 0.2f, 0.15f, 0.9f)
                : isUnlocked
                    ? new Color(0.1f, 0.14f, 0.2f, 0.9f)
                    : new Color(0.09f, 0.09f, 0.1f, 0.75f);
            var nodeColor = isCleared
                ? new Color(0.24f, 0.78f, 0.48f, 1f)
                : isUnlocked
                    ? new Color(0.24f, 0.48f, 0.9f, 1f)
                    : new Color(0.36f, 0.38f, 0.42f, 1f);
            var linkColor = isUnlocked || isCleared
                ? new Color(0.24f, 0.48f, 0.9f, 0.95f)
                : new Color(0.26f, 0.28f, 0.32f, 0.8f);

            row.NameText.text = $"{row.Config.DisplayName} [{status}]";
            row.PowerText.text = $"Enemy: {runtime.CalculateStrongholdEnemyPower(row.Config)}\n{FormatStrongholdGarrison(row.Config)}";
            row.RewardText.text = isCleared
                ? $"Repeat: {FormatCosts(row.Config.RepeatRewards)}\nReq: {FormatStrongholdRequirements(row.Config)}"
                : $"First: {FormatCosts(row.Config.FirstClearRewards)}\nReq: {FormatStrongholdRequirements(row.Config)}";
            row.PanelImage.color = panelColor;
            row.NodeImage.color = nodeColor;
            for (var i = 0; i < row.PrerequisiteLineImages.Count; i++)
            {
                row.PrerequisiteLineImages[i].color = linkColor;
            }

            row.AttackButton.interactable = isUnlocked && !isMarching && runtime.Formation.CalculateTotalCount() > 0;
        }

        private void RefreshBattleReport()
        {
            if (battleReportText == null)
            {
                return;
            }

            var report = runtime.LastBattleReport;
            if (!report.HasValue)
            {
                battleReportText.text = runtime.MarchOrders.Count > 0
                    ? $"Marching: {FormatMarchOrders(runtime.MarchOrders)}"
                    : "Battle Report: None";
                return;
            }

            var value = report.Value;
            if (!value.Victory)
            {
                battleReportText.text = $"Battle Report: Defeat at {value.Stronghold.DisplayName}, Power {value.MarchPower} vs {value.EnemyPower}, Losses {value.Losses}";
                return;
            }

            var rewardType = value.FirstClearReward ? "First Clear" : "Repeat";
            battleReportText.text = $"Battle Report: Victory at {value.Stronghold.DisplayName}, {rewardType}, Power {value.MarchPower} vs {value.EnemyPower}, Losses {value.Losses}";
        }

        private static string FormatProduction(BuildingLevelConfig level)
        {
            if (level == null || level.Production.Count == 0)
            {
                return "No production";
            }

            var parts = new List<string>();
            for (var i = 0; i < level.Production.Count; i++)
            {
                var item = level.Production[i];
                if (item != null && item.AmountPerSecond > 0f)
                {
                    parts.Add($"+{item.AmountPerSecond:0.#} {item.ResourceId}/s");
                }
            }

            return parts.Count == 0 ? "No production" : string.Join(", ", parts);
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
                var cost = costs[i];
                if (cost != null && cost.Amount > 0f)
                {
                    parts.Add($"{cost.Amount:0} {cost.ResourceId}");
                }
            }

            return parts.Count == 0 ? "Free" : string.Join(", ", parts);
        }

        private static string FormatTechnologyEffects(TechnologyLevelConfig level)
        {
            if (level == null || level.Effects.Count == 0)
            {
                return "No effect";
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
                parts.Add($"+{effect.Value * 100f:0}% {target} {FormatEffectType(effect.Type)}");
            }

            return parts.Count == 0 ? "No effect" : string.Join(", ", parts);
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
                    parts.Add($"{prerequisite.TechnologyId} Lv.{prerequisite.RequiredLevel}");
                }
            }

            for (var i = 0; i < level.BuildingRequirements.Count; i++)
            {
                var requirement = level.BuildingRequirements[i];
                if (requirement != null && !string.IsNullOrWhiteSpace(requirement.BuildingId))
                {
                    parts.Add($"{requirement.BuildingId} Lv.{requirement.RequiredLevel}");
                }
            }

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        private static string FormatStrongholdRequirements(StrongholdConfig config)
        {
            if (config == null || config.PrerequisiteStrongholdIds.Count == 0)
            {
                return "None";
            }

            return string.Join(", ", config.PrerequisiteStrongholdIds);
        }

        private static string FormatStrongholdGarrison(StrongholdConfig config)
        {
            if (config == null || config.Garrison.Count == 0)
            {
                return "Fallback";
            }

            var parts = new List<string>();
            for (var i = 0; i < config.Garrison.Count; i++)
            {
                var unit = config.Garrison[i];
                if (unit != null && !string.IsNullOrWhiteSpace(unit.TroopId) && unit.Count > 0)
                {
                    parts.Add($"{unit.TroopId} x{unit.Count}");
                }
            }

            return parts.Count == 0 ? "Fallback" : string.Join(", ", parts);
        }

        private static string FormatMarchOrders(IReadOnlyList<MarchOrder> orders)
        {
            var parts = new List<string>();
            for (var i = 0; i < orders.Count; i++)
            {
                var order = orders[i];
                if (order?.Target != null)
                {
                    parts.Add($"{order.Target.DisplayName} {order.RemainingSeconds:0.0}s");
                }
            }

            return parts.Count == 0 ? "None" : string.Join(", ", parts);
        }

        private int CountClearedStrongholds(ChapterConfig chapter)
        {
            var count = 0;
            if (chapter == null)
            {
                return count;
            }

            for (var i = 0; i < chapter.StrongholdIds.Count; i++)
            {
                var stronghold = FindStrongholdRow(chapter.StrongholdIds[i])?.Config;
                if (stronghold != null && runtime.IsStrongholdCleared(stronghold))
                {
                    count++;
                }
            }

            return count;
        }

        private StrongholdRow FindStrongholdRow(string strongholdId)
        {
            for (var i = 0; i < strongholdRows.Count; i++)
            {
                if (strongholdRows[i].Config != null && strongholdRows[i].Config.Id == strongholdId)
                {
                    return strongholdRows[i];
                }
            }

            return null;
        }

        private static string FormatEffectType(TechnologyEffectType type)
        {
            return type switch
            {
                TechnologyEffectType.ResourceProductionMultiplier => "production",
                TechnologyEffectType.TrainingSpeedMultiplier => "training",
                TechnologyEffectType.TroopPowerMultiplier => "power",
                _ => "effect"
            };
        }
    }

    public sealed class BuildingRow
    {
        public BuildingRow(Text nameText, Text productionText, Text statusText, Button upgradeButton)
        {
            NameText = nameText;
            ProductionText = productionText;
            StatusText = statusText;
            UpgradeButton = upgradeButton;
        }

        public Text NameText { get; }
        public Text ProductionText { get; }
        public Text StatusText { get; }
        public Button UpgradeButton { get; }
    }

    public sealed class TroopRow
    {
        public TroopRow(
            TroopConfig config,
            Text nameText,
            Text powerText,
            Text statusText,
            Button trainButton,
            Button addButton,
            Button removeButton)
        {
            Config = config;
            NameText = nameText;
            PowerText = powerText;
            StatusText = statusText;
            TrainButton = trainButton;
            AddButton = addButton;
            RemoveButton = removeButton;
        }

        public TroopConfig Config { get; }
        public Text NameText { get; }
        public Text PowerText { get; }
        public Text StatusText { get; }
        public Button TrainButton { get; }
        public Button AddButton { get; }
        public Button RemoveButton { get; }
    }

    public sealed class TechnologyRow
    {
        public TechnologyRow(
            TechnologyRuntimeState state,
            Text nameText,
            Text statusText,
            Text effectText,
            Button researchButton)
        {
            State = state;
            NameText = nameText;
            StatusText = statusText;
            EffectText = effectText;
            ResearchButton = researchButton;
        }

        public TechnologyRuntimeState State { get; }
        public Text NameText { get; }
        public Text StatusText { get; }
        public Text EffectText { get; }
        public Button ResearchButton { get; }
    }

    public sealed class StrongholdRow
    {
        public StrongholdRow(
            StrongholdConfig config,
            Image panelImage,
            Image nodeImage,
            IReadOnlyList<Image> prerequisiteLineImages,
            Text nameText,
            Text powerText,
            Text rewardText,
            Button attackButton)
        {
            Config = config;
            PanelImage = panelImage;
            NodeImage = nodeImage;
            PrerequisiteLineImages = prerequisiteLineImages;
            NameText = nameText;
            PowerText = powerText;
            RewardText = rewardText;
            AttackButton = attackButton;
        }

        public StrongholdConfig Config { get; }
        public Image PanelImage { get; }
        public Image NodeImage { get; }
        public IReadOnlyList<Image> PrerequisiteLineImages { get; }
        public Text NameText { get; }
        public Text PowerText { get; }
        public Text RewardText { get; }
        public Button AttackButton { get; }
    }
}
