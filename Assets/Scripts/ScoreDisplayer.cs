using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Yuuta.VRMGo
{
    public class ScoreDisplayer : MonoBehaviour
    {
        [SerializeField] private Text _displayText;

        private int _score = 0;
        
        public int Score => _score;

        public void AddScore(int score)
        {
            _score += score;
        }

        private void Update()
        {
            _displayText.text = $"Score: {_score}";
        }
    }

}