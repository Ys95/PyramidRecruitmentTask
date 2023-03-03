using System;
using PyramidRecruitmentTask.Signals;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Feedbacks
{
    [Serializable]
    internal class AudioFeedback : Feedback
    {
        [SerializeField] private AudioClip _clip;

        private SignalBus _signalBus;

        public override void Initialize(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        public override void Play(Vector3 position)
        {
            _signalBus.Fire(new PlayAudioSignal(_clip, position));
        }
    }
}