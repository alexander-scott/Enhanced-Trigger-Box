using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[Serializable]
public class Subobject : ScriptableObject
{
    [SerializeField]
    protected int m_IntField = 1;

    public virtual void OnGUI()
    {
        m_IntField = EditorGUILayout.IntSlider("IntField", m_IntField, 0, 10);
    }
}