using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yuuta.VRMGo
{
    [Serializable]
    public class StageInfo
    {
        [Serializable]
        public enum Type
        {
            Normal,
            Autorun
        }

        public const int NO_TIME = -1;
        public const int NO_SCORE = -1;
        
        private const string BEST_PLAY_TIME_KEY = "BEST_PLAY_TIME";
        private const string BEST_SCORE_KEY = "BEST_SCORE";
        
        private string CurrentBestPlayTimeKey => $"{BEST_PLAY_TIME_KEY}_{StageId}";
        private string CurrentBestScoreKey => $"{BEST_SCORE_KEY}_{StageId}";
        
        public int CurrentPlayTime { get; private set; } = NO_TIME;
        public int CurrentScore { get; private set; } = NO_SCORE;
        
        public int BestPlayTime => PlayerPrefs.GetInt(CurrentBestPlayTimeKey, NO_TIME);
        public int BestScore => PlayerPrefs.GetInt(CurrentBestScoreKey, NO_SCORE);
        
        public string StageId;
        public Type StageType;
        public string StagePrefabName;

        public void SetCurrentPlayTime(int time)
        {
            CurrentPlayTime = time;
            if (BestPlayTime == NO_TIME || CurrentPlayTime < BestPlayTime)
                PlayerPrefs.SetInt(CurrentBestPlayTimeKey, time);
        }
        
        public void SetCurrentScore(int score)
        {
            CurrentScore = score;
            if (BestScore == NO_SCORE || CurrentScore > BestScore)
                PlayerPrefs.SetInt(CurrentBestScoreKey, score);
        }
    }
}

