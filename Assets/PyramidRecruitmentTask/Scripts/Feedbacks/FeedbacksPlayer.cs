using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Feedbacks
{
    public class FeedbacksPlayer : MonoBehaviour
    {
        [SerializeField] private FeedbacksSetup _feedbacksSetup;

        private          List<Feedback> _allFeedbacks;
        private          bool           _initialized;
        [Inject] private SignalBus      _signalBus;

        public void Play()
        {
            Play(transform.position);
        }

        public void Play(Vector3 position)
        {
            if (!_initialized)
            {
                Initialize();
            }

            foreach (var feedback in _allFeedbacks)
            {
                if (feedback.P_Delay <= 0f)
                {
                    feedback.Play(position);
                    continue;
                }

                StartCoroutine(CO_PlayFeedback(feedback, position));
            }
        }

        private void Initialize()
        {
            _allFeedbacks = _feedbacksSetup.P_GetAllFeedbacks;
            foreach (var feedback in _allFeedbacks)
            {
                feedback.Initialize(_signalBus);
            }
        }

        private IEnumerator CO_PlayFeedback(Feedback feedback, Vector3 position)
        {
            yield return new WaitForSeconds(feedback.P_Delay);

            feedback.Play(position);
        }
    }
}