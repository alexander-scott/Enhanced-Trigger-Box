using UnityEngine;
using UnityEditor;
using System.Collections;

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
            RenderPropertyField(so.FindProperty("triggerTags"));
            RenderPropertyField(so.FindProperty("debugTriggerBox"));
            RenderPropertyField(so.FindProperty("hideWarnings"));
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

        if (!so.FindProperty("hideWarnings").boolValue)
        {
            // If follow transform is enabled, check if the user specified a transform
            if (so.FindProperty("triggerFollow").enumValueIndex == 2)
            {
                if (so.FindProperty("followTransform").objectReferenceValue == null && string.IsNullOrEmpty(so.FindProperty("followTransformName").stringValue))
                {
                    EditorGUILayout.HelpBox("You have selected Follow Transform but you have not specified either a transform reference or a gameobject name!", MessageType.Warning);
                }
                else
                {
                    // If the user has entered both a gameobject name and transform reference
                    if (so.FindProperty("followTransform").objectReferenceValue != null && !string.IsNullOrEmpty(so.FindProperty("followTransformName").stringValue))
                    {
                        EditorGUILayout.HelpBox("You have selected Follow Transform and have entered both a transform reference and a gameobject name! The transform reference will be ignored and the gameobject name will take preference and you should remove one of them.", MessageType.Warning);
                    }
                }
            }
        }

        theObject.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(theObject);

            foreach (EnhancedTriggerBoxComponent o in theObject.listConditions)
            {
                if (o)
                {
                    EditorUtility.SetDirty(o);
                }
            }

            foreach (EnhancedTriggerBoxComponent o in theObject.listResponses)
            {
                if (o)
                {
                    EditorUtility.SetDirty(o);
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