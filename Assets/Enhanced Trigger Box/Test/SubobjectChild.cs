using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class SubobjectChild : Subobject
{
    [SerializeField]
    protected float m_FloatField = 1;

    public override void OnGUI()
    {
        base.OnGUI();
        m_FloatField = EditorGUILayout.FloatField(m_FloatField);
    }
}