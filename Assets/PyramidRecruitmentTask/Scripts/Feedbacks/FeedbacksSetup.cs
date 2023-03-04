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
        [SerializeField] private List<EventFeedback>     _eventFeedbacks;

        public List<AudioFeedback>     P_AudioFeedbacks    => _audioFeedbacks;
        public List<ParticlesFeedback> P_ParticleFeedbacks => _particleFeedbacks;
        public List<EventFeedback>     P_EventFeedbacks    => _eventFeedbacks;

        public List<Feedback> P_GetAllFeedbacks
        {
            get
            {
                List<Feedback> feedbacksList = new List<Feedback>();
                feedbacksList.AddRange(_audioFeedbacks);
                feedbacksList.AddRange(_particleFeedbacks);
                feedbacksList.AddRange(_eventFeedbacks);
                
                return feedbacksList;
            }
        }
    }
}