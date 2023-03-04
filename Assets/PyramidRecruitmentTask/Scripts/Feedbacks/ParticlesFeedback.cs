using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace PyramidRecruitmentTask.Feedbacks
{
    [Serializable]
    internal class ParticlesFeedback : Feedback
    {
        [SerializeField] private ParticleSystem _particles;

        public override void Initialize(SignalBus signalBus)
        {
        }

        public override void Play(Vector3 position)
        {
            _particles.Play();
        }
    }
}