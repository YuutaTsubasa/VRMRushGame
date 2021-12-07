using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yuuta.VRMGo.Metagame
{
    public class StageItem : MonoBehaviour
    {
        [SerializeField] private Text _stageNameText;
        [SerializeField] private Text _bestTimeText;
        [SerializeField] private Button _playButton;

        public void SetStage(StageInfo stageInfo)
        {
            _stageNameText.text = stageInfo.StageId;
            _bestTimeText.text = $"Best: {(stageInfo.BestPlayTime == StageInfo.NO_TIME ? "無" : TimeUtility.GetTimeString(stageInfo.BestPlayTime))}";
            _playButton.OnClickAsObservable().Subscribe(_ =>
            {
                DataContainer.SetCurrentStage(stageInfo);
                SceneManager.LoadScene("Scenes/Main");
            });
        }
    }

}
