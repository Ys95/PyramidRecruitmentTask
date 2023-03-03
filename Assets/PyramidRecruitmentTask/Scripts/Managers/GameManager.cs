using Cinemachine;
using PyramidRecruitmentTask.Signals;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Managers
{
    public enum GameState
    {
        MainMenu = 0,
        InGame   = 1,
        GameOver = 2
    }

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private AudioClip _menuBgm;
        [SerializeField] private AudioClip _inGameBgm;

        [Inject] private AudioManager             _audioManager;
        [Inject] private CinemachineVirtualCamera _cmMainCam;

        private          StateMachine<GameState> _gameState;
        [Inject] private ObjectsSpawner          _objectsSpawner;
        [Inject] private SignalBus               _signalBus;
        [Inject] private Timer                   _timer;
        [Inject] private UIManager               _uiManager;

        public GameState P_CurrentState => _gameState.P_CurrentState;

        private void Awake()
        {
            _audioManager.Initialize();

            _gameState                =  new StateMachine<GameState>(GameState.MainMenu, true);
            _gameState.E_StateChanged += OnGameStateChanged;
            OnMainMenuEnter();
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<DoorOpenedSignal>(TriggerGameOver);
            _signalBus.Subscribe<UISignal>(HandleUISignal);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<DoorOpenedSignal>(TriggerGameOver);
            _signalBus.TryUnsubscribe<UISignal>(HandleUISignal);
        }

        public void TriggerStartGame()
        {
            _gameState.ChangeState(GameState.InGame);
        }

        public void TriggerGameOver()
        {
            _gameState.ChangeState(GameState.GameOver);
        }

        public void TriggerReturnToMenu()
        {
            _gameState.ChangeState(GameState.MainMenu);
        }

        private void HandleUISignal(UISignal signal)
        {
            switch (signal.P_SignalType)
            {
                case UISignal.SignalType.GameStartBTNClick:
                    TriggerStartGame();
                    break;

                case UISignal.SignalType.TryAgainBTNClick:
                    TriggerStartGame();
                    break;

                case UISignal.SignalType.MainMenuBTNClick:
                    TriggerReturnToMenu();
                    break;
            }
        }

        private void OnGameStateChanged()
        {
            switch (_gameState.P_PreviousState)
            {
                case GameState.MainMenu:
                    OnMainMenuExit();
                    break;

                case GameState.InGame:
                    OnInGameExit();
                    break;

                case GameState.GameOver:
                    OnGameOverExit();
                    break;
            }

            switch (_gameState.P_CurrentState)
            {
                case GameState.MainMenu:
                    OnMainMenuEnter();
                    break;

                case GameState.InGame:
                    OnInGameEnter();
                    break;

                case GameState.GameOver:
                    OnGameOverEnter();
                    break;
            }
        }

        private void OnMainMenuEnter()
        {
            _objectsSpawner.WipeEverything();
            _uiManager.P_MainMenuScreen.gameObject.SetActive(true);
            _audioManager.PlayBGM(_menuBgm);
        }

        private void OnMainMenuExit()
        {
            _uiManager.P_MainMenuScreen.gameObject.SetActive(false);
        }

        private void OnInGameEnter()
        {
            _objectsSpawner.RespawnEverything();
            _timer.StartTimer();
            _audioManager.PlayBGM(_inGameBgm);
        }

        private void OnInGameExit()
        {
            _timer.StopTimer();
        }

        private void OnGameOverEnter()
        {
            ScoreManager.ProcessNewScore(_timer.P_Time);
            _uiManager.DisplayGameOverScreen();
        }

        private void OnGameOverExit()
        {
            _uiManager.P_GameOverScreen.gameObject.SetActive(false);
        }
    }
}