using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to modify an individual light source or the scene's lighting settings.
    /// </summary>
    [AddComponentMenu("")]
    public class LightingResponse : ResponseComponent
    {
        /// <summary>
        /// Select whether you want to modify an indivdual light or the scene's lighting settings
        /// </summary>
        public EditType editType;

        #region Modify light

        /// <summary>
        /// The light that will modified
        /// </summary>
        public Light targetLight;

        /// <summary>
        /// Choose to change the colour of this light. Remain the same will not change the colour.
        /// </summary>
        public ChangeColourType changeColourType;

        /// <summary>
        /// The colour that the target light will be set to.
        /// </summary>
        public Color setColour;

        /// <summary>
        /// The intensity you want to set the target light to. If you leave this field blank the light intensity will not be changed.
        /// </summary>
        public string setIntensity;

        /// <summary>
        /// The bounce intensity you want to set the target light to. If you leave this field blank the light bounce intensity will not be changed.
        /// </summary>
        public string setBounceIntensity;

        /// <summary>
        /// The range you want to set the target light's range to. If you leave this field blank the range will not be changed. Only displayed when a spot or point light is selected.
        /// </summary>
        public string setRange;

        #endregion

        #region Modify scene lighting

        /// <summary>
        /// This is the material that you want to set the scene's skybox to. If you leave this field blank the skybox will not be changed.
        /// </summary>
        public Material setSkybox;

        /// <summary>
        /// Choose to change the colour of the scene's ambient light. Remain the same will not change the colour.
        /// </summary>
        public ChangeColourType changeAmbientLightColour;

        /// <summary>
        /// The colour that the scene's ambient light will be set to.
        /// </summary>
        public Color ambientLightColour;

        #endregion

        /// <summary>
        /// The available edit types for this response
        /// </summary>
        public enum EditType
        {
            SingleLightSource,
            SceneLighting,
        }

        /// <summary>
        /// The available change types for the colour enum
        /// </summary>
        public enum ChangeColourType
        {
            RemainTheSame,
            EditColor,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            editType = (EditType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Edit Type",
                "Select whether you want to modify an indivdual light or the scene's lighting settings."), editType);

            // This if statement only displays properties that are appropiate for the edit type selected above
            if (editType == EditType.SingleLightSource)
            {
                targetLight = (Light)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target Light",
                "The light that will modified."), targetLight, typeof(Light), true);

                changeColourType = (ChangeColourType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Change Colour",
                    "Choose to change the colour of this light. Remain the same will not change the colour."), changeColourType);

                if (changeColourType == ChangeColourType.EditColor)
                {
                    setColour = UnityEditor.EditorGUILayout.ColorField(new GUIContent("Set Colour",
                        "The colour that the target light will be set to."), setColour);
                }

                setIntensity = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Intensity",
                    "The intensity you want to set the target light's intensity to. If you leave this field blank the light intensity will not be changed."), setIntensity);

                setBounceIntensity = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Bounce Intensity",
                   "The bounce intensity you want to set the target light's bounce intensity to. If you leave this field blank the light bounce intensity will not be changed."), setBounceIntensity);

                if (targetLight && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
                {
                    setRange = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Range",
                        "The range you want to set the target light's range to. If you leave this field blank the range will not be changed."), setRange);
                }
            }
            else
            {
                setSkybox = (Material)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Skybox Material",
                    "This is the material that you want to set the scene's skybox to. If you leave this field blank the skybox will not be changed."), setSkybox, typeof(Material), true);

                changeAmbientLightColour = (ChangeColourType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Change Ambient Light Colour",
                    "Choose to change the colour of the scene's ambient light. Remain the same will not change the colour."), changeAmbientLightColour);

                if (changeAmbientLightColour == ChangeColourType.EditColor)
                {
                    ambientLightColour = UnityEditor.EditorGUILayout.ColorField(new GUIContent("Set Ambient Light Colour",
                        "The colour that the scene's ambient light will be set to."), ambientLightColour);
                }
            }

            duration = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Change Duration",
                    "The duration that the selected change will happen over in seconds. If you leave it as 0 it will perform the changes instantly."), duration);
        }
#endif

        public override void Validation()
        {
            float f;
            if (!float.TryParse(setIntensity, out f) && !string.IsNullOrEmpty(setIntensity))
            {
                ShowWarningMessage("Set intensity must be a valid float value.");
            }
            else
            {
                if (f > 8 || f < 0)
                {
                    ShowWarningMessage("Set intensity must be between 0 and 8.");
                }
            }

            if (!float.TryParse(setBounceIntensity, out f) && !string.IsNullOrEmpty(setBounceIntensity))
            {
                ShowWarningMessage("Set bounce intensity must be a valid float value.");
            }
            else
            {
                if (f > 8 || f < 0)
                {
                    ShowWarningMessage("Set bounce intensity must be between 0 and 8.");
                }
            }

            if (!float.TryParse(setRange, out f) && !string.IsNullOrEmpty(setRange) && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
            {
                ShowWarningMessage("Set range must be a valid float value.");
            }
        }

        public override bool ExecuteAction()
        {
            if (editType == EditType.SingleLightSource)
            {
                // Make sure we have a reference for the target light
                if (targetLight)
                {
                    // If duration isn't 0 then we'll apply the changes over a set duration using a coroutine
                    if (duration != 0f)
                    {
                        activeCoroutines.Add(StartCoroutine(ChangeLightColourOverTime()));
                    }
                    else // Else we'll instantly apply the changes
                    {
                        if (changeColourType == ChangeColourType.EditColor)
                        {
                            targetLight.color = setColour;
                        }

                        if (!string.IsNullOrEmpty(setIntensity))
                        {
                            float f;
                            if (float.TryParse(setIntensity, out f)) // Attempt to parse the string value to a float
                            {
                                targetLight.intensity = f;
                            }
                            else
                            {
                                Debug.Log("Unable to parse the Set Intensity value to a float. Please make sure it's a valid float.");
                            }
                        }

                        if (!string.IsNullOrEmpty(setBounceIntensity))
                        {
                            float f;
                            if (float.TryParse(setBounceIntensity, out f))
                            {
                                targetLight.bounceIntensity = f;
                            }
                            else
                            {
                                Debug.Log("Unable to parse the Set Bounce Intensity value to a float. Please make sure it's a valid float.");
                            }
                        }

                        if (!string.IsNullOrEmpty(setRange) && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
                        {
                            float f;
                            if (float.TryParse(setRange, out f))
                            {
                                targetLight.range = f;
                            }
                            else
                            {
                                Debug.Log("Unable to parse the Set Range value to a float. Please make sure it's a valid float.");
                            }
                        }
                    }
                }
                else
                {
                    Debug.Log("Unable to modify a light because the Target Light reference hasn't been set.");
                }
            }
            else
            {
                if (duration != 0f)
                {
                    activeCoroutines.Add(StartCoroutine(ChangeSceneLightingOverTime()));
                }
                else
                {
                    if (setSkybox != null)
                    {
                        RenderSettings.skybox = setSkybox;
                    }

                    if (changeAmbientLightColour == ChangeColourType.EditColor)
                    {
                        RenderSettings.ambientLight = ambientLightColour;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// This coroutine will apply changes to lighting over time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeLightColourOverTime()
        {
            float smoothness = 0.02f; // Should the user be able to set this?
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            float targetIntensity, targetBounceIntensity, targetRange;
            float.TryParse(setIntensity, out targetIntensity);
            float.TryParse(setBounceIntensity, out targetBounceIntensity);
            float.TryParse(setRange, out targetRange);

            while (progress < 1)
            {
                if (changeColourType == ChangeColourType.EditColor)
                {
                    // Color.Lerp() linearly interpolates between two colours over the selected duration. 
                    targetLight.color = Color.Lerp(targetLight.color, setColour, progress);
                }

                if (!string.IsNullOrEmpty(setIntensity))
                {
                    // Mathf.Lerp() linearly interpolates a float between two values over a selected duration.
                    targetLight.intensity = Mathf.Lerp(targetLight.intensity, targetIntensity, progress);
                }

                if (!string.IsNullOrEmpty(setBounceIntensity))
                {
                    targetLight.bounceIntensity = Mathf.Lerp(targetLight.bounceIntensity, targetBounceIntensity, progress);
                }

                if (!string.IsNullOrEmpty(setRange) && (targetLight.type == LightType.Point || targetLight.type == LightType.Spot))
                {
                    targetLight.range = Mathf.Lerp(targetLight.range, targetRange, progress);
                }

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        /// <summary>
        /// This coroutine will apply changes to lighting settings over time.
        /// </summary>
        /// <returns></returns>
        private IEnumerator ChangeSceneLightingOverTime()
        {
            float smoothness = 0.02f; // Should the user be able to set this?
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / (duration * 10); // The amount of change to apply. Is the multiply by 10 needed?

            while (progress < 1)
            {
                if (setSkybox != null)
                {
                    RenderSettings.skybox.Lerp(RenderSettings.skybox, setSkybox, progress);
                }

                if (changeAmbientLightColour == ChangeColourType.EditColor)
                {
                    RenderSettings.ambientLight = Color.Lerp(RenderSettings.ambientLight, ambientLightColour, progress);
                }

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }
    }
}

