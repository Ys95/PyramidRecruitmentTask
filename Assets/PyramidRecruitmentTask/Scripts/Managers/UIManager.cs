using System;
using System.Collections;
using PyramidRecruitmentTask.Signals;
using TMPro;
using UnityEngine;
using Zenject;

namespace PyramidRecruitmentTask.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameOverScreen  _gameOverScreen;
        [SerializeField] private GameObject      _mainMenuScreen;
        [SerializeField] private TextMeshProUGUI _timerDisplay;

        [Inject] private SignalBus _signalBus;

        public GameObject      P_GameOverScreen => _gameOverScreen.Screen;
        public GameObject      P_MainMenuScreen => _mainMenuScreen;
        public TextMeshProUGUI P_TimerDisplay   => _timerDisplay;

        private void OnEnable()
        {
            _signalBus.Subscribe<TimerStartSignal>(DisplayTimer);
        }

        private void OnDisable()
        {
            _signalBus.TryUnsubscribe<TimerStartSignal>(StopDisplayingTimer);
        }

        public void DisplayGameOverScreen()
        {
            TimeSpan? currentScore = ScoreManager.P_LatestScore;
            TimeSpan? bestScore    = ScoreManager.P_BestScore;

            if (currentScore.HasValue)
            {
                _gameOverScreen.Screen.SetActive(true);
                _gameOverScreen.CurrentScoreTmp.text = $"{currentScore.Value.Minutes}:{currentScore.Value.Seconds}:{currentScore.Value.Milliseconds}";
            }

            if (bestScore.HasValue)
            {
                _gameOverScreen.BestScoreTmp.text = $"{bestScore.Value.Minutes}:{bestScore.Value.Seconds}:{bestScore.Value.Milliseconds}";
            }
            else
            {
                _gameOverScreen.BestScoreTmp.text = "-";
            }
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
                _timerDisplay.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}:{timeSpan.Milliseconds}";
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
    }
}