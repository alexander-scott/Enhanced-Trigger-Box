using UnityEngine;
using System.Collections;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// Condition that requires a camera to either be looking at or looking away from a gameobject
    /// </summary>
    [AddComponentMenu("")]
    public class InputCondition : ConditionComponent
    {
        /// <summary>
        /// The key that needs to be interacted with to meet the condition.
        /// </summary>
        public KeyCode inputKey;

        /// <summary>
        /// The type of interaction required. OnPressed requires the user to simply press the key down. OnReleased requires the user to let go of the key after pressing it.
        /// </summary>
        public TriggerType triggerType;

        /// <summary>
        /// </summary>
        private bool triggered;

        public enum TriggerType
        {
            OnPressed,
            OnReleased,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            inputKey = (KeyCode)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Input key",
                "The key that needs to be interacted with to meet the condition."), inputKey);

            triggerType = (TriggerType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Trigger Type",
                "The type of interaction required. OnPressed requires the user to simply press the key down. OnReleased requires the user to let go of the key after pressing it. " +
                "NOTE: OnReleased will not work with the condition timer. As soon as it gets met once it will remain true for the remainder of the timer. OnPressed however requires the user to continue holding it down for the duration of the timer."), triggerType);
        }
#endif

        public override bool ExecuteAction()
        {
            switch (triggerType)
            {
                case TriggerType.OnPressed:
                    if (Input.GetKey(inputKey)) // Return true as soon as the key is PRESSED
                    {
                        return true;
                    }
                    break;

                case TriggerType.OnReleased:
                        return true;

                    if (Input.GetKeyUp(inputKey)) // Return true as soon as the key is RELEASED
                    {
                        triggered = true;
                        return true;
                    }
                    break;
            }

            return false;
        }

        public override void ResetComponent()
        {
            triggered = false;
        }
    }
}
