using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to move a gameobject to another position
    /// </summary>
    [AddComponentMenu("")]
    public class TeleportResponse : ResponseComponent
    {
        /// <summary>
        /// This is the gameobject on which will be moved to the below transform.
        /// </summary>
        public GameObject targetObject;

        /// <summary>
        /// This is the position you want to move the above gameobject to.
        /// </summary>
        public Transform destination;

        /// <summary>
        /// If this checkbox is ticked then the target object's rotation will be set to the destination's rotation
        /// </summary>
        public bool copyRotation;

        public override void DrawInspectorGUI()
        {
#if UNITY_EDITOR

            targetObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Target Gameobject",
                     "This is the gameobject on which will be moved to the below transform."), targetObject, typeof(GameObject), true);

            destination = (Transform)EditorGUILayout.ObjectField(new GUIContent("Destination",
                     "This is the position you want to move the above gameobject to."), destination, typeof(Transform), true);

            copyRotation = EditorGUILayout.Toggle(new GUIContent("Copy Rotation",
                "If this checkbox is ticked then the target object's rotation will be set to the destination's rotation"), copyRotation);

#endif
        }

        public override void Validation()
        {
            if (targetObject && !destination)
            {
                ShowWarningMessage("You have added a gameobject but haven't supplied where to move it to.");
            }
            if (!targetObject && destination)
            {
                ShowWarningMessage("You have added a destination but you haven't supplied the gameobject that will be moved there.");
            }
        }

        public override bool ExecuteAction()
        {
            if (targetObject && destination)
            {
                // Set the target object's position to the destination
                targetObject.transform.position = destination.position;

                // Set the target object's rotation to the destionation's rotation
                if (copyRotation)
                {
                    targetObject.transform.rotation = destination.rotation;
                }
            }

            return true;
        }
    }
}


