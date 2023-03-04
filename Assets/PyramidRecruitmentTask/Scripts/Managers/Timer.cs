using System;
using System.Collections;
using PyramidRecruitmentTask.Signals;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Managers
{
    public class Timer : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;

        private TimeSpan _time;
        
        public  bool     P_TimerRunning { get; private set; }
        public  TimeSpan P_Time         => _time;

        public void StartTimer()
        {
            P_TimerRunning = true;
            _signalBus.Fire(new TimerStartSignal(this));
            StartCoroutine(CO_Timer());
        }

        public void StopTimer()
        {
            P_TimerRunning = false;
        }

        private IEnumerator CO_Timer()
        {
            _time = TimeSpan.Zero;
            while (P_TimerRunning)
            {
                _time = _time.Add(TimeSpan.FromSeconds(Time.deltaTime));
                yield return null;
            }
        }
    }
}