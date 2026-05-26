using System;
using System.Collections.Generic;
using SLGLearn.Battle;
using SLGLearn.Building;
using SLGLearn.Data;
using SLGLearn.Resource;
using SLGLearn.Save;
using SLGLearn.Technology;
using SLGLearn.Troop;
using UnityEngine;

namespace SLGLearn.Base
{
    public sealed class SlgBaseRuntime : MonoBehaviour
    {
        [SerializeField] private ResourceConfig[] resources;
        [SerializeField] private BuildingConfig[] buildings;
        [SerializeField] private TroopConfig[] troops;
        [SerializeField] private StrongholdConfig[] strongholds;
        [SerializeField] private ChapterConfig[] chapters;
        [SerializeField] private TechnologyConfig[] technologies;
        [SerializeField] private bool loadSaveOnStart = true;
        [SerializeField, Min(0.5f)] private float autoSaveInterval = 5f;
        [SerializeField, Min(0f)] private float maxOfflineSeconds = 7200f;
        [SerializeField, Min(0f)] private float defaultMarchDurationSeconds = 5f;

        private readonly List<BuildingRuntimeState> buildingStates = new();
        private readonly List<TroopTrainingOrder> trainingOrders = new();
        private readonly List<MarchOrder> marchOrders = new();
        private readonly List<TechnologyRuntimeState> technologyStates = new();
        private readonly HashSet<string> clearedStrongholdIds = new();
        private readonly HashSet<string> claimedChapterRewardIds = new();
        private string saveKey = SlgSaveSystem.DefaultSaveKey;
        private float autoSaveTimer;
        private bool saveDirty;
        private bool initialized;
        private BattleReport? lastBattleReport;

        public event Action Changed;

        public ResourceInventory Inventory { get; } = new();
        public TroopInventory TroopInventory { get; } = new();
        public MarchFormation Formation { get; } = new();
        public IReadOnlyList<BuildingRuntimeState> BuildingStates => buildingStates;
        public IReadOnlyList<TroopTrainingOrder> TrainingOrders => trainingOrders;
        public IReadOnlyList<MarchOrder> MarchOrders => marchOrders;
        public IReadOnlyList<TechnologyRuntimeState> TechnologyStates => technologyStates;
        public IReadOnlyList<StrongholdConfig> Strongholds => strongholds ?? Array.Empty<StrongholdConfig>();
        public IReadOnlyList<ChapterConfig> Chapters => chapters ?? Array.Empty<ChapterConfig>();
        public BattleReport? LastBattleReport => lastBattleReport;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (!initialized)
            {
                Initialize();
            }

            var deltaTime = Time.deltaTime;
            ProduceResources(deltaTime);
            TickBuildings(deltaTime);
            TickTraining(deltaTime);
            TickMarches(deltaTime);
            TickTechnologies(deltaTime);
            SaveIfNeeded(deltaTime);
        }

        public void Configure(ResourceConfig[] resourceConfigs, BuildingConfig[] buildingConfigs)
        {
            Configure(resourceConfigs, buildingConfigs, Array.Empty<TroopConfig>());
        }

        public void Configure(ResourceConfig[] resourceConfigs, BuildingConfig[] buildingConfigs, TroopConfig[] troopConfigs)
        {
            Configure(resourceConfigs, buildingConfigs, troopConfigs, Array.Empty<StrongholdConfig>());
        }

        public void Configure(
            ResourceConfig[] resourceConfigs,
            BuildingConfig[] buildingConfigs,
            TroopConfig[] troopConfigs,
            StrongholdConfig[] strongholdConfigs)
        {
            Configure(resourceConfigs, buildingConfigs, troopConfigs, strongholdConfigs, Array.Empty<TechnologyConfig>());
        }

        public void Configure(
            ResourceConfig[] resourceConfigs,
            BuildingConfig[] buildingConfigs,
            TroopConfig[] troopConfigs,
            StrongholdConfig[] strongholdConfigs,
            TechnologyConfig[] technologyConfigs)
        {
            Configure(resourceConfigs, buildingConfigs, troopConfigs, strongholdConfigs, Array.Empty<ChapterConfig>(), technologyConfigs);
        }

        public void Configure(
            ResourceConfig[] resourceConfigs,
            BuildingConfig[] buildingConfigs,
            TroopConfig[] troopConfigs,
            StrongholdConfig[] strongholdConfigs,
            ChapterConfig[] chapterConfigs,
            TechnologyConfig[] technologyConfigs)
        {
            resources = resourceConfigs;
            buildings = buildingConfigs;
            troops = troopConfigs;
            strongholds = strongholdConfigs;
            chapters = chapterConfigs;
            technologies = technologyConfigs;
            Initialize();
        }

        public void Initialize()
        {
            Inventory.Initialize(resources ?? Array.Empty<ResourceConfig>());
            Inventory.Changed -= NotifyChanged;
            Inventory.Changed += NotifyChanged;

            TroopInventory.Initialize(troops ?? Array.Empty<TroopConfig>());
            TroopInventory.Changed -= NotifyChanged;
            TroopInventory.Changed += NotifyChanged;

            Formation.Initialize(troops ?? Array.Empty<TroopConfig>());
            Formation.Changed -= NotifyChanged;
            Formation.Changed += NotifyChanged;

            buildingStates.Clear();
            if (buildings != null)
            {
                for (var i = 0; i < buildings.Length; i++)
                {
                    if (buildings[i] != null)
                    {
                        buildingStates.Add(new BuildingRuntimeState(buildings[i]));
                    }
                }
            }

            trainingOrders.Clear();
            technologyStates.Clear();
            if (technologies != null)
            {
                for (var i = 0; i < technologies.Length; i++)
                {
                    if (technologies[i] != null)
                    {
                        technologyStates.Add(new TechnologyRuntimeState(technologies[i]));
                    }
                }
            }

            clearedStrongholdIds.Clear();
            claimedChapterRewardIds.Clear();

            if (loadSaveOnStart)
            {
                LoadSave();
            }

            initialized = true;
            NotifyChanged();
        }

        public bool TryUpgrade(BuildingRuntimeState state)
        {
            if (state == null || state.IsMaxLevel || state.IsUpgrading)
            {
                return false;
            }

            var nextLevel = state.NextLevel;
            if (nextLevel == null || !Inventory.Spend(nextLevel.UpgradeCosts))
            {
                return false;
            }

            state.StartUpgrade(nextLevel.UpgradeDurationSeconds);
            NotifyChanged();
            SaveNow();
            return true;
        }

        public bool TryTrain(TroopConfig troop)
        {
            if (troop == null || !TroopInventory.ConfigsById.ContainsKey(troop.Id))
            {
                return false;
            }

            if (!Inventory.Spend(troop.TrainingCosts))
            {
                return false;
            }

            trainingOrders.Add(new TroopTrainingOrder(troop, GetTrainingDuration(troop)));
            NotifyChanged();
            SaveNow();
            return true;
        }

        public bool TryResearch(TechnologyRuntimeState state)
        {
            if (state == null || state.IsMaxLevel || state.IsResearching || !AreTechnologyRequirementsMet(state.NextLevel))
            {
                return false;
            }

            var nextLevel = state.NextLevel;
            if (nextLevel == null || !Inventory.Spend(nextLevel.ResearchCosts))
            {
                return false;
            }

            state.StartResearch(nextLevel.ResearchDurationSeconds);
            NotifyChanged();
            SaveNow();
            return true;
        }

        public bool TryAddToFormation(TroopConfig troop)
        {
            if (troop == null || TroopInventory.GetCount(troop.Id) <= 0)
            {
                return false;
            }

            TroopInventory.Add(troop.Id, -1);
            Formation.Add(troop.Id, 1);
            NotifyChanged();
            SaveNow();
            return true;
        }

        public bool TryRemoveFromFormation(TroopConfig troop)
        {
            if (troop == null || !Formation.Remove(troop.Id, 1))
            {
                return false;
            }

            TroopInventory.Add(troop.Id, 1);
            NotifyChanged();
            SaveNow();
            return true;
        }

        public bool TryRecommendFormation(StrongholdConfig stronghold)
        {
            if (stronghold == null || troops == null)
            {
                return false;
            }

            var movedAny = false;
            var enemyRole = GetStrongholdDominantRole(stronghold);
            var enemyPower = CalculateStrongholdEnemyPower(stronghold);
            movedAny |= MoveRecommendedTroopsToFormation(stronghold, enemyRole, enemyPower, true);
            if (CalculateFormationPowerAgainst(stronghold) < enemyPower)
            {
                movedAny |= MoveRecommendedTroopsToFormation(stronghold, enemyRole, enemyPower, false);
            }

            if (!movedAny)
            {
                return false;
            }

            NotifyChanged();
            SaveNow();
            return true;
        }

        public bool TryAttackStronghold(StrongholdConfig stronghold)
        {
            if (stronghold == null
                || Formation.CalculateTotalCount() <= 0
                || !IsStrongholdUnlocked(stronghold))
            {
                return false;
            }

            marchOrders.Add(new MarchOrder(stronghold, SnapshotFormation(), defaultMarchDurationSeconds));
            Formation.Clear();
            NotifyChanged();
            SaveNow();
            return true;
        }

        public void SaveNow()
        {
            SlgSaveSystem.Save(
                saveKey,
                Inventory,
                buildingStates,
                TroopInventory,
                Formation,
                trainingOrders,
                marchOrders,
                technologyStates,
                clearedStrongholdIds,
                claimedChapterRewardIds);
            saveDirty = false;
            autoSaveTimer = 0f;
        }

        public void DeleteSave()
        {
            SlgSaveSystem.Delete(saveKey);
        }

        public bool IsStrongholdCleared(StrongholdConfig stronghold)
        {
            return stronghold != null && clearedStrongholdIds.Contains(stronghold.Id);
        }

        public bool IsChapterComplete(ChapterConfig chapter)
        {
            if (chapter == null || chapter.StrongholdIds.Count == 0)
            {
                return false;
            }

            for (var i = 0; i < chapter.StrongholdIds.Count; i++)
            {
                if (!clearedStrongholdIds.Contains(chapter.StrongholdIds[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsChapterRewardClaimed(ChapterConfig chapter)
        {
            return chapter != null && claimedChapterRewardIds.Contains(chapter.Id);
        }

        public bool IsStrongholdUnlocked(StrongholdConfig stronghold)
        {
            if (IsStrongholdCleared(stronghold))
            {
                return true;
            }

            var index = FindStrongholdIndex(stronghold);
            if (index < 0)
            {
                return false;
            }

            if (stronghold.PrerequisiteStrongholdIds.Count > 0)
            {
                for (var i = 0; i < stronghold.PrerequisiteStrongholdIds.Count; i++)
                {
                    if (!clearedStrongholdIds.Contains(stronghold.PrerequisiteStrongholdIds[i]))
                    {
                        return false;
                    }
                }

                return true;
            }

            return index == 0 || IsStrongholdCleared(strongholds[index - 1]);
        }

        public bool IsStrongholdMarching(StrongholdConfig stronghold)
        {
            if (stronghold == null)
            {
                return false;
            }

            for (var i = 0; i < marchOrders.Count; i++)
            {
                if (marchOrders[i].Target == stronghold)
                {
                    return true;
                }
            }

            return false;
        }

        public float GetStrongholdMarchRemainingSeconds(StrongholdConfig stronghold)
        {
            if (stronghold == null)
            {
                return 0f;
            }

            for (var i = 0; i < marchOrders.Count; i++)
            {
                if (marchOrders[i].Target == stronghold)
                {
                    return marchOrders[i].RemainingSeconds;
                }
            }

            return 0f;
        }

        private void GrantRewards(IReadOnlyList<ResourceAmount> rewards)
        {
            for (var i = 0; i < rewards.Count; i++)
            {
                var reward = rewards[i];
                if (reward != null)
                {
                    Inventory.Add(reward.ResourceId, reward.Amount);
                }
            }
        }

        private void TryGrantCompletedChapterRewards()
        {
            if (chapters == null)
            {
                return;
            }

            for (var i = 0; i < chapters.Length; i++)
            {
                var chapter = chapters[i];
                if (chapter == null || claimedChapterRewardIds.Contains(chapter.Id) || !IsChapterComplete(chapter))
                {
                    continue;
                }

                GrantRewards(chapter.CompletionRewards);
                claimedChapterRewardIds.Add(chapter.Id);
            }
        }

        public int CalculateReservePower()
        {
            var total = 0;
            if (troops == null)
            {
                return total;
            }

            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop != null)
                {
                    total += GetEffectiveTroopPower(troop) * TroopInventory.GetCount(troop.Id);
                }
            }

            return total;
        }

        public int CalculateFormationPower()
        {
            var total = 0;
            if (troops == null)
            {
                return total;
            }

            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop != null)
                {
                    total += GetEffectiveTroopPower(troop) * Formation.GetCount(troop.Id);
                }
            }

            return total;
        }

        public int CalculateFormationPowerAgainst(StrongholdConfig stronghold)
        {
            var dominantRole = GetStrongholdDominantRole(stronghold);
            var total = 0;
            if (troops == null)
            {
                return total;
            }

            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop != null)
                {
                    total += GetEffectiveTroopPowerAgainst(troop, dominantRole) * Formation.GetCount(troop.Id);
                }
            }

            return total;
        }

        public StrongholdBattlePreview CalculateStrongholdBattlePreview(StrongholdConfig stronghold)
        {
            var enemyMainRole = GetStrongholdDominantRole(stronghold);
            var recommendedTroop = FindRecommendedTroop(enemyMainRole);
            var basePower = CalculateFormationPower();
            var counteredPower = CalculateFormationPowerAgainst(stronghold);
            var enemyPower = CalculateStrongholdEnemyPower(stronghold);
            var result = counteredPower >= enemyPower
                ? StrongholdBattlePreviewResult.ExpectedWin
                : counteredPower >= Mathf.CeilToInt(enemyPower * 0.75f)
                    ? StrongholdBattlePreviewResult.Risky
                    : StrongholdBattlePreviewResult.NotEnoughPower;
            return new StrongholdBattlePreview(enemyMainRole, recommendedTroop, basePower, counteredPower, enemyPower, result);
        }

        public int CalculateStrongholdEnemyPower(StrongholdConfig stronghold)
        {
            if (stronghold == null)
            {
                return 0;
            }

            if (stronghold.Garrison.Count == 0)
            {
                return stronghold.RecommendedPower;
            }

            var total = 0;
            for (var i = 0; i < stronghold.Garrison.Count; i++)
            {
                var unit = stronghold.Garrison[i];
                if (unit == null || unit.Count <= 0)
                {
                    continue;
                }

                var troop = FindTroop(unit.TroopId);
                if (troop != null)
                {
                    total += troop.Power * unit.Count;
                }
            }

            return total > 0 ? total : stronghold.RecommendedPower;
        }

        public int CalculateMarchOrderPower(MarchOrder order)
        {
            return CalculateMarchOrderPower(order, null);
        }

        public int CalculateMarchOrderPower(MarchOrder order, StrongholdConfig target)
        {
            if (order == null)
            {
                return 0;
            }

            var dominantRole = GetStrongholdDominantRole(target);
            var total = 0;
            foreach (var pair in order.CountsByTroopId)
            {
                var troop = FindTroop(pair.Key);
                if (troop != null)
                {
                    total += GetEffectiveTroopPowerAgainst(troop, dominantRole) * pair.Value;
                }
            }

            return total;
        }

        public int GetEffectiveTroopPower(TroopConfig troop)
        {
            if (troop == null)
            {
                return 0;
            }

            return Mathf.RoundToInt(troop.Power * GetTroopPowerMultiplier(troop.Id));
        }

        public int GetEffectiveTroopPowerAgainst(TroopConfig troop, TroopRole? defenderRole)
        {
            if (troop == null)
            {
                return 0;
            }

            var multiplier = GetTroopPowerMultiplier(troop.Id);
            if (defenderRole.HasValue && troop.CounterRole == defenderRole.Value)
            {
                multiplier *= troop.CounterMultiplier;
            }

            return Mathf.RoundToInt(troop.Power * multiplier);
        }

        public float GetTrainingDuration(TroopConfig troop)
        {
            if (troop == null)
            {
                return 0f;
            }

            return troop.TrainingDurationSeconds / GetTrainingSpeedMultiplier(troop.Id);
        }

        public bool AreTechnologyRequirementsMet(TechnologyLevelConfig level)
        {
            if (level == null)
            {
                return false;
            }

            for (var i = 0; i < level.TechnologyPrerequisites.Count; i++)
            {
                var prerequisite = level.TechnologyPrerequisites[i];
                if (prerequisite == null)
                {
                    return false;
                }

                if (GetTechnologyLevel(prerequisite.TechnologyId) < prerequisite.RequiredLevel)
                {
                    return false;
                }
            }

            for (var i = 0; i < level.BuildingRequirements.Count; i++)
            {
                var requirement = level.BuildingRequirements[i];
                if (requirement == null)
                {
                    return false;
                }

                if (GetBuildingLevel(requirement.BuildingId) < requirement.RequiredLevel)
                {
                    return false;
                }
            }

            return true;
        }

        public int GetTechnologyLevel(string technologyId)
        {
            var state = FindTechnology(technologyId);
            return state == null ? 0 : state.Level;
        }

        public int GetBuildingLevel(string buildingId)
        {
            var state = FindBuilding(buildingId);
            return state == null ? 0 : state.Level;
        }

        private void ProduceResources(float deltaTime)
        {
            for (var i = 0; i < buildingStates.Count; i++)
            {
                var level = buildingStates[i].CurrentLevel;
                if (level == null)
                {
                    continue;
                }

                for (var productionIndex = 0; productionIndex < level.Production.Count; productionIndex++)
                {
                    var production = level.Production[productionIndex];
                    if (production != null)
                    {
                        var multiplier = GetResourceProductionMultiplier(production.ResourceId);
                        Inventory.Add(production.ResourceId, production.AmountPerSecond * multiplier * deltaTime);
                    }
                }
            }
        }

        private void TickBuildings(float deltaTime)
        {
            var changed = false;
            for (var i = 0; i < buildingStates.Count; i++)
            {
                var wasUpgrading = buildingStates[i].IsUpgrading;
                buildingStates[i].Tick(deltaTime);
                changed |= wasUpgrading != buildingStates[i].IsUpgrading;
            }

            if (changed)
            {
                NotifyChanged();
                SaveNow();
            }
        }

        private void TickTechnologies(float deltaTime)
        {
            var changed = false;
            for (var i = 0; i < technologyStates.Count; i++)
            {
                var state = technologyStates[i];
                var wasResearching = state.IsResearching;
                state.Tick(deltaTime);
                changed |= wasResearching != state.IsResearching;
            }

            if (changed)
            {
                NotifyChanged();
                SaveNow();
            }
        }

        private void TickTraining(float deltaTime)
        {
            var changed = false;
            for (var i = trainingOrders.Count - 1; i >= 0; i--)
            {
                var order = trainingOrders[i];
                order.Tick(deltaTime);
                if (order.IsComplete)
                {
                    TroopInventory.Add(order.Config.Id, 1);
                    trainingOrders.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
            {
                NotifyChanged();
            }
        }

        private void TickMarches(float deltaTime)
        {
            var changed = false;
            for (var i = marchOrders.Count - 1; i >= 0; i--)
            {
                var order = marchOrders[i];
                order.Tick(deltaTime);
                if (order.IsComplete)
                {
                    ResolveMarchOrder(order);
                    marchOrders.RemoveAt(i);
                    changed = true;
                }
            }

            if (changed)
            {
                NotifyChanged();
                SaveNow();
            }
        }

        private void ResolveMarchOrder(MarchOrder order)
        {
            if (order?.Target == null)
            {
                return;
            }

            var wasCleared = IsStrongholdCleared(order.Target);
            var marchPower = CalculateMarchOrderPower(order, order.Target);
            var enemyPower = CalculateStrongholdEnemyPower(order.Target);
            var victory = marchPower >= enemyPower;
            var lossRate = victory ? order.Target.VictoryLossRate : order.Target.DefeatLossRate;
            var losses = ResolveMarchOrderAfterBattle(order, lossRate);

            if (victory)
            {
                var rewardList = wasCleared ? order.Target.RepeatRewards : order.Target.FirstClearRewards;
                GrantRewards(rewardList);
                clearedStrongholdIds.Add(order.Target.Id);
                TryGrantCompletedChapterRewards();
            }

            lastBattleReport = new BattleReport(order.Target, marchPower, enemyPower, victory, losses, victory && !wasCleared);
        }

        private void NotifyChanged()
        {
            Changed?.Invoke();
            if (initialized)
            {
                saveDirty = true;
            }
        }

        public TroopRole? GetStrongholdDominantRole(StrongholdConfig stronghold)
        {
            if (stronghold == null || stronghold.Garrison.Count == 0)
            {
                return null;
            }

            var countsByRole = new Dictionary<TroopRole, int>();
            for (var i = 0; i < stronghold.Garrison.Count; i++)
            {
                var unit = stronghold.Garrison[i];
                if (unit == null || unit.Count <= 0)
                {
                    continue;
                }

                var troop = FindTroop(unit.TroopId);
                if (troop == null)
                {
                    continue;
                }

                countsByRole.TryGetValue(troop.Role, out var count);
                countsByRole[troop.Role] = count + unit.Count;
            }

            TroopRole? dominantRole = null;
            var bestCount = 0;
            foreach (var pair in countsByRole)
            {
                if (pair.Value > bestCount)
                {
                    dominantRole = pair.Key;
                    bestCount = pair.Value;
                }
            }

            return dominantRole;
        }

        private TroopConfig FindRecommendedTroop(TroopRole? defenderRole)
        {
            if (!defenderRole.HasValue || troops == null)
            {
                return null;
            }

            TroopConfig bestTroop = null;
            var bestPower = 0;
            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop == null || troop.CounterRole != defenderRole.Value)
                {
                    continue;
                }

                var power = GetEffectiveTroopPowerAgainst(troop, defenderRole);
                if (power > bestPower)
                {
                    bestPower = power;
                    bestTroop = troop;
                }
            }

            return bestTroop;
        }

        private bool MoveRecommendedTroopsToFormation(StrongholdConfig stronghold, TroopRole? defenderRole, int enemyPower, bool countersOnly)
        {
            var movedAny = false;
            while (CalculateFormationPowerAgainst(stronghold) < enemyPower)
            {
                var troop = FindBestReserveTroopAgainst(defenderRole, countersOnly);
                if (troop == null)
                {
                    break;
                }

                TroopInventory.Add(troop.Id, -1);
                Formation.Add(troop.Id, 1);
                movedAny = true;
            }

            return movedAny;
        }

        private TroopConfig FindBestReserveTroopAgainst(TroopRole? defenderRole, bool countersOnly)
        {
            TroopConfig bestTroop = null;
            var bestPower = 0;
            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop == null || TroopInventory.GetCount(troop.Id) <= 0)
                {
                    continue;
                }

                if (countersOnly && (!defenderRole.HasValue || troop.CounterRole != defenderRole.Value))
                {
                    continue;
                }

                var power = GetEffectiveTroopPowerAgainst(troop, defenderRole);
                if (power > bestPower)
                {
                    bestPower = power;
                    bestTroop = troop;
                }
            }

            return bestTroop;
        }

        private void LoadSave()
        {
            if (!SlgSaveSystem.TryLoad(saveKey, out var data))
            {
                return;
            }

            for (var i = 0; i < data.resources.Count; i++)
            {
                var item = data.resources[i];
                if (item != null)
                {
                    Inventory.SetAmount(item.id, item.amount);
                }
            }

            for (var i = 0; i < data.buildings.Count; i++)
            {
                var saved = data.buildings[i];
                if (saved == null)
                {
                    continue;
                }

                var state = FindBuilding(saved.id);
                state?.Restore(saved.level, saved.upgradeRemainingSeconds);
            }

            if (data.troops != null)
            {
                for (var i = 0; i < data.troops.Count; i++)
                {
                    var saved = data.troops[i];
                    if (saved != null)
                    {
                        TroopInventory.SetCount(saved.id, saved.count);
                    }
                }
            }

            if (data.formation != null)
            {
                for (var i = 0; i < data.formation.Count; i++)
                {
                    var saved = data.formation[i];
                    if (saved != null)
                    {
                        Formation.SetCount(saved.id, saved.count);
                    }
                }
            }

            marchOrders.Clear();
            if (data.marchOrders != null)
            {
                for (var i = 0; i < data.marchOrders.Count; i++)
                {
                    var saved = data.marchOrders[i];
                    var stronghold = FindStronghold(saved?.strongholdId);
                    if (stronghold == null)
                    {
                        continue;
                    }

                    var counts = new Dictionary<string, int>();
                    if (saved.troops != null)
                    {
                        for (var troopIndex = 0; troopIndex < saved.troops.Count; troopIndex++)
                        {
                            var savedTroop = saved.troops[troopIndex];
                            if (savedTroop != null && FindTroop(savedTroop.id) != null && savedTroop.count > 0)
                            {
                                counts[savedTroop.id] = savedTroop.count;
                            }
                        }
                    }

                    if (counts.Count > 0)
                    {
                        marchOrders.Add(new MarchOrder(stronghold, counts, saved.remainingSeconds));
                    }
                }
            }

            trainingOrders.Clear();
            if (data.trainingOrders != null)
            {
                for (var i = 0; i < data.trainingOrders.Count; i++)
                {
                    var saved = data.trainingOrders[i];
                    var config = FindTroop(saved?.troopId);
                    if (config != null)
                    {
                        trainingOrders.Add(new TroopTrainingOrder(config, saved.remainingSeconds));
                    }
                }
            }

            if (data.technologies != null)
            {
                for (var i = 0; i < data.technologies.Count; i++)
                {
                    var saved = data.technologies[i];
                    if (saved == null)
                    {
                        continue;
                    }

                    var state = FindTechnology(saved.id);
                    state?.Restore(saved.level, saved.researchRemainingSeconds);
                }
            }

            clearedStrongholdIds.Clear();
            if (data.strongholds != null)
            {
                for (var i = 0; i < data.strongholds.Count; i++)
                {
                    var saved = data.strongholds[i];
                    if (saved != null && saved.cleared && FindStrongholdIndex(saved.id) >= 0)
                    {
                        clearedStrongholdIds.Add(saved.id);
                    }
                }
            }

            claimedChapterRewardIds.Clear();
            if (data.chapters != null)
            {
                for (var i = 0; i < data.chapters.Count; i++)
                {
                    var saved = data.chapters[i];
                    if (saved != null && saved.rewardClaimed && FindChapter(saved.id) != null)
                    {
                        claimedChapterRewardIds.Add(saved.id);
                    }
                }
            }

            TryGrantCompletedChapterRewards();
            ApplyOfflineProgress(data.savedUnixTimeSeconds);
        }

        private void ApplyOfflineProgress(long savedUnixTimeSeconds)
        {
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var elapsed = Mathf.Clamp(now - savedUnixTimeSeconds, 0f, maxOfflineSeconds);
            if (elapsed <= 0f)
            {
                return;
            }

            ApplyOfflineProgressSegments(elapsed);
        }

        private void ApplyOfflineProgressSegments(float elapsedSeconds)
        {
            const float minStepSeconds = 0.001f;
            const int maxSegments = 1024;

            var remainingSeconds = elapsedSeconds;
            var segmentCount = 0;
            while (remainingSeconds > minStepSeconds && segmentCount < maxSegments)
            {
                var nextEventSeconds = FindNextOfflineEventSeconds();
                var stepSeconds = nextEventSeconds <= 0f
                    ? remainingSeconds
                    : Mathf.Min(remainingSeconds, nextEventSeconds);

                ProduceResources(stepSeconds);
                TickBuildings(stepSeconds);
                TickTraining(stepSeconds);
                TickMarches(stepSeconds);
                TickTechnologies(stepSeconds);

                remainingSeconds -= stepSeconds;
                segmentCount++;

                if (nextEventSeconds <= 0f || nextEventSeconds > remainingSeconds + stepSeconds)
                {
                    break;
                }
            }

            if (remainingSeconds > minStepSeconds)
            {
                ProduceResources(remainingSeconds);
                TickBuildings(remainingSeconds);
                TickTraining(remainingSeconds);
                TickMarches(remainingSeconds);
                TickTechnologies(remainingSeconds);
            }
        }

        private float FindNextOfflineEventSeconds()
        {
            var nextSeconds = float.PositiveInfinity;
            for (var i = 0; i < buildingStates.Count; i++)
            {
                if (buildingStates[i].IsUpgrading)
                {
                    nextSeconds = Mathf.Min(nextSeconds, buildingStates[i].UpgradeRemainingSeconds);
                }
            }

            for (var i = 0; i < trainingOrders.Count; i++)
            {
                if (!trainingOrders[i].IsComplete)
                {
                    nextSeconds = Mathf.Min(nextSeconds, trainingOrders[i].RemainingSeconds);
                }
            }

            for (var i = 0; i < technologyStates.Count; i++)
            {
                if (technologyStates[i].IsResearching)
                {
                    nextSeconds = Mathf.Min(nextSeconds, technologyStates[i].ResearchRemainingSeconds);
                }
            }

            for (var i = 0; i < marchOrders.Count; i++)
            {
                if (!marchOrders[i].IsComplete)
                {
                    nextSeconds = Mathf.Min(nextSeconds, marchOrders[i].RemainingSeconds);
                }
            }

            return float.IsPositiveInfinity(nextSeconds) ? 0f : nextSeconds;
        }

        private BuildingRuntimeState FindBuilding(string id)
        {
            for (var i = 0; i < buildingStates.Count; i++)
            {
                if (buildingStates[i].Config != null && buildingStates[i].Config.Id == id)
                {
                    return buildingStates[i];
                }
            }

            return null;
        }

        private TroopConfig FindTroop(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || troops == null)
            {
                return null;
            }

            for (var i = 0; i < troops.Length; i++)
            {
                if (troops[i] != null && troops[i].Id == id)
                {
                    return troops[i];
                }
            }

            return null;
        }

        private TechnologyRuntimeState FindTechnology(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return null;
            }

            for (var i = 0; i < technologyStates.Count; i++)
            {
                if (technologyStates[i].Config != null && technologyStates[i].Config.Id == id)
                {
                    return technologyStates[i];
                }
            }

            return null;
        }

        private StrongholdConfig FindStronghold(string id)
        {
            var index = FindStrongholdIndex(id);
            return index < 0 ? null : strongholds[index];
        }

        private ChapterConfig FindChapter(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || chapters == null)
            {
                return null;
            }

            for (var i = 0; i < chapters.Length; i++)
            {
                if (chapters[i] != null && chapters[i].Id == id)
                {
                    return chapters[i];
                }
            }

            return null;
        }

        private int FindStrongholdIndex(StrongholdConfig stronghold)
        {
            return stronghold == null ? -1 : FindStrongholdIndex(stronghold.Id);
        }

        private int FindStrongholdIndex(string id)
        {
            if (string.IsNullOrWhiteSpace(id) || strongholds == null)
            {
                return -1;
            }

            for (var i = 0; i < strongholds.Length; i++)
            {
                if (strongholds[i] != null && strongholds[i].Id == id)
                {
                    return i;
                }
            }

            return -1;
        }

        private int ResolveFormationAfterBattle(float lossRate)
        {
            var losses = 0;
            if (troops == null)
            {
                return losses;
            }

            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop == null)
                {
                    continue;
                }

                var count = Formation.GetCount(troop.Id);
                if (count <= 0)
                {
                    continue;
                }

                var lost = Mathf.Clamp(Mathf.CeilToInt(count * lossRate), 0, count);
                var survivors = count - lost;
                losses += lost;
                if (survivors > 0)
                {
                    TroopInventory.Add(troop.Id, survivors);
                }

                Formation.SetCount(troop.Id, 0);
            }

            return losses;
        }

        private Dictionary<string, int> SnapshotFormation()
        {
            var snapshot = new Dictionary<string, int>();
            if (troops == null)
            {
                return snapshot;
            }

            for (var i = 0; i < troops.Length; i++)
            {
                var troop = troops[i];
                if (troop == null)
                {
                    continue;
                }

                var count = Formation.GetCount(troop.Id);
                if (count > 0)
                {
                    snapshot[troop.Id] = count;
                }
            }

            return snapshot;
        }

        private int ResolveMarchOrderAfterBattle(MarchOrder order, float lossRate)
        {
            var losses = 0;
            foreach (var pair in order.CountsByTroopId)
            {
                var count = pair.Value;
                if (count <= 0)
                {
                    continue;
                }

                var lost = Mathf.Clamp(Mathf.CeilToInt(count * lossRate), 0, count);
                var survivors = count - lost;
                losses += lost;
                if (survivors > 0)
                {
                    TroopInventory.Add(pair.Key, survivors);
                }
            }

            return losses;
        }

        private float GetResourceProductionMultiplier(string resourceId)
        {
            return 1f + GetTechnologyEffectBonus(TechnologyEffectType.ResourceProductionMultiplier, resourceId);
        }

        private float GetTrainingSpeedMultiplier(string troopId)
        {
            return Mathf.Max(0.01f, 1f + GetTechnologyEffectBonus(TechnologyEffectType.TrainingSpeedMultiplier, troopId));
        }

        private float GetTroopPowerMultiplier(string troopId)
        {
            return Mathf.Max(0.01f, 1f + GetTechnologyEffectBonus(TechnologyEffectType.TroopPowerMultiplier, troopId));
        }

        private float GetTechnologyEffectBonus(TechnologyEffectType effectType, string targetId)
        {
            var bonus = 0f;
            for (var i = 0; i < technologyStates.Count; i++)
            {
                var state = technologyStates[i];
                if (state.Level <= 0)
                {
                    continue;
                }

                for (var level = 1; level <= state.Level; level++)
                {
                    var config = state.Config.GetLevel(level);
                    if (config == null)
                    {
                        continue;
                    }

                    for (var effectIndex = 0; effectIndex < config.Effects.Count; effectIndex++)
                    {
                        var effect = config.Effects[effectIndex];
                        if (effect == null || effect.Type != effectType)
                        {
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(effect.TargetId) || effect.TargetId == targetId)
                        {
                            bonus += effect.Value;
                        }
                    }
                }
            }

            return bonus;
        }

        private void SaveIfNeeded(float deltaTime)
        {
            if (!saveDirty)
            {
                return;
            }

            autoSaveTimer += deltaTime;
            if (autoSaveTimer >= autoSaveInterval)
            {
                SaveNow();
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && initialized)
            {
                SaveNow();
            }
        }

        private void OnApplicationQuit()
        {
            if (initialized)
            {
                SaveNow();
            }
        }
    }

    public enum StrongholdBattlePreviewResult
    {
        ExpectedWin,
        Risky,
        NotEnoughPower
    }

    public readonly struct StrongholdBattlePreview
    {
        public StrongholdBattlePreview(
            TroopRole? enemyMainRole,
            TroopConfig recommendedTroop,
            int baseFormationPower,
            int counteredFormationPower,
            int enemyPower,
            StrongholdBattlePreviewResult result)
        {
            EnemyMainRole = enemyMainRole;
            RecommendedTroop = recommendedTroop;
            BaseFormationPower = baseFormationPower;
            CounteredFormationPower = counteredFormationPower;
            EnemyPower = enemyPower;
            Result = result;
        }

        public TroopRole? EnemyMainRole { get; }
        public TroopConfig RecommendedTroop { get; }
        public int BaseFormationPower { get; }
        public int CounteredFormationPower { get; }
        public int EnemyPower { get; }
        public StrongholdBattlePreviewResult Result { get; }
    }
}
