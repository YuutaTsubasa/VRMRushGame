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
        
        [SerializeField] private PageContainer _pageContainer;
        
        async void Start()
        {
            if (DataContainer.CurrentPlayTime != DataContainer.NO_TIME)
            {
                await _pageContainer.Push(VRM_LOADING_PAGE_NAME, true);
                return;
            }
            
            await _pageContainer.Push(TITLE_PAGE_NAME, true);
        }
    }
}