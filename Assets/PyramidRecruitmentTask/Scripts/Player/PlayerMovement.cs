using System;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private InputManager _inputManager;
        
        [Header("Movement")]
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private float _maxAbsoluteVelocity;
        [SerializeField] private bool  _applyCounterMovement;
        [SerializeField] private float _counterMovementForce;

        private CharacterController _characterController;
        private Vector3             _velocity;
        private Vector3             _motionVector;
        
        public Vector2 P_RawMovementVector { get; private set; }

        private void Awake()
        {
            _inputManager        = FindObjectOfType<InputManager>();
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            _inputManager.P_CameraMovementVector2.E_ValueUpdated += UpdateInput;
        }

        private void OnDisable()
        {
            _inputManager.P_CameraMovementVector2.E_ValueUpdated -= UpdateInput;
        }

        private void UpdateInput(Vector2 input)
        {
            P_RawMovementVector = input;
        }

        private void CalculateAcceleration()
        {
            _velocity.z += _acceleration * Time.deltaTime * P_RawMovementVector.y;
            _velocity.x += _acceleration * Time.deltaTime * P_RawMovementVector.x;
        }
        
        private void CalculateDeceleration()
        {
            if (Mathf.Abs(P_RawMovementVector.y) <= 0)
            {
                if (Mathf.Abs(_velocity.z) <= 0.1)
                {
                    _velocity.z = 0;
                }
                else
                {
                    var dir = (Mathf.Sign(_velocity.z) * -1);
                    _velocity.z += _deceleration * Time.deltaTime * dir;
                }
            }

            if (Mathf.Abs(P_RawMovementVector.x) <= 0)
            {
                if (Mathf.Abs(_velocity.x) <= 0.1)
                {
                    _velocity.x = 0;
                }
                else
                {
                    var dir = (Mathf.Sign(_velocity.x) * -1);
                    _velocity.x += _deceleration * Time.deltaTime * dir;
                }
            }
        }
        
        private void CalculateCounterMovement()
        {
            if(!_applyCounterMovement)
            {
                return;
            }
            
            // If player is changing direction, apply additional acceleration until velocity direction matches input direction
            var movementDirectionX = Mathf.Sign(P_RawMovementVector.x);
            var movementDirectionZ = Mathf.Sign(P_RawMovementVector.y);

            if (!Mathf.Approximately(movementDirectionX, _velocity.x))
            {
                _velocity.x += Time.deltaTime * _counterMovementForce * P_RawMovementVector.x;
            }

            if (!Mathf.Approximately(movementDirectionZ, _velocity.z))
            {
                _velocity.z += Time.deltaTime * _counterMovementForce * P_RawMovementVector.y;
            }
        }
        
        private void Move()
        {
            _velocity.x = Mathf.Clamp(_velocity.x, _maxAbsoluteVelocity * -1, _maxAbsoluteVelocity);
            _velocity.z = Mathf.Clamp(_velocity.z, _maxAbsoluteVelocity * -1, _maxAbsoluteVelocity);

            _motionVector = new Vector3(_velocity.x, 0f, _velocity.z);
            _motionVector = _characterController.transform.rotation * _motionVector;

            _characterController.Move(_motionVector * Time.deltaTime);
        }

        private void Update()
        {
            CalculateAcceleration();
            CalculateDeceleration();
            CalculateCounterMovement();
            Move();
        }
    }
}