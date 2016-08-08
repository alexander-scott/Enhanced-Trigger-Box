using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

[Serializable]
public class EnhancedTriggerBoxComponent : ScriptableObject
{
    [SerializeField]
    protected bool hideShowSection = true;

    [SerializeField]
    public bool deleted = false;

    [SerializeField]
    public bool showWarnings = true;

    public void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        // Draw the foldout header
        hideShowSection = RenderHeader(AddSpacesToSentence(ToString().Replace("(","").Replace(")",""), true), hideShowSection, true);

        // Draw the delete button
        if (GUILayout.Button(new GUIContent("X","Remove component"), GUILayout.Width(25)))
        {
            deleted = true;
        }

        GUILayout.EndHorizontal();

        EditorGUI.indentLevel = 1;

        if (hideShowSection)
        {
            DrawInspectorGUI();
        }

        if (showWarnings)
        {
            Validation();
        }
    }

    public virtual void DrawInspectorGUI()
    {

    }

    public virtual bool ExecuteAction()
    {
        return false;
    }

    public virtual void Validation()
    {

    }

    protected bool RenderHeader(string s, bool optionRef, bool bold = true, bool topspace = false)
    {
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);

        if (bold)
            myFoldoutStyle.fontStyle = FontStyle.Bold;

        if (topspace)
            GUILayout.Space(10.0f);

        EditorGUI.indentLevel = 0;
        EditorGUIUtility.LookLikeInspector();

        return EditorGUILayout.Foldout(optionRef, s, myFoldoutStyle);
    }

    protected void RenderDivider()
    {
        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
    }

    protected void ShowErrorMessage(string message)
    {
        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }

    private string AddSpacesToSentence(string text, bool preserveAcronyms)
    {
        if (string.IsNullOrEmpty(text))
            return string.Empty;
        StringBuilder newText = new StringBuilder(text.Length * 2);
        newText.Append(text[0]);
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]))
                if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                    (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                     i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                    newText.Append(' ');
            newText.Append(text[i]);
        }
        return newText.ToString();
    }
}