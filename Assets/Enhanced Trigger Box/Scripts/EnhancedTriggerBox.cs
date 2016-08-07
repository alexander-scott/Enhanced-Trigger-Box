using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

[Serializable, ExecuteInEditMode]
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

    public AfterTriggerOptions afterTrigger;

    public TriggerFollow triggerFollow;

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
        PlayerPrefCondition,
    }

    public enum TriggerBoxResponses
    {
        SelectAResponse,
        AnimationResponse,
        AudioResponse,
        CallFunctionResponse,
        DestroyGameobjectResponse,
        DisableGameobjectResponse,
        EnableGameobjectResponse,
        LoadLevelResponse,
        PlayerPrefResponse,
        SpawnGameobjectResponse,
    }

    public enum AfterTriggerOptions
    {
        SetInactive,
        DestroyTriggerBox,
        DestroyParent,
        DoNothing,
    }

    public enum TriggerFollow
    {
        Static,
        FollowMainCamera,
        FollowTransform,
    }

    public List<EnhancedTriggerBoxComponent> listConditions
    {
        get { return conditions; }
    }

    public List<EnhancedTriggerBoxComponent> listResponses
    {
        get { return responses; }
    }

    void FixedUpdate()
    {
        // This if statement updates the trigger boxes position to either stay on a transform or on the main camera
        if (triggerFollow == TriggerFollow.FollowTransform)
        {
            transform.position = followTransform.position;
        }
        else if (triggerFollow == TriggerFollow.FollowMainCamera)
        {
            transform.position = Camera.main.transform.position;
        }

        if (triggered)
        {
            foreach (var c in conditions)
            {
                conditionMet = c.ExecuteAction();

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

    public void OnInspectorGUI()
    {
        // TODO
        //var types = GetClasses(typeof(EnhancedTriggerBoxComponent));

        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

        foreach (var instance in conditions)
        {
            instance.OnInspectorGUI();
        }

        EditorGUI.BeginChangeCheck();

        EditorGUI.indentLevel = 0;

        if (conditions.Count > 0)
            GUILayout.Space(10.0f);

        var conditionEnum = (TriggerBoxConditions)EditorGUILayout.EnumPopup("Add new condition:", triggerBoxConditions);
         
        if (EditorGUI.EndChangeCheck())
        {
            switch (conditionEnum)
            {
                case TriggerBoxConditions.CameraCondition:
                    conditions.Add(ScriptableObject.CreateInstance<CameraCondition>());
                    break;

                case TriggerBoxConditions.PlayerPrefCondition:
                    conditions.Add(ScriptableObject.CreateInstance<PlayerPrefCondition>());
                    break;
            }

            conditionEnum = TriggerBoxConditions.SelectACondition;
        }

        EditorGUI.indentLevel = 0;
        EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);

        if (responses.Count == 0)
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.TextArea("", GUI.skin.horizontalSlider);
        }
        
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
                case TriggerBoxResponses.AnimationResponse:
                    responses.Add(ScriptableObject.CreateInstance<AnimationResponse>());
                    break;

                case TriggerBoxResponses.AudioResponse:
                    responses.Add(ScriptableObject.CreateInstance<AudioResponse>());
                    break;

                case TriggerBoxResponses.CallFunctionResponse:
                    responses.Add(ScriptableObject.CreateInstance<CallFunctionResponse>());
                    break;

                case TriggerBoxResponses.DestroyGameobjectResponse:
                    responses.Add(ScriptableObject.CreateInstance<DestroyGameobjectResponse>());
                    break;

                case TriggerBoxResponses.DisableGameobjectResponse:
                    responses.Add(ScriptableObject.CreateInstance<DisableGameobjectResponse>());
                    break;

                case TriggerBoxResponses.EnableGameobjectResponse:
                    responses.Add(ScriptableObject.CreateInstance<EnableGameobjectResponse>());
                    break;

                case TriggerBoxResponses.LoadLevelResponse:
                    responses.Add(ScriptableObject.CreateInstance<LoadLevelResponse>());
                    break;

                case TriggerBoxResponses.PlayerPrefResponse:
                    responses.Add(ScriptableObject.CreateInstance<PlayerPrefResponse>());
                    break;

                case TriggerBoxResponses.SpawnGameobjectResponse:
                    responses.Add(ScriptableObject.CreateInstance<SpawnGameobjectResponse>());
                    break;
            }

            responseEnum = TriggerBoxResponses.SelectAResponse;
        }
    }

    public static List<Type> GetClasses(Type baseType)
    {
        return Assembly.GetEntryAssembly().GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList();
    }

    private void ConditionsMet()
    {
        foreach (var c in responses)
        {
            c.ExecuteAction();
        }

        // If debugTriggerBox is selected, write to the console saying the trigger box has successfully been triggered
        if (debugTriggerBox)
        {
            Debug.Log(gameObject.name + " has been triggered!");
        }

        switch (afterTrigger)
        {
            case AfterTriggerOptions.SetInactive:
                gameObject.SetActive(false);
                break;

            case AfterTriggerOptions.DestroyTriggerBox:
                Destroy(gameObject);
                break;

            case AfterTriggerOptions.DestroyParent:
                Destroy(transform.parent.gameObject);
                break;
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

    private void OnDrawGizmos()
    {
        Gizmos.color = triggerboxColour;

        if (!drawWire)
        {
            Gizmos.DrawCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                            new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z));
        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                           new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z));
        }
    }
}