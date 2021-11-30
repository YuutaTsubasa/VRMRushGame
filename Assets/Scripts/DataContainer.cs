using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yuuta.VRMGo
{
    public static class DataContainer
    {
        private const string BEST_PLAY_TIME_KEY = "BEST_PLAY_TIME";
        public const int NO_TIME = -1;

        public static int CurrentPlayTime { get; private set; } = NO_TIME;
        public static int BestPlayTime => PlayerPrefs.GetInt(BEST_PLAY_TIME_KEY, NO_TIME);

        private static GameObject _currentModelObject;

        public static bool HasModel => _currentModelObject != null;
        
        public static void SetCurrentModelObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
            Object.DontDestroyOnLoad(gameObject);
            
            if (_currentModelObject != null)
                Object.Destroy(_currentModelObject);
            _currentModelObject = gameObject;
        }

        public static GameObject GetCurrentModelObjectDuplication()
            => GameObject.Instantiate(_currentModelObject);

        public static void SetCurrentTime(int time)
        {
            CurrentPlayTime = time;
            if (BestPlayTime == NO_TIME && CurrentPlayTime < BestPlayTime)
                PlayerPrefs.SetInt(BEST_PLAY_TIME_KEY, time);
        }

        public static string GetTimeString(int time)
            => $"{_AlignTimeNumber(time / 3600)}:{_AlignTimeNumber(time % 3600 / 60)}:{_AlignTimeNumber(time % 60)}";
        
        private static string _AlignTimeNumber(int number)
            => number.ToString().PadLeft(2, '0');
    }
}
