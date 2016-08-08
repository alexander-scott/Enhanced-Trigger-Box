using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class DestroyGameobjectResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// This is the gameobject that will be destroyed.
    /// </summary>
    public GameObject destroyGameObject;

    /// <summary>
    /// If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and destroyed.
    /// </summary>
    public string destroyGameObjectName;

    public override void DrawInspectorGUI()
    {
        destroyGameObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Destroy Gameobject",
            "This is the gameobject that will be destroyed."), destroyGameObject, typeof(GameObject), true);

        destroyGameObjectName = EditorGUILayout.TextField(new GUIContent("Destroy Object By Name",
            "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and destroyed."), destroyGameObjectName);
    }

    public override bool ExecuteAction()
    {
        if (destroyGameObject)
        {
            Destroy(destroyGameObject);
        }

        if (!string.IsNullOrEmpty(destroyGameObjectName))
        {
            GameObject gameobj = GameObject.Find(destroyGameObjectName);
            if (gameobj == null)
            {
                Debug.Log("Unable to find and destroy the gameobject with the name " + destroyGameObjectName);
            }
            else
            {
                Destroy(gameobj);
            }
        }

        return true;
    }
}
