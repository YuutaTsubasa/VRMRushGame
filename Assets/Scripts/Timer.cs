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
        
        public void StopAndSave()
        {
            _timerProcess.Dispose();
            DataContainer.SetCurrentTime(_seconds);
        }
        
        private void _UpdateText()
        {
            _displayText.text =
                $"Time: {DataContainer.GetTimeString(_seconds)}";
        }
    }

}
