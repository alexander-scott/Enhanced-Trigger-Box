using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class EnhancedTriggerBoxComponent : ScriptableObject
{
    public ComponentType componentType;

    public enum ComponentType
    {
        Condition,
        Response,
    }

    public virtual void OnInspectorGUI()
    {
        RenderDivider();
    }

    public virtual bool ExecuteAction() { return false; }

    protected bool RenderHeader(string s, bool optionRef, bool bold = true, bool topspace = false)
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

    protected void RenderDivider()
    {
        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
    }
}