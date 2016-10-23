using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to load a scene
    /// </summary>
    [AddComponentMenu("")]
    public class LoadLevelResponse : ResponseComponent
    {
        /// <summary>
        /// This is the name of the scene you want to be loaded. Only used in Unity 5.3 and above.
        /// </summary>
        public string loadLevelName;

        /// <summary>
        /// This is the number of the scene you want to be loaded. Only used in Unity 5.2 and below.
        /// </summary>
        public int loadLevelNum;

        public override void DrawInspectorGUI()
        {
#if UNITY_EDITOR
#if UNITY_5_3_OR_NEWER
            loadLevelName = EditorGUILayout.TextField(new GUIContent("Scene Name",
                "This is the name of the scene you want to be loaded."), loadLevelName);
#else
            loadLevelNum = EditorGUILayout.IntField(new GUIContent("Scene Number",
                "This is the number of the scene you want to be loaded."), loadLevelNum);
#endif
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
