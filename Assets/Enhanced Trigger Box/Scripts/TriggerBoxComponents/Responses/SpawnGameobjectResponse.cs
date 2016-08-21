using UnityEngine;
using UnityEditor;

namespace EnhancedTriggerbox.Component
{
    [AddComponentMenu("")]
    public class SpawnGameobjectResponse : ResponseComponent
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
        /// This is the position and rotation the prefab will be spawned with. If left blank it will use the prefab's saved attributes.
        /// </summary>
        public Transform customPositionRotation;

        public override void DrawInspectorGUI()
        {
            prefabToSpawn = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Prefab to spawn",
                "This is the prefab which will be instanstiated (spawned)."), prefabToSpawn, typeof(GameObject), true);

            newInstanceName = EditorGUILayout.TextField(new GUIContent("New instance name",
                "This field is used to set the name of the newly instantiated object. If left blank the name will remain as the prefab's saved name."), newInstanceName);

            customPositionRotation = (Transform)EditorGUILayout.ObjectField(new GUIContent("Custom Position / Rotation",
                "This is the position and rotation the prefab will be spawned with. If left blank it will use the prefab's saved attributes."),
                customPositionRotation, typeof(Transform), true);

            //if (prefabToSpawn)
            //{
            //    Handles.Label((customPositionRotation == null) ? prefabToSpawn.transform.position : customPositionRotation.position, 
            //        "Spawn " + (string.IsNullOrEmpty(newInstanceName) ? prefabToSpawn.name : newInstanceName));

            //    Handles.PositionHandle((customPositionRotation == null) ? prefabToSpawn.transform.position : customPositionRotation.position,
            //        (customPositionRotation == null) ? prefabToSpawn.transform.rotation : customPositionRotation.rotation);
            //}
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
                        GameObject newobj = Instantiate(prefabToSpawn, customPositionRotation.position, customPositionRotation.rotation) as GameObject;
                        newobj.name = newInstanceName;
                    }
                    else
                    {
                        GameObject newobj = Instantiate(prefabToSpawn) as GameObject;
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
    }
}
