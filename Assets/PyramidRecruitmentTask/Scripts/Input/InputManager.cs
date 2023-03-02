using UnityEngine;
using UnityEngine.InputSystem;

namespace PyramidRecruitmentTask
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput _playerInput;
        [SerializeField] private float       _mouseSensitivity = 1;

        private InputButton         _interactionButton     = new InputButton();
        private InputValue<Vector2> _cameraMovementVector2 = new InputValue<Vector2>();
        private InputValue<float>   _cameraRotationFloat   = new InputValue<float>();

        public InputButton         P_InteractionButton     => _interactionButton;
        public InputValue<Vector2> P_CameraMovementVector2 => _cameraMovementVector2;
        public InputValue<float>   P_CameraRotationFloat   => _cameraRotationFloat;

        public void SetCameraMovementVector(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                _cameraMovementVector2.UpdateValue(Vector2.zero);
            }
            else
            {
                _cameraMovementVector2.UpdateValue(context.ReadValue<Vector2>());
            }
        }

        public void SetCameraRotationVector(InputAction.CallbackContext context)
        {
            var rotationValue = context.ReadValue<float>();

            if (rotationValue > 0)
            {
                _cameraRotationFloat.UpdateValue(1);
            }
            else if (rotationValue < 0)
            {
                _cameraRotationFloat.UpdateValue(-1);
            }
            else
            {
                _cameraRotationFloat.UpdateValue(0);
            }

            Debug.Log(_cameraRotationFloat.P_CurrentValue);
        }

        public void SetInteractionButton(InputAction.CallbackContext context) => SetButton(_interactionButton, context);

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