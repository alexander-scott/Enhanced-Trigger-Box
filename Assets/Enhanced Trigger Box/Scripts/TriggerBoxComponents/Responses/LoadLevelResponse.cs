using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[Serializable]
public class LoadLevelResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// This is the scene name you want to be loaded.
    /// </summary>
    public string loadLevelName;

    public override void DrawInspectorGUI()
    {
        loadLevelName = EditorGUILayout.TextField(new GUIContent("Scene Name",
            "This is the name of the scene you want to be loaded."), loadLevelName);
    }

    public override bool ExecuteAction()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadLevelName);

        return true;
    }
}
