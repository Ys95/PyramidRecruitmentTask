using System;
using System.IO;
using UnityEngine;

namespace PyramidRecruitmentTask
{
    public enum GameState
    {
        MainMenu = 0,
        InGame   = 1,
        GameOver = 2
    }
    
    public class GameManager : MonoBehaviour
    {
        [Serializable]
        struct SaveData
        {
            [SerializeField] private long _score;

            public TimeSpan P_Score => TimeSpan.FromTicks(_score);
            
            public SaveData(TimeSpan score)
            {
                _score = score.Ticks;
            }
        }

        private string savePath;
        
        private StateMachine<GameState> _gameState;

        public GameState CurrentState => _gameState.P_CurrentState;

        public void StartGame() => _gameState.ChangeState(GameState.InGame);

        public void GameOver() => _gameState.ChangeState(GameState.GameOver);

        public void BackToMenu() => _gameState.ChangeState(GameState.MainMenu);
        
        private void Awake()
        {
            savePath                  =  Application.persistentDataPath + "/save.json";
            _gameState                =  new StateMachine<GameState>(GameState.MainMenu, true);
            _gameState.E_StateChanged += OnGameStateChanged;
            OnMainMenuEnter();
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

        private void CheckScore(TimeSpan currentScore, out TimeSpan? bestScore)
        {
            bestScore = null;
            if (File.Exists(savePath))
            {
                using StreamReader streamReader    = new StreamReader(savePath);
                string             savedDataString = streamReader.ReadToEnd();
                SaveData           savedData       = JsonUtility.FromJson <SaveData> (savedDataString);

                bestScore = savedData.P_Score;
            }
            
            Debug.Log($"Current score: {currentScore}");
            Debug.Log($"Loaded score: {bestScore}");

            bool scoreBetter;
            if (!bestScore.HasValue)
            {
                scoreBetter = true;
                Debug.Log("Score better");
            }
            else
            {
                scoreBetter = currentScore.Ticks < bestScore.Value.Ticks;
                Debug.Log(scoreBetter ? "Score better" : "Score worse");
            }
            
            if (scoreBetter)
            {
                SaveData           save         = new SaveData(currentScore);
                using StreamWriter streamWriter = new StreamWriter(savePath);
                streamWriter.Write(JsonUtility.ToJson(save));
            }
        }

        private void OnMainMenuEnter()
        {
            FindObjectOfType<UIManager>().P_MainMenuScreen.gameObject.SetActive(true);
        }
        
        private void OnMainMenuExit()
        {
            FindObjectOfType<UIManager>().P_MainMenuScreen.gameObject.SetActive(false);
        }
        
        private void OnInGameEnter()
        {
            FindObjectOfType<Player>().transform.position = Vector3.zero;
            FindObjectOfType<UIManager>().DisplayTimer(FindObjectOfType<Timer>());
            FindObjectOfType<Timer>().StartTimer();
            FindObjectOfType<InteractablesSpawner>().RespawnObjects();
        }
        
        private void OnInGameExit()
        {
            FindObjectOfType<UIManager>().StopDisplayingTimer();
            FindObjectOfType<Timer>().StopTimer();
        }

        private void OnGameOverEnter()
        {
            CheckScore(FindObjectOfType<Timer>().P_Time, out var bestScore);
            FindObjectOfType<UIManager>().DisplayGameOverScreen(FindObjectOfType<Timer>().P_Time, bestScore);
        }
        
        private void OnGameOverExit()
        {
            FindObjectOfType<UIManager>().P_GameOverScreen.gameObject.SetActive(false);
        }
    }
}