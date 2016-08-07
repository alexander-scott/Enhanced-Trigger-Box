using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Serializable]
public class CameraCondition : EnhancedTriggerBoxComponent
{
    public bool showCameraConditions = false;

    public LookType cameraConditionType;

    public GameObject conditionObject;

    public CameraConditionComponentParameters componentParameter;

    public bool ignoreObstacles;

    public float conditionTime = 0f;

    private Vector3 viewConditionScreenPoint = new Vector3();

    private Vector3 viewConditionDirection = new Vector3();

    private RaycastHit viewConditionRaycastHit = new RaycastHit();

    private BoxCollider viewConditionObjectCollider;

    private MeshRenderer viewConditionObjectMeshRenderer;

    private Plane[] viewConditionCameraPlane;

    private float viewTimer = 0f;

    public enum LookType
    {
        None,
        LookingAt,
        LookingAway,
    }

    public enum CameraConditionComponentParameters
    {
        Transform,
        FullBoxCollider,
        MinimumBoxCollider,
        MeshRenderer,
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        showCameraConditions = RenderHeader("Camera Conditions", showCameraConditions, true);

        if (showCameraConditions)
        {
            EditorGUI.indentLevel = 2;

            cameraConditionType = (LookType)EditorGUILayout.EnumPopup("Camera Condition Type", cameraConditionType);

            if (cameraConditionType != LookType.None)
            {
                conditionObject = (GameObject)EditorGUILayout.ObjectField("Condition Object", conditionObject, typeof(GameObject), true);
                componentParameter = (CameraConditionComponentParameters)EditorGUILayout.EnumPopup("Component Parameter", componentParameter);

                if (cameraConditionType == LookType.LookingAt)
                {
                    ignoreObstacles = EditorGUILayout.Toggle("Ignore Obstacles", ignoreObstacles);
                }

                conditionTime = EditorGUILayout.FloatField("Condition Time", conditionTime);
            }
        }
    }
}