using PyramidRecruitmentTask.Input;
using PyramidRecruitmentTask.Signals;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;
        [Inject] private SignalBus    _signalBus;

        public int OwnedKeys { get; private set; }

        private void Awake()
        {
            _inputManager.P_InteractionButton.E_ButtonPress += OnInteraction;
        }

        public void AddKey()
        {
            OwnedKeys++;
        }

        public void UseKey()
        {
            OwnedKeys--;
        }

        private void OnInteraction()
        {
            _signalBus.Fire(new PlayerInteractionAttemptSignal(this));
        }
    }
}