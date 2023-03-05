using UnityEngine;
using UnityEngine.InputSystem;

namespace PyramidRecruitmentTask.Input
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float       _mouseSensitivity = 1;

        public InputButton P_InteractionButton { get; } = new();

        public InputValue<Vector2> P_CameraMovementVector2 { get; } = new();

        public InputValue<float> P_CameraRotationFloat { get; } = new();

        public void SetCameraMovementVector(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                P_CameraMovementVector2.UpdateValue(Vector2.zero);
            }
            else
            {
                P_CameraMovementVector2.UpdateValue(context.ReadValue<Vector2>());
            }
        }

        public void SetCameraRotationVector(InputAction.CallbackContext context)
        {
            float rotationValue = context.ReadValue<float>();

            if (rotationValue > 0)
            {
                P_CameraRotationFloat.UpdateValue(1);
            }
            else if (rotationValue < 0)
            {
                P_CameraRotationFloat.UpdateValue(-1);
            }
            else
            {
                P_CameraRotationFloat.UpdateValue(0);
            }
        }

        public void SetInteractionButton(InputAction.CallbackContext context)
        {
            SetButton(P_InteractionButton, context);
        }

        private void SetButton(InputButton button, InputAction.CallbackContext context)
        {
            if (context.started)
            {
                button.TriggerButtonPress();
            }
            else if (context.performed)
            {
                button.TriggerButtonHeld();
            }
            else if (context.canceled)
            {
                button.TriggerButtonReleased();
            }
        }
    }
}