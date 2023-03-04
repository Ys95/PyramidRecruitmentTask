using System;
using System.IO;
using UnityEngine;

namespace PyramidRecruitmentTask.Managers
{
    public static class ScoreManager
    {
        private static TimeSpan? _bestScore;

        private static readonly string savePath = Application.persistentDataPath + "/save.json";

        public static TimeSpan? P_BestScore
        {
            get
            {
                LoadBestScore();
                return _bestScore;
            }
        }

        public static TimeSpan? P_LatestScore { get; private set; }

        public static void ProcessNewScore(TimeSpan newScore)
        {
            P_LatestScore = newScore;

            bool      newScoreBetter;
            TimeSpan? bestScore = P_BestScore;
            if (!bestScore.HasValue)
            {
                newScoreBetter = true;
            }
            else
            {
                newScoreBetter = newScore.Ticks < bestScore.Value.Ticks;
            }

            if (newScoreBetter)
            {
                var       save         = new SaveData(newScore);
                using var streamWriter = new StreamWriter(savePath);
                streamWriter.Write(JsonUtility.ToJson(save));
            }
        }

        private static void LoadBestScore()
        {
            if (!File.Exists(savePath))
            {
                return;
            }

            using var streamReader    = new StreamReader(savePath);
            string    savedDataString = streamReader.ReadToEnd();
            var       savedData       = JsonUtility.FromJson<SaveData>(savedDataString);

            _bestScore = savedData.P_Score;
        }

        [Serializable]
        private struct SaveData
        {
            [SerializeField] private long _score;

            public SaveData(TimeSpan score)
            {
                _score = score.Ticks;
            }

            public TimeSpan P_Score => TimeSpan.FromTicks(_score);
        }
    }
}