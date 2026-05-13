using UnityEngine;

namespace SLGLearn.Player
{
    public sealed class PlayerSquadFactory
    {
        public GameObject Create()
        {
            var squad = new GameObject("PlayerSquad");
            squad.transform.position = new Vector3(0f, 0.5f, 0f);

            squad.AddComponent<SquadController>();
            squad.AddComponent<SquadManager>();
            squad.AddComponent<SquadDebugInput>();
            return squad;
        }
    }
}
