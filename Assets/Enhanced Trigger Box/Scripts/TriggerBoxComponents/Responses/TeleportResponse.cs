using UnityEngine;

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
        /// The target game object's name
        /// </summary>
        public string targetGameObjectName;

        /// <summary>
        /// This is the position you want to move the above gameobject to.
        /// </summary>
        public Transform destination;

        /// <summary>
        /// If this checkbox is ticked then the target object's rotation will be set to the destination's rotation
        /// </summary>
        public bool copyRotation;

        /// <summary>
        /// This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.
        /// </summary>
        public ReferenceType referenceType = ReferenceType.GameObjectReference;

        public override bool requiresCollisionObjectData
        {
            get
            {
                return true;
            }
        }

        public enum ReferenceType
        {
            GameObjectReference,
            GameObjectName,
            CollisionGameObject,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            referenceType = (ReferenceType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Reference Type",
                   "This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box."), referenceType);

            switch (referenceType)
            {
                case ReferenceType.GameObjectReference:
                    targetObject = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target Gameobject",
                     "This is the gameobject on which will be moved to the below transform."), targetObject, typeof(GameObject), true);
                    break;

                case ReferenceType.GameObjectName:
                    targetGameObjectName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Target Gameobject Name",
                    "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())."), targetGameObjectName);
                    break;
            }

            destination = (Transform)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Destination",
                     "This is the position you want to move the above gameobject to."), destination, typeof(Transform), true);

            copyRotation = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Copy Rotation",
                "If this checkbox is ticked then the target object's rotation will be set to the destination's rotation"), copyRotation);
        }
#endif

        public override void Validation()
        {
            switch (referenceType)
            {
                case ReferenceType.GameObjectReference:
                    if (targetObject && !destination)
                    {
                        ShowWarningMessage("You have added a gameobject but haven't supplied where to move it to.");
                    }
                    if (!targetObject && destination)
                    {
                        ShowWarningMessage("You have added a destination but you haven't supplied the gameobject that will be moved there.");
                    }
                    break;

                case ReferenceType.GameObjectName:
                    if (!string.IsNullOrEmpty(targetGameObjectName) && !destination)
                    {
                        ShowWarningMessage("You have added a gameobject name but haven't supplied where to move it to.");
                    }
                    if (string.IsNullOrEmpty(targetGameObjectName) && destination)
                    {
                        ShowWarningMessage("You have added a destination but you haven't supplied the gameobject name that will be moved there.");
                    }
                    break;
            }

            ShowWarningMessage("This component has been deprecated and replaced with the ModifyTransform component. Please use that instead.");
        }

        public override bool ExecuteAction(GameObject collisionGameObject)
        {
            switch (referenceType)
            {
                case ReferenceType.CollisionGameObject:
                    targetObject = collisionGameObject;
                    break;

                case ReferenceType.GameObjectName:
                    targetObject = GameObject.Find(targetGameObjectName);
                    break;
            }

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


