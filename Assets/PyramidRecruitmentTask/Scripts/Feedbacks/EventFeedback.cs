using System;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace PyramidRecruitmentTask.Feedbacks
{
    [Serializable]
    public class EventFeedback : Feedback
    {
        [SerializeField] private UnityEvent _unityEvent;
        
        public override void Initialize(SignalBus signalBus)
        {
        }

        public override void Play(Vector3 position)
        {
            _unityEvent?.Invoke();
        }
    }
}