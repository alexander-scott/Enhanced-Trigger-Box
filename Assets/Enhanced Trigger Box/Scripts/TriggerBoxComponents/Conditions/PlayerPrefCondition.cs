using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// Condition that requires a certain player pref to be greater/equal/less than a value
    /// </summary>
    [AddComponentMenu("")]
    public class PlayerPrefCondition : ConditionComponent
    {
        /// <summary>
        /// The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than.
        /// </summary>
        public PrefCondition playerPrefCondition;

        /// <summary>
        /// The key (ID) of the player pref that will be compared against the above value.
        /// </summary>
        public string playerPrefKey;

        /// <summary>
        /// This is the type of data stored within the player pref. Options are int, float and string.
        /// </summary>
        public ParameterType playerPrefType;

        /// <summary>
        /// The value that will be used to compare against the value stored in the player pref.
        /// </summary>
        public string playerPrefVal;

        /// <summary>
        /// If true, the value in the player pref will be retrieved every time the condition check happens. If false, it will only retrieve the player pref value once, when the game first starts.
        /// </summary>
        public bool refreshEveryFrame = true;

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
            GreaterThan,
            GreaterThanOrEqualTo,
            EqualTo,
            LessThanOrEqualTo,
            LessThan,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            playerPrefCondition = (PrefCondition)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Condition Type",
                "The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than."), playerPrefCondition);

            playerPrefKey = UnityEditor.EditorGUILayout.TextField(new GUIContent("Player Pref Key",
                "The key (ID) of the player pref that will be compared against the above value."), playerPrefKey);

            playerPrefType = (ParameterType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Player Pref Type",
                   "This is the type of data stored within the player pref. Options are int, float and string."), playerPrefType);

            playerPrefVal = UnityEditor.EditorGUILayout.TextField(new GUIContent("Player Pref Value",
                "This is the value that will be stored in the player pref."), playerPrefVal);

            refreshEveryFrame = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Refresh Every Frame",
                "If true, the value in the player pref will be retrieved every time the condition check happens. If false, it will only retrieve the player pref value once, when the game first starts."), refreshEveryFrame);
        }
#endif

        public override void Validation()
        {
            if (string.IsNullOrEmpty(playerPrefKey))
            {
                ShowWarningMessage("You have set up a player pref condition but haven't entered a player pref key!");
            }
            if (playerPrefType == ParameterType.String && playerPrefCondition != PrefCondition.EqualTo)
            {
                ShowWarningMessage("You can only use the equal to Condition Type when the parameter is a string.");
            }
            if (string.IsNullOrEmpty(playerPrefVal))
            {
                ShowWarningMessage("You have set up a player pref condition but haven't entered a value to be compared against the player pref!");
            }
        }

        public override void OnAwake()
        {
            // Get the player prefs value
            GetUpdatedPlayerPrefs();

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

        public override bool ExecuteAction()
        {
            // If refresh every frame is true, we will retrieve the player prefs value
            if (refreshEveryFrame)
                GetUpdatedPlayerPrefs();

            if (!string.IsNullOrEmpty(playerPrefVal))
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
                                if (playerPrefFloat == playerPrefValFloat)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.GreaterThan:
                                if (playerPrefFloat > playerPrefValFloat)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.GreaterThanOrEqualTo:
                                if (playerPrefFloat >= playerPrefValFloat)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.LessThan:
                                if (playerPrefFloat < playerPrefValFloat)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.LessThanOrEqualTo:
                                if (playerPrefFloat <= playerPrefValFloat)
                                    return true;
                                else
                                    return false;
                        }
                        break;

                    case ParameterType.Int:
                        switch (playerPrefCondition)
                        {
                            case PrefCondition.EqualTo:
                                if (playerPrefInt == playerPrefValInt)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.GreaterThan:
                                if (playerPrefInt > playerPrefValInt)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.GreaterThanOrEqualTo:
                                if (playerPrefInt >= playerPrefValInt)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.LessThan:
                                if (playerPrefInt < playerPrefValInt)
                                    return true;
                                else
                                    return false;

                            case PrefCondition.LessThanOrEqualTo:
                                if (playerPrefInt <= playerPrefValInt)
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
            if (!string.IsNullOrEmpty(playerPrefVal) && !string.IsNullOrEmpty(playerPrefKey))
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
                        playerPrefString = PlayerPrefs.GetString(playerPrefKey);
                        break;
                }
            }
        }
    }
}
