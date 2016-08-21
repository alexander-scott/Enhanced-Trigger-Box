using UnityEngine;
using UnityEditor;

namespace EnhancedTriggerbox.Component
{
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
        /// Used to determine the datatype of a parameter.
        /// </summary>
        public enum ParameterType
        {
            Int,
            Float,
            String,
        }

        public override void DrawInspectorGUI()
        {
            messageTarget = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Target",
                     "This is the gameobject on which the below function is called on."), messageTarget, typeof(GameObject), true);

            messageTargetName = EditorGUILayout.TextField(new GUIContent("Target Name",
                "If you are unable to provide a reference for a gameobject you can enter it's name here and it will be found using GameObject.Find(). If you have entered a gameobject reference above, leave this field blank."), messageTargetName);

            messageFunctionName = EditorGUILayout.TextField(new GUIContent("Function Name",
                "This is the function which is called on the above gameobject."), messageFunctionName);

            parameterType = (ParameterType)EditorGUILayout.EnumPopup(new GUIContent("Message Type",
                   "This is the type of parameter that will be sent to the function. Options are int, float and string."), parameterType);

            parameterValue = EditorGUILayout.TextField(new GUIContent("Value",
                "This is the value of the parameter that will be sent to the function."), parameterValue);
        }

        public override void Validation()
        {
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

        public override bool ExecuteAction()
        {
            // This will send the messages to the selected gameobjects
            if (messageFunctionName != "" && (messageTarget || !string.IsNullOrEmpty(messageTargetName)))
            {
                if (!string.IsNullOrEmpty(messageTargetName))
                {
                    try
                    {
                        messageTarget = GameObject.Find(messageTargetName);
                    }
                    catch
                    {
                        Debug.Log("Unable to find the gameobject with the name " + messageTargetName);
                    }
                }

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

            return true;
        }
    }
}
