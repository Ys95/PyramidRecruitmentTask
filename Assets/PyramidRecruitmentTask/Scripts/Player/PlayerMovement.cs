using PyramidRecruitmentTask.Input;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private Transform _facingDirectionSource;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _deceleration;
        [SerializeField] private float _maxAbsoluteVelocity;

        private CharacterController _characterController;

        [Inject] private InputManager _inputManager;
        private          Vector3      _motionVector;
        private          Vector3      _velocity;

        public Vector2 P_InputVector { get; private set; }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            CalculateAcceleration();
            CalculateDeceleration();
            Move();
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
            P_InputVector = input;
        }

        private void CalculateAcceleration()
        {
            _velocity.z += _acceleration * Time.deltaTime * P_InputVector.y;
            _velocity.x += _acceleration * Time.deltaTime * P_InputVector.x;
        }

        private void CalculateDeceleration()
        {
            if (Mathf.Abs(P_InputVector.y) <= 0)
            {
                if (Mathf.Abs(_velocity.z) <= 0.1)
                {
                    _velocity.z = 0;
                }
                else
                {
                    float dir = Mathf.Sign(_velocity.z) * -1;
                    _velocity.z += _deceleration * Time.deltaTime * dir;
                }
            }

            if (Mathf.Abs(P_InputVector.x) <= 0)
            {
                if (Mathf.Abs(_velocity.x) <= 0.1)
                {
                    _velocity.x = 0;
                }
                else
                {
                    float dir = Mathf.Sign(_velocity.x) * -1;
                    _velocity.x += _deceleration * Time.deltaTime * dir;
                }
            }
        }

        private void Move()
        {
            _velocity.x = Mathf.Clamp(_velocity.x, _maxAbsoluteVelocity * -1, _maxAbsoluteVelocity);
            _velocity.z = Mathf.Clamp(_velocity.z, _maxAbsoluteVelocity * -1, _maxAbsoluteVelocity);

            _motionVector = new Vector3(_velocity.x, 0f, _velocity.z);
            _motionVector = _facingDirectionSource.rotation * _motionVector;

            _characterController.Move(_motionVector * Time.deltaTime);
        }
    }
}