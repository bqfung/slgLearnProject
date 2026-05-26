using System.Collections.Generic;
using SLGLearn.Base;
using SLGLearn.Data;
using SLGLearn.Save;
using SLGLearn.UI;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace SLGLearn.EditorTools
{
    public enum SlgConfigIssueSeverity
    {
        Warning,
        Error
    }

    public sealed class SlgConfigValidationIssue
    {
        public SlgConfigValidationIssue(SlgConfigIssueSeverity severity, string message, string assetPath)
        {
            Severity = severity;
            Message = message;
            AssetPath = assetPath;
        }

        public SlgConfigIssueSeverity Severity { get; }
        public string Message { get; }
        public string AssetPath { get; }
    }

    public static class StageEightSceneBuilder
    {
        private const string ScenePath = "Assets/_Project/Scenes/Stage08_SlgBase.unity";
        private const string FoodPath = "Assets/_Project/ScriptableObjects/Stage08_Food.asset";
        private const string WoodPath = "Assets/_Project/ScriptableObjects/Stage08_Wood.asset";
        private const string FarmPath = "Assets/_Project/ScriptableObjects/Stage08_Farm.asset";
        private const string LumberMillPath = "Assets/_Project/ScriptableObjects/Stage08_LumberMill.asset";
        private const string HeadquartersPath = "Assets/_Project/ScriptableObjects/Stage08_Headquarters.asset";
        private const string InfantryPath = "Assets/_Project/ScriptableObjects/Stage09_Infantry.asset";
        private const string ArcherPath = "Assets/_Project/ScriptableObjects/Stage09_Archer.asset";
        private const string CavalryPath = "Assets/_Project/ScriptableObjects/Stage14_Cavalry.asset";
        private const string SiegePath = "Assets/_Project/ScriptableObjects/Stage14_Siege.asset";
        private const string OutpostPath = "Assets/_Project/ScriptableObjects/Stage10_Outpost.asset";
        private const string RaiderCampPath = "Assets/_Project/ScriptableObjects/Stage10_RaiderCamp.asset";
        private const string SupplyDepotPath = "Assets/_Project/ScriptableObjects/Stage13_SupplyDepot.asset";
        private const string BorderlandsChapterPath = "Assets/_Project/ScriptableObjects/Stage10_BorderlandsChapter.asset";
        private const string AgriculturePath = "Assets/_Project/ScriptableObjects/Stage11_Agriculture.asset";
        private const string MilitaryDrillPath = "Assets/_Project/ScriptableObjects/Stage11_MilitaryDrill.asset";
        private static readonly string[] ConfigSearchFolders = { "Assets/_Project/ScriptableObjects" };

        [MenuItem("SLG Learn/Build Stage 08 SLG Base Scene")]
        public static void BuildScene()
        {
            var food = LoadOrCreateResource(FoodPath, "food", "Food", 120f, 1000f);
            var wood = LoadOrCreateResource(WoodPath, "wood", "Wood", 120f, 1000f);
            var farm = LoadOrCreateBuilding(FarmPath, "farm", "Farm", "food", 1.6f, "wood", 60f);
            var lumberMill = LoadOrCreateBuilding(LumberMillPath, "lumber_mill", "Lumber Mill", "wood", 1.4f, "food", 50f);
            var headquarters = LoadOrCreateHeadquarters();
            var infantry = LoadOrCreateTroop(InfantryPath, "infantry", "Infantry", TroopRole.Infantry, TroopRole.Cavalry, 1.25f, 10, 4f, "food", 35f);
            var archer = LoadOrCreateTroop(ArcherPath, "archer", "Archer", TroopRole.Archer, TroopRole.Infantry, 1.25f, 14, 6f, "food", 45f, "wood", 20f);
            var cavalry = LoadOrCreateTroop(CavalryPath, "cavalry", "Cavalry", TroopRole.Cavalry, TroopRole.Archer, 1.3f, 18, 8f, "food", 70f, "wood", 35f);
            var siege = LoadOrCreateTroop(SiegePath, "siege", "Siege", TroopRole.Siege, TroopRole.Infantry, 1.4f, 28, 12f, "wood", 120f, "food", 40f);
            var outpost = LoadOrCreateStronghold(OutpostPath, "outpost", "Outpost", 20, 0.15f, 0.45f, "food", 70f, "wood", 30f);
            var raiderCamp = LoadOrCreateStronghold(RaiderCampPath, "raider_camp", "Raider Camp", 60, 0.25f, 0.6f, "food", 140f, "wood", 90f);
            var supplyDepot = LoadOrCreateStronghold(SupplyDepotPath, "supply_depot", "Supply Depot", 45, 0.2f, 0.55f, "wood", 160f, "food", 60f);
            var borderlands = LoadOrCreateChapter(BorderlandsChapterPath, "borderlands", "Borderlands", "food", 220f, "wood", 160f);
            var agriculture = LoadOrCreateTechnology(
                AgriculturePath,
                "agriculture",
                "Agriculture",
                TechnologyEffectType.ResourceProductionMultiplier,
                "food",
                0.2f,
                "wood",
                40f);
            var militaryDrill = LoadOrCreateTechnology(
                MilitaryDrillPath,
                "military_drill",
                "Military Drill",
                TechnologyEffectType.TrainingSpeedMultiplier,
                string.Empty,
                0.25f,
                "food",
                60f,
                TechnologyEffectType.TroopPowerMultiplier,
                string.Empty,
                0.15f);

            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            CreateCamera();
            CreateLight();
            CreateGround();

            var runtimeObject = new GameObject("SlgBaseRuntime");
            var runtime = runtimeObject.AddComponent<SlgBaseRuntime>();
            runtime.Configure(
                new[] { food, wood },
                new[] { headquarters, farm, lumberMill },
                new[] { infantry, archer, cavalry, siege },
                new[] { outpost, raiderCamp, supplyDepot },
                new[] { borderlands },
                new[] { agriculture, militaryDrill });
            runtimeObject.AddComponent<RuntimeBaseUiInstaller>().Configure(runtime);

            EditorSceneManager.SaveScene(scene, ScenePath);
            EditorSceneManager.OpenScene(ScenePath);
            Debug.Log($"Stage 08 SLG base scene created: {ScenePath}");
        }

        [MenuItem("SLG Learn/Clear Stage 08 SLG Save")]
        public static void ClearSave()
        {
            SlgSaveSystem.Delete(SlgSaveSystem.DefaultSaveKey);
            Debug.Log("Stage 08 SLG save cleared.");
        }

        [MenuItem("SLG Learn/Validate All SLG Configs")]
        public static void ValidateSlgConfigs()
        {
            var issues = RunSlgConfigValidation();
            var errorCount = CountIssues(issues, SlgConfigIssueSeverity.Error);
            var warningCount = CountIssues(issues, SlgConfigIssueSeverity.Warning);

            for (var i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == SlgConfigIssueSeverity.Error)
                {
                    Debug.LogError(issues[i].Message);
                }
                else
                {
                    Debug.LogWarning(issues[i].Message);
                }
            }

            var message = errorCount == 0
                ? $"SLG config validation passed with {warningCount} warning(s)."
                : $"SLG config validation found {errorCount} error(s) and {warningCount} warning(s). Check Console for details.";
            Debug.Log(message);
            EditorUtility.DisplayDialog("SLG Config Validation", message, "OK");
        }

        public static IReadOnlyList<SlgConfigValidationIssue> RunSlgConfigValidation()
        {
            var resources = LoadAllConfigs<ResourceConfig>();
            var buildings = LoadAllConfigs<BuildingConfig>();
            var troops = LoadAllConfigs<TroopConfig>();
            var strongholds = LoadAllConfigs<StrongholdConfig>();
            var chapters = LoadAllConfigs<ChapterConfig>();
            var technologies = LoadAllConfigs<TechnologyConfig>();

            var errors = new List<string>();
            var warnings = new List<string>();
            var resourceIds = ValidateResources(resources, errors);
            var troopIds = ValidateTroops(troops, resourceIds, errors, warnings);
            var buildingIdsByMaxLevel = CollectBuildingIds(buildings);
            var technologyIdsByMaxLevel = CollectTechnologyIds(technologies);
            var strongholdIds = CollectStrongholdIds(strongholds);

            ValidateBuildings(buildings, resourceIds, errors, warnings);
            ValidateStrongholds(strongholds, resourceIds, troopIds, strongholdIds, errors, warnings);
            ValidateChapters(chapters, resourceIds, strongholdIds, errors, warnings);
            ValidateTechnologies(technologies, resourceIds, troopIds, buildingIdsByMaxLevel, technologyIdsByMaxLevel, errors, warnings);

            var issues = new List<SlgConfigValidationIssue>();
            for (var i = 0; i < errors.Count; i++)
            {
                issues.Add(new SlgConfigValidationIssue(SlgConfigIssueSeverity.Error, errors[i], ExtractAssetPath(errors[i])));
            }

            for (var i = 0; i < warnings.Count; i++)
            {
                issues.Add(new SlgConfigValidationIssue(SlgConfigIssueSeverity.Warning, warnings[i], ExtractAssetPath(warnings[i])));
            }

            return issues;
        }

        private static T[] LoadAllConfigs<T>() where T : ScriptableObject
        {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}", ConfigSearchFolders);
            var configs = new List<T>();
            for (var i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var config = AssetDatabase.LoadAssetAtPath<T>(path);
                if (config != null)
                {
                    configs.Add(config);
                }
            }

            configs.Sort((left, right) => string.CompareOrdinal(AssetDatabase.GetAssetPath(left), AssetDatabase.GetAssetPath(right)));
            return configs.ToArray();
        }

        private static ResourceConfig LoadOrCreateResource(string path, string id, string displayName, float startingAmount, float capacity)
        {
            var config = AssetDatabase.LoadAssetAtPath<ResourceConfig>(path);
            if (config != null)
            {
                return config;
            }

            config = ScriptableObject.CreateInstance<ResourceConfig>();
            AssetDatabase.CreateAsset(config, path);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = id;
            serialized.FindProperty("displayName").stringValue = displayName;
            serialized.FindProperty("startingAmount").floatValue = startingAmount;
            serialized.FindProperty("capacity").floatValue = capacity;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static HashSet<string> ValidateResources(ResourceConfig[] resources, ICollection<string> errors)
        {
            var ids = new HashSet<string>();
            if (resources.Length == 0)
            {
                errors.Add("No ResourceConfig assets found under Assets/_Project/ScriptableObjects.");
            }

            for (var i = 0; i < resources.Length; i++)
            {
                var config = resources[i];
                if (config == null)
                {
                    errors.Add($"Resource config {i} is missing.");
                    continue;
                }

                var label = AssetLabel("Resource", config);
                ValidateUniqueId(label, config.Id, ids, errors);
                if (config.Capacity <= 0f)
                {
                    errors.Add($"{label} capacity must be greater than 0.");
                }

                if (config.StartingAmount > config.Capacity)
                {
                    errors.Add($"{label} starting amount exceeds capacity.");
                }
            }

            return ids;
        }

        private static void ValidateBuildings(
            BuildingConfig[] buildings,
            HashSet<string> resourceIds,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            var ids = new HashSet<string>();
            if (buildings.Length == 0)
            {
                errors.Add("No BuildingConfig assets found under Assets/_Project/ScriptableObjects.");
            }

            for (var i = 0; i < buildings.Length; i++)
            {
                var config = buildings[i];
                if (config == null)
                {
                    errors.Add($"Building config {i} is missing.");
                    continue;
                }

                var label = AssetLabel("Building", config);
                ValidateUniqueId(label, config.Id, ids, errors);
                if (config.MaxLevel <= 0)
                {
                    errors.Add($"{label} must have at least one level.");
                    continue;
                }

                for (var level = 1; level <= config.MaxLevel; level++)
                {
                    var levelConfig = config.GetLevel(level);
                    if (levelConfig == null)
                    {
                        errors.Add($"{label} level {level} is missing.");
                        continue;
                    }

                    ValidateAmounts($"{label} level {level} upgrade cost", levelConfig.UpgradeCosts, resourceIds, errors);
                    for (var productionIndex = 0; productionIndex < levelConfig.Production.Count; productionIndex++)
                    {
                        var production = levelConfig.Production[productionIndex];
                        if (production == null)
                        {
                            errors.Add($"{label} level {level} production {productionIndex} is null.");
                            continue;
                        }

                        ValidateResourceReference(
                            $"{label} level {level} production {productionIndex}",
                            production.ResourceId,
                            resourceIds,
                            errors);
                        if (production.AmountPerSecond <= 0f)
                        {
                            warnings.Add($"{label} level {level} production {productionIndex} has no positive output.");
                        }
                    }
                }
            }
        }

        private static HashSet<string> ValidateTroops(
            TroopConfig[] troops,
            HashSet<string> resourceIds,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            var ids = new HashSet<string>();
            if (troops.Length == 0)
            {
                errors.Add("No TroopConfig assets found under Assets/_Project/ScriptableObjects.");
            }

            for (var i = 0; i < troops.Length; i++)
            {
                var config = troops[i];
                if (config == null)
                {
                    errors.Add($"Troop config {i} is missing.");
                    continue;
                }

                var label = AssetLabel("Troop", config);
                ValidateUniqueId(label, config.Id, ids, errors);
                if (config.Power <= 0)
                {
                    errors.Add($"{label} power must be greater than 0.");
                }

                if (config.CounterMultiplier < 1f)
                {
                    errors.Add($"{label} counter multiplier must be at least 1.");
                }

                if (config.TrainingDurationSeconds <= 0f)
                {
                    warnings.Add($"{label} training duration is instant.");
                }

                ValidateAmounts($"{label} training cost", config.TrainingCosts, resourceIds, errors);
            }

            return ids;
        }

        private static void ValidateStrongholds(
            StrongholdConfig[] strongholds,
            HashSet<string> resourceIds,
            HashSet<string> troopIds,
            HashSet<string> strongholdIds,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            var ids = new HashSet<string>();
            var previousRecommendedPower = 0;
            if (strongholds.Length == 0)
            {
                warnings.Add("No StrongholdConfig assets found under Assets/_Project/ScriptableObjects.");
            }

            for (var i = 0; i < strongholds.Length; i++)
            {
                var config = strongholds[i];
                if (config == null)
                {
                    errors.Add($"Stronghold config {i} is missing.");
                    continue;
                }

                var label = AssetLabel("Stronghold", config);
                ValidateUniqueId(label, config.Id, ids, errors);
                if (config.RecommendedPower <= 0)
                {
                    errors.Add($"{label} recommended power must be greater than 0.");
                }

                if (i > 0 && config.RecommendedPower < previousRecommendedPower)
                {
                    warnings.Add($"{label} recommended power is lower than the previous stronghold.");
                }

                ValidateAmounts($"{label} first clear rewards", config.FirstClearRewards, resourceIds, errors);
                ValidateAmounts($"{label} repeat rewards", config.RepeatRewards, resourceIds, errors);
                ValidateStrongholdPrerequisites(label, config, strongholdIds, errors);
                ValidateStrongholdGarrison(label, config, troopIds, errors, warnings);
                previousRecommendedPower = config.RecommendedPower;
            }
        }

        private static void ValidateStrongholdGarrison(
            string label,
            StrongholdConfig config,
            HashSet<string> troopIds,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            if (config.Garrison.Count == 0)
            {
                warnings.Add($"{label} has no garrison and will fall back to recommended power.");
                return;
            }

            var totalCount = 0;
            for (var i = 0; i < config.Garrison.Count; i++)
            {
                var unit = config.Garrison[i];
                if (unit == null)
                {
                    errors.Add($"{label} garrison {i} is null.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(unit.TroopId))
                {
                    errors.Add($"{label} garrison {i} has empty troop id.");
                    continue;
                }

                if (!troopIds.Contains(unit.TroopId))
                {
                    errors.Add($"{label} garrison {i} references unknown troop id '{unit.TroopId}'.");
                }

                if (unit.Count <= 0)
                {
                    errors.Add($"{label} garrison {i} count must be greater than 0.");
                }

                totalCount += Mathf.Max(0, unit.Count);
            }

            if (totalCount <= 0)
            {
                errors.Add($"{label} garrison has no positive troop count.");
            }
        }

        private static void ValidateStrongholdPrerequisites(
            string label,
            StrongholdConfig config,
            HashSet<string> strongholdIds,
            ICollection<string> errors)
        {
            var seen = new HashSet<string>();
            for (var i = 0; i < config.PrerequisiteStrongholdIds.Count; i++)
            {
                var prerequisiteId = config.PrerequisiteStrongholdIds[i];
                if (string.IsNullOrWhiteSpace(prerequisiteId))
                {
                    errors.Add($"{label} prerequisite {i} has empty stronghold id.");
                    continue;
                }

                if (prerequisiteId == config.Id)
                {
                    errors.Add($"{label} prerequisite {i} references itself.");
                    continue;
                }

                if (!strongholdIds.Contains(prerequisiteId))
                {
                    errors.Add($"{label} prerequisite {i} references unknown stronghold id '{prerequisiteId}'.");
                }

                if (!seen.Add(prerequisiteId))
                {
                    errors.Add($"{label} prerequisite {i} duplicates stronghold id '{prerequisiteId}'.");
                }
            }
        }

        private static void ValidateChapters(
            ChapterConfig[] chapters,
            HashSet<string> resourceIds,
            HashSet<string> strongholdIds,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            var ids = new HashSet<string>();
            if (chapters.Length == 0)
            {
                warnings.Add("No ChapterConfig assets found under Assets/_Project/ScriptableObjects.");
            }

            for (var i = 0; i < chapters.Length; i++)
            {
                var config = chapters[i];
                if (config == null)
                {
                    errors.Add($"Chapter config {i} is missing.");
                    continue;
                }

                var label = AssetLabel("Chapter", config);
                ValidateUniqueId(label, config.Id, ids, errors);
                if (config.StrongholdIds.Count == 0)
                {
                    errors.Add($"{label} must reference at least one stronghold.");
                }

                var seenStrongholds = new HashSet<string>();
                for (var strongholdIndex = 0; strongholdIndex < config.StrongholdIds.Count; strongholdIndex++)
                {
                    var strongholdId = config.StrongholdIds[strongholdIndex];
                    if (string.IsNullOrWhiteSpace(strongholdId))
                    {
                        errors.Add($"{label} stronghold {strongholdIndex} has empty id.");
                        continue;
                    }

                    if (!strongholdIds.Contains(strongholdId))
                    {
                        errors.Add($"{label} stronghold {strongholdIndex} references unknown stronghold id '{strongholdId}'.");
                    }

                    if (!seenStrongholds.Add(strongholdId))
                    {
                        errors.Add($"{label} stronghold {strongholdIndex} duplicates id '{strongholdId}'.");
                    }
                }

                ValidateAmounts($"{label} completion rewards", config.CompletionRewards, resourceIds, errors);
            }
        }

        private static void ValidateTechnologies(
            TechnologyConfig[] technologies,
            HashSet<string> resourceIds,
            HashSet<string> troopIds,
            IReadOnlyDictionary<string, int> buildingIdsByMaxLevel,
            IReadOnlyDictionary<string, int> technologyIdsByMaxLevel,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            var ids = new HashSet<string>();
            if (technologies.Length == 0)
            {
                warnings.Add("No TechnologyConfig assets found under Assets/_Project/ScriptableObjects.");
            }

            for (var i = 0; i < technologies.Length; i++)
            {
                var config = technologies[i];
                if (config == null)
                {
                    errors.Add($"Technology config {i} is missing.");
                    continue;
                }

                var label = AssetLabel("Technology", config);
                ValidateUniqueId(label, config.Id, ids, errors);
                if (config.MaxLevel <= 0)
                {
                    errors.Add($"{label} must have at least one level.");
                    continue;
                }

                for (var level = 1; level <= config.MaxLevel; level++)
                {
                    var levelConfig = config.GetLevel(level);
                    if (levelConfig == null)
                    {
                        errors.Add($"{label} level {level} is missing.");
                        continue;
                    }

                    ValidateAmounts($"{label} level {level} research cost", levelConfig.ResearchCosts, resourceIds, errors);
                    ValidateTechnologyPrerequisites(
                        $"{label} level {level}",
                        config.Id,
                        levelConfig.TechnologyPrerequisites,
                        technologyIdsByMaxLevel,
                        errors);
                    ValidateBuildingRequirements(
                        $"{label} level {level}",
                        levelConfig.BuildingRequirements,
                        buildingIdsByMaxLevel,
                        errors);
                    if (levelConfig.ResearchDurationSeconds <= 0f)
                    {
                        warnings.Add($"{label} level {level} research duration is instant.");
                    }

                    for (var effectIndex = 0; effectIndex < levelConfig.Effects.Count; effectIndex++)
                    {
                        ValidateTechnologyEffect(
                            $"{label} level {level} effect {effectIndex}",
                            levelConfig.Effects[effectIndex],
                            resourceIds,
                            troopIds,
                            errors,
                            warnings);
                    }
                }
            }
        }

        private static void ValidateTechnologyEffect(
            string label,
            TechnologyEffect effect,
            HashSet<string> resourceIds,
            HashSet<string> troopIds,
            ICollection<string> errors,
            ICollection<string> warnings)
        {
            if (effect == null)
            {
                errors.Add($"{label} is null.");
                return;
            }

            if (effect.Value == 0f)
            {
                warnings.Add($"{label} has zero value.");
            }

            if (string.IsNullOrWhiteSpace(effect.TargetId))
            {
                return;
            }

            if (effect.Type == TechnologyEffectType.ResourceProductionMultiplier)
            {
                ValidateResourceReference(label, effect.TargetId, resourceIds, errors);
                return;
            }

            if (!troopIds.Contains(effect.TargetId))
            {
                errors.Add($"{label} references unknown troop id '{effect.TargetId}'.");
            }
        }

        private static void ValidateTechnologyPrerequisites(
            string label,
            string selfTechnologyId,
            IReadOnlyList<TechnologyPrerequisite> prerequisites,
            IReadOnlyDictionary<string, int> technologyIdsByMaxLevel,
            ICollection<string> errors)
        {
            if (prerequisites == null)
            {
                return;
            }

            for (var i = 0; i < prerequisites.Count; i++)
            {
                var prerequisite = prerequisites[i];
                if (prerequisite == null)
                {
                    errors.Add($"{label} prerequisite {i} is null.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(prerequisite.TechnologyId))
                {
                    errors.Add($"{label} prerequisite {i} has empty technology id.");
                    continue;
                }

                if (prerequisite.TechnologyId == selfTechnologyId)
                {
                    errors.Add($"{label} prerequisite {i} references itself.");
                    continue;
                }

                if (!technologyIdsByMaxLevel.TryGetValue(prerequisite.TechnologyId, out var maxLevel))
                {
                    errors.Add($"{label} prerequisite {i} references unknown technology id '{prerequisite.TechnologyId}'.");
                    continue;
                }

                if (prerequisite.RequiredLevel <= 0 || prerequisite.RequiredLevel > maxLevel)
                {
                    errors.Add($"{label} prerequisite {i} requires {prerequisite.TechnologyId} Lv.{prerequisite.RequiredLevel}, but max level is {maxLevel}.");
                }
            }
        }

        private static void ValidateBuildingRequirements(
            string label,
            IReadOnlyList<BuildingLevelRequirement> requirements,
            IReadOnlyDictionary<string, int> buildingIdsByMaxLevel,
            ICollection<string> errors)
        {
            if (requirements == null)
            {
                return;
            }

            for (var i = 0; i < requirements.Count; i++)
            {
                var requirement = requirements[i];
                if (requirement == null)
                {
                    errors.Add($"{label} building requirement {i} is null.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(requirement.BuildingId))
                {
                    errors.Add($"{label} building requirement {i} has empty building id.");
                    continue;
                }

                if (!buildingIdsByMaxLevel.TryGetValue(requirement.BuildingId, out var maxLevel))
                {
                    errors.Add($"{label} building requirement {i} references unknown building id '{requirement.BuildingId}'.");
                    continue;
                }

                if (requirement.RequiredLevel <= 0 || requirement.RequiredLevel > maxLevel)
                {
                    errors.Add($"{label} building requirement {i} requires {requirement.BuildingId} Lv.{requirement.RequiredLevel}, but max level is {maxLevel}.");
                }
            }
        }

        private static Dictionary<string, int> CollectBuildingIds(BuildingConfig[] buildings)
        {
            var ids = new Dictionary<string, int>();
            for (var i = 0; i < buildings.Length; i++)
            {
                var config = buildings[i];
                if (config != null && !string.IsNullOrWhiteSpace(config.Id))
                {
                    ids[config.Id] = config.MaxLevel;
                }
            }

            return ids;
        }

        private static Dictionary<string, int> CollectTechnologyIds(TechnologyConfig[] technologies)
        {
            var ids = new Dictionary<string, int>();
            for (var i = 0; i < technologies.Length; i++)
            {
                var config = technologies[i];
                if (config != null && !string.IsNullOrWhiteSpace(config.Id))
                {
                    ids[config.Id] = config.MaxLevel;
                }
            }

            return ids;
        }

        private static HashSet<string> CollectStrongholdIds(StrongholdConfig[] strongholds)
        {
            var ids = new HashSet<string>();
            for (var i = 0; i < strongholds.Length; i++)
            {
                var config = strongholds[i];
                if (config != null && !string.IsNullOrWhiteSpace(config.Id))
                {
                    ids.Add(config.Id);
                }
            }

            return ids;
        }

        private static void ValidateAmounts(
            string label,
            IReadOnlyList<ResourceAmount> amounts,
            HashSet<string> resourceIds,
            ICollection<string> errors)
        {
            if (amounts == null)
            {
                return;
            }

            for (var i = 0; i < amounts.Count; i++)
            {
                var amount = amounts[i];
                if (amount == null)
                {
                    errors.Add($"{label} item {i} is null.");
                    continue;
                }

                ValidateResourceReference($"{label} item {i}", amount.ResourceId, resourceIds, errors);
            }
        }

        private static void ValidateResourceReference(
            string label,
            string resourceId,
            HashSet<string> resourceIds,
            ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(resourceId))
            {
                errors.Add($"{label} has empty resource id.");
                return;
            }

            if (!resourceIds.Contains(resourceId))
            {
                errors.Add($"{label} references unknown resource id '{resourceId}'.");
            }
        }

        private static void ValidateUniqueId(string label, string id, ISet<string> ids, ICollection<string> errors)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                errors.Add($"{label} id is empty.");
                return;
            }

            if (!ids.Add(id))
            {
                errors.Add($"{label} id '{id}' is duplicated.");
            }
        }

        private static string AssetLabel(string label, Object asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            return string.IsNullOrWhiteSpace(path)
                ? $"{label} '{asset.name}'"
                : $"{label} '{asset.name}' ({path})";
        }

        private static string ExtractAssetPath(string message)
        {
            var start = message.IndexOf("(Assets/");
            if (start < 0)
            {
                return string.Empty;
            }

            start++;
            var end = message.IndexOf(')', start);
            return end <= start ? string.Empty : message.Substring(start, end - start);
        }

        private static int CountIssues(IReadOnlyList<SlgConfigValidationIssue> issues, SlgConfigIssueSeverity severity)
        {
            var count = 0;
            for (var i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }

        private static BuildingConfig LoadOrCreateBuilding(
            string path,
            string id,
            string displayName,
            string productionResourceId,
            float baseProduction,
            string costResourceId,
            float baseCost)
        {
            var config = AssetDatabase.LoadAssetAtPath<BuildingConfig>(path);
            if (config != null)
            {
                return config;
            }

            config = ScriptableObject.CreateInstance<BuildingConfig>();
            AssetDatabase.CreateAsset(config, path);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = id;
            serialized.FindProperty("displayName").stringValue = displayName;

            var levels = serialized.FindProperty("levels");
            levels.arraySize = 3;
            WriteLevel(levels.GetArrayElementAtIndex(0), 0f, productionResourceId, baseProduction);
            WriteLevel(levels.GetArrayElementAtIndex(1), 4f, productionResourceId, baseProduction * 1.8f, costResourceId, baseCost);
            WriteLevel(levels.GetArrayElementAtIndex(2), 6f, productionResourceId, baseProduction * 3f, costResourceId, baseCost * 2f);

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static BuildingConfig LoadOrCreateHeadquarters()
        {
            var config = AssetDatabase.LoadAssetAtPath<BuildingConfig>(HeadquartersPath);
            if (config != null)
            {
                return config;
            }

            config = ScriptableObject.CreateInstance<BuildingConfig>();
            AssetDatabase.CreateAsset(config, HeadquartersPath);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = "headquarters";
            serialized.FindProperty("displayName").stringValue = "Headquarters";

            var levels = serialized.FindProperty("levels");
            levels.arraySize = 3;
            WriteLevel(levels.GetArrayElementAtIndex(0), 0f);
            WriteLevel(levels.GetArrayElementAtIndex(1), 5f, string.Empty, 0f, "food", 80f, "wood", 80f);
            WriteLevel(levels.GetArrayElementAtIndex(2), 8f, string.Empty, 0f, "food", 160f, "wood", 160f);

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static TroopConfig LoadOrCreateTroop(
            string path,
            string id,
            string displayName,
            TroopRole role,
            TroopRole counterRole,
            float counterMultiplier,
            int power,
            float trainingDuration,
            string costResourceId,
            float costAmount,
            string secondCostResourceId = "",
            float secondCostAmount = 0f)
        {
            var config = AssetDatabase.LoadAssetAtPath<TroopConfig>(path);
            if (config != null)
            {
                ApplyDefaultTroop(config, role, counterRole, counterMultiplier);
                return config;
            }

            config = ScriptableObject.CreateInstance<TroopConfig>();
            AssetDatabase.CreateAsset(config, path);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = id;
            serialized.FindProperty("displayName").stringValue = displayName;
            serialized.FindProperty("role").enumValueIndex = (int)role;
            serialized.FindProperty("counterRole").enumValueIndex = (int)counterRole;
            serialized.FindProperty("counterMultiplier").floatValue = counterMultiplier;
            serialized.FindProperty("power").intValue = power;
            serialized.FindProperty("trainingDurationSeconds").floatValue = trainingDuration;

            var costs = serialized.FindProperty("trainingCosts");
            costs.arraySize = secondCostAmount > 0f ? 2 : 1;
            WriteAmount(costs.GetArrayElementAtIndex(0), costResourceId, costAmount);
            if (costs.arraySize > 1)
            {
                WriteAmount(costs.GetArrayElementAtIndex(1), secondCostResourceId, secondCostAmount);
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static void ApplyDefaultTroop(TroopConfig config, TroopRole role, TroopRole counterRole, float counterMultiplier)
        {
            if (config == null)
            {
                return;
            }

            var serialized = new SerializedObject(config);
            serialized.FindProperty("role").enumValueIndex = (int)role;
            serialized.FindProperty("counterRole").enumValueIndex = (int)counterRole;
            serialized.FindProperty("counterMultiplier").floatValue = counterMultiplier;
            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }

        private static StrongholdConfig LoadOrCreateStronghold(
            string path,
            string id,
            string displayName,
            int recommendedPower,
            float victoryLossRate,
            float defeatLossRate,
            string rewardResourceId,
            float rewardAmount,
            string secondRewardResourceId = "",
            float secondRewardAmount = 0f)
        {
            var config = AssetDatabase.LoadAssetAtPath<StrongholdConfig>(path);
            if (config != null)
            {
                ApplyDefaultStrongholdPrerequisites(config);
                return config;
            }

            config = ScriptableObject.CreateInstance<StrongholdConfig>();
            AssetDatabase.CreateAsset(config, path);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = id;
            serialized.FindProperty("displayName").stringValue = displayName;
            serialized.FindProperty("recommendedPower").intValue = recommendedPower;
            serialized.FindProperty("victoryLossRate").floatValue = victoryLossRate;
            serialized.FindProperty("defeatLossRate").floatValue = defeatLossRate;

            var rewards = serialized.FindProperty("rewards");
            rewards.arraySize = secondRewardAmount > 0f ? 2 : 1;
            WriteAmount(rewards.GetArrayElementAtIndex(0), rewardResourceId, rewardAmount);
            if (rewards.arraySize > 1)
            {
                WriteAmount(rewards.GetArrayElementAtIndex(1), secondRewardResourceId, secondRewardAmount);
            }

            var repeatRewards = serialized.FindProperty("repeatRewards");
            if (repeatRewards != null)
            {
                WriteStrongholdRepeatRewards(repeatRewards, rewardResourceId, rewardAmount, secondRewardResourceId, secondRewardAmount);
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            ApplyDefaultStrongholdPrerequisites(config);
            return config;
        }

        private static void ApplyDefaultStrongholdPrerequisites(StrongholdConfig config)
        {
            if (config == null)
            {
                return;
            }

            var serialized = new SerializedObject(config);
            var prerequisites = serialized.FindProperty("prerequisiteStrongholdIds");
            if (prerequisites != null && (config.Id == "raider_camp" || config.Id == "supply_depot"))
            {
                prerequisites.arraySize = 1;
                prerequisites.GetArrayElementAtIndex(0).stringValue = "outpost";
            }
            else if (prerequisites != null && config.Id == "outpost")
            {
                prerequisites.arraySize = 0;
            }

            var mapPosition = serialized.FindProperty("mapPosition");
            if (mapPosition != null)
            {
                if (config.Id == "outpost")
                {
                    mapPosition.vector2Value = new Vector2(32f, -548f);
                }
                else if (config.Id == "raider_camp")
                {
                    mapPosition.vector2Value = new Vector2(360f, -494f);
                }
                else if (config.Id == "supply_depot")
                {
                    mapPosition.vector2Value = new Vector2(360f, -610f);
                }
            }

            var garrison = serialized.FindProperty("garrison");
            if (garrison != null)
            {
                if (config.Id == "outpost")
                {
                    WriteStrongholdGarrison(garrison, "infantry", 2);
                }
                else if (config.Id == "raider_camp")
                {
                    WriteStrongholdGarrison(garrison, "infantry", 4, "archer", 2);
                }
                else if (config.Id == "supply_depot")
                {
                    WriteStrongholdGarrison(garrison, "archer", 2, "cavalry", 2);
                }
            }

            var repeatRewards = serialized.FindProperty("repeatRewards");
            if (repeatRewards != null && repeatRewards.arraySize == 0)
            {
                if (config.Id == "outpost")
                {
                    WriteStrongholdRepeatRewards(repeatRewards, "food", 70f, "wood", 30f);
                }
                else if (config.Id == "raider_camp")
                {
                    WriteStrongholdRepeatRewards(repeatRewards, "food", 140f, "wood", 90f);
                }
                else if (config.Id == "supply_depot")
                {
                    WriteStrongholdRepeatRewards(repeatRewards, "wood", 160f, "food", 60f);
                }
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }

        private static void WriteStrongholdRepeatRewards(
            SerializedProperty repeatRewards,
            string firstResourceId,
            float firstAmount,
            string secondResourceId = "",
            float secondAmount = 0f)
        {
            repeatRewards.arraySize = secondAmount > 0f ? 2 : 1;
            WriteAmount(repeatRewards.GetArrayElementAtIndex(0), firstResourceId, Mathf.Max(1f, firstAmount * 0.25f));
            if (repeatRewards.arraySize > 1)
            {
                WriteAmount(repeatRewards.GetArrayElementAtIndex(1), secondResourceId, Mathf.Max(1f, secondAmount * 0.25f));
            }
        }

        private static void WriteStrongholdGarrison(
            SerializedProperty garrison,
            string firstTroopId,
            int firstCount,
            string secondTroopId = "",
            int secondCount = 0)
        {
            garrison.arraySize = secondCount > 0 ? 2 : 1;
            WriteGarrisonUnit(garrison.GetArrayElementAtIndex(0), firstTroopId, firstCount);
            if (garrison.arraySize > 1)
            {
                WriteGarrisonUnit(garrison.GetArrayElementAtIndex(1), secondTroopId, secondCount);
            }
        }

        private static void WriteGarrisonUnit(SerializedProperty unit, string troopId, int count)
        {
            unit.FindPropertyRelative("troopId").stringValue = troopId;
            unit.FindPropertyRelative("count").intValue = count;
        }

        private static ChapterConfig LoadOrCreateChapter(
            string path,
            string id,
            string displayName,
            string rewardResourceId,
            float rewardAmount,
            string secondRewardResourceId = "",
            float secondRewardAmount = 0f)
        {
            var config = AssetDatabase.LoadAssetAtPath<ChapterConfig>(path);
            if (config != null)
            {
                ApplyDefaultChapter(config);
                return config;
            }

            config = ScriptableObject.CreateInstance<ChapterConfig>();
            AssetDatabase.CreateAsset(config, path);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = id;
            serialized.FindProperty("displayName").stringValue = displayName;
            WriteChapterStrongholds(serialized.FindProperty("strongholdIds"));
            var rewards = serialized.FindProperty("completionRewards");
            rewards.arraySize = secondRewardAmount > 0f ? 2 : 1;
            WriteAmount(rewards.GetArrayElementAtIndex(0), rewardResourceId, rewardAmount);
            if (rewards.arraySize > 1)
            {
                WriteAmount(rewards.GetArrayElementAtIndex(1), secondRewardResourceId, secondRewardAmount);
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            return config;
        }

        private static void ApplyDefaultChapter(ChapterConfig config)
        {
            if (config == null || config.Id != "borderlands")
            {
                return;
            }

            var serialized = new SerializedObject(config);
            WriteChapterStrongholds(serialized.FindProperty("strongholdIds"));
            var rewards = serialized.FindProperty("completionRewards");
            if (rewards != null && rewards.arraySize == 0)
            {
                rewards.arraySize = 2;
                WriteAmount(rewards.GetArrayElementAtIndex(0), "food", 220f);
                WriteAmount(rewards.GetArrayElementAtIndex(1), "wood", 160f);
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }

        private static void WriteChapterStrongholds(SerializedProperty strongholdIds)
        {
            if (strongholdIds == null)
            {
                return;
            }

            strongholdIds.arraySize = 3;
            strongholdIds.GetArrayElementAtIndex(0).stringValue = "outpost";
            strongholdIds.GetArrayElementAtIndex(1).stringValue = "raider_camp";
            strongholdIds.GetArrayElementAtIndex(2).stringValue = "supply_depot";
        }

        private static TechnologyConfig LoadOrCreateTechnology(
            string path,
            string id,
            string displayName,
            TechnologyEffectType firstEffectType,
            string firstEffectTarget,
            float firstEffectValue,
            string costResourceId,
            float costAmount,
            TechnologyEffectType secondEffectType = TechnologyEffectType.ResourceProductionMultiplier,
            string secondEffectTarget = "",
            float secondEffectValue = 0f)
        {
            var config = AssetDatabase.LoadAssetAtPath<TechnologyConfig>(path);
            if (config != null)
            {
                if (!HasAnyTechnologyRequirements(config))
                {
                    ApplyDefaultTechnologyRequirements(config);
                }

                return config;
            }

            config = ScriptableObject.CreateInstance<TechnologyConfig>();
            AssetDatabase.CreateAsset(config, path);

            var serialized = new SerializedObject(config);
            serialized.FindProperty("id").stringValue = id;
            serialized.FindProperty("displayName").stringValue = displayName;

            var levels = serialized.FindProperty("levels");
            levels.arraySize = 2;
            WriteTechnologyLevel(
                levels.GetArrayElementAtIndex(0),
                5f,
                costResourceId,
                costAmount,
                firstEffectType,
                firstEffectTarget,
                firstEffectValue);
            WriteTechnologyLevel(
                levels.GetArrayElementAtIndex(1),
                8f,
                costResourceId,
                costAmount * 2f,
                firstEffectType,
                firstEffectTarget,
                firstEffectValue,
                secondEffectType,
                secondEffectTarget,
                secondEffectValue);

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
            ApplyDefaultTechnologyRequirements(config);
            return config;
        }

        private static void WriteLevel(
            SerializedProperty level,
            float upgradeDuration,
            string productionResourceId = "",
            float productionPerSecond = 0f,
            string costResourceId = "",
            float costAmount = 0f,
            string secondCostResourceId = "",
            float secondCostAmount = 0f)
        {
            level.FindPropertyRelative("upgradeDurationSeconds").floatValue = upgradeDuration;

            var costs = level.FindPropertyRelative("upgradeCosts");
            costs.arraySize = costAmount > 0f ? secondCostAmount > 0f ? 2 : 1 : 0;
            if (costs.arraySize > 0)
            {
                WriteAmount(costs.GetArrayElementAtIndex(0), costResourceId, costAmount);
            }

            if (costs.arraySize > 1)
            {
                WriteAmount(costs.GetArrayElementAtIndex(1), secondCostResourceId, secondCostAmount);
            }

            var production = level.FindPropertyRelative("production");
            production.arraySize = productionPerSecond > 0f ? 1 : 0;
            if (production.arraySize > 0)
            {
                var item = production.GetArrayElementAtIndex(0);
                item.FindPropertyRelative("resourceId").stringValue = productionResourceId;
                item.FindPropertyRelative("amountPerSecond").floatValue = productionPerSecond;
            }
        }

        private static void WriteAmount(SerializedProperty amount, string resourceId, float value)
        {
            amount.FindPropertyRelative("resourceId").stringValue = resourceId;
            amount.FindPropertyRelative("amount").floatValue = value;
        }

        private static void WriteTechnologyLevel(
            SerializedProperty level,
            float researchDuration,
            string costResourceId,
            float costAmount,
            TechnologyEffectType firstEffectType,
            string firstEffectTarget,
            float firstEffectValue,
            TechnologyEffectType secondEffectType = TechnologyEffectType.ResourceProductionMultiplier,
            string secondEffectTarget = "",
            float secondEffectValue = 0f)
        {
            level.FindPropertyRelative("researchDurationSeconds").floatValue = researchDuration;

            var costs = level.FindPropertyRelative("researchCosts");
            costs.arraySize = 1;
            WriteAmount(costs.GetArrayElementAtIndex(0), costResourceId, costAmount);

            var effects = level.FindPropertyRelative("effects");
            effects.arraySize = secondEffectValue != 0f ? 2 : 1;
            WriteTechnologyEffect(effects.GetArrayElementAtIndex(0), firstEffectType, firstEffectTarget, firstEffectValue);
            if (effects.arraySize > 1)
            {
                WriteTechnologyEffect(effects.GetArrayElementAtIndex(1), secondEffectType, secondEffectTarget, secondEffectValue);
            }
        }

        private static void ApplyDefaultTechnologyRequirements(TechnologyConfig config)
        {
            if (config == null)
            {
                return;
            }

            var serialized = new SerializedObject(config);
            var levels = serialized.FindProperty("levels");
            if (levels == null || levels.arraySize < 2)
            {
                return;
            }

            for (var i = 0; i < levels.arraySize; i++)
            {
                var level = levels.GetArrayElementAtIndex(i);
                level.FindPropertyRelative("technologyPrerequisites").arraySize = 0;
                level.FindPropertyRelative("buildingRequirements").arraySize = 0;
            }

            if (config.Id == "agriculture")
            {
                WriteBuildingRequirement(levels.GetArrayElementAtIndex(1), "headquarters", 2);
            }
            else if (config.Id == "military_drill")
            {
                WriteBuildingRequirement(levels.GetArrayElementAtIndex(0), "headquarters", 2);
                WriteTechnologyPrerequisite(levels.GetArrayElementAtIndex(1), "agriculture", 1);
                WriteBuildingRequirement(levels.GetArrayElementAtIndex(1), "headquarters", 2);
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssets();
        }

        private static bool HasAnyTechnologyRequirements(TechnologyConfig config)
        {
            if (config == null)
            {
                return false;
            }

            for (var level = 1; level <= config.MaxLevel; level++)
            {
                var levelConfig = config.GetLevel(level);
                if (levelConfig == null)
                {
                    continue;
                }

                if (levelConfig.TechnologyPrerequisites.Count > 0 || levelConfig.BuildingRequirements.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static void WriteTechnologyPrerequisite(SerializedProperty level, string technologyId, int requiredLevel)
        {
            var prerequisites = level.FindPropertyRelative("technologyPrerequisites");
            var index = prerequisites.arraySize;
            prerequisites.arraySize++;
            var prerequisite = prerequisites.GetArrayElementAtIndex(index);
            prerequisite.FindPropertyRelative("technologyId").stringValue = technologyId;
            prerequisite.FindPropertyRelative("requiredLevel").intValue = requiredLevel;
        }

        private static void WriteBuildingRequirement(SerializedProperty level, string buildingId, int requiredLevel)
        {
            var requirements = level.FindPropertyRelative("buildingRequirements");
            var index = requirements.arraySize;
            requirements.arraySize++;
            var requirement = requirements.GetArrayElementAtIndex(index);
            requirement.FindPropertyRelative("buildingId").stringValue = buildingId;
            requirement.FindPropertyRelative("requiredLevel").intValue = requiredLevel;
        }

        private static void WriteTechnologyEffect(
            SerializedProperty effect,
            TechnologyEffectType type,
            string targetId,
            float value)
        {
            effect.FindPropertyRelative("type").enumValueIndex = (int)type;
            effect.FindPropertyRelative("targetId").stringValue = targetId;
            effect.FindPropertyRelative("value").floatValue = value;
        }

        private static void CreateCamera()
        {
            var cameraObject = new GameObject("Main Camera");
            var camera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
            cameraObject.transform.position = new Vector3(0f, 9f, -9f);
            cameraObject.transform.rotation = Quaternion.Euler(55f, 0f, 0f);
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.09f, 0.12f, 0.16f, 1f);
        }

        private static void CreateLight()
        {
            var lightObject = new GameObject("Directional Light");
            var light = lightObject.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.1f;
            lightObject.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
        }

        private static void CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "BaseGround";
            ground.transform.localScale = new Vector3(1.6f, 1f, 1.1f);
        }
    }

    public sealed class SlgConfigValidatorWindow : EditorWindow
    {
        private readonly List<SlgConfigValidationIssue> issues = new();
        private Vector2 scrollPosition;
        private bool hasScanned;

        [MenuItem("SLG Learn/Config Validator")]
        public static void Open()
        {
            var window = GetWindow<SlgConfigValidatorWindow>("SLG Config Validator");
            window.minSize = new Vector2(720f, 420f);
            window.Show();
        }

        private void OnGUI()
        {
            DrawToolbar();
            DrawSummary();
            DrawIssues();
        }

        private void DrawToolbar()
        {
            using (new EditorGUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                if (GUILayout.Button("Scan All SLG Configs", EditorStyles.toolbarButton, GUILayout.Width(160f)))
                {
                    Scan();
                }

                GUILayout.FlexibleSpace();
                EditorGUILayout.LabelField("Assets/_Project/ScriptableObjects", EditorStyles.miniLabel, GUILayout.Width(230f));
            }
        }

        private void DrawSummary()
        {
            if (!hasScanned)
            {
                EditorGUILayout.HelpBox("Click Scan All SLG Configs to validate resources, buildings, troops, strongholds, and technologies.", MessageType.Info);
                return;
            }

            var errorCount = CountIssues(SlgConfigIssueSeverity.Error);
            var warningCount = CountIssues(SlgConfigIssueSeverity.Warning);
            var messageType = errorCount > 0 ? MessageType.Error : warningCount > 0 ? MessageType.Warning : MessageType.Info;
            EditorGUILayout.HelpBox($"Errors: {errorCount}   Warnings: {warningCount}", messageType);
        }

        private void DrawIssues()
        {
            if (!hasScanned)
            {
                return;
            }

            if (issues.Count == 0)
            {
                EditorGUILayout.HelpBox("No validation issues.", MessageType.Info);
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            DrawIssueSection(SlgConfigIssueSeverity.Error, "Errors");
            DrawIssueSection(SlgConfigIssueSeverity.Warning, "Warnings");
            EditorGUILayout.EndScrollView();
        }

        private void DrawIssueSection(SlgConfigIssueSeverity severity, string title)
        {
            var count = CountIssues(severity);
            if (count == 0)
            {
                return;
            }

            EditorGUILayout.Space(8f);
            EditorGUILayout.LabelField($"{title} ({count})", EditorStyles.boldLabel);

            for (var i = 0; i < issues.Count; i++)
            {
                var issue = issues[i];
                if (issue.Severity != severity)
                {
                    continue;
                }

                DrawIssue(issue);
            }
        }

        private static void DrawIssue(SlgConfigValidationIssue issue)
        {
            var messageType = issue.Severity == SlgConfigIssueSeverity.Error ? MessageType.Error : MessageType.Warning;
            using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
            {
                EditorGUILayout.HelpBox(issue.Message, messageType);
                using (new EditorGUILayout.HorizontalScope())
                {
                    EditorGUILayout.SelectableLabel(
                        string.IsNullOrWhiteSpace(issue.AssetPath) ? "No asset path" : issue.AssetPath,
                        EditorStyles.miniLabel,
                        GUILayout.Height(18f));

                    using (new EditorGUI.DisabledScope(string.IsNullOrWhiteSpace(issue.AssetPath)))
                    {
                        if (GUILayout.Button("Select", GUILayout.Width(72f)))
                        {
                            SelectAsset(issue.AssetPath);
                        }
                    }
                }
            }
        }

        private void Scan()
        {
            issues.Clear();
            issues.AddRange(StageEightSceneBuilder.RunSlgConfigValidation());
            hasScanned = true;
        }

        private int CountIssues(SlgConfigIssueSeverity severity)
        {
            var count = 0;
            for (var i = 0; i < issues.Count; i++)
            {
                if (issues[i].Severity == severity)
                {
                    count++;
                }
            }

            return count;
        }

        private static void SelectAsset(string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset == null)
            {
                return;
            }

            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }

    public sealed class SlgConfigBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            var issues = StageEightSceneBuilder.RunSlgConfigValidation();
            var errorCount = 0;
            var warningCount = 0;

            for (var i = 0; i < issues.Count; i++)
            {
                var issue = issues[i];
                if (issue.Severity == SlgConfigIssueSeverity.Error)
                {
                    errorCount++;
                    Debug.LogError($"Build blocked by SLG config error: {issue.Message}");
                }
                else
                {
                    warningCount++;
                    Debug.LogWarning($"SLG config warning before build: {issue.Message}");
                }
            }

            if (errorCount > 0)
            {
                throw new BuildFailedException($"Build blocked: SLG config validation found {errorCount} error(s) and {warningCount} warning(s). Open SLG Learn > Config Validator for details.");
            }
        }
    }
}
