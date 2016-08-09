using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text;

[Serializable, HideInInspector]
public class EnhancedTriggerBoxComponent : MonoBehaviour
{
    /// <summary>
    /// Used in tandem with the GUI Foldout feature to show and hide this component
    /// </summary>
    [SerializeField]
    protected bool hideShowSection = true;

    /// <summary>
    /// If the delete button is selected then this becomes true, letting the main script know to remove and destroy this item from the list
    /// </summary>
    [SerializeField]
    public bool deleted = false;

    /// <summary>
    /// Global setting that affects whether warnings for each component are displayed or not
    /// </summary>
    [SerializeField]
    public bool showWarnings = true;

    /// <summary>
    /// This function display the custom fields for each component in the inspector
    /// </summary>
    public void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();

        // Draw the foldout header
        hideShowSection = RenderHeader(AddSpacesToSentence(GetType().Name.Replace("(", "").Replace(")", ""), true), hideShowSection, true);

        hideFlags = HideFlags.HideInInspector;

        // Draw the delete button
        if (GUILayout.Button(new GUIContent("X", "Remove component"), GUILayout.Width(25)))
        {
            deleted = true;
        }

        GUILayout.EndHorizontal();

        EditorGUI.indentLevel = 1;

        // Draw the specific components fields (only if the section is folded out). This function should be overriden by each inheirited component.
        if (hideShowSection)
        {
            DrawInspectorGUI();
        }

        // Check and draw warnings for the fields. This function should be overriden by each inheirited component.
        if (showWarnings)
        {
            Validation();
        }
    }

    /// <summary>
    /// This optional function is called when the game first starts and should be used if anything needs to be initalised or retrieved in-game. This function should be overriden by each inheirited component.
    /// </summary>
    public virtual void OnAwake()
    {

    }

    /// <summary>
    /// This function draws the specific component fields. This function should be overriden by each inheirited component.
    /// </summary>
    public virtual void DrawInspectorGUI()
    {

    }

    /// <summary>
    /// If this component is a condition this function is called when the trigger box is triggered (player enters it) and must returns true or
    /// false depending on if the condition has been met. If this component is a response then this function is called when all conditions
    /// have been met can and returns true or false depending on if the response has executed correctly. This function should be overriden by 
    /// each inheirited component.
    /// </summary>
    /// <returns></returns>
    public virtual bool ExecuteAction()
    {
        return false;
    }

    /// <summary>
    /// This optional function is called after the inspector GUI has been drawn and is used to display warnings if any of the inputted data
    /// is invalid or missing. Only shown if showWarnings is true. Use ShowErrorMessage("") to display the warnings. 
    /// </summary>
    public virtual void Validation()
    {

    }

    /// <summary>
    /// Draws the component header
    /// </summary>
    /// <param name="s">The name of the header</param>
    /// <param name="optionRef">The bool that stores if the foldout section is open or closed</param>
    /// <param name="bold">Should the header text be in bold</param>
    /// <param name="topspace">Should there be a space above the header</param>
    /// <returns></returns>
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

    /// <summary>
    /// Draws a horizontal divider
    /// </summary>
    protected void RenderDivider()
    {
        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
    }

    /// <summary>
    /// Draws the warning message
    /// </summary>
    /// <param name="message">The message to display in the warning</param>
    protected void ShowErrorMessage(string message)
    {
        EditorGUILayout.HelpBox(message, MessageType.Warning);
    }

    /// <summary>
    /// Adds spaces before captial letters in a string
    /// </summary>
    /// <param name="text">The string to manipulate</param>
    /// <param name="preserveAcronyms">Should acronyms be unaffected?</param>
    /// <returns></returns>
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