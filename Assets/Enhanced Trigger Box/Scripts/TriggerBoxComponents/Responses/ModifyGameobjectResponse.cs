using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to enable, disable or destroy a gameobject
    /// </summary>
    [AddComponentMenu("")]
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
        /// The index of the currently selected component that will be enabled/disabled
        /// </summary>
        public int selectedComponentIndex = 0;

        /// <summary>
        /// A reference of the component itself that will be enabled/disabled
        /// </summary>
        public UnityEngine.Component selectedComponent;

        /// <summary>
        /// The number of components. Used to check for changes in gameobject, ie removal of a component
        /// </summary>
        public int componentCount = 0;

        /// <summary>
        /// This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.
        /// </summary>
        public ReferenceType referenceType;

        public override bool requiresCollisionObjectData
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// The available types of modification to a gameobject
        /// </summary>
        public enum ModifyType
        {
            Destroy,
            Disable,
            Enable,
            DisableComponent,
            EnableComponent,
        }

        public enum ReferenceType
        {
            Null,
            GameObjectReference,
            GameObjectName,
            CollisionGameObject,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            referenceType = (ReferenceType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Reference Type",
                   "This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box."), referenceType);

            switch (referenceType)
            {
                case ReferenceType.GameObjectReference:
                    obj = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("GameObject",
                    "The gameobject that will modified."), obj, typeof(GameObject), true);
                    break;

                case ReferenceType.GameObjectName:
                    gameObjectName = UnityEditor.EditorGUILayout.TextField(new GUIContent("GameObject Name",
                    "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and modified."), gameObjectName);
                    break;
            }

            modifyType = (ModifyType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Modification Type",
                   "This is the type of modification. Destroy, disable and enable are applied to the gameobject itself whereas disable component and enable component are applied to specific components that belong to the gameobject."), modifyType);

            if (modifyType == ModifyType.DisableComponent || modifyType == ModifyType.EnableComponent)
            {
                if (obj)
                {
                    // Get all components that can be disabled/enabled. Should this be cached?
                    List<UnityEngine.Component> components = GetObjectComponents();

                    if (components.Count == 0) // No components so return out of the function.
                    {
                        componentCount = 0;
                        return;
                    }

                    // The number of components belonging to this gameobject has changed.
                    if (componentCount != components.Count && selectedComponent != null)
                    {
                        // Find the index of the component previously selected
                        selectedComponentIndex = components.FindIndex(c => c == selectedComponent);

                        componentCount = components.Count;
                    }
                    else if (selectedComponent == null) // The selected component has been deleted from the gameobject
                    {
                        selectedComponentIndex = 0;
                    }

                    // TODO: Figure out how to add tooltip
                    selectedComponentIndex = UnityEditor.EditorGUILayout.Popup("Select Component", selectedComponentIndex, components.Select(n => n.GetType().ToString()).ToArray());

                    selectedComponent = components[selectedComponentIndex];
                }
            }
        }
#endif

        public override void Validation()
        {
            if (referenceType == ReferenceType.Null)
            {
                if (!obj && !string.IsNullOrEmpty(gameObjectName))
                {
                    referenceType = ReferenceType.GameObjectName;
                }
                else
                {
                    referenceType = ReferenceType.GameObjectReference;
                }
            }

            // If the user is disabling/enabling a gameobject but hasn't supplied a gameobject reference
            if ((modifyType == ModifyType.DisableComponent || modifyType == ModifyType.EnableComponent) && referenceType != ReferenceType.GameObjectReference)
            {
                ShowWarningMessage("You cannot enable or disable a component on a gameobject without supplying a gameobject reference.");
            }

            // If the user is disabling/enabling a gameobject but the gameobject hasn't got any components that can be disabled/enabled
            if ((modifyType == ModifyType.DisableComponent || modifyType == ModifyType.EnableComponent) && referenceType == ReferenceType.GameObjectReference && componentCount == 0)
            {
                ShowWarningMessage("The  gameobject you've chosen to enable or disable a component on hasn't got any components attached to it that can be enabled or disabled.");
            }
        }

        public override bool ExecuteAction(GameObject collisionGameObject)
        {
            switch (referenceType)
            {
                case ReferenceType.Null:
                    if (!obj && !string.IsNullOrEmpty(gameObjectName))  // Prevents error
                    {
                        obj = GameObject.Find(gameObjectName);
                    }
                    break;

                case ReferenceType.CollisionGameObject:
                    obj = collisionGameObject;
                    break;

                case ReferenceType.GameObjectName:
                    obj = GameObject.Find(gameObjectName);
                    break;
            }

            if (!obj)
            {
                return false;
            }

            switch (modifyType)
            {
                case ModifyType.Destroy:
                    Destroy(obj);
                    break;

                case ModifyType.Disable:
                    obj.SetActive(false);
                    break;

                case ModifyType.Enable:
                    obj.SetActive(true);
                    break;

                case ModifyType.DisableComponent:
                    if (referenceType == ReferenceType.GameObjectReference)
                    {
                        var propInfo = selectedComponent.GetType().GetProperty("enabled");
                        if (propInfo != null)
                        {
                            propInfo.SetValue(selectedComponent, false, null);
                        }
                        else
                        {
                            Debug.Log("ETB Error: Unable to disable component because the 'enabled' property could not be found.");
                        }
                    }
                    break;

                case ModifyType.EnableComponent:
                    if (referenceType == ReferenceType.GameObjectReference)
                    {
                        var propInfo = selectedComponent.GetType().GetProperty("enabled");
                        if (propInfo != null)
                        {
                            propInfo.SetValue(selectedComponent, true, null);
                        }
                        else
                        {
                            Debug.Log("ETB Error: Unable to enable component because the 'enabled' property could not be found.");
                        }
                    }
                    break;
            }

            return true;
        }

        /// <summary>
        /// Returns all Unity Components that can be enabled/disabled on the selected gameobject
        /// </summary>
        /// <returns>List of components</returns>
        private List<UnityEngine.Component> GetObjectComponents()
        {
            List<UnityEngine.Component> returnList = new List<UnityEngine.Component>();

            foreach (UnityEngine.Component c in obj.GetComponents<UnityEngine.Component>())
            {
                // Does the component have the 'enabled' property. 
                // This makes this assumption that it will never be called anything other than 'enabled'
                if (c.GetType().GetProperty("enabled") != null) 
                {
                    if (!c.GetType().ToString().Contains("EnhancedTriggerbox.Component")) // Don't include trigger box components
                    {
                        returnList.Add(c);
                    }
                }
            }

            return returnList;
        }
    }
}
