using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof(TriggerBox))]
public class TriggerBoxInspector : Editor
{
    private string dispName;
    private new string name;
    private SerializedProperty[] properties;
    private SerializedObject so;
    private List<InspectorPlusVar> vars;

    private void RefreshVars()
    {
        for (int i = 0; i < vars.Count; i += 1)
        {
            properties[i] = so.FindProperty(vars[i].name);
        }
    }

    private void OnEnable()
    {
        vars = new List<InspectorPlusVar>();
        so = serializedObject;

        #region Base Options

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                                      null, null, InspectorPlusVar.VectorDrawType.None,
                                      false, false, 47, new[] { true, false, false, false },
                                      new[] { "Base Options:", "", "", "" }, new[] { true, false, false, false },
                                      new[] { false, false, false, false }, new[] { 0, 0, 0, 0 },
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                                      new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                                      new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean", "drawWire",
                              "Draw Wirebox", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", true, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean", "debugTriggerBox",
                              "Debug Trigger Box", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "List<String>",
                            "triggerTags", "Trigger Tags", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "DestroyTriggerBox", "destroyOnTrigger",
                              "On Trigger Destroy", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Color",
                              "triggerboxColour", "Triggerbox Colour", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "TriggerFollow", "triggerFollow",
                              "Follow Options", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Transform", "followTransform",
                              "Follow Transform", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String", "followTransformName",
                              "Follow Transform Name", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Camera Conditions

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 47, new[] { true, false, false, false },
                            new[] { "Camera Conditions:", "", "", "" }, new[] { true, false, false, false },
                            new[] { false, false, false, false }, new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "LookType",
                            "viewConditionType", "Camera Condition", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                            "viewObject", "GameObject Target", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "LookObjectCondition",
                            "lookObjectCondition", "GameObject Type", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean",
                            "ignoreObstacles", "Ignore Obstacles", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean",
                            "canWander", "Can Wander", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Float",
                            "conditionTime", "Condition Timer", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Player Prefs Conditions

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 47, new[] { true, false, false, false },
                            new[] { "Player Prefs Conditions:", "", "", "" }, new[] { true, false, false, false },
                            new[] { false, false, false, false }, new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "PlayerPrefCondition",
                            "playerPrefCondition", "Player Pref Condition", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "playerPrefVal", "Player Pref Value", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "playerPrefKey", "Player Pref Key", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "ParameterType",
                            "playerPrefType", "Player Pref Type", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Animations

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 47, new[] { true, false, false, false },
                            new[] { "Animation Responses:", "", "", "" }, new[] { true, false, false, false },
                            new[] { false, false, false, false }, new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                            "animationTarget", "Target Gameobject", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "setMecanimTrigger", "Set Mecanim Trigger", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean",
                            "stopAnim", "Stop playing animation", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                        {
                                            false, false, false, false, false, false, false, false, false, false, false,
                                            false, false, false, false, false
                                        },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "AnimationClip",
                            "playAnimation", "Play Legacy Animation", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Audio

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 45, new[] { true, false, false, false }, new[] { "Audio Responses:", "", "", "" },
                            new[] { true, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Bool", "mute",
                            "Stop Music", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "AudioClip", "playAudio",
                            "Play Music", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                    {
                                        false, false, false, false, false, false, false, false, false, false, false,
                                        false, false, false, false, false
                                    },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean", "loop",
                            "Loop Music", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                    {
                                        false, false, false, false, false, false, false, false, false, false, false,
                                        false, false, false, false, false
                                    },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.Range, 0, 1, false, 0, 0, true, "Single", "musicVolume",
                             "Music Volume", InspectorPlusVar.VectorDrawType.None,
                             false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                             new[] { false, false, false, false }, new[] { false, false, false, false },
                             new[] { 0, 0, 0, 0 },
                             new[]
                                              {
                                                  false, false, false, false, false, false, false, false, false, false, false,
                                                  false, false, false, false, false
                                              },
                             new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                             new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                             new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                             false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "AudioClip", "soundEffect",
                             "Play Sound Effect", InspectorPlusVar.VectorDrawType.None,
                             false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                             new[] { false, false, false, false }, new[] { false, false, false, false },
                             new[] { 0, 0, 0, 0 },
                             new[]
                                              {
                                                  false, false, false, false, false, false, false, false, false, false, false,
                                                  false, false, false, false, false
                                              },
                             new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                             new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                             new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                             false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Call Function

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null, null,
                            null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 44, new[] { true, false, false, false }, new[] { "Call Function Responses:", "", "", "" },
                            new[] { true, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                            "messageTarget", "Send Message To", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "messageMethodName", "Send Message Method Name", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                    {
                                        false, false, false, false, false, false, false, false, false, false, false,
                                        false, false, false, false, false
                                    },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "ParameterType",
                            "parameterType", "Parameter Type", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String", "parameterValue",
                            "Parameter Value", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Player Prefs

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null, null,
                            null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 44, new[] { true, false, false, false }, new[] { "Player Prefs Responses:", "", "", "" },
                            new[] { true, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "setPlayerPrefKey", "Player Pref Key", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "ParameterType",
                            "setPlayerPrefType", "Player Pref Type", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "setPlayerPrefVal", "Player Pref Value", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Spawn Gameobject

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null, null,
                            null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 43, new[] { true, false, false, false }, new[] { "Spawn GameObject Responses:", "", "", "" },
                            new[] { true, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                            "spawnGameobject", "Prefab to Spawn", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                            "spawnedObjectName", "Object Name", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Vector3",
                            "spawnPosition", "Spawn Position", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        #endregion

        #region Destroy Gameobject

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 43,
                            new[] { true, false, false, false }, new[] { "Destroy GameObject Responses:", "", "", "" },
                            new[] { true, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "List<GameObject>",
                            "destroyGameobjects", "GameObject Target", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "List<String>",
                            "destroyObjectNames", "GameObject Name", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Enable Gameobject

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 37, new[] { true, false, false, false },
                            new[] { "Enable GameObject Responses:", "", "", "" }, new[] { true, false, false, false },
                            new[] { false, false, false, false }, new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "List<GameObject>",
                            "enableGameObject", "GameObject Target", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Disable Gameobject

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null,
                            null, null, InspectorPlusVar.VectorDrawType.None,
                            false, false, 37, new[] { true, false, false, false },
                            new[] { "Disable GameObject Responses:", "", "", "" }, new[] { true, false, false, false },
                            new[] { false, false, false, false }, new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "List<GameObject>",
                            "disableGameObject", "GameObject Target", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "List<String>",
                            "disableGameObjectName", "GameObject Name", InspectorPlusVar.VectorDrawType.None,
                            false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                            new[] { false, false, false, false }, new[] { false, false, false, false },
                            new[] { 0, 0, 0, 0 },
                            new[]
                                {
                                    false, false, false, false, false, false, false, false, false, false, false,
                                    false, false, false, false, false
                                },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                            new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                            false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        #region Load Level

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, null, null,
                              null, InspectorPlusVar.VectorDrawType.None,
                              false, false, 44, new[] { true, false, false, false }, new[] { "Load Level Responses:", "", "", "" },
                              new[] { true, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                              "loadLevelName", "Load Level Name", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Single",
                              "loadDelay", "Load Delay", InspectorPlusVar.VectorDrawType.None,
                              false, false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
                              new[] { false, false, false, false }, new[] { false, false, false, false },
                              new[] { 0, 0, 0, 0 },
                              new[]
                                                  {
                                                      false, false, false, false, false, false, false, false, false, false, false,
                                                      false, false, false, false, false
                                                  },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                              new[] { false, false, false, false }, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                              false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        #endregion

        properties = new SerializedProperty[vars.Count];
    }

    private void ProgressBar(float value, string label)
    {
        GUILayout.Space(3.0f);
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        GUILayout.Space(3.0f);
    }

    private void FloatField(SerializedProperty sp, InspectorPlusVar v)
    {
        if (v.limitType == InspectorPlusVar.LimitType.Min && !sp.hasMultipleDifferentValues)
            sp.floatValue = Mathf.Max(v.min, sp.floatValue);
        else if (v.limitType == InspectorPlusVar.LimitType.Max && !sp.hasMultipleDifferentValues)
            sp.floatValue = Mathf.Min(v.max, sp.floatValue);

        if (v.limitType == InspectorPlusVar.LimitType.Range)
        {
            if (!v.progressBar)
                EditorGUILayout.Slider(sp, v.min, v.max);
            else
            {
                if (!sp.hasMultipleDifferentValues)
                {
                    sp.floatValue = Mathf.Clamp(sp.floatValue, v.min, v.max);
                    ProgressBar((sp.floatValue - v.min) / v.max, dispName);
                }
                else
                    ProgressBar((sp.floatValue - v.min) / v.max, dispName);
            }
        }
        else EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
    }

    private void IntField(SerializedProperty sp, InspectorPlusVar v)
    {
        if (v.limitType == InspectorPlusVar.LimitType.Min && !sp.hasMultipleDifferentValues)
            sp.intValue = Mathf.Max(v.iMin, sp.intValue);
        else if (v.limitType == InspectorPlusVar.LimitType.Max && !sp.hasMultipleDifferentValues)
            sp.intValue = Mathf.Min(v.iMax, sp.intValue);

        if (v.limitType == InspectorPlusVar.LimitType.Range)
        {
            if (!v.progressBar)
            {
                EditorGUI.BeginProperty(new Rect(0.0f, 0.0f, 0.0f, 0.0f), new GUIContent(), sp);
                EditorGUI.BeginChangeCheck();

                int newValue = EditorGUI.IntSlider(GUILayoutUtility.GetRect(18.0f, 18.0f), new GUIContent(dispName),
                                                   sp.intValue, v.iMin, v.iMax);

                if (EditorGUI.EndChangeCheck())
                    sp.intValue = newValue;
                EditorGUI.EndProperty();
            }
            else
            {
                if (!sp.hasMultipleDifferentValues)
                {
                    sp.intValue = Mathf.Clamp(sp.intValue, v.iMin, v.iMax);
                    ProgressBar((float)(sp.intValue - v.iMin) / v.iMax, dispName);
                }
                else
                    ProgressBar((float)(sp.intValue - v.iMin) / v.iMax, dispName);
            }
        }
        else EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
    }

    private int BoolField(SerializedProperty sp, InspectorPlusVar v)
    {
        if (v.toggleStart)
        {
            EditorGUI.BeginProperty(new Rect(0.0f, 0.0f, 0.0f, 0.0f), new GUIContent(), sp);

            EditorGUI.BeginChangeCheck();
            bool newValue = EditorGUILayout.Toggle(dispName, sp.boolValue);

            if (EditorGUI.EndChangeCheck())
                sp.boolValue = newValue;

            EditorGUI.EndProperty();

            if (!sp.boolValue)
                return v.toggleSize;
        }
        else EditorGUILayout.PropertyField(sp, new GUIContent(dispName));

        return 0;
    }

    private int PropertyField(SerializedProperty sp, string name)
    {
        if (sp.hasChildren)
        {
            GUILayout.BeginVertical();
            while (true)
            {
                if (sp.propertyPath != name && !sp.propertyPath.StartsWith(name + "."))
                    break;

                EditorGUI.indentLevel = sp.depth;
                bool child = false;

                if (sp.depth == 0)
                    child = EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
                else
                    child = EditorGUILayout.PropertyField(sp);

                if (!sp.NextVisible(child))
                    break;
            }
            EditorGUI.indentLevel = 0;
            GUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
        }

        if (name == "viewConditionType")
        {
            if (sp.enumValueIndex == 0)
            {
                // Hides the 5 beneath it if it is set to none. TODO: More robust
                return 5;
            }
        }

        if (name == "playerPrefCondition")
        {
            if (sp.enumValueIndex == 0)
            {
                // Hides the 3 beneath it if it is set to none. TODO: More robust
                return 3;
            }
        }

        if (name == "triggerFollow")
        {
            if (sp.enumValueIndex != 2)
            {
                // Hides the 2 beneath it if it is set to none. TODO: More robust
                return 2;
            }
        }

        return 0;
    }

    public void OnInspectorGUI1()
    {
        so.Update();
        RefreshVars();

        EditorGUIUtility.LookLikeControls(135.0f, 50.0f);

        for (int i = 0; i < properties.Length; i += 1)
        {
            InspectorPlusVar v = vars[i];

            if (v.active && properties[i] != null)
            {
                SerializedProperty sp = properties[i];
                string s = v.type;
                bool skip = false;
                name = v.name;
                dispName = v.dispName;

                GUI.enabled = v.canWrite;

                GUILayout.BeginHorizontal();

                if (v.toggleLevel != 0)
                    GUILayout.Space(v.toggleLevel * 10.0f);

                if (s == typeof(float).Name)
                {
                    FloatField(sp, v);
                    skip = true;
                }
                if (s == typeof(int).Name)
                {
                    IntField(sp, v);
                    skip = true;
                }
                if (s == typeof(bool).Name)
                {
                    i += BoolField(sp, v);
                    skip = true;
                }
                if (!skip)
                {
                    i += PropertyField(sp, name);
                }

                GUILayout.EndHorizontal();
                GUI.enabled = true;
            }
            if (v.space == 0.0f)
                continue;
            float usedSpace = 0.0f;
            for (int j = 0; j < v.numSpace; j += 1)
            {
                if (v.labelEnabled[j] || v.buttonEnabled[j])
                    usedSpace += 18.0f;
            }
            if (v.space == 0.0f)
                continue;
            float space = Mathf.Max(0.0f, (v.space - usedSpace) / 2.0f);
            GUILayout.Space(space);
            for (int j = 0; j < v.numSpace; j += 1)
            {
                bool buttonLine = false;
                for (int k = 0; k < 4; k += 1) if (v.buttonEnabled[j * 4 + k]) buttonLine = true;
                if (!v.labelEnabled[j] && !buttonLine)
                    continue;

                GUILayout.BeginHorizontal();
                if (v.labelEnabled[j])
                {
                    var boldItalic = new GUIStyle();
                    boldItalic.margin = new RectOffset(5, 5, 5, 5);

                    if (v.labelAlign[j] == 0)
                        boldItalic.alignment = TextAnchor.MiddleLeft;
                    else if (v.labelAlign[j] == 1)
                        boldItalic.alignment = TextAnchor.MiddleCenter;
                    else if (v.labelAlign[j] == 2)
                        boldItalic.alignment = TextAnchor.MiddleRight;

                    if (v.labelBold[j] && v.labelItalic[j])
                        boldItalic.fontStyle = FontStyle.BoldAndItalic;
                    else if (v.labelBold[j])
                        boldItalic.fontStyle = FontStyle.Bold;
                    else if (v.labelItalic[j])
                        boldItalic.fontStyle = FontStyle.Italic;

                    GUILayout.Label(v.label[j], boldItalic);
                    boldItalic.alignment = TextAnchor.MiddleLeft;
                }

                bool alignRight = (v.labelEnabled[j] && buttonLine);

                if (!alignRight)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }

                GUILayout.FlexibleSpace();
                for (int k = 0; k < 4; k += 1)
                {
                    if (v.buttonEnabled[j * 4 + k])
                    {
                        if (!v.buttonCondense[j] && !alignRight)
                            GUILayout.FlexibleSpace();

                        string style = "Button";
                        if (v.buttonCondense[j])
                        {
                            bool hasLeft = false;
                            bool hasRight = false;
                            for (int p = k - 1; p >= 0; p -= 1)
                                if (v.buttonEnabled[j * 4 + p])
                                    hasLeft = true;
                            for (int p = k + 1; p < 4; p += 1)
                                if (v.buttonEnabled[j * 4 + p])
                                    hasRight = true;

                            if (!hasLeft && hasRight)
                                style = "ButtonLeft";
                            else if (hasLeft && hasRight)
                                style = "ButtonMid";
                            else if (hasLeft && !hasRight)
                                style = "ButtonRight";
                            else if (!hasLeft && !hasRight)
                                style = "Button";
                        }

                        if (GUILayout.Button(v.buttonText[j * 4 + k], style, GUILayout.MinWidth(60.0f)))
                        {
                            foreach (object t in targets)
                            {
                                MethodInfo m = t.GetType()
                                                .GetMethod(v.buttonCallback[j * 4 + k],
                                                           BindingFlags.Public | BindingFlags.DeclaredOnly |
                                                           BindingFlags.Instance | BindingFlags.NonPublic);
                                if (m != null)
                                    m.Invoke(target, null);
                            }
                        }

                        if (!v.buttonCondense[j] && !alignRight)
                            GUILayout.FlexibleSpace();
                    }
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }
            GUILayout.Space(space);
        }
        so.ApplyModifiedProperties();
    }

    public override void OnInspectorGUI()
    {
        so.Update();
        //RefreshVars();

        GUI.enabled = true;

        #region Base Options

        so.FindProperty("showBaseOptions").boolValue = RenderHeader("Base Options", so.FindProperty("showBaseOptions").boolValue);

        if (so.FindProperty("showBaseOptions").boolValue)
        {
            RenderPropertyField(so.FindProperty("drawWire"));
            RenderPropertyField(so.FindProperty("debugTriggerBox"));
            RenderPropertyField(so.FindProperty("triggerTags"));
            RenderPropertyField(so.FindProperty("destroyOnTrigger"));
            RenderPropertyField(so.FindProperty("triggerboxColour"));

            var triggerFollowProp = so.FindProperty("triggerFollow");
            RenderPropertyField(triggerFollowProp);

            if (triggerFollowProp.enumValueIndex != 0)
            {
                RenderPropertyField(so.FindProperty("followTransform"));
                RenderPropertyField(so.FindProperty("followTransformName"));
            }
        }

        #endregion

        #region Camera Conditions

        so.FindProperty("showCameraConditions").boolValue = RenderHeader("Camera Conditions", so.FindProperty("showCameraConditions").boolValue, (so.FindProperty("viewConditionType").enumValueIndex != 0));

        if (so.FindProperty("showCameraConditions").boolValue)
        {
            var viewConditionTypeProp = so.FindProperty("viewConditionType");
            RenderPropertyField(viewConditionTypeProp);

            if (viewConditionTypeProp.enumValueIndex != 0)
            {
                RenderPropertyField(so.FindProperty("viewObject"));
                RenderPropertyField(so.FindProperty("lookObjectCondition"));
                RenderPropertyField(so.FindProperty("ignoreObstacles"));
                RenderPropertyField(so.FindProperty("canWander"));
                RenderPropertyField(so.FindProperty("conditionTime"));
            }
        }

        #endregion

        #region Player Pref Conditions

        so.FindProperty("showPPrefConditions").boolValue = RenderHeader("Player Prefs Conditions", so.FindProperty("showPPrefConditions").boolValue, (so.FindProperty("playerPrefCondition").enumValueIndex != 0));

        if (so.FindProperty("showPPrefConditions").boolValue)
        {
            var viewPPrefTypeProp = so.FindProperty("playerPrefCondition");
            RenderPropertyField(viewPPrefTypeProp);

            if (viewPPrefTypeProp.enumValueIndex != 0)
            {
                RenderPropertyField(so.FindProperty("playerPrefVal"));
                RenderPropertyField(so.FindProperty("playerPrefKey"));
                RenderPropertyField(so.FindProperty("playerPrefType"));
            }
        }

        #endregion

        #region Animation Responses

        so.FindProperty("showAnimResponses").boolValue = RenderHeader("Animation Responses", so.FindProperty("showAnimResponses").boolValue, (so.FindProperty("animationTarget").objectReferenceValue != null || so.FindProperty("stopAnim").boolValue));

        if (so.FindProperty("showAnimResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("animationTarget"));
            RenderPropertyField(so.FindProperty("setMecanimTrigger"));
            RenderPropertyField(so.FindProperty("stopAnim"));
            RenderPropertyField(so.FindProperty("playLegacyAnimation"));
        }

        #endregion

        #region Audio Responses

        so.FindProperty("showAudioResponses").boolValue = RenderHeader("Audio Responses", so.FindProperty("showAudioResponses").boolValue, (so.FindProperty("muteAllAudio").boolValue || so.FindProperty("playMusic").objectReferenceValue != null || so.FindProperty("playSoundEffect").objectReferenceValue != null));

        if (so.FindProperty("showAudioResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("muteAllAudio"));
            RenderPropertyField(so.FindProperty("playMusic"));
            RenderPropertyField(so.FindProperty("loopMusic"));
            RenderSlider(so.FindProperty("musicVolume"), 0f, 1f);
            RenderPropertyField(so.FindProperty("playSoundEffect"));
        }

        #endregion

        #region Call Function Responses

        so.FindProperty("showCallFResponses").boolValue = RenderHeader("Call Function Responses", so.FindProperty("showCallFResponses").boolValue, (so.FindProperty("messageTarget").objectReferenceValue != null && !string.IsNullOrEmpty(so.FindProperty("messageMethodName").stringValue)));

        if (so.FindProperty("showCallFResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("messageTarget"));
            RenderPropertyField(so.FindProperty("messageMethodName"));
            RenderPropertyField(so.FindProperty("parameterType"));
            RenderPropertyField(so.FindProperty("parameterValue"));
        }

        #endregion

        #region Player Pref Responses

        so.FindProperty("showPPrefResponses").boolValue = RenderHeader("Player Pref Responses", so.FindProperty("showPPrefResponses").boolValue, (!string.IsNullOrEmpty(so.FindProperty("setPlayerPrefKey").stringValue) && !string.IsNullOrEmpty(so.FindProperty("setPlayerPrefVal").stringValue)));

        if (so.FindProperty("showPPrefResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("setPlayerPrefKey"));
            RenderPropertyField(so.FindProperty("setPlayerPrefType"));
            RenderPropertyField(so.FindProperty("setPlayerPrefVal"));
        }

        #endregion

        #region Spawn Responses

        so.FindProperty("showSpawnResponses").boolValue = RenderHeader("Spawn GameObject Responses", so.FindProperty("showSpawnResponses").boolValue, (so.FindProperty("prefabToSpawn").objectReferenceValue != null));

        if (so.FindProperty("showSpawnResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("prefabToSpawn"));
            RenderPropertyField(so.FindProperty("newInstanceName"));
            RenderPropertyField(so.FindProperty("spawnPosition"));
        }

        #endregion

        #region Destroy Responses

        so.FindProperty("showDestroyResponses").boolValue = RenderHeader("Destroy GameObject Responses", so.FindProperty("showDestroyResponses").boolValue, (so.FindProperty("destroyGameobjects").arraySize > 0 || so.FindProperty("destroyObjectNames").arraySize > 0));

        if (so.FindProperty("showDestroyResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("destroyGameobjects"));
            RenderPropertyField(so.FindProperty("destroyObjectNames"));
        }

        #endregion

        #region Enable Responses

        so.FindProperty("showEnableResponses").boolValue = RenderHeader("Enable GameObject Responses", so.FindProperty("showEnableResponses").boolValue, (so.FindProperty("enableGameObject").arraySize > 0));

        if (so.FindProperty("showEnableResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("enableGameObject"));
        }

        #endregion

        #region Disable Responses

        so.FindProperty("showDisableResponses").boolValue = RenderHeader("Disable GameObject Responses", so.FindProperty("showDisableResponses").boolValue, (so.FindProperty("disableGameObject").arraySize > 0 || so.FindProperty("disableGameObjectName").arraySize > 0));

        if (so.FindProperty("showDisableResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("disableGameObject"));
            RenderPropertyField(so.FindProperty("disableGameObjectName"));
        }

        #endregion

        #region Level Responses

        so.FindProperty("showLevelResponses").boolValue = RenderHeader("Load Level Responses", so.FindProperty("showLevelResponses").boolValue, (!string.IsNullOrEmpty(so.FindProperty("loadLevelName").stringValue)));

        if (so.FindProperty("showLevelResponses").boolValue)
        {
            RenderPropertyField(so.FindProperty("loadLevelName"));
            RenderPropertyField(so.FindProperty("loadDelay"));
        }

        #endregion

        so.ApplyModifiedProperties();
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

    private void RenderSlider(SerializedProperty sp, float min, float max)
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

                EditorGUILayout.Slider(sp, min, max);

                if (!sp.NextVisible(child))
                    break;
            }

            GUILayout.EndVertical();
        }
        else
        {
            EditorGUILayout.Slider(sp, min, max);
        }

        GUILayout.EndHorizontal();
    }

    private bool RenderHeader(string s, bool optionRef, bool bold = true)
    {
        GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);

        if (bold)
            myFoldoutStyle.fontStyle = FontStyle.Bold;

        GUILayout.Space(10.0f);

        EditorGUI.indentLevel = 1;
        EditorGUIUtility.LookLikeInspector();

        return EditorGUILayout.Foldout(optionRef, s, myFoldoutStyle);
    }

    private void VectorScene(InspectorPlusVar v, string s, Transform t)
    {
        Vector3 val;

        if (s == typeof(Vector3).Name)
            val = (Vector3)GetTargetField(name);
        else
            val = ((Vector2)GetTargetField(name));

        Vector3 newVal = Vector3.zero;
        Vector3 curVal = Vector3.zero;
        bool setVal = false;
        bool relative = v.relative;
        bool scale = v.scale;

        switch (v.vectorDrawType)
        {
            case InspectorPlusVar.VectorDrawType.Direction:
                curVal = relative ? val : val - t.position;
                float size = scale ? Mathf.Min(2.0f, Mathf.Sqrt(curVal.magnitude) / 2.0f) : 1.0f;
                size *= HandleUtility.GetHandleSize(t.position);
                Handles.ArrowCap(0, t.position,
                                 curVal != Vector3.zero ? Quaternion.LookRotation(val.normalized) : Quaternion.identity,
                                 size);
                break;

            case InspectorPlusVar.VectorDrawType.Point:
                curVal = relative ? val : t.position + val;
                Handles.SphereCap(0, curVal, Quaternion.identity, 0.1f);
                break;

            case InspectorPlusVar.VectorDrawType.PositionHandle:
                curVal = relative ? t.position + val : val;
                setVal = true;
                newVal = Handles.PositionHandle(curVal, Quaternion.identity) - (relative ? t.position : Vector3.zero);
                break;

            case InspectorPlusVar.VectorDrawType.Scale:
                setVal = true;
                curVal = relative ? t.localScale + val : val;
                newVal =
                    Handles.ScaleHandle(curVal, t.position + v.offset, t.rotation,
                                        HandleUtility.GetHandleSize(t.position + v.offset)) -
                    (relative ? t.localScale : Vector3.zero);
                break;

            case InspectorPlusVar.VectorDrawType.Rotation:
                setVal = true;
                curVal = relative ? val + t.rotation.eulerAngles : val;
                newVal = Handles.RotationHandle(Quaternion.Euler(curVal), t.position + v.offset).eulerAngles -
                         (relative ? t.rotation.eulerAngles : Vector3.zero);
                break;
        }

        if (setVal)
        {
            object newObjectVal = newVal;

            if (s == typeof(Vector2).Name)
                newObjectVal = (Vector2)newVal;
            else if (s == typeof(Quaternion).Name)
                newObjectVal = Quaternion.Euler(newVal);

            SetTargetField(name, newObjectVal);
        }
    }

    private object GetTargetField(string name)
    {
        return target.GetType().GetField(name).GetValue(target);
    }

    private void SetTargetField(string name, object value)
    {
        target.GetType().GetField(name).SetValue(target, value);
    }

    //some magic to draw the handles
    public void OnSceneGUI()
    {
        var script = (TriggerBox)target;
        if (script.prefabToSpawn)
        {
            Handles.Label(script.spawnPosition, "Spawn " + script.prefabToSpawn.name);

            script.spawnPosition = Handles.PositionHandle(script.spawnPosition, Quaternion.identity);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);

        Transform t = ((MonoBehaviour)target).transform;

        foreach (InspectorPlusVar v in vars)
        {
            if (!v.active)
                continue;

            string s = v.type;
            name = v.name;
            if (s == typeof(Vector3).Name || s == typeof(Vector2).Name)
                VectorScene(v, s, t);
        }
    }

    public class InspectorPlusVar
    {
        public enum LimitType
        {
            None,
            Max,
            Min,
            Range
        };

        public enum VectorDrawType
        {
            None,
            Direction,
            Point,
            PositionHandle,
            Scale,
            Rotation
        };

        public bool QuaternionHandle;

        public bool active = true;
        public string[] buttonCallback = new string[16];
        public bool[] buttonCondense = new bool[4];
        public bool[] buttonEnabled = new bool[16];
        public string[] buttonText = new string[16];
        public bool canWrite = true;
        public string classType;
        public string dispName;
        public bool hasTooltip = false;
        public int iMax = -0;
        public int iMin = -0;
        public string[] label = new string[4];
        public int[] labelAlign = new int[4];
        public bool[] labelBold = new bool[4];
        public bool[] labelEnabled = new bool[4];
        public bool[] labelItalic = new bool[4];
        public bool largeTexture;
        public LimitType limitType = LimitType.None;
        public float max = -0.0f;
        public float min = -0.0f;
        public string name;

        public int numSpace = 0;
        public Vector3 offset = new Vector3(0.5f, 0.5f);
        public bool progressBar;
        public bool relative = false;
        public bool scale = false;
        public float space = 0.0f;
        public bool textArea;
        public string textFieldDefault;
        public float textureSize;
        public int toggleLevel = 0;
        public int toggleSize = 0;
        public bool toggleStart = false;
        public string tooltip;
        public string type;
        public VectorDrawType vectorDrawType;

        public InspectorPlusVar(LimitType _limitType, float _min, float _max, bool _progressBar, int _iMin, int _iMax,
                                bool _active, string _type, string _name, string _dispName,
                                VectorDrawType _vectorDrawType, bool _relative, bool _scale, float _space,
                                bool[] _labelEnabled, string[] _label, bool[] _labelBold, bool[] _labelItalic,
                                int[] _labelAlign, bool[] _buttonEnabled, string[] _buttonText,
                                string[] _buttonCallback, bool[] buttonCondense, int _numSpace, string _classType,
                                Vector3 _offset, bool _QuaternionHandle, bool _canWrite, string _tooltip,
                                bool _hasTooltip,
                                bool _toggleStart, int _toggleSize, int _toggleLevel, bool _largeTexture,
                                float _textureSize, string _textFieldDefault, bool _textArea)
        {
            limitType = _limitType;
            min = _min;
            max = _max;
            progressBar = _progressBar;
            iMax = _iMax;
            iMin = _iMin;
            active = _active;
            type = _type;
            name = _name;
            dispName = _dispName;
            vectorDrawType = _vectorDrawType;
            relative = _relative;
            scale = _scale;
            space = _space;
            labelEnabled = _labelEnabled;
            label = _label;
            labelBold = _labelBold;
            labelItalic = _labelItalic;
            labelAlign = _labelAlign;
            buttonEnabled = _buttonEnabled;
            buttonText = _buttonText;
            buttonCallback = _buttonCallback;
            numSpace = _numSpace;
            classType = _classType;
            offset = _offset;
            QuaternionHandle = _QuaternionHandle;
            canWrite = _canWrite;
            tooltip = _tooltip;
            hasTooltip = _hasTooltip;
            toggleStart = _toggleStart;
            toggleSize = _toggleSize;
            toggleLevel = _toggleLevel;
            largeTexture = _largeTexture;
            textureSize = _textureSize;
            textFieldDefault = _textFieldDefault;
            textArea = _textArea;
        }
    }
}