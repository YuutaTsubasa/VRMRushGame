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
        
        private const string BEST_PLAY_TIME_KEY = "BEST_PLAY_TIME";
        private string CurrentBestPlayTimeKey => $"{BEST_PLAY_TIME_KEY}_{StageId}";
        public int CurrentPlayTime { get; private set; } = NO_TIME;
        public int BestPlayTime => PlayerPrefs.GetInt(CurrentBestPlayTimeKey, NO_TIME);
        
        public string StageId;
        public Type StageType;
        public string StagePrefabName;

        public void SetCurrentPlayTime(int time)
        {
            CurrentPlayTime = time;
            if (BestPlayTime == NO_TIME || CurrentPlayTime < BestPlayTime)
                PlayerPrefs.SetInt(CurrentBestPlayTimeKey, time);
        }
    }
}

