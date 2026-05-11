using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SLGLearn.Player
{
    public sealed class SquadDebugInput : MonoBehaviour
    {
        [SerializeField] private SquadManager squadManager;
        [SerializeField, Min(1)] private int changeAmount = 1;

        private void Awake()
        {
            if (squadManager == null)
            {
                squadManager = GetComponent<SquadManager>();
            }
        }

        private void Update()
        {
            if (squadManager == null)
            {
                return;
            }

            if (WasIncreasePressed())
            {
                squadManager.AddMembers(changeAmount);
            }

            if (WasDecreasePressed())
            {
                squadManager.RemoveMembers(changeAmount);
            }
        }

        private static bool WasIncreasePressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.E);
#endif
        }

        private static bool WasDecreasePressed()
        {
#if ENABLE_INPUT_SYSTEM
            return Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame;
#else
            return Input.GetKeyDown(KeyCode.Q);
#endif
        }
    }
}
