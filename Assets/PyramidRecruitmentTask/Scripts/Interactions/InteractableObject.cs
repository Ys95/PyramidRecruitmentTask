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
    public abstract class InteractableObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Material    _mouseOverMat;
        [SerializeField] private Renderer _mainRenderer;

        private Material _regularMat;

        protected          bool      _pointerEventsAllowed;
        protected          Collider  _collider;
        [Inject] protected SignalBus _signalBus;

        private void Awake()
        {
            _regularMat           = _mainRenderer.material;
            _pointerEventsAllowed = true;
            _collider             = GetComponent<Collider>();
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<PlayerInteractionAttemptSignal>(OnPlayerInteractionAttempt);
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
            HandleInteraction(signal.P_PlayerInteraction);
        }

        protected virtual void HandlePointerEnter()
        {
            _signalBus.Subscribe<PlayerInteractionAttemptSignal>(OnPlayerInteractionAttempt);
            _mainRenderer.material = _mouseOverMat;
        }

        protected virtual void HandlePointerExit()
        {
            _signalBus.TryUnsubscribe<PlayerInteractionAttemptSignal>(OnPlayerInteractionAttempt);
            _mainRenderer.material = _regularMat;
        }
    }
}