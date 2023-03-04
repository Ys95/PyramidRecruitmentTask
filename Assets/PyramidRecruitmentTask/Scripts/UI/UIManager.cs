using System;
using System.Collections;
using PyramidRecruitmentTask.Managers;
using PyramidRecruitmentTask.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameOverScreen  _gameOverScreen;
        [SerializeField] private MainMenuScreen  _mainMenuScreen;
        [SerializeField] private GameObject      _inGameUI;
        [SerializeField] private TextMeshProUGUI _timerDisplay;

        [Inject] private SignalBus _signalBus;
        
        private void OnEnable()
        {
            _signalBus.Subscribe<TimerStartSignal>(DisplayTimer);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<TimerStartSignal>(StopDisplayingTimer);
        }

        public void DisplayInGameUI(bool display) => _inGameUI.SetActive(display);
        
        public void DisplayGameOverScreen(bool display)
        {
            if (!display)
            {
                _gameOverScreen.Screen.SetActive(false);
                return;
            }
            
            TimeSpan? currentScore = ScoreManager.P_LatestScore;
            TimeSpan? bestScore    = ScoreManager.P_BestScore;

            if (currentScore.HasValue)
            {
                _gameOverScreen.Screen.SetActive(true);
                _gameOverScreen.CurrentScoreTmp.text = $"{currentScore:mm\\:ss\\:ff}";
            }

            if (bestScore.HasValue)
            {
                _gameOverScreen.BestScoreTmp.text = $"{bestScore:mm\\:ss\\:ff}";
            }
            else
            {
                _gameOverScreen.BestScoreTmp.text = "-";
            }
            
            _gameOverScreen.Screen.SetActive(true);
        }

        public void DisplayMainMenu(bool display)
        {
            if (!display)
            {
                _mainMenuScreen.Screen.SetActive(false);
                return;
            }
            
            TimeSpan? bestScore = ScoreManager.P_BestScore;
            
            if (bestScore.HasValue)
            {
                _mainMenuScreen.BestScoreTmp.text = $"{bestScore:mm\\:ss\\:ff}";
            }
            else
            {
                _mainMenuScreen.BestScoreTmp.text = "-";
            }
            
            _mainMenuScreen.Screen.SetActive(true);
        }

        public void DisplayTimer(TimerStartSignal timer)
        {
            _timerDisplay.gameObject.SetActive(true);
            StartCoroutine(CO_UpdateTimerDisplay(timer.P_Timer));
        }

        public void StopDisplayingTimer()
        {
            StopAllCoroutines();
            _timerDisplay.gameObject.SetActive(false);
        }

        public void StartGameBTNClick()
        {
            _signalBus.Fire(new UISignal(UISignal.SignalType.GameStartBTNClick));
        }

        public void MainMenuBTNClick()
        {
            _signalBus.Fire(new UISignal(UISignal.SignalType.MainMenuBTNClick));
        }

        public void TryAgainBTNClick()
        {
            _signalBus.Fire(new UISignal(UISignal.SignalType.TryAgainBTNClick));
        }

        private IEnumerator CO_UpdateTimerDisplay(Timer timer)
        {
            while (timer.P_TimerRunning)
            {
                var timeSpan = timer.P_Time;
                _timerDisplay.text = $"<mspace=55>{timeSpan:mm\\:ss\\:ff}";
                yield return null;
            }
        }

        [Serializable]
        private struct GameOverScreen
        {
            public GameObject      Screen;
            public TextMeshProUGUI CurrentScoreTmp;
            public TextMeshProUGUI BestScoreTmp;
        }
        
        [Serializable]
        private struct MainMenuScreen
        {
            public GameObject      Screen;
            public TextMeshProUGUI BestScoreTmp;
        }
    }
}