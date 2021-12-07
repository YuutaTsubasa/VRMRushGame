using UnityEditor;
using UnityEngine;

namespace Yuuta.VRMGo
{
    public class StageInfoCollection : ScriptableObject
    {
        public const string STAGE_INFO_COLLECTION_NAME = "StageInfoCollection";
        public static readonly string STAGE_INFO_COLLECTION_PATH = $"Assets/Resources/{STAGE_INFO_COLLECTION_NAME}.asset";

        public StageInfo[] StageInfos;
        
        [MenuItem("Assets/Create/Stage Info Collection")]
        public static void Generate()
        {
            StageInfoCollection asset = CreateInstance<StageInfoCollection>();

            AssetDatabase.CreateAsset(asset, STAGE_INFO_COLLECTION_PATH);
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
    }
}
