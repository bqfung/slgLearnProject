using System;
using System.Collections.Generic;
using SLGLearn.Data;

namespace SLGLearn.Troop
{
    public sealed class MarchFormation
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

        public int GetCount(string troopId)
        {
            return countsById.TryGetValue(troopId, out var count) ? count : 0;
        }

        public void Add(string troopId, int count)
        {
            if (count <= 0 || !configsById.ContainsKey(troopId))
            {
                return;
            }

            countsById[troopId] = GetCount(troopId) + count;
            Changed?.Invoke();
        }

        public bool Remove(string troopId, int count)
        {
            if (count <= 0 || GetCount(troopId) < count)
            {
                return false;
            }

            countsById[troopId] = GetCount(troopId) - count;
            Changed?.Invoke();
            return true;
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

        public void Clear()
        {
            foreach (var key in configsById.Keys)
            {
                countsById[key] = 0;
            }

            Changed?.Invoke();
        }

        public int CalculateTotalCount()
        {
            var total = 0;
            foreach (var pair in countsById)
            {
                total += pair.Value;
            }

            return total;
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
