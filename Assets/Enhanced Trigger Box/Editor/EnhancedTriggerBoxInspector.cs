using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(EnhancedTriggerBox))]
public class EnhancedTriggerBoxInspector : Editor
{
    string dispName;
    string name;

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

        EditorGUILayout.Space();

        so.FindProperty("showBaseOptions").boolValue = RenderHeader("Base Options", so.FindProperty("showBaseOptions").boolValue, true, true);

        if (so.FindProperty("showBaseOptions").boolValue)
        {
            RenderPropertyField(so.FindProperty("triggerTags"));
            RenderPropertyField(so.FindProperty("debugTriggerBox"));
            RenderPropertyField(so.FindProperty("disableStartupChecks"));
            RenderPropertyField(so.FindProperty("drawWire"));
            RenderPropertyField(so.FindProperty("triggerboxColour"));
            //RenderPropertyField(so.FindProperty("afterTrigger"));

            //var triggerFollowProp = so.FindProperty("triggerFollow");
            //RenderPropertyField(triggerFollowProp);

            //if (triggerFollowProp.enumValueIndex == 2)
            //{
            //    RenderPropertyField(so.FindProperty("followTransform"));
            //    RenderPropertyField(so.FindProperty("followTransformName"));
            //}

            RenderPropertyField(so.FindProperty("canWander"));
        }

        theObject.OnInspectorGUI();

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(theObject);

            foreach (EnhancedTriggerBoxComponent o in theObject.listConditions)
            {
                EditorUtility.SetDirty(o);
            }

            foreach (EnhancedTriggerBoxComponent o in theObject.listResponses)
            {
                EditorUtility.SetDirty(o);
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

        EditorGUI.indentLevel = 1;
        EditorGUIUtility.LookLikeInspector();

        return EditorGUILayout.Foldout(optionRef, s, myFoldoutStyle);
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
}