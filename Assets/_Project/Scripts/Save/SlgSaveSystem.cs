using System;
using System.Collections.Generic;
using SLGLearn.Building;
using SLGLearn.Resource;
using SLGLearn.Technology;
using SLGLearn.Troop;
using UnityEngine;

namespace SLGLearn.Save
{
    public static class SlgSaveSystem
    {
        public const string DefaultSaveKey = "SLGLearn.Stage08.BaseSave";

        public static bool TryLoad(string saveKey, out SlgSaveData data)
        {
            data = null;
            if (!PlayerPrefs.HasKey(saveKey))
            {
                return false;
            }

            var json = PlayerPrefs.GetString(saveKey);
            if (string.IsNullOrWhiteSpace(json))
            {
                return false;
            }

            data = JsonUtility.FromJson<SlgSaveData>(json);
            var originalVersion = SlgSaveMigrator.MigrateToCurrent(data);
            if (data != null && originalVersion > 0 && originalVersion < data.version)
            {
                Debug.Log($"Migrated SLG save from version {originalVersion} to version {data.version}.");
            }

            if (data != null && originalVersion > SlgSaveData.CurrentVersion)
            {
                Debug.LogWarning($"SLG save version {originalVersion} is newer than supported version {SlgSaveData.CurrentVersion}. Loading with best effort.");
            }

            return data != null;
        }

        public static void Save(
            string saveKey,
            ResourceInventory inventory,
            IReadOnlyList<BuildingRuntimeState> buildings,
            TroopInventory troops,
            MarchFormation formation,
            IReadOnlyList<TroopTrainingOrder> trainingOrders,
            IReadOnlyList<MarchOrder> marchOrders,
            IReadOnlyList<TechnologyRuntimeState> technologies,
            IReadOnlyCollection<string> clearedStrongholdIds,
            IReadOnlyCollection<string> claimedChapterRewardIds)
        {
            var data = new SlgSaveData
            {
                version = SlgSaveData.CurrentVersion,
                savedUnixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            foreach (var pair in inventory.ConfigsById)
            {
                data.resources.Add(new ResourceSaveData
                {
                    id = pair.Key,
                    amount = inventory.GetAmount(pair.Key)
                });
            }

            for (var i = 0; i < buildings.Count; i++)
            {
                var state = buildings[i];
                if (state?.Config == null)
                {
                    continue;
                }

                data.buildings.Add(new BuildingSaveData
                {
                    id = state.Config.Id,
                    level = state.Level,
                    upgradeRemainingSeconds = state.UpgradeRemainingSeconds
                });
            }

            foreach (var pair in troops.ConfigsById)
            {
                data.troops.Add(new TroopSaveData
                {
                    id = pair.Key,
                    count = troops.GetCount(pair.Key)
                });
            }

            foreach (var pair in troops.ConfigsById)
            {
                data.formation.Add(new TroopSaveData
                {
                    id = pair.Key,
                    count = formation.GetCount(pair.Key)
                });
            }

            for (var i = 0; i < trainingOrders.Count; i++)
            {
                var order = trainingOrders[i];
                if (order?.Config == null)
                {
                    continue;
                }

                data.trainingOrders.Add(new TrainingOrderSaveData
                {
                    troopId = order.Config.Id,
                    remainingSeconds = order.RemainingSeconds
                });
            }

            for (var i = 0; i < marchOrders.Count; i++)
            {
                var order = marchOrders[i];
                if (order?.Target == null)
                {
                    continue;
                }

                var savedOrder = new MarchOrderSaveData
                {
                    strongholdId = order.Target.Id,
                    remainingSeconds = order.RemainingSeconds
                };

                foreach (var pair in order.CountsByTroopId)
                {
                    if (pair.Value <= 0)
                    {
                        continue;
                    }

                    savedOrder.troops.Add(new TroopSaveData
                    {
                        id = pair.Key,
                        count = pair.Value
                    });
                }

                data.marchOrders.Add(savedOrder);
            }

            if (clearedStrongholdIds != null)
            {
                foreach (var strongholdId in clearedStrongholdIds)
                {
                    if (string.IsNullOrWhiteSpace(strongholdId))
                    {
                        continue;
                    }

                    data.strongholds.Add(new StrongholdSaveData
                    {
                        id = strongholdId,
                        cleared = true
                    });
                }
            }

            if (claimedChapterRewardIds != null)
            {
                foreach (var chapterId in claimedChapterRewardIds)
                {
                    if (string.IsNullOrWhiteSpace(chapterId))
                    {
                        continue;
                    }

                    data.chapters.Add(new ChapterSaveData
                    {
                        id = chapterId,
                        rewardClaimed = true
                    });
                }
            }

            for (var i = 0; i < technologies.Count; i++)
            {
                var state = technologies[i];
                if (state?.Config == null)
                {
                    continue;
                }

                data.technologies.Add(new TechnologySaveData
                {
                    id = state.Config.Id,
                    level = state.Level,
                    researchRemainingSeconds = state.ResearchRemainingSeconds
                });
            }

            PlayerPrefs.SetString(saveKey, JsonUtility.ToJson(data));
            PlayerPrefs.Save();
        }

        public static void Delete(string saveKey)
        {
            PlayerPrefs.DeleteKey(saveKey);
            PlayerPrefs.Save();
        }
    }
}
