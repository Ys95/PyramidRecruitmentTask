using System;

namespace PyramidRecruitmentTask
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
}