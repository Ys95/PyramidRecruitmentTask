using System;
using PyramidRecruitmentTask.Feedbacks;
using PyramidRecruitmentTask.Input;
using PyramidRecruitmentTask.Player;
using PyramidRecruitmentTask.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace PyramidRecruitmentTask.Interactions
{
    [RequireComponent(typeof(Collider))]
    public abstract class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDisposable, IInitializable
    {
        [SerializeField] private Material        _mouseOverMaterial;
        [SerializeField] private FeedbacksPlayer _interactionFeedbacks;

        protected InputManager _inputManager;
        protected bool         _pointerEventsAllowed;
        private   Material     _regularMaterial;
        private   Renderer     _renderer;

        [Inject] protected SignalBus _signalBus;

        private void Awake()
        {
            _renderer             = GetComponent<Renderer>();
            _regularMaterial      = _renderer.material;
            _inputManager         = FindObjectOfType<InputManager>();
            _pointerEventsAllowed = true;
        }

        public void Dispose()
        {
            _signalBus.TryUnsubscribe<PlayerInteractionAttemptSignal>(OnPlayerInteractionAttempt);
        }

        public void Initialize()
        {
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!_pointerEventsAllowed)
            {
                return;
            }

            HandlePointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!_pointerEventsAllowed)
            {
                return;
            }

            HandlePointerExit();
        }

        protected abstract void HandleInteraction(PlayerInteraction playerInteraction);

        protected virtual void OnPlayerInteractionAttempt(PlayerInteractionAttemptSignal signal)
        {
            _interactionFeedbacks?.Play();
            HandleInteraction(signal.P_PlayerInteraction);
        }

        protected virtual void HandlePointerEnter()
        {
            _signalBus.Subscribe<PlayerInteractionAttemptSignal>(OnPlayerInteractionAttempt);
            if (_mouseOverMaterial != null)
            {
                _renderer.material = _mouseOverMaterial;
            }
        }

        protected virtual void HandlePointerExit()
        {
            _signalBus.TryUnsubscribe<PlayerInteractionAttemptSignal>(OnPlayerInteractionAttempt);
            if (_mouseOverMaterial != null)
            {
                _renderer.material = _regularMaterial;
            }
        }
    }
}