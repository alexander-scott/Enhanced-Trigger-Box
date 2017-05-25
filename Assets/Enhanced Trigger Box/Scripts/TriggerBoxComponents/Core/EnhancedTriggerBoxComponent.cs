using System;
using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// All conditions and responses inherit this class allowing them to share the same functionality and work nicely together
    /// </summary>
    [Serializable, HideInInspector, AddComponentMenu("")]
    public abstract class EnhancedTriggerBoxComponent : MonoBehaviour
    {
        /// <summary>
        /// Used in tandem with the GUI Foldout feature to show and hide this component
        /// </summary>
        protected bool hideShowSection = true;

        /// <summary>
        /// If the delete button is selected then this becomes true, letting the main script know to remove and destroy this item from the list
        /// </summary>
        public bool deleted = false;

        /// <summary>
        /// Global setting that affects whether warnings for each component are displayed or not
        /// </summary>
        public bool showWarnings = true;

        /// <summary>
        /// The duration that the selected change will happen over in seconds. If you leave it as 0 it will perform the changes instantly. Currently only used within the lighting response.
        /// </summary>
        public float duration = 0f;

        /// <summary>
        /// If this is true, ExecuteAction(Gameobject) will be called instead of ExecuteAction() which allows components to utilise collision data
        /// </summary>
        public virtual bool requiresCollisionObjectData { get; protected set;}

        /// <summary>
        /// Holds a list containing the active coroutines on this component.
        /// </summary>
        protected List<Coroutine> activeCoroutines = new List<Coroutine>();

        /// <summary>
        /// This function display the custom fields for each component in the inspector
        /// </summary>
        public void OnInspectorGUI()
        {
#if UNITY_EDITOR

            // This try catch is in place to fix an issue which happens when deleting a component. This essentially refreshes the draw sequence.
            try
            {
                GUILayout.BeginHorizontal();
            }
            catch
            {
                return;
            }

            // Draw the foldout header
            hideShowSection = RenderHeader(EnhancedTriggerBox.AddSpacesToSentence(GetType().Name.Replace("(", "").Replace(")", ""), true), hideShowSection, true);

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
                UnityEditor.Undo.RecordObject(this, "Value Change");
                DrawInspectorGUI();
            }

            // Check and draw warnings for the fields. This function should be overriden by each inheirited component.
            if (showWarnings)
            {
                Validation();
            }

#endif
        }

        /// <summary>
        /// This optional function is called when the game first starts and should be used if anything needs to be initalised or retrieved in-game. 
        /// This function should be overriden by each inheirited component.
        /// </summary>
        public virtual void OnAwake()
        {

        }

        /// <summary>
        /// This function draws the specific component fields. If not overridden some of the GUI fields will still be drawn however 
        /// certain types such as enums will not be drawn and the fields will have no tooltips. Ideally you should override this function
        /// in each component.
        /// </summary>
        public virtual void DrawInspectorGUI()
        {
            var fieldValues = GetType()
                         .GetFields()
                         .Select(field => field)
                         .ToList();

            // Draw all the fields
            foreach (var f in fieldValues)
            {
                // The if statement ensures the fields in EnhancedTriggerBoxComponent aren't drawn. Is there a better way to do this?
                if (f.Name != "showWarnings" && f.Name != "deleted" && f.Name != "hideShowSection")
                    RenderGeneric(f);
            }
        }

        public virtual void ResetComponent()
        {

        }

        public void EndComponentCoroutine()
        {
            for (int i = activeCoroutines.Count - 1; i >= 0; i--)
            {
                if (activeCoroutines[i] != null)
                {
                    StopCoroutine(activeCoroutines[i]);
                }

                activeCoroutines.RemoveAt(i);
            }
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
        /// Same as the function above, however this version of it uses a gameobject reference from the colliding object.
        /// NOTE: To enable this function to be called over the above one, requiresCollisionObjectData needs to be set to true within the component.
        /// </summary>
        /// <param name="collidingObject"></param>
        /// <returns></returns>
        public virtual bool ExecuteAction(GameObject collidingObject)
        {
            return false;
        }

        /// <summary>
        /// This optional function is called after the inspector GUI has been drawn and is used to display warnings if any of the inputted data
        /// is invalid or missing. Only shown if showWarnings is true. Use ShowWarningMessage("") to display the warnings. 
        /// </summary>
        public virtual void Validation()
        {

        }

        /// <summary>
        /// Draws each generic variable in the component. Not all types can be drawn. Enums or structs are the most prominent that cannot be drawn.
        /// </summary>
        /// <param name="o">The field that will be drawn</param>
        protected void RenderGeneric(System.Reflection.FieldInfo o)
        {
#if UNITY_EDITOR

            if (o.FieldType == typeof(GameObject))
            {
                o.SetValue(this, (GameObject)EditorGUILayout.ObjectField(new GUIContent(EnhancedTriggerBox.AddSpacesToSentence(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(o.Name), true)
                    , "No description provided"), (GameObject)o.GetValue(this), typeof(GameObject), true));
            }
            else if (o.FieldType == typeof(Camera))
            {
                o.SetValue(this, (Camera)EditorGUILayout.ObjectField(new GUIContent(EnhancedTriggerBox.AddSpacesToSentence(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(o.Name), true)
                    , "No description provided"), (Camera)o.GetValue(this), typeof(Camera), true));
            }
            else if (o.FieldType == typeof(Material))
            {
                o.SetValue(this, (Material)EditorGUILayout.ObjectField(new GUIContent(EnhancedTriggerBox.AddSpacesToSentence(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(o.Name), true)
                    , "No description provided"), (Material)o.GetValue(this), typeof(Material), true));
            }
            else if (o.FieldType == typeof(bool))
            {
                o.SetValue(this, EditorGUILayout.Toggle(new GUIContent(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(EnhancedTriggerBox.AddSpacesToSentence(o.Name, true)),
                        "No description provided"), (bool)o.GetValue(this)));
            }
            else if (o.FieldType == typeof(float))
            {
                o.SetValue(this, EditorGUILayout.FloatField(new GUIContent(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(EnhancedTriggerBox.AddSpacesToSentence(o.Name, true)),
                        "No description provided"), (float)o.GetValue(this)));
            }
            else if (o.FieldType == typeof(int))
            {
                o.SetValue(this, EditorGUILayout.IntField(new GUIContent(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(EnhancedTriggerBox.AddSpacesToSentence(o.Name, true)),
                        "No description provided"), (int)o.GetValue(this)));
            }
            else if (o.FieldType == typeof(string))
            {
                o.SetValue(this, EditorGUILayout.TextField(new GUIContent(System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(EnhancedTriggerBox.AddSpacesToSentence(o.Name, true)),
                        "No description provided"), (string)o.GetValue(this)));
            }
            // TODO: Add more generic types

#endif
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
#if UNITY_EDITOR
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);

            if (bold)
                myFoldoutStyle.fontStyle = FontStyle.Bold;

            if (topspace)
                GUILayout.Space(10.0f);

            EditorGUI.indentLevel = 0;

            return EditorGUILayout.Foldout(optionRef, new GUIContent(s, ""), myFoldoutStyle);
#else
            return true;
#endif
        }

        /// <summary>
        /// Draws a horizontal divider
        /// </summary>
        protected void RenderDivider()
        {
#if UNITY_EDITOR
            EditorGUI.indentLevel = 0;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
#endif
        }

        /// <summary>
        /// Draws the warning message
        /// </summary>
        /// <param name="message">The message to display in the warning</param>
        protected void ShowWarningMessage(string message)
        {
#if UNITY_EDITOR
            EditorGUILayout.HelpBox(message, MessageType.Warning);
#endif
        }
    }
}