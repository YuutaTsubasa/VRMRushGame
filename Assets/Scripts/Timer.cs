using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Yuuta.VRMGo
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private Text _displayText;

        private int _seconds = 0;
        private IDisposable _timerProcess;

        public int Time => _seconds;

        void Start()
        {
            _UpdateText();
            _timerProcess = Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(_ =>
                {
                    ++_seconds;
                    _UpdateText();
                }).AddTo(this);
        }
        
        public void Stop()
        {
            _timerProcess.Dispose();
        }
        
        private void _UpdateText()
        {
            _displayText.text =
                $"Time: {TimeUtility.GetTimeString(_seconds)}";
        }
    }

}
