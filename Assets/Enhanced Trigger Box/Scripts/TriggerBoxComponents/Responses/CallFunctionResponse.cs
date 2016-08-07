using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class CallFunctionResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// This is the gameobject on which the below function is called on.
    /// </summary>
    public GameObject messageTarget;

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

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (hideShowSection)
        {
            messageTarget = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Message Target",
                     "This is the gameobject on which the below function is called on."), messageTarget, typeof(GameObject), true);

            messageFunctionName = EditorGUILayout.TextField(new GUIContent("Message Function Name",
                "This is the function which is called on the above gameobject."), messageFunctionName);

            parameterType = (ParameterType)EditorGUILayout.EnumPopup(new GUIContent("Message Type",
                   "This is the type of parameter that will be sent to the function. Options are int, float and string."), parameterType);

            parameterValue = EditorGUILayout.TextField(new GUIContent("Message Value",
                "This is the value of the parameter that will be sent to the function."), parameterValue);
        }
    }

    public override bool ExecuteAction()
    {
        // This will send the messages to the selected gameobjects
        if (messageFunctionName != "" && messageTarget)
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

        return true;
    }
}
