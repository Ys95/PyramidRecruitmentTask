using System;
using System.Collections;
using UnityEngine;

namespace PyramidRecruitmentTask
{
    public class Timer : MonoBehaviour
    {
        public bool     P_TimerRunning         { get; private set; }
        public int      P_CurrentTimeInSeconds { get; private set; }
        public int      P_CurrentTimeString    { get; private set; }
        public TimeSpan P_Time                 => TimeSpan.FromSeconds(_timeInSeconds);

        public event Action<TimeSpan> E_TimerUpdated;

        private float _timeInSeconds;

        public void StartTimer()
        {
            P_TimerRunning = true;
            StartCoroutine(CO_Timer());
        }

        public TimeSpan StopTimer()
        {
            P_TimerRunning = false;
            return P_Time;
        }
        
        private IEnumerator CO_Timer()
        {
            _timeInSeconds = 0;
            while (P_TimerRunning)
            {
                _timeInSeconds += Time.deltaTime;
                E_TimerUpdated?.Invoke(P_Time);
                yield return null;
            }
        }
    }
}