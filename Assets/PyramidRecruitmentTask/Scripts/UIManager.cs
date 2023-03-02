using System;
using TMPro;
using UnityEngine;

namespace PyramidRecruitmentTask
{
    public class UIManager : MonoBehaviour
    {
        [Serializable]
        struct GameOverScreen
        {
            public GameObject      Screen;
            public TextMeshProUGUI CurrentScoreTmp;
            public TextMeshProUGUI BestScoreTmp;
        }
        
        [SerializeField] private GameOverScreen  _gameOverScreen;
        [SerializeField] private GameObject      _mainMenuScreen;
        [SerializeField] private TextMeshProUGUI _timerDisplay;
        

        private Timer currentTimer;

        public GameObject      P_GameOverScreen => _gameOverScreen.Screen;
        public GameObject      P_MainMenuScreen => _mainMenuScreen;
        public TextMeshProUGUI P_TimerDisplay   => _timerDisplay;

        public void DisplayGameOverScreen(TimeSpan currentScore, TimeSpan? bestScore)
        {
            _gameOverScreen.Screen.SetActive(true);
            _gameOverScreen.CurrentScoreTmp.text = $"{currentScore.Minutes}:{currentScore.Seconds}:{currentScore.Milliseconds}";

            if (bestScore.HasValue)
            {
                _gameOverScreen.BestScoreTmp.text = $"{bestScore.Value.Minutes}:{bestScore.Value.Seconds}:{bestScore.Value.Milliseconds}";
            }
            else
            {
                _gameOverScreen.BestScoreTmp.text = $"-";
            }
        }
        
        public void DisplayTimer(Timer timer)
        {
            currentTimer         =  timer;
            _timerDisplay.gameObject.SetActive(true);
            timer.E_TimerUpdated += UpdateTimerDisplay;
        }

        public void StopDisplayingTimer()
        {
            currentTimer.E_TimerUpdated -= UpdateTimerDisplay;
            _timerDisplay.gameObject.SetActive(false);
            currentTimer = null;
        }

        private void UpdateTimerDisplay(TimeSpan timeSpan)
        {
            _timerDisplay.text = $"{timeSpan.Minutes}:{timeSpan.Seconds}:{timeSpan.Milliseconds}";
        }
    }
}