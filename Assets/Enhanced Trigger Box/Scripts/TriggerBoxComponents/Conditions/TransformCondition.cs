#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// Condition that requires either a position or rotation value of a transform to be greater/equal/less than a value
    /// </summary>
    [AddComponentMenu("")]
    public class TransformCondition : ConditionComponent
    {
        /// <summary>
        /// The transform to apply the condition to.
        /// </summary>
        public Transform targetTransform;

        /// <summary>
        /// The transform component that will be used for the condition. Either position or rotation.
        /// </summary>
        public TransformComponent transformComponent;

        /// <summary>
        /// The axis that the condition will be based on.
        /// </summary>
        public Axis axis;

        /// <summary>
        /// The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than.
        /// </summary>
        public ConditionType conditionType;

        /// <summary>
        /// The value that will be compared against the value in the axis selected above.
        /// </summary>
        public float value;

        /// <summary>
        /// The types of transform components
        /// </summary>
        public enum TransformComponent
        {
            Position,
            Rotation,
            LocalPosition,
            LocalRotation,
        }

        /// <summary>
        /// The available types of player pref conditions such as greater than and less than.
        /// </summary>
        public enum ConditionType
        {
            GreaterThan,
            GreaterThanOrEqualTo,
            EqualTo,
            LessThanOrEqualTo,
            LessThan,
        }

        /// <summary>
        /// The available axis'
        /// </summary>
        public enum Axis
        {
            X,
            Y,
            Z,
        }

        public override void DrawInspectorGUI()
        {
#if UNITY_EDITOR

            targetTransform = (Transform)EditorGUILayout.ObjectField(new GUIContent("Target Transform",
                   "The transform to apply the condition to."), targetTransform, typeof(Transform), true);

            transformComponent = (TransformComponent)EditorGUILayout.EnumPopup(new GUIContent("Transform Component",
                "The transform component that will be used for the condition. Either position or rotation."), transformComponent);

            axis = (Axis)EditorGUILayout.EnumPopup(new GUIContent("Target Axis",
                "The axis that the condition will be based on."), axis);

            conditionType = (ConditionType)EditorGUILayout.EnumPopup(new GUIContent("Condition Type",
                "The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than."), conditionType);

            value = EditorGUILayout.FloatField(new GUIContent("Value", 
                "The value that will be compared against the value in the axis selected above."), value);

#endif
        }

        public override bool ExecuteAction()
        {
            if (!targetTransform)
            {
                Debug.Log("You haven't assigned a target transform in the transform condition.");
                return false;
            }

            switch (transformComponent)
            {
                case TransformComponent.Position:
                    switch (axis)
                    {
                        case Axis.X:
                            return CompareValue(targetTransform.position.x);

                        case Axis.Y:
                            return CompareValue(targetTransform.position.y);

                        case Axis.Z:
                            return CompareValue(targetTransform.position.z);
                    }
                    break;

                case TransformComponent.Rotation:
                    switch (axis)
                    {
                        case Axis.X:
                            return CompareValue(targetTransform.rotation.x);

                        case Axis.Y:
                            return CompareValue(targetTransform.rotation.y);

                        case Axis.Z:
                            return CompareValue(targetTransform.rotation.z);
                    }
                    break;

                case TransformComponent.LocalPosition:
                    switch (axis)
                    {
                        case Axis.X:
                            return CompareValue(targetTransform.localPosition.x);

                        case Axis.Y:
                            return CompareValue(targetTransform.localPosition.y);

                        case Axis.Z:
                            return CompareValue(targetTransform.localPosition.z);
                    }
                    break;

                case TransformComponent.LocalRotation:
                    switch (axis)
                    {
                        case Axis.X:
                            return CompareValue(targetTransform.localRotation.x);

                        case Axis.Y:
                            return CompareValue(targetTransform.localRotation.y);

                        case Axis.Z:
                            return CompareValue(targetTransform.localRotation.z);
                    }
                    break;
            }

            return false;
        }

        /// <summary>
        /// Returns true or false depending on how the parameter compares to the inputted value
        /// </summary>
        /// <param name="val">The value pulled from the transform</param>
        /// <returns>True or false depending on how the parameter compares to the inputted value</returns>
        private bool CompareValue(float val)
        {
            switch (conditionType)
            {
                case ConditionType.EqualTo:
                    if (val == value)
                        return true;
                    else
                        return false;

                case ConditionType.GreaterThan:
                    if (val > value)
                        return true;
                    else
                        return false;

                case ConditionType.GreaterThanOrEqualTo:
                    if (val >= value)
                        return true;
                    else
                        return false;

                case ConditionType.LessThan:
                    if (val < value)
                        return true;
                    else
                        return false;

                case ConditionType.LessThanOrEqualTo:
                    if (val <= value)
                        return true;
                    else
                        return false;

                default:
                    return false;
            }
        }
    }
}
