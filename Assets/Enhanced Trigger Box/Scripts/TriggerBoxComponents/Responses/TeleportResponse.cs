using UnityEngine;
using UnityEditor;

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

        public override void DrawInspectorGUI()
        {
            targetObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Target Gameobject",
                     "This is the gameobject on which will be moved to the below transform."), targetObject, typeof(GameObject), true);

            destination = (Transform)EditorGUILayout.ObjectField(new GUIContent("Destination",
                     "This is the position you want to move the above gameobject to."), destination, typeof(Transform), true);
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
                // Set the target objects position to the destination
                targetObject.transform.position = destination.position;
            }

            return true;
        }
    }
}


