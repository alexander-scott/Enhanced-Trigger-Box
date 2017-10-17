using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    public class ModifyTransformResponse : ResponseComponent
    {
        public Transform targetTransform;

        public SelectAttribute targetAttribute;

        public Axis targetAxis;

        public bool localSpace;

        public float targetValue;

        public enum SelectAttribute
        {
            Position,
            Rotation,
            Scale,
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
                     "The target transform that will be modified."), targetTransform, typeof(Transform), true);

            targetAttribute = (SelectAttribute)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Target Attribute",
                "The attribute on the target transform you want to modify. This can be either Transform, Rotation or Scale."), targetAttribute);

            targetAxis = (Axis)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Target Axis",
                "The Axis that you want to modify on this attribute on this transform. This can be either X, Y or Z."), targetAxis);

            localSpace = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Local Space",
                   "If this value is true, the modifications will be done in local space rather than world space."), localSpace);

            targetValue = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Target Value",
                    "The value you would like to set this"), targetValue);

            duration = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Change Duration",
                    "The duration that the selected change will happen over in seconds. If you leave it as 0 it will perform the changes instantly."), duration);
        }
#endif

        public override bool ExecuteAction()
        {
            if (!targetTransform)
            {
                Debug.LogError("Target Transform has not been assigned a reference on the ModifyTransformResponse.");
                return false;
            }

            if (duration != 0f)
            {
                activeCoroutines.Add(StartCoroutine(ChangeAttributeOverTime()));
            }
            else
            {
                SetValue(targetValue);
            }

            return false;
        }

        private IEnumerator ChangeAttributeOverTime()
        {
            float smoothness = 0.02f; 
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            float startValue = GetStartValue();

            while (progress < 1)
            {
                SetValue(Mathf.Lerp(startValue, targetValue, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private float GetStartValue()
        {
            switch (targetAttribute)
            {
                case SelectAttribute.Position:
                    switch (targetAxis)
                    {
                        case Axis.X:
                            if (localSpace)
                                return targetTransform.localPosition.x;
                            else
                                return targetTransform.position.x;

                        case Axis.Y:
                            if (localSpace)
                                return targetTransform.localPosition.y;
                            else
                                return targetTransform.position.y;

                        case Axis.Z:
                            if (localSpace)
                                return targetTransform.localPosition.z;
                            else
                                return targetTransform.position.z;
                    }

                    break;

                case SelectAttribute.Rotation:
                    switch (targetAxis)
                    {
                        case Axis.X:
                            if (localSpace)
                                return targetTransform.localEulerAngles.x;
                            else
                                return targetTransform.eulerAngles.x;

                        case Axis.Y:
                            if (localSpace)
                                return targetTransform.localEulerAngles.y;
                            else
                                return targetTransform.eulerAngles.y;

                        case Axis.Z:
                            if (localSpace)
                                return targetTransform.localEulerAngles.z;
                            else
                                return targetTransform.eulerAngles.z;
                    }

                    break;

                case SelectAttribute.Scale:
                    switch (targetAxis)
                    {
                        case Axis.X:
                            if (localSpace)
                                return targetTransform.localScale.x;
                            else
                                return targetTransform.lossyScale.x;

                        case Axis.Y:
                            if (localSpace)
                                return targetTransform.localScale.y;
                            else
                                return targetTransform.lossyScale.y;

                        case Axis.Z:
                            if (localSpace)
                                return targetTransform.localScale.z;
                            else
                                return targetTransform.lossyScale.z;
                    }

                    break;
            }

            return 0f;
        }

        private void SetValue(float value)
        {
            switch (targetAttribute)
            {
                case SelectAttribute.Position:
                    switch (targetAxis)
                    {
                        case Axis.X:
                            if (localSpace)
                                targetTransform.localPosition = new Vector3(value, targetTransform.localPosition.y, targetTransform.localPosition.z);
                            else
                                targetTransform.position = new Vector3(value, targetTransform.position.y, targetTransform.position.z);
                            break;

                        case Axis.Y:
                            if (localSpace)
                                targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, value, targetTransform.localPosition.z);
                            else
                                targetTransform.position = new Vector3(targetTransform.position.x, value, targetTransform.position.z);
                            break;

                        case Axis.Z:
                            if (localSpace)
                                targetTransform.localPosition = new Vector3(targetTransform.localPosition.x, targetTransform.localPosition.y, value);
                            else
                                targetTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, value);
                            break;
                    }

                    break;

                case SelectAttribute.Rotation:
                    switch (targetAxis)
                    {
                        case Axis.X:
                            if (localSpace)
                                targetTransform.localEulerAngles = new Vector3(value, targetTransform.localEulerAngles.y, targetTransform.localEulerAngles.z);
                            else
                                targetTransform.eulerAngles = new Vector3(value, targetTransform.eulerAngles.y, targetTransform.eulerAngles.z);
                            break;

                        case Axis.Y:
                            if (localSpace)
                                targetTransform.localEulerAngles = new Vector3(targetTransform.localEulerAngles.x, value, targetTransform.localEulerAngles.z);
                            else
                                targetTransform.eulerAngles = new Vector3(targetTransform.eulerAngles.x, value, targetTransform.eulerAngles.z);
                            break;

                        case Axis.Z:
                            if (localSpace)
                                targetTransform.localEulerAngles = new Vector3(targetTransform.localEulerAngles.x, targetTransform.localEulerAngles.y, value);
                            else
                                targetTransform.eulerAngles = new Vector3(targetTransform.eulerAngles.x, targetTransform.eulerAngles.y, value);
                            break;
                    }

                    break;

                case SelectAttribute.Scale:
                    switch (targetAxis)
                    {
                        case Axis.X:
                            if (localSpace)
                                targetTransform.localScale = new Vector3(value, targetTransform.localScale.y, targetTransform.localScale.z);
                            else
                                targetTransform.lossyScale.Set(value, targetTransform.lossyScale.y, targetTransform.lossyScale.z);
                            break;

                        case Axis.Y:
                            if (localSpace)
                                targetTransform.localScale = new Vector3(targetTransform.localScale.x, value, targetTransform.localScale.z);
                            else
                                targetTransform.lossyScale.Set(targetTransform.lossyScale.x, value, targetTransform.lossyScale.z);
                            break;

                        case Axis.Z:
                            if (localSpace)
                                targetTransform.localScale = new Vector3(targetTransform.localScale.x, targetTransform.localScale.y, value);
                            else
                                targetTransform.lossyScale.Set(targetTransform.lossyScale.x, targetTransform.lossyScale.y, value);
                            break;
                    }

                    break;
            }
        }
    }
}