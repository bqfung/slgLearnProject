using System.Collections.Generic;
using SLGLearn.Data;

namespace SLGLearn.Troop
{
    public sealed class MarchOrder
    {
        private readonly Dictionary<string, int> countsByTroopId = new();

        public MarchOrder(StrongholdConfig target, IReadOnlyDictionary<string, int> troopCounts, float remainingSeconds)
        {
            Target = target;
            RemainingSeconds = remainingSeconds;

            if (troopCounts == null)
            {
                return;
            }

            foreach (var pair in troopCounts)
            {
                if (!string.IsNullOrWhiteSpace(pair.Key) && pair.Value > 0)
                {
                    countsByTroopId[pair.Key] = pair.Value;
                }
            }
        }

        public StrongholdConfig Target { get; }
        public float RemainingSeconds { get; private set; }
        public IReadOnlyDictionary<string, int> CountsByTroopId => countsByTroopId;
        public bool IsComplete => RemainingSeconds <= 0f;

        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f || IsComplete)
            {
                return;
            }

            RemainingSeconds -= deltaTime;
            if (RemainingSeconds < 0f)
            {
                RemainingSeconds = 0f;
            }
        }
    }
}
