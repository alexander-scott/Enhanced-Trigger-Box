using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to set a new material on a gameobject
    /// </summary>
    [AddComponentMenu("")]
    public class ModifyMaterialResponse : ResponseComponent
    {
        /// <summary>
        /// The target game object which will have it's material modified.
        /// </summary>
        public GameObject targetGameObject;

        /// <summary>
        /// This is the material that will be set to the target gameobject.
        /// </summary>
        public Material material;

        /// <summary>
        /// This allows you to apply the selected change to adjacent gameobjects, either children or parents.
        /// </summary>
        public AffectOthers affectOthers;

        /// <summary>
        /// Available options for affecting others gameobjects.
        /// </summary>
        public enum AffectOthers
        {
            None,
            Children,
            Parents,
            ChildrenAndParents,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            targetGameObject = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target GameObject",
                "The target game object which will have it's material modified."), targetGameObject, typeof(GameObject), true);

            material = (Material)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Set Material",
                "This is the material that will be set to the target gameobject."), material, typeof(Material), true);

            affectOthers = (AffectOthers)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Affect Others",
                "This allows you to apply the selected change to adjacent gameobjects, either children or parents."), affectOthers);
        }
#endif

        public override bool ExecuteAction()
        {
            // Sets the material to the target gameobject
            targetGameObject.GetComponent<MeshRenderer>().material = material;

            switch (affectOthers)
            {
                case AffectOthers.Children: // Sets the material to the gameobjects children
                    MeshRenderer[] childMeshRenderers = targetGameObject.GetComponentsInChildren<MeshRenderer>();

                    for (int i = 0; i < childMeshRenderers.Length; i++)
                    {
                        childMeshRenderers[i].material = material;
                    }
                    break;

                case AffectOthers.Parents: // Sets the material to the gameobjects parents
                    MeshRenderer[] parentMeshRenderers = targetGameObject.GetComponentsInParent<MeshRenderer>();

                    for (int i = 0; i < parentMeshRenderers.Length; i++)
                    {
                        parentMeshRenderers[i].material = material;
                    }
                    break;

                case AffectOthers.ChildrenAndParents: // Sets the material to the gameobjects children and parents
                    MeshRenderer[] parentMeshRenderers1 = targetGameObject.GetComponentsInParent<MeshRenderer>();

                    for (int i = 0; i < parentMeshRenderers1.Length; i++)
                    {
                        parentMeshRenderers1[i].material = material;
                    }

                    MeshRenderer[] childMeshRenderers1 = targetGameObject.GetComponentsInChildren<MeshRenderer>();

                    for (int i = 0; i < childMeshRenderers1.Length; i++)
                    {
                        childMeshRenderers1[i].material = material;
                    }
                    break;
            }

            return true;
        }

        public override void Validation()
        {
            if (!targetGameObject && material)
            {
                ShowWarningMessage("You need to add a reference to a target gameobject for the modify material response to work.");
            }

            if (!material && targetGameObject)
            {
                ShowWarningMessage("You need to add a reference to a material for the modify material response to work.");
            }
        }
    }
}
