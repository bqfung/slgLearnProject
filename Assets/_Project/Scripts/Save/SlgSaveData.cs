using System.Collections.Generic;

namespace SLGLearn.Save
{
    [System.Serializable]
    public sealed class SlgSaveData
    {
        public const int CurrentVersion = 4;

        public int version = CurrentVersion;
        public long savedUnixTimeSeconds;
        public List<ResourceSaveData> resources = new();
        public List<BuildingSaveData> buildings = new();
        public List<TroopSaveData> troops = new();
        public List<TroopSaveData> formation = new();
        public List<TrainingOrderSaveData> trainingOrders = new();
        public List<MarchOrderSaveData> marchOrders = new();
        public List<StrongholdSaveData> strongholds = new();
        public List<ChapterSaveData> chapters = new();
        public List<TechnologySaveData> technologies = new();

        public void Normalize()
        {
            if (version <= 0)
            {
                version = 1;
            }

            resources ??= new List<ResourceSaveData>();
            buildings ??= new List<BuildingSaveData>();
            troops ??= new List<TroopSaveData>();
            formation ??= new List<TroopSaveData>();
            trainingOrders ??= new List<TrainingOrderSaveData>();
            marchOrders ??= new List<MarchOrderSaveData>();
            strongholds ??= new List<StrongholdSaveData>();
            chapters ??= new List<ChapterSaveData>();
            technologies ??= new List<TechnologySaveData>();
        }
    }

    public static class SlgSaveMigrator
    {
        public static int MigrateToCurrent(SlgSaveData data)
        {
            if (data == null)
            {
                return 0;
            }

            data.Normalize();
            var originalVersion = data.version;
            if (data.version > SlgSaveData.CurrentVersion)
            {
                return originalVersion;
            }

            if (data.version < 2)
            {
                MigrateToVersion2(data);
            }

            if (data.version < 3)
            {
                MigrateToVersion3(data);
            }

            if (data.version < 4)
            {
                MigrateToVersion4(data);
            }

            data.Normalize();
            return originalVersion;
        }

        private static void MigrateToVersion2(SlgSaveData data)
        {
            data.strongholds ??= new List<StrongholdSaveData>();
            data.technologies ??= new List<TechnologySaveData>();
            data.version = 2;
        }

        private static void MigrateToVersion3(SlgSaveData data)
        {
            data.marchOrders ??= new List<MarchOrderSaveData>();
            data.version = 3;
        }

        private static void MigrateToVersion4(SlgSaveData data)
        {
            data.chapters ??= new List<ChapterSaveData>();
            data.version = 4;
        }
    }

    [System.Serializable]
    public sealed class ResourceSaveData
    {
        public string id;
        public float amount;
    }

    [System.Serializable]
    public sealed class BuildingSaveData
    {
        public string id;
        public int level;
        public float upgradeRemainingSeconds;
    }

    [System.Serializable]
    public sealed class TroopSaveData
    {
        public string id;
        public int count;
    }

    [System.Serializable]
    public sealed class TrainingOrderSaveData
    {
        public string troopId;
        public float remainingSeconds;
    }

    [System.Serializable]
    public sealed class MarchOrderSaveData
    {
        public string strongholdId;
        public float remainingSeconds;
        public List<TroopSaveData> troops = new();
    }

    [System.Serializable]
    public sealed class StrongholdSaveData
    {
        public string id;
        public bool cleared;
    }

    [System.Serializable]
    public sealed class ChapterSaveData
    {
        public string id;
        public bool rewardClaimed;
    }

    [System.Serializable]
    public sealed class TechnologySaveData
    {
        public string id;
        public int level;
        public float researchRemainingSeconds;
    }
}
