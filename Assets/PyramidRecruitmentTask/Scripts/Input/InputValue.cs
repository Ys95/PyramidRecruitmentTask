using System;

namespace PyramidRecruitmentTask.Input
{
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
}