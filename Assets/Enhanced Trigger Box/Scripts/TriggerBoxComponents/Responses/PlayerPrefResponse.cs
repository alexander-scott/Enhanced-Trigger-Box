using UnityEngine;
using UnityEditor;
using System;

public class PlayerPrefResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// This is the key (ID) of the player pref which will have its value set.
    /// </summary>
    public string setPlayerPrefKey;

    /// <summary>
    /// This is the type of data stored within the player pref.
    /// </summary>
    public ParameterType setPlayerPrefType;

    /// <summary>
    /// This is the value that will be stored in the player pref.
    /// </summary>
    public string setPlayerPrefVal;

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
        setPlayerPrefKey = EditorGUILayout.TextField(new GUIContent("Player Pref Key",
            "This is the key (ID) of the player pref which will have its value set."), setPlayerPrefKey);

        setPlayerPrefType = (ParameterType)EditorGUILayout.EnumPopup(new GUIContent("Player Pref Type",
               "This is the type of data stored within the player pref. Options are int, float and string."), setPlayerPrefType);

        setPlayerPrefVal = EditorGUILayout.TextField(new GUIContent("Player Pref Value",
            "This is the value that will be stored in the player pref."), setPlayerPrefVal);
    }

    public override void Validation()
    {
        // Check that the correct combination of fields have been filled in
        if (string.IsNullOrEmpty(setPlayerPrefKey) && !string.IsNullOrEmpty(setPlayerPrefVal))
        {
            ShowWarningMessage("You have entered a value to save to a player pref but you haven't specified which player pref to save it to!");
        }
        else if (!string.IsNullOrEmpty(setPlayerPrefKey) && string.IsNullOrEmpty(setPlayerPrefVal))
        {
            ShowWarningMessage("You have set the player pref key but the value to save in it is empty!");
        }
    }

    public override bool ExecuteAction()
    {
        if (!string.IsNullOrEmpty(setPlayerPrefKey))
        {
            switch (setPlayerPrefType)
            {
                case ParameterType.String:
                    PlayerPrefs.SetString(setPlayerPrefKey, setPlayerPrefVal);
                    break;

                case ParameterType.Int:
                    PlayerPrefs.SetInt(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
                    break;

                case ParameterType.Float:
                    PlayerPrefs.SetFloat(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
                    break;
            }
        }

        return true;
    }
}
