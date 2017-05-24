using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to trigger an animation, stop animations or play an animation clip
    /// </summary>
    [AddComponentMenu("")]
    public class AnimationResponse : ResponseComponent
    {
        /// <summary>
        /// The gameobject to apply the animation to
        /// </summary>
        public GameObject animationTarget;

        /// <summary>
        /// The target game object's name
        /// </summary>
        public string targetGameObjectName;

        /// <summary>
        /// The name of the trigger on the gameobject animator that you want to trigger.
        /// </summary>
        public string setMecanimTrigger;

        /// <summary>
        /// Stops the current animation on the animation target.
        /// </summary>
        public bool stopAnim;

        /// <summary>
        /// The animation clip to play.
        /// </summary>
        public AnimationClip animationClip;

        /// <summary>
        /// This is how you will provide the response access to a specific gameobject. You can either use a reference, name or use the gameobject that collides with this trigger box.
        /// </summary>
        public ReferenceType referenceType = ReferenceType.GameObjectReference;

        public override bool requiresCollisionObjectData
        {
            get
            {
                return true;
            }
        }

        public enum ReferenceType
        {
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
                    animationTarget = (GameObject)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Animation Target",
                    "The gameobject to apply the animation to."), animationTarget, typeof(GameObject), true);
                    break;

                case ReferenceType.GameObjectName:
                    targetGameObjectName = UnityEditor.EditorGUILayout.TextField(new GUIContent("Animation Target Name",
                    "If you cannot get a reference for a gameobject you can enter it's name here and it will be found (GameObject.Find())."), targetGameObjectName);
                    break;
            }

            setMecanimTrigger = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Mecanim Trigger",
                "The name of the trigger on the gameobject animator that you want to trigger."), setMecanimTrigger);

            stopAnim = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Stop Animation",
                "Stops the current animation on the animation target."), stopAnim);

            animationClip = (AnimationClip)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Play Animation Clip",
                "Fades the animation in on the animation target over 0.3 seconds and fades other animations out."),
                animationClip, typeof(AnimationClip), true);
        }
#endif

        public override void Validation()
        {
            // If there is a mecanim trigger check there is a target for it
            if (!string.IsNullOrEmpty((setMecanimTrigger)))
            {
                if (animationTarget == null)
                {
                    ShowWarningMessage("You have set a Mecanim Trigger as an Animation Response but haven't set an Animation Target to apply it to!");
                }
            }

            // If stop anim is set check there is a target for it
            if (stopAnim)
            {
                if (animationTarget == null)
                {
                    ShowWarningMessage("You have set Stop Animation as an Animation Response but haven't set an Animation Target to apply it to!");
                }
            }

            // If legacy animat is set to play check there is a target for it
            if (animationClip != null)
            {
                if (animationTarget == null)
                {
                    ShowWarningMessage("You have chosen to play a legacy animation as an Animation Response but haven't set an Animation Target to apply it to!");
                }
            }
        }

        public override bool ExecuteAction(GameObject collisionGameObject)
        {
            switch (referenceType)
            {
                case ReferenceType.CollisionGameObject:
                    animationTarget = collisionGameObject;
                    break;

                case ReferenceType.GameObjectName:
                    animationTarget = GameObject.Find(targetGameObjectName);
                    break;
            }

            if (stopAnim && animationTarget)
            {
#if UNITY_5_4 || UNITY_5_5
                animationTarget.GetComponent<Animator>().Stop();
#else
                animationTarget.GetComponent<Animator>().StopPlayback();
#endif
            }

            if (animationClip && animationTarget)
            {
                // Plays an animation clip on the target animation over 0.3 seconds and fades other animations out
                animationTarget.GetComponent<Animation>().CrossFade(animationClip.name, 0.3f, PlayMode.StopAll);
            }

            if (!string.IsNullOrEmpty(setMecanimTrigger) && animationTarget)
            {
                animationTarget.GetComponent<Animator>().SetTrigger(setMecanimTrigger);
            }

            return true;
        }
    }
}
