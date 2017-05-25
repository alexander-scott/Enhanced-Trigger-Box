using System;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to load/unload a scene
    /// </summary>
    [AddComponentMenu("")]
    public class LoadLevelResponse : ResponseComponent
    {
        public ResponseType responseType;

        /// <summary>
        /// This is the name of the scene you want to be loaded. Only used in Unity 5.3 and above.
        /// </summary>
        public string loadLevelName;

        /// <summary>
        /// This is the number of the scene you want to be loaded. Only used in Unity 5.2 and below.
        /// </summary>
        public int loadLevelNum;

        /// <summary>
        /// If this is true, the scene will be loaded asynchronously. This means that the playing scene won't freeze as it's loading this new scene on a background thread. If this is false everything will freeze/wait until the new scene is loaded.
        /// </summary>
        public bool async;

        /// <summary>
        /// If this is true the new scene will be displayed alongside/as well as the current scene. If this is false the current scene will be unloaded.
        /// </summary>
        public bool additive;

        /// <summary>
        /// The type of response that you want be executed. Either loading or unloading a scene.
        /// </summary>
        public enum ResponseType { LoadScene, UnloadScene, };

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
#if UNITY_5_3_OR_NEWER
            responseType = (ResponseType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Response Type",
                "The type of response that you want be executed. Either loading or unloading a scene."), responseType);

            loadLevelName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Scene Name",
                "This is the name of the scene you want to be loaded."), loadLevelName);

            if (responseType == ResponseType.LoadScene)
            {
                async = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Asynchronously",
                    "If this is true, the scene will be loaded asynchronously. This means that the playing scene won't freeze as it's loading this new scene on a background thread. If this is false everything will freeze/wait until the new scene is loaded."), async);

                additive = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Additive",
                    "If this is true the new scene will be displayed alongside/as well as the current scene. If this is false the current scene will be unloaded."), additive);
            }

#else
            loadLevelNum = UnityEditor.EditorGUILayout.IntField(new GUIContent("Scene Number",
                "This is the index of the scene you want to be loaded."), loadLevelNum);
#endif
        }
#endif

        public override bool ExecuteAction()
        {
#if UNITY_5_3_OR_NEWER

            if (responseType == ResponseType.LoadScene)
            {
                if (async)
                {
                    if (additive)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadLevelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(loadLevelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    }
                }
                else
                {
                    if (additive)
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(loadLevelName, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    }
                    else
                    {
                        UnityEngine.SceneManagement.SceneManager.LoadScene(loadLevelName, UnityEngine.SceneManagement.LoadSceneMode.Single);
                    }
                }
            }
            else
            {
#if UNITY_5_5_OR_NEWER
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(loadLevelName);
#else
                UnityEngine.SceneManagement.SceneManager.UnloadScene(loadLevelName);
#endif
            }

#else
            Application.LoadLevel(loadLevelNum);
#endif
            return true;
        }
    }
}