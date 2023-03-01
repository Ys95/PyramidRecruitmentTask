using System;

namespace PyramidRecruitmentTask
{
    public class StateMachine<T> where T : Enum
    {
        public event Action E_StateChanged;

        public T P_CurrentState  { get; private set; }
        public T P_PreviousState { get; private set; }

        private bool _invokeEvents;

        public void ChangeState(T newState)
        {
            if (newState.Equals(P_CurrentState))
            {
                return;
            }

            P_PreviousState = P_CurrentState;
            P_CurrentState  = newState;

            if (_invokeEvents)
            {
                E_StateChanged?.Invoke();
            }
        }

        public StateMachine(T initialState, bool invokeEvents)
        {
            P_PreviousState = initialState;
            P_CurrentState  = initialState;
            _invokeEvents = invokeEvents;
        }
    }
}