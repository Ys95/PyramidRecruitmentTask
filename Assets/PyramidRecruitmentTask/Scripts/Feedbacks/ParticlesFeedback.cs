using System;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace PyramidRecruitmentTask.Feedbacks
{
    [Serializable]
    internal class ParticlesFeedback : Feedback
    {
        [SerializeField] private ParticleSystem _particlesPrefab;

        private ParticleSystem _particleSystem;

        public override void Initialize(SignalBus signalBus)
        {
            _particleSystem = Object.Instantiate(_particlesPrefab);
        }

        public override void Play(Vector3 position)
        {
            _particleSystem.transform.position = position;
            _particleSystem.Play();
        }
    }
}