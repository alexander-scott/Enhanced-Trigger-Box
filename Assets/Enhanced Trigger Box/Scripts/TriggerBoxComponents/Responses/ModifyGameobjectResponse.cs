using UnityEngine;
using UnityEditor;

namespace EnhancedTriggerbox.Component
{
    public class ModifyGameobjectResponse : ResponseComponent
    {
        /// <summary>
        /// The gameobject that will modified
        /// </summary>
        public GameObject obj;

        /// <summary>
        /// If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and modified
        /// </summary>
        public string gameObjectName;

        /// <summary>
        /// This is the type of modification you want to happen to the gameobject. Options are destroy, disable and enable.
        /// </summary>
        public ModifyType modifyType;

        /// <summary>
        /// The available types of modification to a gameobject
        /// </summary>
        public enum ModifyType
        {
            Destroy,
            Disable,
            Enable,
        }

        public override void DrawInspectorGUI()
        {
            obj = (GameObject)EditorGUILayout.ObjectField(new GUIContent("GameObject",
                 "The gameobject that will modified."), obj, typeof(GameObject), true);

            gameObjectName = EditorGUILayout.TextField(new GUIContent("GameObject Name",
               "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and modified."), gameObjectName);

            modifyType = (ModifyType)EditorGUILayout.EnumPopup(new GUIContent("Modify Type",
                   "This is the type of modification. "), modifyType);
        }

        public override void Validation()
        {
            if (obj && !string.IsNullOrEmpty(gameObjectName))
            {
                ShowWarningMessage("You cannot have both a gameobject reference and gameobject name. Remove one or the other.");
            }

            if (modifyType == ModifyType.Enable && !string.IsNullOrEmpty(gameObjectName))
            {
                ShowWarningMessage("You cannot enable a gameobject by name. Please pass in a reference instead.");
            }
        }

        public override bool ExecuteAction()
        {
            switch (modifyType)
            {
                case ModifyType.Destroy:
                    if (obj)
                    {
                        Destroy(obj);
                    }
                    else if (!string.IsNullOrEmpty(gameObjectName))
                    {
                        GameObject gameobj = GameObject.Find(gameObjectName);
                        if (gameobj == null)
                        {
                            Debug.Log("Unable to find and destroy the gameobject with the name " + gameObjectName);
                        }
                        else
                        {
                            Destroy(gameobj);
                        }
                    }
                    break;

                case ModifyType.Disable:
                    if (obj)
                    {
                        obj.SetActive(false);
                    }
                    else if (!string.IsNullOrEmpty(gameObjectName))
                    {
                        GameObject gameobj = GameObject.Find(gameObjectName);
                        if (gameobj == null)
                        {
                            Debug.Log("Unable to find and disable the gameobject with the name " + gameObjectName);
                        }
                        else
                        {
                            gameobj.SetActive(false);
                        }
                    }
                    break;

                case ModifyType.Enable:
                    if (obj)
                    {
                        obj.SetActive(true);
                    }
                    break;
            }

            return true;
        }
    }
}
