using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;
using VRMLoader;

namespace Yuuta.VRMGo.Metagame
{
    public class VRMLoadingPage : Page
    {
        [SerializeField] private ModelLoaderLegacy _vrmLoader;
        [SerializeField] private Button _playButton;
        [SerializeField] private Text _currentTimeText;
        [SerializeField] private Text _bestTimeText;
        
        public override IEnumerator Initialize()
        {
            _currentTimeText.text = DataContainer.CurrentPlayTime == DataContainer.NO_TIME
                ? "本次遊玩紀錄：無"
                : $"本次遊玩紀錄：{DataContainer.GetTimeString(DataContainer.CurrentPlayTime)}";

            _bestTimeText.text = DataContainer.BestPlayTime == DataContainer.NO_TIME
                ? "最佳遊玩紀錄：無"
                : $"最佳遊玩紀錄：{DataContainer.GetTimeString(DataContainer.BestPlayTime)}";
            
            _vrmLoader.OnLoaded.Subscribe(gameObject =>
            {
                DataContainer.SetCurrentModelObject(gameObject);
                SceneManager.LoadScene("Scenes/Main");
            });

            _playButton.interactable = DataContainer.HasModel;
            _playButton.OnClickAsObservable().Subscribe(_ =>
            {
                SceneManager.LoadScene("Scenes/Main");
            });
            
            yield break;
        }
    }
}