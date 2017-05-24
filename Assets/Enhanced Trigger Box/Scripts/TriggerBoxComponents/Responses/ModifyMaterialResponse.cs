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
        /// The target game object's name
        /// </summary>
        public string targetGameObjectName;

        /// <summary>
        /// This is the material that will be set to the target gameobject.
        /// </summary>
        public Material material;

        /// <summary>
        /// This lets you set who this change applies to. Self is the gameobject. Children is all the children of the gameobject. Parents is all the parents of the gameobject.
        /// NOTE: The reason this is called affectOthers and not affectOptions is to prevent breaking other peoples saved/seralised data.
        /// </summary>
        public AffectOthers affectOthers;

        /// <summary>
        /// This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.
        /// </summary>
        public GameobjectTargetType gameObjectTargetType = GameobjectTargetType.GameObjectReference;

        /// <summary>
        /// Available options for affecting others gameobjects.
        /// </summary>
        public enum AffectOthers
        {
            Self,
            SelfAndChildren,
            SelfAndParents,
            SelfChildrenAndParents,
            Children,
            Parents,
            ChildrenAndParents,
        }

        /// <summary>
        /// Type of gameobject
        /// </summary>
        public enum GameobjectTargetType
        {
            GameObjectReference,
            GameObjectName,
            CollisionGameObject,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            gameObjectTargetType = (GameobjectTargetType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Gameobject Type",
                   "This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box."), gameObjectTargetType);

            switch (gameObjectTargetType)
            {
                case GameobjectTargetType.GameObjectReference:
                    targetGameObject = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Target GameObject",
                    "The target game object which will have it's material modified."), targetGameObject, typeof(GameObject), true);
                    break;

                case GameobjectTargetType.GameObjectName:
                    targetGameObjectName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Target Gameobject Name",
                    "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())."), targetGameObjectName);
                    break;
            }
            
            material = (Material)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Set Material",
                "This is the material that will be set to the target gameobject."), material, typeof(Material), true);

            affectOthers = (AffectOthers)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Affect Options",
                "This lets you set who this change applies to. Self is the gameobject. Children is all the children of the gameobject. Parents is all the parents of the gameobject."), affectOthers);
        }
#endif

        public override bool ExecuteAction(GameObject collisionGameObject)
        {
            switch (gameObjectTargetType)
            {
                case GameobjectTargetType.CollisionGameObject:
                    targetGameObject = collisionGameObject;
                    break;

                case GameobjectTargetType.GameObjectName:
                    targetGameObject = GameObject.Find(targetGameObjectName);
                    break;
            }

            if (targetGameObject == null || material == null)
            {
                Debug.Log("Unable to execute Modify Material Response. Missing gameobject or material reference!");
                return true;
            }

            // What a magnificent switch statement...
            switch (affectOthers)
            {
                case AffectOthers.Self:
                    AffectSelf();
                    break;

                case AffectOthers.Children:
                    AffectChildren();
                    break;

                case AffectOthers.Parents:
                    AffectParents();
                    break;

                case AffectOthers.ChildrenAndParents:
                    AffectChildren();
                    AffectParents();
                    break;

                case AffectOthers.SelfAndChildren:
                    AffectSelf();
                    AffectChildren();
                    break;

                case AffectOthers.SelfAndParents:
                    AffectSelf();
                    AffectParents();
                    break;

                case AffectOthers.SelfChildrenAndParents:
                    AffectSelf();
                    AffectChildren();
                    AffectParents();
                    break;
            }

            return true;
        }

        private void AffectSelf()
        {
            targetGameObject.GetComponent<MeshRenderer>().material = material;
        }

        private void AffectChildren()
        {
            MeshRenderer[] childMeshRenderers = targetGameObject.GetComponentsInChildren<MeshRenderer>();

            for (int i = 0; i < childMeshRenderers.Length; i++)
            {
                childMeshRenderers[i].material = material;
            }
        }

        private void AffectParents()
        {
            MeshRenderer[] parentMeshRenderers = targetGameObject.GetComponentsInParent<MeshRenderer>();

            for (int i = 0; i < parentMeshRenderers.Length; i++)
            {
                parentMeshRenderers[i].material = material;
            }
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

        public override void OnAwake()
        {
            base.OnAwake();
            requiresCollisionObjectData = true;
        }
    }
}
