using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
 
[Serializable]
public class SObject : MonoBehaviour
{
   public static string assetPath = "Assets/Resources/Bar.asset";
   public static string dataAssetPath = "Assets/Resources/DataBar.asset";
   
   [SerializeField]
   private List<Subobject> m_Instances;
   [SerializeField]
   private ScriptableObject data;
 
   public List<Subobject> list
   {
     get { return m_Instances; }
   }
   
   public void OnEnable ()
   {
     if (m_Instances == null)
     {
       m_Instances = new List<Subobject>();
       data = (ScriptableObject) AssetDatabase.LoadAssetAtPath(dataAssetPath, typeof(ScriptableObject));
     }
   }
   
   public void OnGUI ()
   {
     if(m_Instances == null)
     {
       return;
     }
     
     foreach (var instance in m_Instances)
     {
       instance.OnGUI ();
     }
 
     if (GUILayout.Button ("Add Base"))
     {
       Subobject newObject = (Subobject) ScriptableObject.CreateInstance<Subobject>();
       //AssetDatabase.AddObjectToAsset(newObject, data);
       m_Instances.Add(newObject);
     }
     if (GUILayout.Button ("Add Child"))
     {
       SubobjectChild newObject = (SubobjectChild) ScriptableObject.CreateInstance<SubobjectChild>();
       //AssetDatabase.AddObjectToAsset(newObject, data);
       m_Instances.Add(newObject);
 
     }
   }
}