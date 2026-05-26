using SLGLearn.Base;
using UnityEngine;

namespace SLGLearn.UI
{
    public sealed class RuntimeBaseUiInstaller : MonoBehaviour
    {
        [SerializeField] private SlgBaseRuntime runtime;

        private void Start()
        {
            if (runtime != null)
            {
                WorldSlgVisualBuilder.Build(runtime);
            }
        }

        public void Configure(SlgBaseRuntime baseRuntime)
        {
            runtime = baseRuntime;
        }
    }
}
