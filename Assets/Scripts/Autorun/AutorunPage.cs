using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Yuuta.VRMGo
{
    public class AutorunPage : MonoBehaviour
    {
        [SerializeField] private AutorunCharacter _character;
        [SerializeField] private GameObject _winGameObject;
        [SerializeField] private Timer _timer;
        
        private void Start()
        {
            Instantiate(Resources.Load<GameObject>(DataContainer.CurrentStage.StagePrefabName));
            _character.SetModel(DataContainer.GetCurrentModelObjectDuplication());
            _character.transform.position = StageUtility.FindStart().position;

            var finishGameObject = StageUtility.FindFinish().gameObject;
            var eventCollections = finishGameObject.AddComponent<EventCollections>();
            var eventObservable = Observable.FromEvent(
                handler => new UnityAction(handler),
                handler => eventCollections.Events += handler,
                handler => eventCollections.Events -= handler);

            IDisposable finishStageDisposable = null;
            finishStageDisposable = eventObservable.Subscribe(_ => UniTask.Void(async () =>
            {
                finishStageDisposable?.Dispose();
                _timer.Stop();
                DataContainer.CurrentStage.SetCurrentPlayTime(_timer.Time);
                _winGameObject.SetActive(true);
                
                await Observable.Timer(TimeSpan.FromSeconds(5f));
                SceneManager.LoadScene("Scenes/Metagame");
            })).AddTo(this);
        }
    }
}
