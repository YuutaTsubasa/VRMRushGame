using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Page;

namespace Yuuta.VRMGo.Metagame
{
    public class MetagameRunner : MonoBehaviour
    {
        public const string TITLE_PAGE_NAME = "Title";
        public const string VRM_LOADING_PAGE_NAME = "VRMLoading";
        public const string STAGE_SELECT_PAGE_NAME = "StageSelect";
        
        [SerializeField] private PageContainer _pageContainer;
        
        async void Start()
        {
            if (DataContainer.HasModel.Value)
            {
                await _pageContainer.Push(TITLE_PAGE_NAME, false);
                await _pageContainer.Push(VRM_LOADING_PAGE_NAME, false);
                await _pageContainer.Push(STAGE_SELECT_PAGE_NAME, true);
                return;
            }
            
            await _pageContainer.Push(TITLE_PAGE_NAME, true);
        }
    }
}