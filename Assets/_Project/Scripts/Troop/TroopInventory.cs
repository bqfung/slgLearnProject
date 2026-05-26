using System;
using System.Collections.Generic;
using SLGLearn.Data;

namespace SLGLearn.Troop
{
    public sealed class TroopInventory
    {
        private readonly Dictionary<string, TroopConfig> configsById = new();
        private readonly Dictionary<string, int> countsById = new();

        public event Action Changed;

        public void Initialize(IEnumerable<TroopConfig> configs)
        {
            configsById.Clear();
            countsById.Clear();

            foreach (var config in configs)
            {
                if (config == null || string.IsNullOrWhiteSpace(config.Id))
                {
                    continue;
                }

                configsById[config.Id] = config;
                countsById[config.Id] = 0;
            }

            Changed?.Invoke();
        }

        public IReadOnlyDictionary<string, TroopConfig> ConfigsById => configsById;

        public int GetCount(string troopId)
        {
            return countsById.TryGetValue(troopId, out var count) ? count : 0;
        }

        public void Add(string troopId, int count)
        {
            if (count == 0 || !configsById.ContainsKey(troopId))
            {
                return;
            }

            countsById[troopId] = Math.Max(0, GetCount(troopId) + count);
            Changed?.Invoke();
        }

        public void SetCount(string troopId, int count)
        {
            if (!configsById.ContainsKey(troopId))
            {
                return;
            }

            countsById[troopId] = Math.Max(0, count);
            Changed?.Invoke();
        }

        public int CalculateTotalPower()
        {
            var total = 0;
            foreach (var pair in configsById)
            {
                total += GetCount(pair.Key) * pair.Value.Power;
            }

            return total;
        }
    }
}
