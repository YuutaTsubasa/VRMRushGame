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
        [SerializeField] private Text _bestScoreText;
        [SerializeField] private Button _playButton;

        public void SetStage(StageInfo stageInfo)
        {
            _stageNameText.text = stageInfo.StageId;
            _bestTimeText.text = $"Best: {(stageInfo.BestPlayTime == StageInfo.NO_TIME ? "無" : TimeUtility.GetTimeString(stageInfo.BestPlayTime))}";
            _bestScoreText.text = $"Best: {(stageInfo.BestScore == StageInfo.NO_SCORE ? "無" : stageInfo.BestScore.ToString())}";
            _playButton.OnClickAsObservable().Subscribe(_ =>
            {
                DataContainer.SetCurrentStage(stageInfo);

                switch (stageInfo.StageType)
                {
                    case StageInfo.Type.Normal:
                        SceneManager.LoadScene("Scenes/Main");
                        break;
                    case StageInfo.Type.Autorun:
                        SceneManager.LoadScene("Scenes/Autorun");
                        break;
                }
            });
        }
    }

}
