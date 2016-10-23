using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to alter that properties of a rigidbody on a gameobject
    /// </summary>
    [AddComponentMenu("")]
    public class ModifyRigidbodyResponse : ResponseComponent
    {
        /// <summary>
        /// The rigidbody that will be changed
        /// </summary>
        public Rigidbody rigbody;

        /// <summary>
        /// Set the mass of this rigidbody. If left blank the value will not be changed.
        /// </summary>
        public string setMass;

        /// <summary>
        /// Set the drag of this rigidbody. If left blank the value will not be changed.
        /// </summary>
        public string setDrag;

        /// <summary>
        /// Set the angular drag of this rigidbody. If left blank the value will not be changed.
        /// </summary>
        public string setAngularDrag;

        /// <summary>
        /// Choose to set whether this rigidbody should use gravity. Remain the same will not change the value and toggle will invert the value.
        /// </summary>
        public ChangeBool changeGravity;

        /// <summary>
        /// Choose to set whether this rigidbody is kinematic. Remain the same will not change the value and toggle will invert the value.
        /// </summary>
        public ChangeBool changeKinematic;

        /// <summary>
        /// Choose to set this rigidbody to interpolate or extrapolate. Remain the same will not change the value.
        /// </summary>
        public ChangeInterpolate changeInterpolate;

        /// <summary>
        /// Choose to set this rigidbody's collision detection between discrete or continuous. Remain the same will not change the value.
        /// </summary>
        public ChangeCollisionDetection changeCollisionDetection;

        /// <summary>
        /// Allows you to freeze position or rotation. If you want these to be unaffected uncheck this box.
        /// </summary>
        public bool editConstraints;

        /// <summary>
        /// Freeze movement in x axis
        /// </summary>
        public bool xPos;

        /// <summary>
        /// Freeze movement in y axis
        /// </summary>
        public bool yPos;

        /// <summary>
        /// Freeze movement in z axis
        /// </summary>
        public bool zPos;

        /// <summary>
        /// Freeze rotation in x axis
        /// </summary>
        public bool xRot;

        /// <summary>
        /// Freeze rotation in y axis
        /// </summary>
        public bool yRot;

        /// <summary>
        /// Freeze rotation in z axis
        /// </summary>
        public bool zRot;

        /// <summary>
        /// Enum containing available options for changing bools such as set true and set false or toggle.
        /// </summary>
        public enum ChangeBool
        {
            RemainTheSame,
            SetTrue,
            SetFalse,
            Toggle,
        }

        /// <summary>
        /// Enum contanining available options for rigidbody interpolation
        /// </summary>
        public enum ChangeInterpolate
        {
            RemainTheSame,
            SetInterpolate,
            SetExtrapolate,
        }

        /// <summary>
        /// Enum containing available options for rigidbody collision detection.
        /// </summary>
        public enum ChangeCollisionDetection
        {
            RemainTheSame,
            SetContinuous,
            SetContinuousDynamic,
            SetDiscrete,
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            rigbody = (Rigidbody)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Rigidbody",
                    "The rigidbody that will be changed"), rigbody, typeof(Rigidbody), true);

            setMass = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Mass",
                "Set the mass of this rigidbody. If left blank the value will not be changed."), setMass);

            setDrag = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Drag",
                "Set the drag of this rigidbody. If left blank the value will not be changed."), setDrag);

            setAngularDrag = UnityEditor.EditorGUILayout.TextField(new GUIContent("Set Angular Drag",
                "Set the angular drag of this rigidbody. If left blank the value will not be changed."), setAngularDrag);

            changeGravity = (ChangeBool)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Change Gravity",
                "Choose to set whether this rigidbody should use gravity. Remain the same will not change the value and toggle will invert the value."), changeGravity);

            changeKinematic = (ChangeBool)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Change Kinematic",
                "Choose to set whether this rigidbody should be kinematic. Remain the same will not change the value and toggle will invert the value."), changeKinematic);

            changeInterpolate = (ChangeInterpolate)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Change Interpolate",
                "Choose to set this rigidbody to interpolate or extrapolate. Remain the same will not change the value."), changeInterpolate);

            changeCollisionDetection = (ChangeCollisionDetection)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Change Collision Detection",
                "Choose to set this rigidbody's collision detection between discrete or continuous. Remain the same will not change the value."), changeCollisionDetection);

            editConstraints = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Edit Constraints", "Allows you to freeze position or rotation. If you want these to be unaffected uncheck this box."), editConstraints);

            if (editConstraints)
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();

                float prevWidth = UnityEditor.EditorGUIUtility.labelWidth;

                UnityEditor.EditorGUILayout.LabelField(new GUIContent("Freeze Position", "Allows you to freeze movement in a specific axis."));

                UnityEditor.EditorGUIUtility.labelWidth = 30f;

                xPos = UnityEditor.EditorGUILayout.Toggle("X", xPos);
                yPos = UnityEditor.EditorGUILayout.Toggle("Y", yPos);
                zPos = UnityEditor.EditorGUILayout.Toggle("Z", zPos);

                UnityEditor.EditorGUIUtility.labelWidth = prevWidth;

                UnityEditor.EditorGUILayout.EndHorizontal();

                UnityEditor.EditorGUILayout.BeginHorizontal();

                UnityEditor.EditorGUILayout.LabelField(new GUIContent("Freeze Rotation", "Allows you to freeze rotation in a specific axis."));

                UnityEditor.EditorGUIUtility.labelWidth = 30f;

                xRot = UnityEditor.EditorGUILayout.Toggle("X", xRot);
                yRot = UnityEditor.EditorGUILayout.Toggle("Y", yRot);
                zRot = UnityEditor.EditorGUILayout.Toggle("Z", zRot);

                UnityEditor.EditorGUIUtility.labelWidth = prevWidth;

                UnityEditor.EditorGUILayout.EndHorizontal();
            }
        }
#endif

        public override bool ExecuteAction()
        {
            // If the user has entered a mass, attempt to set the objects mass
            if (!string.IsNullOrEmpty(setMass))
            {
                int mass;
                if (int.TryParse(setMass, out mass))
                {
                    rigbody.mass = mass;
                }
                else
                {
                    Debug.Log("Unable to parse setMass, " + setMass + ", to an integer.");
                }
            }

            if (!string.IsNullOrEmpty(setDrag))
            {
                int drag;
                if (int.TryParse(setDrag, out drag))
                {
                    rigbody.drag = drag;
                }
                else
                {
                    Debug.Log("Unable to parse setDrag, " + setMass + ", to an integer.");
                }
            }

            if (!string.IsNullOrEmpty(setAngularDrag))
            {
                float aDrag;
                if (float.TryParse(setAngularDrag, out aDrag))
                {
                    rigbody.angularDrag = aDrag;
                }
                else
                {
                    Debug.Log("Unable to parse setAngularDrag, " + setAngularDrag + ", to a float.");
                }
            }

            switch (changeGravity)
            {
                case ChangeBool.SetFalse:
                    rigbody.useGravity = false;
                    break;

                case ChangeBool.SetTrue:
                    rigbody.useGravity = true;
                    break;

                case ChangeBool.Toggle:
                    rigbody.useGravity = !rigbody.useGravity;
                    break;
            }

            switch (changeKinematic)
            {
                case ChangeBool.SetFalse:
                    rigbody.isKinematic = false;
                    break;

                case ChangeBool.SetTrue:
                    rigbody.isKinematic = true;
                    break;

                case ChangeBool.Toggle:
                    rigbody.isKinematic = !rigbody.isKinematic;
                    break;
            }

            switch (changeInterpolate)
            {
                case ChangeInterpolate.SetExtrapolate:
                    rigbody.interpolation = RigidbodyInterpolation.Extrapolate;
                    break;

                case ChangeInterpolate.SetInterpolate:
                    rigbody.interpolation = RigidbodyInterpolation.Interpolate;
                    break;
            }

            switch (changeCollisionDetection)
            {
                case ChangeCollisionDetection.SetContinuous:
                    rigbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                    break;

                case ChangeCollisionDetection.SetDiscrete:
                    rigbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
                    break;

                case ChangeCollisionDetection.SetContinuousDynamic:
                    rigbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
                    break;
            }

            if (editConstraints)
            {
                rigbody.constraints = RigidbodyConstraints.FreezeAll;

                if (!xPos)
                {
                    rigbody.constraints &= ~RigidbodyConstraints.FreezePositionX;
                }
                if (!yPos)
                {
                    rigbody.constraints &= ~RigidbodyConstraints.FreezePositionY;
                }
                if (!zPos)
                {
                    rigbody.constraints &= ~RigidbodyConstraints.FreezePositionZ;
                }
                if (!xRot)
                {
                    rigbody.constraints &= ~RigidbodyConstraints.FreezeRotationX;
                }
                if (!yRot)
                {
                    rigbody.constraints &= ~RigidbodyConstraints.FreezeRotationY;
                }
                if (!zRot)
                {
                    rigbody.constraints &= ~RigidbodyConstraints.FreezeRotationZ;
                }
            }
            
            return true;
        }

        public override void Validation()
        {
            if (!rigbody)
            {
                ShowWarningMessage("For the rigidbody response you need to add a rigidbody reference!");
            }

            if (!string.IsNullOrEmpty(setMass))
            {
                int temp;
                if (!int.TryParse(setMass, out temp))
                {
                    ShowWarningMessage("You have entered a mass but it couldn't be parsed. Please make sure it is a valid integer.");
                }
            }

            if (!string.IsNullOrEmpty(setDrag))
            {
                int temp2;
                if (!int.TryParse(setDrag, out temp2))
                {
                    ShowWarningMessage("You have entered a drag but it couldn't be parsed. Please make sure it is a valid integer.");
                }
            }

            if (!string.IsNullOrEmpty(setAngularDrag))
            {
                float temp3;
                if (!float.TryParse(setAngularDrag, out temp3))
                {
                    ShowWarningMessage("You have entered a angular drag but it couldn't be parsed. Please make sure it is a valid float.");
                }
            }
        }
    }
}
