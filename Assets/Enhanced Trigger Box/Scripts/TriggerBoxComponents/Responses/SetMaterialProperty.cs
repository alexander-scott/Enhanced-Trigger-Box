using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to set a value within a material
    /// </summary>
    [AddComponentMenu("")]
    public class SetMaterialProperty : ResponseComponent
    {
        /// <summary>
        /// The gameobject with the material you want to edit.
        /// </summary>
        public GameObject targetGameobject;

        /// <summary>
        /// If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and modified
        /// </summary>
        public string targetGameobjectName;

        /// <summary>
        /// A reference of the material that you want to set a properties value.
        /// </summary>
        public Material targetMaterial;

        /// <summary>
        /// If true, the script will work with a clone of the material on this gameobject. 
        /// If false, it will use the original material in the project directory. WARNING: If false, it will permanently change that materials values.
        /// </summary>
        public bool cloneMaterial;

        /// <summary>
        /// The name of the property that you want to set.
        /// </summary>
        public string propertyName;

        /// <summary>
        /// The type of the property that you want to set. Float, Int, Colour, Vector4 or Texture.
        /// </summary>
        public PropertyType propertyType;

        /// <summary>
        /// The method of obtaining a reference for the material. GameObject will use targetGameObject.GetComponent<MeshRenderer>().material to get the material. 
        /// Material allows you to pass in a reference from a material in the project directory. WARNING: If you select material, it will permanently change that materials values.
        /// </summary>
        public ReferenceType referenceType;

        public override bool requiresCollisionObjectData
        {
            get
            {
                return true;
            }
        }

        public float propertyFloat;
        public int propertyInt;
        public Color propertyColour;
        public Vector4 propertyVector4;
        public Texture propertyTexture;

        private MeshRenderer meshRenderer;

        /// <summary>
        /// Available types of property
        /// </summary>
        public enum PropertyType
        {
            Float,
            Int,
            Colour,
            Vector4,
            Texture,
        }

        /// <summary>
        /// Available types of reference
        /// </summary>
        public enum ReferenceType
        {
            GameObjectReference,
            GameObjectName,
            CollisionGameObject,
            Material
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            referenceType = (ReferenceType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Reference Type",
                "The method of obtaining a reference for the material. GameObject will use targetGameObject.GetComponent<MeshRenderer>().material to get the material. " +
                "Material allows you to pass in a reference from a material in the project directory. WARNING: If you select material, it will permanently change that materials values."), referenceType);

            switch (referenceType)
            {
                case ReferenceType.GameObjectName:
                    targetGameobjectName = UnityEditor.EditorGUILayout.TextField(new GUIContent("GameObject Name",
                    "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find()) and modified."), targetGameobjectName);
                    break;

                case ReferenceType.GameObjectReference:
                    targetGameobject = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("GameObject",
                "The gameobject with the material you want to edit."), targetGameobject, typeof(GameObject), true);
                    break;

                case ReferenceType.Material:
                    targetMaterial = (Material)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Material",
                "A reference of the material that you want to set a properties value."), targetMaterial, typeof(Material), true);
                    break;
            }

            if (referenceType != ReferenceType.Material)
            {
                cloneMaterial = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Clone Material",
                "If true, the script will work with a clone of the material on this gameobject. " +
                "If false, it will use the original material in the project directory. WARNING: If false, it will permanently change that materials values."), cloneMaterial);
            }
            
            propertyName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Material Property Name",
                   "The name of the property that you want to set."), propertyName);

            propertyType = (PropertyType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Material Property Type",
                "The type of the property that you want to set. Float, Int, Colour, Vector4 or Texture."), propertyType);

            switch (propertyType)
            {
                case PropertyType.Float:
                    propertyFloat = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyFloat);
                    break;

                case PropertyType.Int:
                    propertyInt = UnityEditor.EditorGUILayout.IntField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyInt);
                    break;

                case PropertyType.Colour:
                    propertyColour = UnityEditor.EditorGUILayout.ColorField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyColour);
                    break;

                case PropertyType.Vector4:
                    propertyVector4 = UnityEditor.EditorGUILayout.Vector4Field(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyVector4);
                    break;

                case PropertyType.Texture:
                    propertyTexture = (Texture)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Material Property Value",
                    "The new value of this property."), propertyTexture, typeof(Texture), true);
                    break;
            }

            if (propertyType != PropertyType.Texture)
            {
                duration = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Change Duration",
                    "The duration you want this change to happen over."), duration);
            }
        }
#endif

        public override bool ExecuteAction(GameObject collisionGameObject)
        {
            if (referenceType != ReferenceType.Material)
            {
                switch (referenceType)
                {
                    case ReferenceType.CollisionGameObject:
                        targetGameobject = collisionGameObject;
                        break;

                    case ReferenceType.GameObjectName:
                        targetGameobject = GameObject.Find(targetGameobjectName);
                        break;
                }

                if (targetGameobject)
                {
                    if (cloneMaterial)
                    {
                        meshRenderer = targetGameobject.GetComponent<MeshRenderer>();
                        if (!meshRenderer)
                        {
                            Debug.Log("Unable to execute Set Material Property Response. Missing mesh renderer component!");
                            return true;
                        }
                        targetMaterial = new Material(meshRenderer.material);
                        meshRenderer.material = targetMaterial;
                    }
                    else
                    {
                        targetMaterial = targetGameobject.GetComponent<MeshRenderer>().material;
                    }
                }
                else
                {
                    Debug.Log("Unable to execute Set Material Property Response. Missing gameobject reference!");
                    return true;
                }
            }

            if (targetMaterial == null || string.IsNullOrEmpty(propertyName))
            {
                Debug.Log("Unable to execute Set Material Property Response. Missing material or property name!");
                return true;
            }

            if (duration > 0)
            {
                ApplyOverTime();
            }
            else
            {
                ApplyInstantly();
            }
            
            return true;
        }

        private void ApplyInstantly()
        {
            switch (propertyType)
            {
                case PropertyType.Float:
                    targetMaterial.SetFloat(propertyName, propertyFloat);
                    break;

                case PropertyType.Int:
                    targetMaterial.SetInt(propertyName, propertyInt);
                    break;

                case PropertyType.Colour:
                    targetMaterial.SetColor(propertyName, propertyColour);
                    break;

                case PropertyType.Vector4:
                    targetMaterial.SetVector(propertyName, propertyVector4);
                    break;

                case PropertyType.Texture:
                    targetMaterial.SetTexture(propertyName, propertyTexture);
                    break;
            }
        }

        private void ApplyOverTime()
        {
            switch (propertyType)
            {
                case PropertyType.Float:
                    activeCoroutines.Add(StartCoroutine(ModifyMaterialFloatOverTime()));
                    break;

                case PropertyType.Int:
                    activeCoroutines.Add(StartCoroutine(ModifyMaterialIntOverTime()));
                    break;

                case PropertyType.Colour:
                    activeCoroutines.Add(StartCoroutine(ModifyMaterialColourOverTime()));
                    break;

                case PropertyType.Vector4:
                    activeCoroutines.Add(StartCoroutine(ModifyMaterialVector4OverTime()));
                    break;

                case PropertyType.Texture:
                    targetMaterial.SetTexture(propertyName, propertyTexture);
                    break;
            }
        }

        #region Modify Material Coroutines

        private IEnumerator ModifyMaterialFloatOverTime()
        {
            Material mat = targetMaterial;

            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            float originalValue = mat.GetFloat(propertyName);

            while (progress < 1)
            {
                mat.SetFloat(propertyName, Mathf.Lerp(originalValue, propertyFloat, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private IEnumerator ModifyMaterialIntOverTime()
        {
            Material mat = targetMaterial;

            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            int originalValue = mat.GetInt(propertyName);

            while (progress < 1)
            {
                mat.SetInt(propertyName, Mathf.RoundToInt(Mathf.Lerp(originalValue, propertyFloat, progress)));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private IEnumerator ModifyMaterialColourOverTime()
        {
            Material mat = targetMaterial;

            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            Color originalValue = mat.GetColor(propertyName);

            while (progress < 1)
            {
                mat.SetColor(propertyName, Color.Lerp(originalValue, propertyColour, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

        private IEnumerator ModifyMaterialVector4OverTime()
        {
            Material mat = targetMaterial;

            float smoothness = 0.02f;
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            Vector4 originalValue = mat.GetVector(propertyName);

            while (progress < 1)
            {
                mat.SetVector(propertyName, Vector4.Lerp(originalValue, propertyVector4, progress));

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }

#endregion

        public override void Validation()
        {
            if (targetMaterial)
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    switch (propertyType)
                    {
                        case PropertyType.Texture:
                            if (propertyTexture == null)
                            {
                                ShowWarningMessage("You need to enter a texture to set the property with!");
                            }
                            break;
                    }
                }
                else
                {
                    ShowWarningMessage("You need to enter the name of the property to want to set!");
                }
            }
        }
    }
}
