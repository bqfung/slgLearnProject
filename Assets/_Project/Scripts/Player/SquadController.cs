using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace SLGLearn.Player
{
    public sealed class SquadController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float horizontalSpeed = 8f;
        [SerializeField] private float horizontalLimit = 4f;

        [Header("Input")]
        [SerializeField] private float dragSensitivity = 0.015f;

        private bool isDragging;
        private Vector2 lastPointerPosition;

        private void Update()
        {
            var input = ReadHorizontalInput();
            var position = transform.position;

            position.z += forwardSpeed * Time.deltaTime;
            position.x += input * horizontalSpeed * Time.deltaTime;
            position.x = Mathf.Clamp(position.x, -horizontalLimit, horizontalLimit);

            transform.position = position;
        }

        private float ReadHorizontalInput()
        {
            var keyboardInput = ReadKeyboardInput();
            var pointerInput = ReadPointerInput();
            return Mathf.Clamp(keyboardInput + pointerInput, -1f, 1f);
        }

        private float ReadKeyboardInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current == null)
            {
                return 0f;
            }

            var value = 0f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                value -= 1f;
            }

            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                value += 1f;
            }

            return value;
#else
            return Input.GetAxisRaw("Horizontal");
#endif
        }

        private float ReadPointerInput()
        {
#if ENABLE_INPUT_SYSTEM
            if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
            {
                var touch = Touchscreen.current.primaryTouch;
                return ReadDragDelta(touch.position.ReadValue());
            }

            if (Mouse.current != null && Mouse.current.leftButton.isPressed)
            {
                return ReadDragDelta(Mouse.current.position.ReadValue());
            }

            isDragging = false;
            return 0f;
#else
            if (Input.touchCount > 0)
            {
                return ReadDragDelta(Input.GetTouch(0).position);
            }

            if (Input.GetMouseButton(0))
            {
                return ReadDragDelta(Input.mousePosition);
            }

            isDragging = false;
            return 0f;
#endif
        }

        private float ReadDragDelta(Vector2 pointerPosition)
        {
            if (!isDragging)
            {
                isDragging = true;
                lastPointerPosition = pointerPosition;
                return 0f;
            }

            var deltaX = pointerPosition.x - lastPointerPosition.x;
            lastPointerPosition = pointerPosition;
            return Mathf.Clamp(deltaX * dragSensitivity, -1f, 1f);
        }
    }
}
