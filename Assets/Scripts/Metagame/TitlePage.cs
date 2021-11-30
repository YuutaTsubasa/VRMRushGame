using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityScreenNavigator.Runtime.Core.Page;

namespace Yuuta.VRMGo.Metagame
{
    public class TitlePage : Page
    {
        [SerializeField] private Button _fullScreenButton;
        
        public override IEnumerator Initialize()
        {
            var pageContainer = PageContainer.Of(transform);
            _fullScreenButton.OnClickAsObservable()
                .Subscribe(_ => UniTask.Void(async () =>
                {
                    await pageContainer.Push(MetagameRunner.VRM_LOADING_PAGE_NAME, true);
                })).AddTo(this);
            
            yield break;
        }
    }
}