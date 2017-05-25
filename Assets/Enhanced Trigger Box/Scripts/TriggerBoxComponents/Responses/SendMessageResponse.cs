using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to call a function on a gameobject and pass in a parameter
    /// </summary>
    [AddComponentMenu("")]
    public class SendMessageResponse : ResponseComponent
    {
        /// <summary>
        /// This is the gameobject on which the below function is called on.
        /// </summary>
        public GameObject messageTarget;

        /// <summary>
        /// If you are unable to provide a reference for a gameobject you can enter it's name here and it will be found using GameObject.Find()
        /// </summary>
        public string messageTargetName;

        /// <summary>
        /// This is the function which is called on the above gameobject.
        /// </summary>
        public string messageFunctionName;

        /// <summary>
        /// This is the type of parameter that will be sent to the function. Options are int, float and string.
        /// </summary>
        public ParameterType parameterType;

        /// <summary>
        /// This is the value of the parameter that will be sent to the function.
        /// </summary>
        public string parameterValue;

        /// <summary>
        /// This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.
        /// </summary>
        public ReferenceType referenceType;

        public override bool requiresCollisionObjectData
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Used to determine the datatype of a parameter.
        /// </summary>
        public enum ParameterType
        {
            Int,
            Float,
            String,
        }

        public enum ReferenceType
        {
            Null,
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
                    messageTarget = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target Gameobject",
                     "This is the gameobject on which will be moved to the below transform."), messageTarget, typeof(GameObject), true);
                    break;

                case ReferenceType.GameObjectName:
                    messageTargetName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Target Gameobject Name",
                    "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())."), messageTargetName);
                    break;
            }

            messageFunctionName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Function Name",
                "This is the function which is called on the above gameobject."), messageFunctionName);

            parameterType = (ParameterType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Message Type",
                   "This is the type of parameter that will be sent to the function. Options are int, float and string."), parameterType);

            parameterValue = UnityEditor.EditorGUILayout.TextField(new GUIContent("Value",
                "This is the value of the parameter that will be sent to the function."), parameterValue);
        }
#endif

        public override void Validation()
        {
            if (referenceType == ReferenceType.Null)
            {
                if (!messageTarget && !string.IsNullOrEmpty(messageTargetName))
                {
                    referenceType = ReferenceType.GameObjectName;
                }
                else
                {
                    referenceType = ReferenceType.GameObjectReference;
                }
            }

            // Check that the correct combination of fields have been filled in
            if (messageTarget && string.IsNullOrEmpty(messageFunctionName))
            {
                ShowWarningMessage("You have selected a object for the message to be sent to but haven't specified which function to call!");
            }

            if (!messageTarget && !string.IsNullOrEmpty(messageFunctionName))
            {
                ShowWarningMessage("You have entered a function to call but haven't specified the object to send it to!");
            }

            if (messageTarget && !string.IsNullOrEmpty(messageFunctionName) && string.IsNullOrEmpty(parameterValue))
            {
                ShowWarningMessage("You have entered a function and gameobject to send a message to but the message has no value!");
            }

            if (messageTarget && !string.IsNullOrEmpty(messageTargetName))
            {
                ShowWarningMessage("You cannot input a gameobject reference and a gameobject name. Please remove one or the other.");
            }

            if (parameterType == ParameterType.Int && !string.IsNullOrEmpty(parameterValue))
            {
                int i;
                if (!int.TryParse(parameterValue, out i))
                {
                    ShowWarningMessage("The message value you have entered is not a valid int. Please make sure you enter a valid int for the message value.");
                }
            }
            else if (parameterType == ParameterType.Float && !string.IsNullOrEmpty(parameterValue))
            {
                float f;
                if (!float.TryParse(parameterValue, out f))
                {
                    ShowWarningMessage("The message value you have entered is not a valid float. Please make sure you enter a valid float for the message value.");
                }
            }
        }

        public override bool ExecuteAction(GameObject collisionGameObject)
        {
            switch (referenceType)
            {
                case ReferenceType.Null:
                    if (!messageTarget && !string.IsNullOrEmpty(messageTargetName)) // Prevents error
                    {
                        messageTarget = GameObject.Find(messageTargetName);
                    }
                    break;

                case ReferenceType.CollisionGameObject:
                    messageTarget = collisionGameObject;
                    break;

                case ReferenceType.GameObjectName:
                    messageTarget = GameObject.Find(messageTargetName);
                    break;
            }

            // This will send the messages to the selected gameobjects
            if (messageTarget)
            {
                if (parameterValue != "")
                {
                    switch (parameterType)
                    {
                        case ParameterType.Int:
                            messageTarget.SendMessage(messageFunctionName, int.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
                            break;
                        case ParameterType.Float:
                            messageTarget.SendMessage(messageFunctionName, float.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
                            break;
                        case ParameterType.String:
                            messageTarget.SendMessage(messageFunctionName, parameterValue, SendMessageOptions.DontRequireReceiver);
                            break;
                    }
                }
                else
                {
                    messageTarget.SendMessage(messageFunctionName, SendMessageOptions.DontRequireReceiver);
                }
            }
            else
            {
                Debug.Log("Unable to execute Send Message Response. Gameobject not found!");
            }

            return true;
        }
    }
}
