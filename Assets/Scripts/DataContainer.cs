using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Yuuta.VRMGo
{
    public static class DataContainer
    {
        public readonly static StageInfo[] Stages =
            Resources.Load<StageInfoCollection>(StageInfoCollection.STAGE_INFO_COLLECTION_NAME)
                .StageInfos;
        
        private static GameObject _currentModelObject;

        public static bool HasModel => _currentModelObject != null;

        public static StageInfo CurrentStage { get; private set; } = Stages.First();
        
        public static void SetCurrentModelObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
            Object.DontDestroyOnLoad(gameObject);
            
            if (_currentModelObject != null)
                Object.Destroy(_currentModelObject);
            _currentModelObject = gameObject;
        }

        public static GameObject GetCurrentModelObjectDuplication()
            => GameObject.Instantiate(_currentModelObject);

        public static void SetCurrentStage(StageInfo stageInfo)
        {
            CurrentStage = stageInfo;
        }
    }
}
