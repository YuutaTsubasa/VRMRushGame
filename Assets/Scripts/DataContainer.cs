using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Yuuta.VRMGo
{
    public static class DataContainer
    {
        public readonly static StageInfo[] Stages =
            Resources.Load<StageInfoCollection>(StageInfoCollection.STAGE_INFO_COLLECTION_NAME)
                .StageInfos;
        
        private static ReactiveProperty<GameObject> _currentModelObject = new ReactiveProperty<GameObject>();

        public static IReadOnlyReactiveProperty<bool> HasModel =>
            _currentModelObject.Select(gameObject => gameObject != null)
                .ToReactiveProperty();

        public static StageInfo CurrentStage { get; private set; } = Stages.First();
        
        public static void SetCurrentModelObject(GameObject gameObject)
        {
            gameObject.SetActive(false);
            Object.DontDestroyOnLoad(gameObject);
            
            if (_currentModelObject != null)
                Object.Destroy(_currentModelObject.Value);
            _currentModelObject.Value = gameObject;
        }

        public static GameObject GetCurrentModelObjectDuplication()
            => GameObject.Instantiate(_currentModelObject.Value);

        public static void SetCurrentStage(StageInfo stageInfo)
        {
            CurrentStage = stageInfo;
        }
    }
}
