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
                RenderPropertyField(so.FindProperty("disableEntryCheck"));

                if (!so.FindProperty("disableEntryCheck").boolValue)
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
                RenderPropertyField(so.FindProperty("canWander"));
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

                    // Check that this gameobject has a box collider
                    if (theObject.gameObject.GetComponent<BoxCollider>() == null)
                    {
                        EditorGUILayout.HelpBox("You need to add a box collider to this gameobject. Or alternatively you can check Disable Entry Check to remove the need for one.", MessageType.Warning);
                    }
                }
            }

            theObject.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(theObject);

                for (int i = 0; i < theObject.listConditions.Count; i++)
                {
                    if (theObject.listConditions[i])
                    {
                        EditorUtility.SetDirty(theObject.listConditions[i]);
                    }
                }

                for (int i = 0; i < theObject.listResponses.Count; i++)
                {
                    if (theObject.listResponses[i])
                    {
                        EditorUtility.SetDirty(theObject.listResponses[i]);
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