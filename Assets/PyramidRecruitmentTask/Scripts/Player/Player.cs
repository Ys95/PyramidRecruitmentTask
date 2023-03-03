using UnityEngine;

namespace PyramidRecruitmentTask.Player
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTarget;

        public Transform P_CameraTarget => _cameraTarget;

        public int OwnedKeys { get; private set; }

        public void AddKey()
        {
            OwnedKeys++;
        }

        public void UseKey()
        {
            OwnedKeys--;
        }
    }
}