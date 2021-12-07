using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        [SerializeField] private Button _backButton;
        
        public override IEnumerator Initialize()
        {
            var pageContainer = PageContainer.Of(transform);
            
            _vrmLoader.OnLoaded.Subscribe(gameObject => UniTask.Void(async () =>
            {
                DataContainer.SetCurrentModelObject(gameObject);
                await pageContainer.Push(MetagameRunner.STAGE_SELECT_PAGE_NAME, true);
            })).AddTo(this);

            _playButton.interactable = DataContainer.HasModel;
            _playButton.OnClickAsObservable().Subscribe(_ => UniTask.Void(async () =>
            {
                await pageContainer.Push(MetagameRunner.STAGE_SELECT_PAGE_NAME, true);
            })).AddTo(this);

            _backButton.OnClickAsObservable().Subscribe(_ => UniTask.Void(async () => 
                    await pageContainer.Pop(true))).AddTo(this);
            
            yield break;
        }
    }
}