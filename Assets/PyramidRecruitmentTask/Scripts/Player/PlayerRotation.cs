using PyramidRecruitmentTask.Input;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Player
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField]                   private Transform _rotatingTransform;
        [SerializeField]                   private float     _speed;

        [Inject] private InputManager _inputManager;

        public float P_RotationDirectionInput { get; private set; }

        private void Update()
        {
            Rotate();
        }

        private void OnEnable()
        {
            _inputManager.P_CameraRotationFloat.E_ValueUpdated += SetLookVector;
        }

        private void OnDisable()
        {
            _inputManager.P_CameraRotationFloat.E_ValueUpdated -= SetLookVector;
        }

        public void SetLookVector(float value)
        {
            P_RotationDirectionInput = value;
        }

        private void Rotate()
        {
            _rotatingTransform.Rotate(new Vector3(0, P_RotationDirectionInput * _speed * Time.deltaTime, 0));
        }
    }
}