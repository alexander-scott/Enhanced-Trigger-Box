using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EnhancedTriggerbox.Component
{
    [Serializable, HideInInspector, AddComponentMenu("")]
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
                DrawInspectorGUI();
            }

            // Check and draw warnings for the fields. This function should be overriden by each inheirited component.
            if (showWarnings)
            {
                Validation();
            }
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
        /// is invalid or missing. Only shown if showWarnings is true. Use ShowWarningMessage("") to display the warnings. 
        /// </summary>
        public virtual void Validation()
        {

        }

        /// <summary>
        /// Draws each generic variable in the component. Not all types can be drawn. Enums are the most prominent that cannot be drawn.
        /// </summary>
        /// <param name="o">The field that will be drawn</param>
        protected void RenderGeneric(System.Reflection.FieldInfo o)
        {
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
        protected void ShowWarningMessage(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
        }
    }
}