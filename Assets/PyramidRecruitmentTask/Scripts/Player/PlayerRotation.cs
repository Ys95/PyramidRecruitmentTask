using System;
using UnityEngine;

namespace PyramidRecruitmentTask
{
    public class PlayerRotation : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;

        [Space]
        [SerializeField] private Transform _rotatingTransform;
        [SerializeField]                   private float _speed;
        [SerializeField] [Range(0, 0.99f)] private float _smoothing;

        public float P_RotationDirectionInput { get; private set; }

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

        private void Update()
        {
            Rotate();
        }
    }
}