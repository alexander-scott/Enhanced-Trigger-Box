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
        /// This flag is used to store if the key has been released when used in conjunction with the condition timer. As it is impossible to release a key for a certain duration
        /// </summary>
        private bool triggered;

        [SerializeField]
        private EnhancedTriggerBox etb; // Serialise and save it so we don't do GetComponent every frame

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

        public override void Validation()
        {
            if (triggerType == TriggerType.OnReleased) // This check means most of the time we don't actually need to do the get component, yet still display the warning if needed
            {
                if (etb == null)
                {
                    etb = GetComponent<EnhancedTriggerBox>();
                }

                if (etb.conditionTime > 0f)
                {
                    ShowWarningMessage("Using a Condition Timer with the OnReleased trigger type will have no effect! This is because it is impossible to release a key for a certain duration.");
                }
            }
        }

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
                    if (triggered) // If has been released before
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