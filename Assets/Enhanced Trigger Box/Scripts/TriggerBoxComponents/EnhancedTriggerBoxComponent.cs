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

    public virtual void OnInspectorGUI()
    {
        hideShowSection = RenderHeader(AddSpacesToSentence(ToString().Replace("(","").Replace(")",""), true), hideShowSection, true);

        EditorGUI.indentLevel = 1;
    }

    public virtual bool ExecuteAction() { return false; }

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