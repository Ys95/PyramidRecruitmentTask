using System;
using System.Collections.Generic;
using UnityEngine;

namespace PyramidRecruitmentTask.Feedbacks
{
    [Serializable]
    internal class FeedbacksSetup
    {
        [SerializeField] private List<AudioFeedback>     _audioFeedbacks;
        [SerializeField] private List<ParticlesFeedback> _particleFeedbacks;

        public List<AudioFeedback>     P_AudioFeedbacks    => _audioFeedbacks;
        public List<ParticlesFeedback> P_ParticleFeedbacks => _particleFeedbacks;

        public List<Feedback> P_GetAllFeedbacks
        {
            get
            {
                List<Feedback> feedbacksList = new List<Feedback>();
                feedbacksList.AddRange(_audioFeedbacks);
                feedbacksList.AddRange(_particleFeedbacks);

                return feedbacksList;
            }
        }
    }
}