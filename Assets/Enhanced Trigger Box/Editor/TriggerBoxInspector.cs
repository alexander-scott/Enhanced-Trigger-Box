using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(TriggerBox))]
public class TriggerBoxInspector : Editor
{
    private string dispName;
    private new string name;
    private SerializedObject so;

    private void OnEnable()
    {
        so = serializedObject;
    }

    public override void OnInspectorGUI()
    {
        so.Update();

        GUI.enabled = true;

        #region Base Options

        so.FindProperty("showBaseOptions").boolValue = RenderHeader("Base Options", so.FindProperty("showBaseOptions").boolValue, true, true);

        if (so.FindProperty("showBaseOptions").boolValue)
        {
            RenderPropertyField(so.FindProperty("triggerTags"));
            RenderPropertyField(so.FindProperty("debugTriggerBox"));
            RenderPropertyField(so.FindProperty("drawWire"));
            RenderPropertyField(so.FindProperty("triggerboxColour"));
            RenderPropertyField(so.FindProperty("afterTrigger"));

            var triggerFollowProp = so.FindProperty("triggerFollow");
            RenderPropertyField(triggerFollowProp);

            if (triggerFollowProp.enumValueIndex == 2)
            {
                RenderPropertyField(so.FindProperty("followTransform"));
                RenderPropertyField(so.FindProperty("followTransformName"));
            }

            RenderPropertyField(so.FindProperty("canWander"));
        }

        #endregion

        RenderDivider();

        #region Camera Conditions

        so.FindProperty("showCameraConditions").boolValue = RenderHeader("Camera Conditions", so.FindProperty("showCameraConditions").boolValue, (so.FindProperty("cameraConditionType").enumValueIndex != 0));

        if (so.FindProperty("showCameraConditions").boolValue)
        {
            var viewConditionTypeProp = so.FindProperty("cameraConditionType");
            RenderPropertyField(viewConditionTypeProp);

            if (viewConditionTypeProp.enumValueIndex != 0)
            {
                RenderPropertyField(so.FindProperty("conditionObject"));
                RenderPropertyField(so.FindProperty("componentParameter"));

                if ((so.FindProperty("cameraConditionType").enumValueIndex == 1))
                    RenderPropertyField(so.FindProperty("ignoreObstacles"));

                RenderPropertyField(so.FindProperty("conditionTime"));
            }
        }

        #endregion

        #region Player Pref Conditions

        so.FindProperty("showPPrefConditions").boolValue = RenderHeader("Player Prefs Conditions", so.FindProperty("showPPrefConditions").boolValue, (so.FindProperty("playerPrefCondition").enumValueIndex != 0), true);

        if (so.FindProperty("showPPrefConditions").boolValue)
        {
            var viewPPrefTypeProp = so.FindProperty("playerPrefCondition");
            RenderPropertyField(viewPPrefTypeProp);

            if (viewPPrefTypeProp.enumValueIndex != 0)
            {
                RenderPropertyField(so.FindProperty("playerPrefVal"));
                RenderPropertyField(so.FindProperty("playerPrefKey"));
                RenderPropertyField(so.FindProperty("playerPrefType"));
            }
        }

        #endregion

        RenderDivider();

        #region Animation Responses

        so.FindProperty("showAnimResponses").boolValue = RenderHeader("Animation Responses", so.FindProperty("showAnimResponses").boolValue, (so.FindProperty("animationTarget").objectReferenceValue != null || so.FindProperty("stopAnim").boolValue));

        if (so.FindProperty("showAnimResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("animationTarget"));
            RenderPropertyField(so.FindProperty("setMecanimTrigger"));
            RenderPropertyField(so.FindProperty("stopAnim"));
            RenderPropertyField(so.FindProperty("playLegacyAnimation"));
        }

        #endregion

        #region Audio Responses

        so.FindProperty("showAudioResponses").boolValue = RenderHeader("Audio Responses", so.FindProperty("showAudioResponses").boolValue, (so.FindProperty("muteAllAudio").boolValue || so.FindProperty("playMusic").objectReferenceValue != null || so.FindProperty("playSoundEffect").objectReferenceValue != null), true);

        if (so.FindProperty("showAudioResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("muteAllAudio"));
            RenderPropertyField(so.FindProperty("playMusic"));
            RenderPropertyField(so.FindProperty("loopMusic"));
            RenderSlider(so.FindProperty("musicVolume"), 0f, 1f);
            RenderPropertyField(so.FindProperty("playSoundEffect"));
        }

        #endregion

        #region Call Function Responses

        so.FindProperty("showCallFResponses").boolValue = RenderHeader("Call Function Responses", so.FindProperty("showCallFResponses").boolValue, (so.FindProperty("messageTarget").objectReferenceValue != null && !string.IsNullOrEmpty(so.FindProperty("messageMethodName").stringValue)), true);

        if (so.FindProperty("showCallFResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("messageTarget"));
            RenderPropertyField(so.FindProperty("messageMethodName"));
            RenderPropertyField(so.FindProperty("parameterType"));
            RenderPropertyField(so.FindProperty("parameterValue"));
        }

        #endregion

        #region Player Pref Responses

        so.FindProperty("showPPrefResponses").boolValue = RenderHeader("Player Pref Responses", so.FindProperty("showPPrefResponses").boolValue, (!string.IsNullOrEmpty(so.FindProperty("setPlayerPrefKey").stringValue) && !string.IsNullOrEmpty(so.FindProperty("setPlayerPrefVal").stringValue)), true);

        if (so.FindProperty("showPPrefResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("setPlayerPrefKey"));
            RenderPropertyField(so.FindProperty("setPlayerPrefType"));
            RenderPropertyField(so.FindProperty("setPlayerPrefVal"));
        }

        #endregion

        #region Spawn Responses

        so.FindProperty("showSpawnResponses").boolValue = RenderHeader("Spawn GameObject Responses", so.FindProperty("showSpawnResponses").boolValue, (so.FindProperty("prefabToSpawn").objectReferenceValue != null), true);

        if (so.FindProperty("showSpawnResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("prefabToSpawn"));
            RenderPropertyField(so.FindProperty("newInstanceName"));
            RenderPropertyField(so.FindProperty("spawnPosition"));
        }

        #endregion

        #region Destroy Responses

        so.FindProperty("showDestroyResponses").boolValue = RenderHeader("Destroy GameObject Responses", so.FindProperty("showDestroyResponses").boolValue, (so.FindProperty("destroyGameobjects").arraySize > 0 || so.FindProperty("destroyObjectNames").arraySize > 0), true);

        if (so.FindProperty("showDestroyResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("destroyGameobjects"));
            RenderPropertyField(so.FindProperty("destroyObjectNames"));
        }

        #endregion

        #region Enable Responses

        so.FindProperty("showEnableResponses").boolValue = RenderHeader("Enable GameObject Responses", so.FindProperty("showEnableResponses").boolValue, (so.FindProperty("enableGameObject").arraySize > 0), true);

        if (so.FindProperty("showEnableResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("enableGameObject"));
        }

        #endregion

        #region Disable Responses

        so.FindProperty("showDisableResponses").boolValue = RenderHeader("Disable GameObject Responses", so.FindProperty("showDisableResponses").boolValue, (so.FindProperty("disableGameObject").arraySize > 0 || so.FindProperty("disableGameObjectName").arraySize > 0), true);

        if (so.FindProperty("showDisableResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("disableGameObject"));
            RenderPropertyField(so.FindProperty("disableGameObjectName"));
        }

        #endregion

        #region Level Responses

        so.FindProperty("showLevelResponses").boolValue = RenderHeader("Load Level Responses", so.FindProperty("showLevelResponses").boolValue, (!string.IsNullOrEmpty(so.FindProperty("loadLevelName").stringValue)), true);

        if (so.FindProperty("showLevelResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("loadLevelName"));
            RenderPropertyField(so.FindProperty("loadDelay"));
        }

        #endregion

        so.ApplyModifiedProperties();
    }

    private void RenderPropertyField(SerializedProperty sp, bool slider = false)
    {
        name = sp.name;
        dispName = sp.displayName;

        EditorGUIUtility.LookLikeControls(180.0f, 50.0f);

        GUILayout.BeginHorizontal();

        EditorGUI.indentLevel = 2;

        if (sp.hasChildren)
        {
            GUILayout.BeginVertical();
            while (true)
            {
                if (sp.propertyPath != name && !sp.propertyPath.StartsWith(name + "."))
                    break;

                EditorGUI.indentLevel = sp.depth;
                bool child = false;

                EditorGUI.indentLevel = 2;

                if (sp.depth == 0)
                {
                    child = EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
                }
                else
                {
                    child = EditorGUILayout.PropertyField(sp);
                }

                if (!sp.NextVisible(child))
                    break;
            }

            GUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
        }

        GUILayout.EndHorizontal();
    }

    private void RenderSlider(SerializedProperty sp, float min, float max)
    {
        name = sp.name;
        dispName = sp.displayName;

        EditorGUIUtility.LookLikeControls(180.0f, 50.0f);

        GUILayout.BeginHorizontal();

        EditorGUI.indentLevel = 2;

        if (sp.hasChildren)
        {
            GUILayout.BeginVertical();
            while (true)
            {
                if (sp.propertyPath != name && !sp.propertyPath.StartsWith(name + "."))
                    break;

                EditorGUI.indentLevel = sp.depth;
                bool child = false;

                EditorGUI.indentLevel = 2;

                EditorGUILayout.Slider(sp, min, max);

                if (!sp.NextVisible(child))
                    break;
            }

            GUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.Slider(sp, min, max);
        }

        GUILayout.EndHorizontal();
    }

    private bool RenderHeader(string s, bool optionRef, bool bold = true, bool topspace = false)
    {
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);

        if (bold)
            myFoldoutStyle.fontStyle = FontStyle.Bold;

        if (topspace)
            GUILayout.Space(10.0f);

        EditorGUI.indentLevel = 1; 
        EditorGUIUtility.LookLikeInspector();

        return EditorGUILayout.Foldout(optionRef, s, myFoldoutStyle);
    }

    private void RenderDivider()
    {
        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
    }

    public void OnSceneGUI()
    {
        var script = (TriggerBox)target;
        if (script.prefabToSpawn)
        {
            Handles.Label(script.spawnPosition, "Spawn " + script.prefabToSpawn.name);

            script.spawnPosition = Handles.PositionHandle(script.spawnPosition, Quaternion.identity);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }
}