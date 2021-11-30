using System;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yuuta.VRMGo
{
    public class MainPage : MonoBehaviour
    {
        public async void ChangeToMetaGameScene()
        {
            await Observable.Timer(TimeSpan.FromSeconds(5f));
            SceneManager.LoadScene("Scenes/Metagame");
        }
    }
}
