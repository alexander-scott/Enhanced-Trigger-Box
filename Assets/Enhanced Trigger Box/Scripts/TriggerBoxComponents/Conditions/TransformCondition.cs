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
        /// The destination transform that is used for distance checks between it and the target transform
        /// </summary>
        public Transform destinationTransform;

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
        /// The type of transform check that will be performed. Single Axis only applies to the target object on a specified axis. 
        /// Distance To Object checks calculate distance between the target object and a destination object.
        /// </summary>
        public TransformConditionType transformConditionType;

        /// <summary>
        /// The types of conditions
        /// </summary>
        public enum TransformConditionType
        {
            SingleAxis,
            DistanceToObject2D,
            DistanceToObject3D,
            LocalDistanceToObject2D,
            LocalDistanceToObject3D,
        }

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
            Z
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            targetTransform = (Transform)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target Transform",
                   "The transform to apply the condition to."), targetTransform, typeof(Transform), true);

            transformConditionType = (TransformConditionType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Condition Type",
                "The type of transform check that will be performed. Single Axis only applies to the target object on a specified axis. Distance To Object checks calculate distance between the target object and a destination object."), transformConditionType);

            if (transformConditionType == TransformConditionType.SingleAxis)
            {
                transformComponent = (TransformComponent)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Transform Component",
                "The transform component that will be used for the condition. Either position or rotation."), transformComponent);

                axis = (Axis)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Target Axis",
                "The axis that the condition will be based on."), axis);
            }
            else
            {
                destinationTransform = (Transform)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Destination Transform",
                   "The transform that will be used alongside the target transform to calculate the distance."), destinationTransform, typeof(Transform), true);
            }

            conditionType = (ConditionType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Compare Type",
                "The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to or less than."), conditionType);

            value = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Value", 
                "The value that will be compared against the value calculated above."), value);
        }
#endif

        public override bool ExecuteAction()
        {
            if (!targetTransform)
            {
                Debug.Log("You haven't assigned a target transform in the transform condition.");
                return false;
            }

            switch (transformConditionType)
            {
                case TransformConditionType.SingleAxis:
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
                    break;

                case TransformConditionType.DistanceToObject2D:
                    return CompareValue(Vector2.Distance(targetTransform.position, destinationTransform.position));

                case TransformConditionType.DistanceToObject3D:
                    return CompareValue(Vector3.Distance(targetTransform.position, destinationTransform.position));

                case TransformConditionType.LocalDistanceToObject2D:
                    return CompareValue(Vector2.Distance(targetTransform.localPosition, destinationTransform.localPosition));

                case TransformConditionType.LocalDistanceToObject3D:
                    return CompareValue(Vector3.Distance(targetTransform.localPosition, destinationTransform.localPosition));
            }

            return false;
        }

        public override void Validation()
        {
            if (transformConditionType != TransformConditionType.SingleAxis && !destinationTransform)
            {
                ShowWarningMessage("You need to assign a transform to the Destination Transform to calculate the distance between that and the Target Transform.");
            }
            base.Validation();
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
