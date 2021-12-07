using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityScreenNavigator.Runtime.Core.Page;
using Button = UnityEngine.UI.Button;

namespace Yuuta.VRMGo.Metagame
{
    public class StageSelectPage : Page
    {
        [SerializeField] private StageItem _stageItemPrefab;
        [SerializeField] private Transform _stageItemRootTransform;
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private Button _backButton;
        
        public override IEnumerator Initialize()
        {
            var pageContainer = PageContainer.Of(transform);
            
            foreach (var stageInfo in DataContainer.Stages)
            {
                var stageItemView = Instantiate(_stageItemPrefab, _stageItemRootTransform);
                stageItemView.SetStage(stageInfo);
            }
            _scrollRect.normalizedPosition = Vector2.zero;
            
            _backButton.OnClickAsObservable().Subscribe(_ => UniTask.Void(async () => 
                await pageContainer.Pop(true))).AddTo(this);
            
            yield break;
        }
    }
}