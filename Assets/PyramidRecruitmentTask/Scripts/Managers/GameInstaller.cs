using Cinemachine;
using PyramidRecruitmentTask.Input;
using PyramidRecruitmentTask.Signals;
using PyramidRecruitmentTask.UI;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Managers
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        [SerializeField] private CinemachineVirtualCamera _mainCmCam;

        [Header("Managers")]
        [SerializeField] private GameManager _gameManager;
        [SerializeField] private InputManager   _inputManager;
        [SerializeField] private Timer          _timer;
        [SerializeField] private UIManager      _uiManager;
        [SerializeField] private ObjectsSpawner _objectsSpawner;
        [SerializeField] private AudioManager   _audioManager;

        public override void InstallBindings()
        {
            DeclareSignals();
            BindFromInstance();
        }

        private void BindFromInstance()
        {
            Container.Bind<InputManager>().FromInstance(_inputManager);
            Container.Bind<CinemachineVirtualCamera>().FromInstance(_mainCmCam);
            Container.Bind<GameManager>().FromInstance(_gameManager);
            Container.Bind<Timer>().FromInstance(_timer);
            Container.Bind<UIManager>().FromInstance(_uiManager);
            Container.Bind<ObjectsSpawner>().FromInstance(_objectsSpawner);
            Container.Bind<AudioManager>().FromInstance(_audioManager);
        }

        private void DeclareSignals()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<PlayerInteractionAttemptSignal>().OptionalSubscriber();
            Container.DeclareSignal<TimerStartSignal>().OptionalSubscriber();
            Container.DeclareSignal<TimerStopSignal>().OptionalSubscriber();
            Container.DeclareSignal<ShowInteractionPopupSignal>().OptionalSubscriber();
            Container.DeclareSignal<HideInteractionPopupSignal>().OptionalSubscriber();
            Container.DeclareSignal<DoorOpenedSignal>().OptionalSubscriber();
            Container.DeclareSignal<UISignal>().OptionalSubscriber();
            Container.DeclareSignal<PlayAudioSignal>().OptionalSubscriber();
        }
    }
}