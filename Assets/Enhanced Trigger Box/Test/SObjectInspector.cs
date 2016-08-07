using UnityEngine;
using UnityEditor;
using System.Collections;
 
[CustomEditor(typeof(SObject))]
public class SObjectInspector : Editor
{
    //[MenuItem("Test/Prompt2")]
    //public static void Prompt2()
    //{
    //    if (AssetDatabase.LoadAssetAtPath(SObject.assetPath, typeof(SObject)) == null)
    //    {
    //        ScriptableObject data = (ScriptableObject)ScriptableObject.CreateInstance<ScriptableObject>();
    //        AssetDatabase.CreateAsset(data, SObject.dataAssetPath);

    //        SObject asset = (SObject)ScriptableObject.CreateInstance<SObject>();
    //        AssetDatabase.CreateAsset(asset, SObject.assetPath);
    //        AssetDatabase.SaveAssets();
    //    }
    //    Selection.activeObject = AssetDatabase.LoadAssetAtPath(SObject.assetPath, typeof(SObject));

    //}

    SObject theObject;
   
   void OnEnable()
   {
     Init();
   }
   
   void Init()
   {
     theObject = (SObject) target;
   }
   
   public override void OnInspectorGUI()
   {
     if(theObject == null)
       Init();
     
     EditorGUI.BeginChangeCheck ();
     
     EditorGUILayout.Space();
     
     theObject.OnGUI();
     
     if(EditorGUI.EndChangeCheck())
     {
       EditorUtility.SetDirty(theObject);
       
       foreach(Subobject o in theObject.list)
       {
         EditorUtility.SetDirty(o);
       }
       
       AssetDatabase.SaveAssets();
     }
   }
}