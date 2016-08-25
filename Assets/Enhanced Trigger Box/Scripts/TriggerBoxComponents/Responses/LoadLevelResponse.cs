using UnityEngine;
using UnityEditor;

namespace EnhancedTriggerbox.Component
{
    [AddComponentMenu("")]
    public class LoadLevelResponse : ResponseComponent
    {
        public string loadLevelName;

        public int loadLevelNum;

        public override void DrawInspectorGUI()
        {
#if UNITY_5_3_OR_NEWER
            loadLevelName = EditorGUILayout.TextField(new GUIContent("Scene Name",
                "This is the name of the scene you want to be loaded."), loadLevelName);
#else
            loadLevelNum = EditorGUILayout.IntField(new GUIContent("Scene Number",
                "This is the number of the scene you want to be loaded."), loadLevelNum);
#endif
        }

        public override bool ExecuteAction()
        {
#if UNITY_5_3_OR_NEWER
            UnityEngine.SceneManagement.SceneManager.LoadScene(loadLevelName);
#else
            Application.LoadLevel(loadLevelNum);
#endif

            return true;
        }
    }
}
