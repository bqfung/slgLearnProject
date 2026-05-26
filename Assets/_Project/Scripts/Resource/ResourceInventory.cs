using System;
using System.Collections.Generic;
using SLGLearn.Data;

namespace SLGLearn.Resource
{
    public sealed class ResourceInventory
    {
        private readonly Dictionary<string, ResourceConfig> configsById = new();
        private readonly Dictionary<string, float> amountsById = new();

        public event Action Changed;

        public void Initialize(IEnumerable<ResourceConfig> configs)
        {
            configsById.Clear();
            amountsById.Clear();

            foreach (var config in configs)
            {
                if (config == null || string.IsNullOrWhiteSpace(config.Id))
                {
                    continue;
                }

                configsById[config.Id] = config;
                amountsById[config.Id] = Clamp(config.Id, config.StartingAmount);
            }

            Changed?.Invoke();
        }

        public IReadOnlyDictionary<string, ResourceConfig> ConfigsById => configsById;

        public float GetAmount(string resourceId)
        {
            return amountsById.TryGetValue(resourceId, out var amount) ? amount : 0f;
        }

        public void Add(string resourceId, float amount)
        {
            if (amount <= 0f || !configsById.ContainsKey(resourceId))
            {
                return;
            }

            amountsById[resourceId] = Clamp(resourceId, GetAmount(resourceId) + amount);
            Changed?.Invoke();
        }

        public void SetAmount(string resourceId, float amount)
        {
            if (!configsById.ContainsKey(resourceId))
            {
                return;
            }

            amountsById[resourceId] = Clamp(resourceId, amount);
            Changed?.Invoke();
        }

        public bool CanSpend(IReadOnlyList<ResourceAmount> costs)
        {
            if (costs == null)
            {
                return true;
            }

            for (var i = 0; i < costs.Count; i++)
            {
                var cost = costs[i];
                if (cost != null && GetAmount(cost.ResourceId) < cost.Amount)
                {
                    return false;
                }
            }

            return true;
        }

        public bool Spend(IReadOnlyList<ResourceAmount> costs)
        {
            if (costs == null)
            {
                return true;
            }

            if (!CanSpend(costs))
            {
                return false;
            }

            for (var i = 0; i < costs.Count; i++)
            {
                var cost = costs[i];
                if (cost == null || cost.Amount <= 0f)
                {
                    continue;
                }

                amountsById[cost.ResourceId] = Clamp(cost.ResourceId, GetAmount(cost.ResourceId) - cost.Amount);
            }

            Changed?.Invoke();
            return true;
        }

        private float Clamp(string resourceId, float value)
        {
            return configsById.TryGetValue(resourceId, out var config)
                ? MathF.Min(MathF.Max(0f, value), config.Capacity)
                : MathF.Max(0f, value);
        }
    }
}
