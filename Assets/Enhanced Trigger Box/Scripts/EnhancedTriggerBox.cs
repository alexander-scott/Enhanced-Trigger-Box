using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class EnhancedTriggerBox : MonoBehaviour
{
    [SerializeField]
    private List<EnhancedTriggerBoxComponent> conditions = new List<EnhancedTriggerBoxComponent>();

    [SerializeField]
    private List<EnhancedTriggerBoxComponent> responses = new List<EnhancedTriggerBoxComponent>();

    public TriggerBoxConditions triggerBoxConditions;

    public TriggerBoxResponses triggerBoxResponses;

    #region Base Options

    public bool showBaseOptions = true;

    public List<string> triggerTags;

    public bool debugTriggerBox;

    public bool disableStartupChecks;

    public bool drawWire;

    public Color triggerboxColour;

    //public AfterTriggerOptions afterTrigger;

    //public TriggerFollow triggerFollow;

    public Transform followTransform;

    public string followTransformName;

    public bool canWander;

    private bool triggered = false;

    private bool conditionMet = false;

    #endregion

    public enum TriggerBoxConditions
    {
        SelectACondition,
        CameraCondition,
    }

    public enum TriggerBoxResponses
    {
        SelectAResponse,
    }

    public List<EnhancedTriggerBoxComponent> listConditions
    {
        get { return conditions; }
    }

    public List<EnhancedTriggerBoxComponent> listResponses
    {
        get { return responses; }
    }

    public void OnEnable()
    {

    }

    public void OnInspectorGUI()
    {
        foreach (var instance in conditions)
        {
            instance.OnInspectorGUI();
        }

        EditorGUI.BeginChangeCheck();

        if (conditions.Count > 0)
            GUILayout.Space(10.0f);

        EditorGUI.indentLevel = 0;
        var conditionEnum = (TriggerBoxConditions)EditorGUILayout.EnumPopup("Add new condition:", triggerBoxConditions);
         
        if (EditorGUI.EndChangeCheck())
        {
            switch (conditionEnum)
            {
                case TriggerBoxConditions.CameraCondition:
                    CameraCondition newObject = ScriptableObject.CreateInstance<CameraCondition>();
                    conditions.Add(newObject);
                    conditionEnum = TriggerBoxConditions.SelectACondition;
                    break;
            }
        }

        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

        foreach (var instance in responses)
        {
            instance.OnInspectorGUI();
        }

        EditorGUI.BeginChangeCheck();

        if (responses.Count > 0)
            GUILayout.Space(10.0f);

        EditorGUI.indentLevel = 0;

        var responseEnum = (TriggerBoxResponses)EditorGUILayout.EnumPopup("Add new response:", triggerBoxResponses);

        if (EditorGUI.EndChangeCheck())
        {
            switch (responseEnum)
            {
                
            }
        }
    }

    void FixedUpdate()
    {
        if (triggered)
        {
            bool conditionsMet = false;

            foreach (var c in conditions)
            {
                conditionsMet = c.ExecuteAction();

                if (!conditionMet)
                {
                    break;
                }
            }

            if (conditionMet)
            {
                ConditionsMet();
            }
        }
    }

    private void ConditionsMet()
    {
        foreach (var c in responses)
        {
            c.ExecuteAction();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Count >= 0 && (triggerTags.Contains(other.gameObject.tag)))
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!canWander)
        {
            if (triggerTags.Count >= 0 && (triggerTags.Contains(other.gameObject.tag)))
            {
                triggered = false;
            }
        }
    }
}