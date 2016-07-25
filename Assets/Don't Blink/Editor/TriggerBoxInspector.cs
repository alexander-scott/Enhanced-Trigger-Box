using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CanEditMultipleObjects]
[CustomEditor(typeof (TriggerBox))]
public class TriggerBoxInspector : Editor
{
    private string dispName;
    private new string name;
    private SerializedProperty[] properties;
    private SerializedObject so;
    private Rect tooltipRect;
    private List<InspectorPlusVar> vars;
    public SerializedProperty longStringProp;

    private void RefreshVars()
    {
        for (int i = 0; i < vars.Count; i += 1) properties[i] = so.FindProperty(vars[i].name);
    }

    private void OnEnable()
    {
        longStringProp = serializedObject.FindProperty("showText");


        vars = new List<InspectorPlusVar>();
        so = serializedObject;

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean", "drawWire",
                              "Draw Wirebox", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                              new[] { false, false, false, false }, new[] { "", "", "", "" },
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
                              "triggerboxColor", "Triggerbox Color", InspectorPlusVar.VectorDrawType.None, false,
                              false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
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

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Int32", "triggerCount",
                                      "Trigger Count", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                                      "triggerByTag1", "Trigger By Tag 1", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                                      "triggerByTag2", "Trigger By Tag 2", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 47, new[] {true, false, false, false},
                                      new[] {"Animation:", "", "", ""}, new[] {true, false, false, false},
                                      new[] {false, false, false, false}, new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
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
                                      "playAnimation", "Play Legacy Animation", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 45, new[] {true, false, false, false}, new[] {"Audio:", "", "", ""},
                                      new[] {true, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Bool", "mute",
                                      "Stop Music", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "AudioClip", "playAudio",
                                    "Play Music", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                    new[] { false, false, false, false }, new[] { "", "", "", "" },
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
                                 "Loop Music", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                 new[] { false, false, false, false }, new[] { "", "", "", "" },
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
                         "Music Volume", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                         new[] { false, false, false, false }, new[] { "", "", "", "" },
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

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "AudioClip", "sfx",
                         "Play Sound Effect", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                         new[] { false, false, false, false }, new[] { "", "", "", "" },
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

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.Range, 0, 1, false, 0, 0, true, "Single", "soundEffectVolume",
                                      "Sound Effect Volume", InspectorPlusVar.VectorDrawType.None, false, false, 48,
                                      new[] {true, false, false, false}, new[] {"Display Subtitle:", "", "", ""},
                                      new[] {true, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));


 
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Int32", "textSize",
                                      "Text Size", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Color", "textColor",
                                      "Text Color", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Font", "textFont",
                                      "Text Font", InspectorPlusVar.VectorDrawType.None, false, false, 0,
                                      new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "FontStyle",
                                      "textFontStyle", "Text Font Style", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 0, new[] {true, false, false, false}, new[] {"Call Function:", "", "", ""},
                                      new[] {true, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

       

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Single", "textTime",
                                      "Display Time", InspectorPlusVar.VectorDrawType.None, false, false, 44,
                                      new[] {true, false, false, false}, new[] {"Call Function:", "", "", ""},
                                      new[] {true, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, false, "String",
                                 "showText", "Trigger By Tag 1", InspectorPlusVar.VectorDrawType.None, false,
                                 false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
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
                                 false, true, "Tooltip", false, false, 0, 0, false, 70, "", true));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                                      "messageTarget", "Send Message To", InspectorPlusVar.VectorDrawType.None, false,
                                      false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "String",
                                    "sendMessage", "Send Message Name", InspectorPlusVar.VectorDrawType.None, false,
                                    false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
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

        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "msgtype",
                            "parameterType", "Parameter Type", InspectorPlusVar.VectorDrawType.None, false,
                            false, 0, new[] { false, false, false, false }, new[] { "", "", "", "" },
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
                                      "Parameter Value", InspectorPlusVar.VectorDrawType.None, false, false, 43,
                                      new[] {true, false, false, false}, new[] {"Spawn GameObject:", "", "", ""},
                                      new[] {true, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                                      "spawnGameobject", "Prefab to Spawn", InspectorPlusVar.VectorDrawType.None,
                                      false, false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Vector3",
                                      "spawnPosition", "Spawn Position", InspectorPlusVar.VectorDrawType.None,
                                      false, false, 37, new[] {true, false, false, false},
                                      new[] {"Enable/disable:", "", "", ""}, new[] {true, false, false, false},
                                      new[] {false, false, false, false}, new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 1, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));
        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "GameObject",
                                      "targetgameObject", "GameObject Target", InspectorPlusVar.VectorDrawType.None,
                                      false, false, 0, new[] {false, false, false, false}, new[] {"", "", "", ""},
                                      new[] {false, false, false, false}, new[] {false, false, false, false},
                                      new[] {0, 0, 0, 0},
                                      new[]
                                          {
                                              false, false, false, false, false, false, false, false, false, false, false,
                                              false, false, false, false, false
                                          },
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", ""},
                                      new[] {false, false, false, false}, 0, "TriggerBox", new Vector3(0.5f, 0.5f, 0f),
                                      false, true, "Tooltip", false, false, 0, 0, false, 70, "", false));


        vars.Add(new InspectorPlusVar(InspectorPlusVar.LimitType.None, 0, 0, false, 0, 0, true, "Boolean", "isEnabled",
                              "Enabled", InspectorPlusVar.VectorDrawType.None, false, false, 44,
                              new[] { true, false, false, false }, new[] { "Load Level:", "", "", "" },
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
        int count = vars.Count;
        properties = new SerializedProperty[count];
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
                    ProgressBar((sp.floatValue - v.min)/v.max, dispName);
                }
                else
                    ProgressBar((sp.floatValue - v.min)/v.max, dispName);
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
                    ProgressBar((float) (sp.intValue - v.iMin)/v.iMax, dispName);
                }
                else
                    ProgressBar((float) (sp.intValue - v.iMin)/v.iMax, dispName);
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

    private void PropertyField(SerializedProperty sp, string name)
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
        else EditorGUILayout.PropertyField(sp, new GUIContent(dispName));
    }

    public override void OnInspectorGUI()
    {
        so.Update();
        RefreshVars();

        longStringProp.stringValue = EditorGUILayout.TextArea(longStringProp.stringValue, GUILayout.MaxHeight(75));


        
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
                    GUILayout.Space(v.toggleLevel*10.0f);

                if (s == typeof (float).Name)
                {
                    FloatField(sp, v);
                    skip = true;
                }
                if (s == typeof (int).Name)
                {
                    IntField(sp, v);
                    skip = true;
                }
                if (s == typeof (bool).Name)
                {
                    i += BoolField(sp, v);
                    skip = true;
                }
                if (!skip)
                    PropertyField(sp, name);
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
            float space = Mathf.Max(0.0f, (v.space - usedSpace)/2.0f);
            GUILayout.Space(space);
            for (int j = 0; j < v.numSpace; j += 1)
            {
                bool buttonLine = false;
                for (int k = 0; k < 4; k += 1) if (v.buttonEnabled[j*4 + k]) buttonLine = true;
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
                    if (v.buttonEnabled[j*4 + k])
                    {
                        if (!v.buttonCondense[j] && !alignRight)
                            GUILayout.FlexibleSpace();

                        string style = "Button";
                        if (v.buttonCondense[j])
                        {
                            bool hasLeft = false;
                            bool hasRight = false;
                            for (int p = k - 1; p >= 0; p -= 1)
                                if (v.buttonEnabled[j*4 + p])
                                    hasLeft = true;
                            for (int p = k + 1; p < 4; p += 1)
                                if (v.buttonEnabled[j*4 + p])
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

                        if (GUILayout.Button(v.buttonText[j*4 + k], style, GUILayout.MinWidth(60.0f)))
                        {
                            foreach (object t in targets)
                            {
                                MethodInfo m = t.GetType()
                                                .GetMethod(v.buttonCallback[j*4 + k],
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

    private void VectorScene(InspectorPlusVar v, string s, Transform t)
    {
        Vector3 val;

        if (s == typeof (Vector3).Name)
            val = (Vector3) GetTargetField(name);
        else
            val = ((Vector2) GetTargetField(name));

        Vector3 newVal = Vector3.zero;
        Vector3 curVal = Vector3.zero;
        bool setVal = false;
        bool relative = v.relative;
        bool scale = v.scale;

        switch (v.vectorDrawType)
        {
            case InspectorPlusVar.VectorDrawType.Direction:
                curVal = relative ? val : val - t.position;
                float size = scale ? Mathf.Min(2.0f, Mathf.Sqrt(curVal.magnitude)/2.0f) : 1.0f;
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

            if (s == typeof (Vector2).Name)
                newObjectVal = (Vector2) newVal;
            else if (s == typeof (Quaternion).Name)
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

        var script = (TriggerBox) target;
        if (script.spawnGameobject)
            Handles.Label(script.spawnPosition, "Spawn " + script.spawnGameobject.name);

        script.spawnPosition =
            Handles.PositionHandle(script.spawnPosition, Quaternion.identity);

        if (GUI.changed) 
    EditorUtility.SetDirty (target);
		


        Transform t = ((MonoBehaviour) target).transform;

        foreach (InspectorPlusVar v in vars)
        {
            if (!v.active)
                continue;

            string s = v.type;
            name = v.name;
            if (s == typeof (Vector3).Name || s == typeof (Vector2).Name)
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