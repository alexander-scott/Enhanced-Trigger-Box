using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class DisableGameobjectResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// The gameobject that will be set to inactive
    /// </summary>
    public GameObject disableGameObject;

    /// <summary>
    /// If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and set to inactive.
    /// </summary>
    public string disableGameObjectName;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (hideShowSection)
        {
            disableGameObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Disable GameObject",
                 "The gameobject that will be set to inactive."), disableGameObject, typeof(GameObject), true);

            disableGameObjectName = EditorGUILayout.TextField(new GUIContent("Disable Object By Name",
               "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and set to inactive."), disableGameObjectName);
        }
    }

    public override bool ExecuteAction()
    {
        if (disableGameObject)
        {
            disableGameObject.SetActive(false);
        }

        if (!string.IsNullOrEmpty(disableGameObjectName))
        {
            GameObject gameobj = GameObject.Find(disableGameObjectName);
            if (gameobj == null)
            {
                Debug.Log("Unable to find and destroy the gameobject with the name " + disableGameObjectName);
            }
            else
            {
                gameobj.SetActive(false);
            }
        }

        return true;
    }
}
