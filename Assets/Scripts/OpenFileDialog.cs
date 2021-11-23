using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;

namespace Yuuta.VRMGo
{
    
    public class OpenFileDialog : MonoBehaviour, IPointerDownHandler
    {
        public string Path { get; private set; }
        
        public static async UniTask<string> Open()
        {
            var gameObject = new GameObject("OpenFileDialog");
            var openFileDialog = gameObject.AddComponent<OpenFileDialog>();
            await UniTask.WaitWhile(() => string.IsNullOrEmpty(openFileDialog.Path));
            return openFileDialog.Path;
        }
        
        void Start() {
            Application.ExternalEval(
                @"
document.addEventListener('click', function() {

    var fileuploader = document.getElementById('fileuploader');
    if (!fileuploader) {
        fileuploader = document.createElement('input');
        fileuploader.setAttribute('style','display:none;');
        fileuploader.setAttribute('type', 'file');
        fileuploader.setAttribute('id', 'fileuploader');
        fileuploader.setAttribute('class', 'focused');
        document.getElementsByTagName('body')[0].appendChild(fileuploader);

        fileuploader.onchange = function(e) {
        var files = e.target.files;
            for (var i = 0, f; f = files[i]; i++) {
                window.alert(URL.createObjectURL(f));
                SendMessage('" + gameObject.name +@"', 'FileDialogResult', URL.createObjectURL(f));
            }
        };
    }
    if (fileuploader.getAttribute('class') == 'focused') {
        fileuploader.setAttribute('class', '');
        fileuploader.click();
    }
});
            ");
        }

        public void OnPointerDown (PointerEventData eventData)  {
            Application.ExternalEval(
                @"
var fileuploader = document.getElementById('fileuploader');
if (fileuploader) {
    fileuploader.setAttribute('class', 'focused');
}
            ");
        }

        public void FileDialogResult(string fileUrl) {
            Debug.Log(fileUrl);
            Path = fileUrl;
        }
    }
}