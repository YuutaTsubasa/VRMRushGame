using System.Collections;
using System.Collections.Generic;
using UniGLTF;
using UnityEngine;
using UnityEngine.UI;
using VRM;
using Yuuta.VRMGo;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace VRMLoader
{
    public class ModelLoaderLegacy : MonoBehaviour
    {
        [SerializeField, Header("GUI")]
        Canvas m_canvas;

        [SerializeField]
        GameObject m_modalWindowPrefab;

        [SerializeField]
        RuntimeAnimatorController m_animationController;

        [SerializeField]
        Dropdown m_language;

        [SerializeField] private Character _character;

        VRMImporterContext m_context;
        UniHumanoid.HumanPoseTransfer m_target;
        VRMBlendShapeProxy m_blendShape;
        VRMFirstPerson m_firstPerson;

        public void OpenVRM()
        {
            // 環境依存: ファイルを開く
#if UNITY_EDITOR
            var path = EditorUtility.OpenFilePanel("Open VRM file", "", "vrm");
#if UNITY_EDITOR_WIN
            path = "file:///" + path;
#else
            path = "file://" + path;
#endif
#elif UNITY_STANDALONE_WIN
            var path = VRM.RuntimeExporterSample.FileDialogForWindows.FileDialog("open VRM", ".vrm");
            path = "file:///" + path;
#else
            var path = Application.dataPath + "/default.vrm";
            path = "file:///" + path;
#endif
            if (path.Length != 0)
            {
                StartCoroutine(LoadVRMCoroutine(path));
            }
        }
        
        IEnumerator LoadVRMCoroutine(string path)
        {
            var www = new WWW(path);
            yield return www;
            
            // GLB形式のparse
            var glbBinaryParser = new GlbBinaryParser(www.bytes, "VRMModel");
            var gltfData = glbBinaryParser.Parse();
            var vrmData = new VRMData(gltfData);
            m_context = new VRMImporterContext(vrmData);

            // meta情報を読み込む
            bool createThumbnail = true;
            var meta = m_context.ReadMeta(createThumbnail);

            // ファイル読み込みモーダルウィンドウの呼び出し
            GameObject modalObject = Instantiate(m_modalWindowPrefab, m_canvas.transform) as GameObject;

            // 言語設定を取得・反映する
            var modalLocale = modalObject.GetComponentInChildren<VRMPreviewLocale>();
            modalLocale.SetLocale(m_language.captionText.text);

            // meta情報の反映
            var modalUI = modalObject.GetComponentInChildren<VRMPreviewUI>();
            modalUI.setMeta(meta);

            // ファイルを開くことの許可
            // ToDo: ファイルの読み込み許可を制御する場合はここで
            modalUI.setLoadable(true);

            modalUI.m_ok.onClick.AddListener(ModelLoad);
        }

        private async void ModelLoad()
        {
            var now = Time.time;
            var gltfInstance = await m_context.LoadAsync();
            
            gltfInstance.ShowMeshes();
            var go = gltfInstance.Root;
            // load完了
            var delta = Time.time - now;
            Debug.LogFormat("LoadVrmAsync {0:0.0} seconds", delta);
            OnLoaded(go);
        }

        void OnLoaded(GameObject root)
        {
            // 設置先 hierarchy を決める
            _character.SetModel(root);

            // humanPoseTransfer 追加
            var humanPoseTransfer = root.AddComponent<UniHumanoid.HumanPoseTransfer>();
            if (m_target != null)
            {
                GameObject.Destroy(m_target.gameObject);
            }
            m_target = humanPoseTransfer;
            SetupPlayer();
        }

        void SetupPlayer()
        {
            if (m_target != null)
            {
                m_blendShape = m_target.GetComponent<VRMBlendShapeProxy>();
                // ToDo: blendShape コントローラーへの紐づけ 

                m_firstPerson = m_target.GetComponent<VRMFirstPerson>();
                m_firstPerson.Setup();

                // AnimationController の紐づけ
                var animator = m_target.GetComponent<Animator>();
                if (animator != null)
                {
                    animator.runtimeAnimatorController = m_animationController;
                }

/*
                // VRIKのセットアップサンプル
                VRIK m_vrik = m_target.gameObject.AddComponent<VRIK>();
                m_vrik.AutoDetectReferences();
                m_vrik.solver.spine.headTarget = m_headTarget;
                m_vrik.solver.leftArm.target = m_leftHandTarget;
                m_vrik.solver.rightArm.target = m_rightHandTarget;
                m_vrik.solver.leftArm.stretchCurve = new AnimationCurve();
                m_vrik.solver.rightArm.stretchCurve = new AnimationCurve();
                IKSolverVR.Locomotion m_vrikLoco = m_vrik.solver.locomotion;
                m_vrikLoco.footDistance = 0.1f;
                m_vrikLoco.stepThreshold = 0.2f;
*/
            }
        }
    }
}