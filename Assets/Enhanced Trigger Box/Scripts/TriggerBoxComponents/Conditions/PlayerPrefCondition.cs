using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class PlayerPrefCondition : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to, less than
    /// </summary>
    public PrefCondition playerPrefCondition;

    /// <summary>
    /// The value that will be used to compare against the value stored in the player pref.
    /// </summary>
    public string playerPrefVal;

    /// <summary>
    /// The key (ID) of the player pref that will be compared against the above value.
    /// </summary>
    public string playerPrefKey;

    /// <summary>
    /// The type of the player pref. Options are int, float or string
    /// </summary>
    public ParameterType playerPrefType;

    /// <summary>
    /// Holds the value stored in the player pref as a float
    /// </summary>
    private float playerPrefFloat;

    /// <summary>
    /// Holds the value stored in the player pref as a int
    /// </summary>
    private int playerPrefInt;

    /// <summary>
    /// Holds the value stored in the player pref as a string
    /// </summary>
    private string playerPrefString;

    /// <summary>
    /// This is a conversion of playerPrefVal to a float
    /// </summary>
    private float playerPrefValFloat;

    /// <summary>
    /// This is a conversion of playerPrefVal to a int
    /// </summary>
    private int playerPrefValInt;

    /// <summary>
    /// Used to determine the datatype of a parameter.
    /// </summary>
    public enum ParameterType
    {
        Int,
        Float,
        String,
    }

    /// <summary>
    /// The available types of player pref conditions such as greater than and less than.
    /// </summary>
    public enum PrefCondition
    {
        None,
        GreaterThan,
        GreaterThanOrEqualTo,
        EqualTo,
        LessThanOrEqualTo,
        LessThan,
    }

    public override void DrawInspectorGUI()
    {
        playerPrefCondition = (PrefCondition)EditorGUILayout.EnumPopup(new GUIContent("Condition Type",
            " The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than"), playerPrefCondition);

        playerPrefKey = EditorGUILayout.TextField(new GUIContent("Player Pref Key",
            "The key (ID) of the player pref that will be compared against the above value."), playerPrefKey);

        playerPrefType = (ParameterType)EditorGUILayout.EnumPopup(new GUIContent("Player Pref Type",
               "This is the type of data stored within the player pref. Options are int, float and string."), playerPrefType);

        playerPrefVal = EditorGUILayout.TextField(new GUIContent("Player Pref Value",
            "This is the value that will be stored in the player pref."), playerPrefVal);

        switch (playerPrefType)
        {
            case ParameterType.Float:
                float.TryParse(playerPrefVal, out playerPrefValFloat);
                break;

            case ParameterType.Int:
                int.TryParse(playerPrefVal, out playerPrefValInt);
                break;
        }
    }

    public override void Validation()
    {
        // If there is a player pref condition check that there is a value for the condition
        if (playerPrefCondition != PrefCondition.None)
        {
            if (string.IsNullOrEmpty(playerPrefVal))
            {
                ShowErrorMessage("You have set up a player pref condition but haven't entered a value to be compared against the player pref!");
            }
            else if (string.IsNullOrEmpty(playerPrefKey))
            {
                ShowErrorMessage("You have set up a player pref condition but haven't entered a player pref key!");
            }
        }
    }

    public override bool ExecuteAction()
    {
        // Get the player pref values. We need to do this regularly in case they change at runtime.
        GetUpdatedPlayerPrefs();

        if (playerPrefCondition != PrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.String:
                    if (playerPrefVal == playerPrefString)
                        return true;
                    else
                        return false;

                case ParameterType.Float:
                    switch (playerPrefCondition)
                    {
                        case PrefCondition.EqualTo:
                            if (playerPrefValFloat == playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PrefCondition.GreaterThan:
                            if (playerPrefValFloat > playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PrefCondition.GreaterThanOrEqualTo:
                            if (playerPrefValFloat >= playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PrefCondition.LessThan:
                            if (playerPrefValFloat < playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PrefCondition.LessThanOrEqualTo:
                            if (playerPrefValFloat <= playerPrefFloat)
                                return true;
                            else
                                return false;
                    }
                    break;

                case ParameterType.Int:
                    switch (playerPrefCondition)
                    {
                        case PrefCondition.EqualTo:
                            if (playerPrefValInt == playerPrefInt)
                                return true;
                            else
                                return false;

                        case PrefCondition.GreaterThan:
                            if (playerPrefValInt > playerPrefInt)
                                return true;
                            else
                                return false;

                        case PrefCondition.GreaterThanOrEqualTo:
                            if (playerPrefValInt >= playerPrefInt)
                                return true;
                            else
                                return false;

                        case PrefCondition.LessThan:
                            if (playerPrefValInt < playerPrefInt)
                                return true;
                            else
                                return false;

                        case PrefCondition.LessThanOrEqualTo:
                            if (playerPrefValInt <= playerPrefInt)
                                return true;
                            else
                                return false;
                    }
                    break;
            }
        }

        return false;
    }

    /// <summary>
    /// This function gets the values set in the player pref
    /// </summary>
    private void GetUpdatedPlayerPrefs()
    {
        if (playerPrefCondition != PrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.Float:
                    playerPrefFloat = PlayerPrefs.GetFloat(playerPrefKey);
                    break;

                case ParameterType.Int:
                    playerPrefInt = PlayerPrefs.GetInt(playerPrefKey);
                    break;

                case ParameterType.String:
                    playerPrefString = PlayerPrefs.GetString(playerPrefString);
                    break;
            }
        }
    }
}
