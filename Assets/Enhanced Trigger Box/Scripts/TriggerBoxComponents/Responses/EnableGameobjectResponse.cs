using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class EnableGameobjectResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// The gameobject that will be set to active
    /// </summary>
    public GameObject enableGameObject;

    public override void DrawInspectorGUI()
    {
        enableGameObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Enable GameObject",
             "The gameobject that will be set to active."), enableGameObject, typeof(GameObject), true);
    }

    public override bool ExecuteAction()
    {
        if (enableGameObject)
        {
            enableGameObject.SetActive(true);
        }

        return true;
    }
}
