﻿using UnityEngine;
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

    public override void DrawInspectorGUI()
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

    public override void Validation()
    {
        // Check that the correct combination of fields have been filled in
        if (messageTarget && string.IsNullOrEmpty(messageFunctionName))
        {
            ShowErrorMessage("You have selected a object for the message to be sent to but haven't specified which function to call!");
        }
        else if (!messageTarget && !string.IsNullOrEmpty(messageFunctionName))
        {
            ShowErrorMessage("You have entered a function to call but haven't specified the object to send it to!");
        }
        else if (messageTarget && !string.IsNullOrEmpty(messageFunctionName) && string.IsNullOrEmpty(parameterValue))
        {
            ShowErrorMessage("You have entered a function and gameobject to send a message to but the message has no value!");
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