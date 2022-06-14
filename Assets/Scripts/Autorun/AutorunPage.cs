using System;
using System.Linq;
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
        [SerializeField] private ScoreDisplayer _scoreDisplayer;
        
        private void Start()
        {
            Instantiate(Resources.Load<GameObject>(DataContainer.CurrentStage.StagePrefabName));
            _character.SetModel(DataContainer.GetCurrentModelObjectDuplication());
            _character.transform.position = StageUtility.FindStart().position;

            var finishGameObject = StageUtility.FindFinish().gameObject;
            var finishEventCollections = finishGameObject.AddComponent<EventCollections>();
            var finishEventObservable = Observable.FromEvent(
                handler => new UnityAction(handler),
                handler => finishEventCollections.Events += handler,
                handler => finishEventCollections.Events -= handler);

            IDisposable finishStageDisposable = null;
            finishStageDisposable = finishEventObservable.Subscribe(_ => UniTask.Void(async () =>
            {
                finishStageDisposable?.Dispose();
                _timer.Stop();
                DataContainer.CurrentStage.SetCurrentPlayTime(_timer.Time);
                DataContainer.CurrentStage.SetCurrentScore(_scoreDisplayer.Score);
                _winGameObject.SetActive(true);
                
                await Observable.Timer(TimeSpan.FromSeconds(5f));
                SceneManager.LoadScene("Scenes/Metagame");
            })).AddTo(this);

            var addScoreItems = StageUtility.FindAddScoreItems();
            var addScoreItemsEventCollections = addScoreItems.Select(
                addScoreItem => 
                    (
                        Item: addScoreItem,
                        EventCollections: addScoreItem.gameObject.AddComponent<EventCollections>()
                    ));
            var addScoreEventObservables = addScoreItemsEventCollections.Select(
                eventCollection => (
                    Item: eventCollection.Item,
                    Observable: Observable.FromEvent(
                        handler => new UnityAction(handler),
                        handler => eventCollection.EventCollections.Events += handler,
                        handler => eventCollection.EventCollections.Events -= handler)));

            IDisposable addScoreDisposable = new CompositeDisposable(
                addScoreEventObservables.Select(eventObservable =>
                    eventObservable.Observable.Subscribe(
                        _ =>
                        {
                            _scoreDisplayer.AddScore(eventObservable.Item.Score);
                            Destroy(eventObservable.Item.gameObject);
                        }).AddTo(this)));
        }
    }
}
