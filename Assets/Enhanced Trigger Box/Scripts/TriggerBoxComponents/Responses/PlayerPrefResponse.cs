using UnityEngine;
using System;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to set a player pref value
    /// </summary>
    [AddComponentMenu("")]
    public class PlayerPrefResponse : ResponseComponent
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
        /// This is the value that will be stored in the player pref. If you enter ++ or -- the value in the player pref will be incremented or decremented respectively.
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

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            setPlayerPrefKey = UnityEditor.EditorGUILayout.TextField(new GUIContent("Player Pref Key",
                "This is the key (ID) of the player pref which will have its value set."), setPlayerPrefKey);

            setPlayerPrefType = (ParameterType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Player Pref Type",
                   "This is the type of data stored within the player pref. Options are int, float and string."), setPlayerPrefType);

            setPlayerPrefVal = UnityEditor.EditorGUILayout.TextField(new GUIContent("Player Pref Value",
                "This is the value that will be stored in the player pref. If you enter ++ or -- the value in the player pref will be incremented or decremented respectively."), setPlayerPrefVal);
        }
#endif

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

            if (!string.IsNullOrEmpty(setPlayerPrefVal) && setPlayerPrefVal != "++" && setPlayerPrefVal != "--")
            {
                switch (setPlayerPrefType)
                {
                    case ParameterType.Float:
                        float f;
                        if (!float.TryParse(setPlayerPrefVal, out f))
                        {
                            ShowWarningMessage("Unable to parse player pref value to a float. Make sure you have entered a valid float.");
                        }
                        break;

                    case ParameterType.Int:
                        int i;
                        if (!int.TryParse(setPlayerPrefVal, out i))
                        {
                            ShowWarningMessage("Unable to parse player pref value to a integer. Make sure you have entered a valid integer.");
                        }
                        break;
                }
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
                        if (setPlayerPrefVal == "++")
                        {
                            int ppV = PlayerPrefs.GetInt(setPlayerPrefKey);
                            ppV = ppV + 1;
                            PlayerPrefs.SetInt(setPlayerPrefKey, ppV);
                        }
                        else if (setPlayerPrefVal == "--")
                        {
                            int ppV = PlayerPrefs.GetInt(setPlayerPrefKey);
                            ppV = ppV - 1;
                            PlayerPrefs.SetInt(setPlayerPrefKey, ppV);
                        }
                        else
                        {
                            PlayerPrefs.SetInt(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
                        }
                        break;

                    case ParameterType.Float:
                        if (setPlayerPrefVal == "++")
                        {
                            float ppV = PlayerPrefs.GetFloat(setPlayerPrefKey);
                            ppV = ppV + 1;
                            PlayerPrefs.SetFloat(setPlayerPrefKey, ppV);
                        }
                        else if (setPlayerPrefVal == "--")
                        {
                            float ppV = PlayerPrefs.GetFloat(setPlayerPrefKey);
                            ppV = ppV - 1;
                            PlayerPrefs.SetFloat(setPlayerPrefKey, ppV);
                        }
                        else
                        {
                            PlayerPrefs.SetFloat(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
                        }
                        break;
                }
            }

            return true;
        }
    }
}
