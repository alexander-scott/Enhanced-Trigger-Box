using System;
using UnityEngine;
using UnityEditor;

[Serializable]
public class SpawnGameobjectResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// This is the prefab which will be instanstiated (spawned).
    /// </summary>
    public GameObject prefabToSpawn;

    /// <summary>
    /// This field is used to set the name of the newly instantiated object. If left blank the name will remain as the prefab's saved name.
    /// </summary>
    public string newInstanceName;

    /// <summary>
    /// This is the position which the prefab will be spawned on. If left blank it will use the prefab's saved position.
    /// </summary>
    public Transform customPositionRotation;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (hideShowSection)
        {
            prefabToSpawn = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Prefab to spawn",
                "This is the prefab which will be instanstiated (spawned)."), prefabToSpawn, typeof(GameObject), true);

            newInstanceName = EditorGUILayout.TextField(new GUIContent("New instance name",
                "This field is used to set the name of the newly instantiated object. If left blank the name will remain as the prefab's saved name."), newInstanceName);

            customPositionRotation = (Transform)EditorGUILayout.ObjectField(new GUIContent("Custom Position / Rotation",
                "This is the position and rotation the prefab will be spawned with. If left blank it will use the prefab's saved attributes."),
                customPositionRotation, typeof(Transform), true);
        }
    }

    public override bool ExecuteAction()
    {
        if (prefabToSpawn)
        {
            // If a newinstancename has been set then we will re-name the instance after it has been created
            if (!string.IsNullOrEmpty(newInstanceName))
            {
                if (customPositionRotation)
                {
                    var newobj = Instantiate(prefabToSpawn, customPositionRotation.position, customPositionRotation.rotation);
                    newobj.name = newInstanceName;
                }
                else
                {
                    var newobj = Instantiate(prefabToSpawn);
                    newobj.name = newInstanceName;
                }
            }
            else
            {
                if (customPositionRotation)
                {
                    Instantiate(prefabToSpawn, customPositionRotation.position, customPositionRotation.rotation);
                }
                else
                {
                    Instantiate(prefabToSpawn);
                }
            }
        }

        return true;
    }

    //public void OnSceneGUI()
    //{
    //    var script = (EnhancedTriggerBox)target;
    //    if (script.prefabToSpawn)
    //    {
    //        Handles.Label(script.spawnPosition, "Spawn " + script.prefabToSpawn.name);

    //        script.spawnPosition = Handles.PositionHandle(script.spawnPosition, Quaternion.identity);
    //    }

    //    if (GUI.changed)
    //        EditorUtility.SetDirty(target);
    //}
}
