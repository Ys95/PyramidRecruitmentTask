using System;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Feedbacks
{
    [Serializable]
    public abstract class Feedback
    {
        [SerializeField] private float _delay;

        public float P_Delay => _delay;

        public abstract void Initialize(SignalBus signalBus);

        public abstract void Play(Vector3 position);
    }
}