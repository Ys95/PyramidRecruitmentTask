using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PyramidRecruitmentTask
{
    public class InputManager : MonoBehaviour
    {
        public enum ButtonState
        {
            Released,
            Pressed,
            Held,
        }

        [Serializable]
        public class InputButton
        {
            private StateMachine<ButtonState> _state;

            public ButtonState P_CurrentState  => _state.P_CurrentState;
            public ButtonState P_PreviousState => _state.P_PreviousState;

            public void TriggerButtonPress()
            {
                if (_state.P_CurrentState == ButtonState.Pressed)
                {
                    return;
                }

                _state.ChangeState(ButtonState.Pressed);
                E_ButtonPress?.Invoke();
            }

            public void TriggerButtonHeld()
            {
                if (_state.P_CurrentState == ButtonState.Held)
                {
                    return;
                }

                _state.ChangeState(ButtonState.Held);
                E_ButtonHold?.Invoke();
            }

            public void TriggerButtonReleased()
            {
                if (_state.P_CurrentState == ButtonState.Released)
                {
                    return;
                }

                _state.ChangeState(ButtonState.Released);
                E_ButtonRelease?.Invoke();
            }

            public event Action E_ButtonPress;
            public event Action E_ButtonHold;
            public event Action E_ButtonRelease;

            public InputButton()
            {
                _state = new StateMachine<ButtonState>(ButtonState.Released, false);
            }
        }

        [Serializable]
        public class InputValue<T>
        {
            public T P_PreviousValue { get; private set; }
            public T P_CurrentValue  { get; private set; }

            public event Action<T> E_ValueUpdated;

            public void UpdateValue(T newValue)
            {
                P_PreviousValue = P_CurrentValue;
                P_CurrentValue  = newValue;
                E_ValueUpdated?.Invoke(newValue);
            }
        }

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