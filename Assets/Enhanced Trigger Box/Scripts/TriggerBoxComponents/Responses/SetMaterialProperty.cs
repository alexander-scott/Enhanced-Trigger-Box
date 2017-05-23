using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to set a value within a material
    /// </summary>
    [AddComponentMenu("")]
    public class SetMaterialProperty : ResponseComponent
    {
        public Material targetMaterial;

        public string propertyName;
        public PropertyType propertyType;

        public float propertyFloat;
        public int propertyInt;
        public Color propertyColour;
        public Vector4 propertyVector4;
        public Texture propertyTexture;

        public enum PropertyType
        {
            Float,
            Int,
            Colour,
            Vector4,
            Texture,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            targetMaterial = (Material)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Material",
                "A reference of the material that you want to set a properties value."), targetMaterial, typeof(Material), true);

            propertyName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Material Property Name",
                   "The name of the property that you want to set."), propertyName);

            propertyType = (PropertyType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Material Property Type",
                "The type of the property that you want to set."), propertyType);

            switch (propertyType)
            {
                case PropertyType.Float:
                    propertyFloat = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyFloat);
                    break;

                case PropertyType.Int:
                    propertyInt = UnityEditor.EditorGUILayout.IntField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyInt);
                    break;

                case PropertyType.Colour:
                    propertyColour = UnityEditor.EditorGUILayout.ColorField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyColour);
                    break;

                case PropertyType.Vector4:
                    propertyVector4 = UnityEditor.EditorGUILayout.Vector4Field(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyVector4);
                    break;

                case PropertyType.Texture:
                    propertyTexture = (Texture)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyTexture, typeof(Texture), true);
                    break;
            }

            if (propertyType != PropertyType.Texture)
            {
                duration = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Change Duration",
                    "The duration you want this change to happen over."), duration);
            }
        }
#endif

        public override bool ExecuteAction()
        {
            if (targetMaterial == null || string.IsNullOrEmpty(propertyName))
            {
                return true;
            }

            if (duration > 0)
            {
                ApplyOverTime();
            }
            else
            {
                ApplyInstantly();
            }
            
            return true;
        }

        private void ApplyInstantly()
        {
            switch (propertyType)
            {
                case PropertyType.Float:
                    targetMaterial.SetFloat(propertyName, propertyFloat);
                    break;

                case PropertyType.Int:
                    targetMaterial.SetInt(propertyName, propertyInt);
                    break;

                case PropertyType.Colour:
                    targetMaterial.SetColor(propertyName, propertyColour);
                    break;

                case PropertyType.Vector4:
                    targetMaterial.SetVector(propertyName, propertyVector4);
                    break;

                case PropertyType.Texture:
                    targetMaterial.SetTexture(propertyName, propertyTexture);
                    break;
            }
        }

        private void ApplyOverTime()
        {
            switch (propertyType)
            {
                case PropertyType.Float:
                    StartCoroutine(ModifyMaterialFloatOverTime());
                    break;

                case PropertyType.Int:
                    StartCoroutine(ModifyMaterialIntOverTime());
                    break;

                case PropertyType.Colour:
                    StartCoroutine(ModifyMaterialColourOverTime());
                    break;

                case PropertyType.Vector4:
                    StartCoroutine(ModifyMaterialVector4OverTime());
                    break;

                case PropertyType.Texture:
                    targetMaterial.SetTexture(propertyName, propertyTexture);
                    break;
            }
        }

        #region Modify Material Coroutines

        private IEnumerator ModifyMaterialFloatOverTime()
        {
            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            float originalValue = targetMaterial.GetFloat(propertyName);

            while (progress < 1)
            {
                targetMaterial.SetFloat(propertyName, Mathf.Lerp(originalValue, propertyFloat, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private IEnumerator ModifyMaterialIntOverTime()
        {
            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            int originalValue = targetMaterial.GetInt(propertyName);

            while (progress < 1)
            {
                targetMaterial.SetInt(propertyName, Mathf.RoundToInt(Mathf.Lerp(originalValue, propertyFloat, progress)));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private IEnumerator ModifyMaterialColourOverTime()
        {
            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            Color originalValue = targetMaterial.GetColor(propertyName);

            while (progress < 1)
            {
                targetMaterial.SetColor(propertyName, Color.Lerp(originalValue, propertyColour, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private IEnumerator ModifyMaterialVector4OverTime()
        {
            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            Vector4 originalValue = targetMaterial.GetVector(propertyName);

            while (progress < 1)
            {
                targetMaterial.SetVector(propertyName, Vector4.Lerp(originalValue, propertyVector4, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

#endregion

        public override void Validation()
        {
            if (targetMaterial)
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    switch (propertyType)
                    {
                        case PropertyType.Float:
                            if (string.IsNullOrEmpty(propertyFloat.ToString()))
                            {
                                ShowWarningMessage("You need to enter a value to set the property with!");
                            }
                            break;

                        case PropertyType.Int:
                            if (string.IsNullOrEmpty(propertyInt.ToString()))
                            {
                                ShowWarningMessage("You need to enter a value to set the property with!");
                            }
                            break;

                        case PropertyType.Colour:
                            if (propertyColour == null)
                            {
                                ShowWarningMessage("You need to enter a colour to set the property with!");
                            }
                            break;

                        case PropertyType.Vector4:
                            if (propertyVector4 == null)
                            {
                                ShowWarningMessage("You need to enter a vector to set the property with!");
                            }
                            break;

                        case PropertyType.Texture:
                            if (propertyTexture == null)
                            {
                                ShowWarningMessage("You need to enter a texture to set the property with!");
                            }
                            break;
                    }
                }
                else
                {
                    ShowWarningMessage("You need to enter the name of the property to want to set!");
                }
            }
        }
    }
}
