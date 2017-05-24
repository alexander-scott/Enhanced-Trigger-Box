using UnityEngine;
using UnityEditor;

namespace EnhancedTriggerbox
{
    [CustomEditor(typeof(EnhancedTriggerBox))]
    public class EnhancedTriggerBoxInspector : Editor
    {
        string dispName;
        string baseName;

        EnhancedTriggerBox theObject;
        SerializedObject so;

        void OnEnable()
        {
            Init();
        }

        void Init()
        {
            theObject = (EnhancedTriggerBox)target;
            so = serializedObject;
        }

        public override void OnInspectorGUI()
        {
            if (theObject == null)
                Init();

            so.Update();

            EditorGUI.BeginChangeCheck();

            so.FindProperty("showBaseOptions").boolValue = RenderHeader("Base Options", so.FindProperty("showBaseOptions").boolValue, true, true);

            if (so.FindProperty("showBaseOptions").boolValue)
            {
                RenderPropertyField(so.FindProperty("debugTriggerBox"));
                RenderPropertyField(so.FindProperty("hideWarnings"));

                if (so.FindProperty("afterTrigger").enumValueIndex != 4)
                {
                    RenderPropertyField(so.FindProperty("disableEntryCheck"));
                    if (!so.FindProperty("disableEntryCheck").boolValue)
                    {
                        RenderPropertyField(so.FindProperty("canWander"));
                    }   
                }

                if (!so.FindProperty("disableEntryCheck").boolValue || so.FindProperty("afterTrigger").enumValueIndex == 4)
                {
                    RenderPropertyField(so.FindProperty("triggerTags"));

                    RenderPropertyField(so.FindProperty("triggerboxColour"));

                    RenderPropertyField(so.FindProperty("triggerFollow"));

                    if (so.FindProperty("triggerFollow").enumValueIndex == 2)
                    {
                        RenderPropertyField(so.FindProperty("followTransform"));
                        RenderPropertyField(so.FindProperty("followTransformName"));
                    }
                }
                
                RenderPropertyField(so.FindProperty("afterTrigger"));

                RenderPropertyField(so.FindProperty("conditionTime"));
            }

            if (!so.FindProperty("hideWarnings").boolValue)
            {
                if (!so.FindProperty("disableEntryCheck").boolValue)
                {
                    // If follow transform is enabled, check if the user specified a transform
                    if (so.FindProperty("triggerFollow").enumValueIndex == 2)
                    {
                        if (so.FindProperty("followTransform").objectReferenceValue == null && string.IsNullOrEmpty(so.FindProperty("followTransformName").stringValue))
                        {
                            EditorGUILayout.HelpBox("You have selected Follow Transform but you have not specified either a transform reference or a gameobject name!", MessageType.Warning);
                        }
                    }
                }

                // Check that this gameobject has a box collider which is set to trigger
                if (!theObject.gameObject.GetComponent<BoxCollider>().isTrigger)
                {
                    EditorGUILayout.HelpBox("You should check the Is Trigger checkbox on this object's box collider as this will allow gameobjects to enter it.", MessageType.Warning);
                }

                if (!theObject.gameObject.GetComponent<BoxCollider>().enabled)
                {
                    EditorGUILayout.HelpBox("You should enable this object's box collider as this will allow gameobjects to enter it.", MessageType.Warning);
                }
            }

            theObject.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(theObject);

                for (int i = 0; i < theObject.Conditions.Count; i++)
                {
                    if (theObject.Conditions[i])
                    {
                        EditorUtility.SetDirty(theObject.Conditions[i]);
                    }
                }

                for (int i = 0; i < theObject.Responses.Count; i++)
                {
                    if (theObject.Responses[i])
                    {
                        EditorUtility.SetDirty(theObject.Responses[i]);
                    }
                }

                for (int i = 0; i < theObject.ExitResponses.Count; i++)
                {
                    if (theObject.ExitResponses[i])
                    {
                        EditorUtility.SetDirty(theObject.ExitResponses[i]);
                    }
                }
            }

            so.ApplyModifiedProperties();
        }

        private bool RenderHeader(string s, bool optionRef, bool bold = true, bool topspace = false)
        {
            GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);

            if (bold)
                myFoldoutStyle.fontStyle = FontStyle.Bold;

            if (topspace)
                GUILayout.Space(10.0f);

            EditorGUI.indentLevel = 0;

            return EditorGUILayout.Foldout(optionRef, s, myFoldoutStyle);
        }

        private void RenderPropertyField(SerializedProperty sp, bool slider = false)
        {
            baseName = sp.name;
            dispName = sp.displayName;

            GUILayout.BeginHorizontal();

            EditorGUI.indentLevel = 1;

            if (sp.hasChildren)
            {
                GUILayout.BeginVertical();
                while (true)
                {
                    if (sp.propertyPath != baseName && !sp.propertyPath.StartsWith(baseName + "."))
                        break;

                    EditorGUI.indentLevel = sp.depth;
                    bool child = false;

                    EditorGUI.indentLevel = 1;

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
    }
}