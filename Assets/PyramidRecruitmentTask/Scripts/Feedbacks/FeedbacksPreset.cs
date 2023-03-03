using UnityEngine;

namespace PyramidRecruitmentTask.Feedbacks
{
    [CreateAssetMenu(fileName = "FeedbacksPreset", menuName = "FeedbacksPreset", order = 0)]
    public class FeedbacksPreset : ScriptableObject
    {
        [SerializeField] private FeedbacksSetup _feedbacksSetup;

        internal FeedbacksSetup P_FeedbacksSetup => _feedbacksSetup;
    }
}