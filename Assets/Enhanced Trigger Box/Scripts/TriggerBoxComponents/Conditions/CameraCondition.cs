using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraCondition : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// The type of condition you want. The Looking At condition only passes when the user can see a specific transform or gameobject. The Looking Away condition only passes when a transform or gameobject is out of the users camera frustum.
    /// </summary>
    public LookType cameraConditionType;

    /// <summary>
    /// This is the object that the condition is based upon.
    /// </summary>
    public GameObject conditionObject;

    /// <summary>
    /// The type of component the condition will be checked against.  Either transform (a single point in space), minimum box collider (any part of a box collider), full box collider (the entire box collider) or mesh renderer (any part of a mesh). For example with the Looking At condition and Minimum Box Collider, if any part of the box collider were to enter the camera's view, the condition would be met.
    /// </summary>
    public CameraConditionComponentParameters componentParameter;

    /// <summary>
    /// If this is true, when checking if the user is looking at an object no raycast checks will be performed to check if there is something preventing the line of sight. This means that as long as the objects position is within the camera frustum the condition will pass.
    /// </summary>
    public bool ignoreObstacles;

    /// <summary>
    /// This is the time that this camera condition must be met for in seconds. E.g. camera must be looking at object for 2 seconds for the condition to pass.
    /// </summary>
    public float conditionTime = 0f;

    /// <summary>
    /// The world to viewport point of the object the viewObject
    /// </summary>
    private Vector3 viewConditionScreenPoint = new Vector3();

    /// <summary>
    /// The direction from the main camera to the viewObject
    /// </summary>
    private Vector3 viewConditionDirection = new Vector3();

    /// <summary>
    /// Holds raycast information when raycast checks are taking place. Only used when using the Looking At condition
    /// </summary>
    private RaycastHit viewConditionRaycastHit = new RaycastHit();

    /// <summary>
    /// This is the box collider of the viewObject. Only used when the condition involves Minimum Box Collider or Full Box Collider
    /// </summary>
    private BoxCollider viewConditionObjectCollider;

    /// <summary>
    /// This is the mesh renderer of the viewobject. Only used when the condition involves mesh renderer.
    /// </summary>
    private MeshRenderer viewConditionObjectMeshRenderer;

    /// <summary>
    /// The view planes of the camera
    /// </summary>
    private Plane[] viewConditionCameraPlane;

    /// <summary>
    /// This is a timer used to make sure the time the condition has been met for is longer than conditionTime
    /// </summary>
    private float viewTimer = 0f;

    /// <summary>
    /// The available types of camera conditions.
    /// </summary>
    public enum LookType
    {
        None,
        LookingAt,
        LookingAway,
    }

    /// <summary>
    /// The available component parameters that can be used with the camera condition.
    /// </summary>
    public enum CameraConditionComponentParameters
    {
        Transform,
        FullBoxCollider,
        MinimumBoxCollider,
        MeshRenderer,
    }

    public override void DrawInspectorGUI()
    {
        cameraConditionType = (LookType)EditorGUILayout.EnumPopup(new GUIContent("Condition Type",
            "The type of condition you want. The Looking At condition only passes when the user can see a specific transform or gameobject. The Looking Away condition only passes when a transform or gameobject is out of the users camera frustum."), cameraConditionType);

        if (cameraConditionType != LookType.None)
        {
            conditionObject = (GameObject)EditorGUILayout.ObjectField(new GUIContent("Condition Object",
                "This is the object that the condition is based upon."), conditionObject, typeof(GameObject), true);

            componentParameter = (CameraConditionComponentParameters)EditorGUILayout.EnumPopup(new GUIContent("Component Parameter",
                "The type of component the condition will be checked against.  Either transform (a single point in space), minimum box collider (any part of a box collider), full box collider (the entire box collider) or mesh renderer (any part of a mesh). For example with the Looking At condition and Minimum Box Collider, if any part of the box collider were to enter the camera's view, the condition would be met."), componentParameter);

            if (cameraConditionType == LookType.LookingAt)
            {
                ignoreObstacles = EditorGUILayout.Toggle(new GUIContent("Ignore Obstacles",
                    "If this is true, when checking if the user is looking at an object no raycast checks will be performed to check if there is something preventing the line of sight. This means that as long as the objects position is within the camera frustum the condition will pass."), ignoreObstacles);
            }

            conditionTime = EditorGUILayout.FloatField(new GUIContent("Condition Time",
                "This is the time that this camera condition must be met for in seconds. E.g. camera must be looking at object for 2 seconds for the condition to pass."), conditionTime);
        }
    }

    public override void Validation()
    {
        // If there's a camera condition
        if (cameraConditionType != LookType.None)
        {
            // Check if the user specified a gameobject to focus on
            if (conditionObject == null)
            {
                ShowErrorMessage("You have selected the " + ((cameraConditionType == LookType.LookingAt) ? "Looking At" : "Looking Away") + " camera condition but have not specified a gameobject reference!");
            }
            else
            {
                // If the user has selected full box collider check the object has a box collider
                if (componentParameter == CameraConditionComponentParameters.FullBoxCollider || componentParameter == CameraConditionComponentParameters.MinimumBoxCollider)
                {
                    if (conditionObject.GetComponent<BoxCollider>() == null)
                    {
                        ShowErrorMessage("You have selected the Component Parameter for the camera condition to be " + ((componentParameter == CameraConditionComponentParameters.FullBoxCollider) ? "Full Box Collider" : "Minimum Box Collider") + " but the object doesn't have a Box Collider component!");
                    }
                } // Else if the user selected mesh render check the object has mesh renderer
                else if (componentParameter == CameraConditionComponentParameters.MeshRenderer)
                {
                    if (conditionObject.GetComponent<MeshRenderer>() == null)
                    {
                        ShowErrorMessage("You have selected the Component Parameter for the camera condition to be Mesh Renderer but the object doesn't have a Mesh Renderer component!");
                    }
                }
            }

            // Check that condition time is above 0
            if (conditionTime < 0f)
            {
                ShowErrorMessage("You have set the camera condition timer to be less than 0 which isn't possible!");
            }
        }
    }

    /// <summary>
    /// Cache the collider or mesh renderer so we do not do GetComponent every frame
    /// </summary>
    public override void OnAwake()
    {
        if (conditionObject)
        {
            if (componentParameter == CameraConditionComponentParameters.FullBoxCollider || componentParameter == CameraConditionComponentParameters.MinimumBoxCollider)
            {
                if (conditionObject.GetComponent<BoxCollider>() != null)
                {
                    viewConditionObjectCollider = conditionObject.GetComponent<BoxCollider>();
                }
            }
            else if (componentParameter != CameraConditionComponentParameters.MeshRenderer) // Cache the mesh renderer
            {
                if (conditionObject.GetComponent<MeshRenderer>() == null)
                {
                    viewConditionObjectMeshRenderer = conditionObject.GetComponent<MeshRenderer>();
                }
            }
        }
    }

    public override bool ExecuteAction()
    {
        // This fixes a bug that occured when the player was very close to an object. Is this necessary? TODO: Find out if this is necessary
        if (Vector3.Distance(Camera.main.transform.position, conditionObject.transform.position) < 2f)
        {
            return false;
        }

        switch (cameraConditionType)
        {
            case LookType.LookingAt:
                switch (componentParameter)
                {
                    case CameraConditionComponentParameters.Transform:
                        // Get the viewport point of the object from its position
                        viewConditionScreenPoint = Camera.main.WorldToViewportPoint(conditionObject.transform.position);

                        // This checks that the objects position is within the camera frustum
                        if (viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                            viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1)
                        {
                            // If ignore obstacles is true we don't need to raycast to determine if there's anything blocking
                            // the line of sight
                            if (ignoreObstacles)
                            {
                                return CheckConditionTimer();
                            }

                            // Get the direction vector from the object to the camera
                            viewConditionDirection = (conditionObject.transform.position - Camera.main.transform.position);

                            // Check if there's any objects in the way
                            if (CheckRaycast())
                            {
                                // Check if we this condition has been met for longer than the conditionTimer
                                return CheckConditionTimer();
                            }
                        }
                        break;

                    case CameraConditionComponentParameters.MinimumBoxCollider:
                        // Get the camera's view planes
                        viewConditionCameraPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main);

                        // This test determines wether the bounds are within the planes. What happens if the bounds are larger and
                        // encapsulate the planes? TODO: Test up close with an object.
                        if (GeometryUtility.TestPlanesAABB(viewConditionCameraPlane, viewConditionObjectCollider.bounds))
                        {
                            if (ignoreObstacles)
                            {
                                return CheckConditionTimer();
                            }

                            viewConditionDirection = (conditionObject.transform.position - Camera.main.transform.position);

                            if (CheckRaycast())
                            {
                                return CheckConditionTimer();
                            }
                        }
                        break;

                    case CameraConditionComponentParameters.FullBoxCollider:
                        viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.min);

                        // Check that the min bound position is in the camera frustum
                        if (viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                            viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1)
                        {
                            viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.max);

                            // Check the max bound position is in the camera frustum
                            if (viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                            viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1)
                            {
                                if (ignoreObstacles)
                                {
                                    return CheckConditionTimer();
                                }

                                viewConditionDirection = (conditionObject.transform.position - Camera.main.transform.position);

                                if (CheckRaycast())
                                {
                                    return CheckConditionTimer();
                                }
                            }
                        }
                        break;

                    case CameraConditionComponentParameters.MeshRenderer:
                        // This is much simpler. Uses the built in isVisible checks to determine if the mesh can be seen by any camera.
                        if (viewConditionObjectMeshRenderer.isVisible)
                        {
                            return true;
                        }
                        break;
                }
                break;

            case LookType.LookingAway:
                switch (componentParameter)
                {
                    case CameraConditionComponentParameters.Transform:
                        viewConditionScreenPoint = Camera.main.WorldToViewportPoint(conditionObject.transform.position);

                        if (!(viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                            viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1))
                        {
                            return CheckConditionTimer();
                        }
                        break;

                    case CameraConditionComponentParameters.MinimumBoxCollider:
                        viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.min);

                        if (!(viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                            viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1))
                        {
                            return CheckConditionTimer();
                        }
                        else
                        {
                            viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.max);

                            if (!(viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                            viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1))
                            {
                                return CheckConditionTimer();
                            }
                        }
                        break;

                    case CameraConditionComponentParameters.FullBoxCollider:
                        viewConditionCameraPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main);

                        if (!GeometryUtility.TestPlanesAABB(viewConditionCameraPlane, viewConditionObjectCollider.bounds))
                        {
                            viewConditionScreenPoint = Camera.main.WorldToViewportPoint(conditionObject.transform.position);

                            if (!(viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                                viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1))
                            {
                                return CheckConditionTimer();
                            }
                        }
                        break;

                    case CameraConditionComponentParameters.MeshRenderer:
                        if (!viewConditionObjectMeshRenderer.isVisible)
                        {
                            return true;
                        }
                        break;
                }
                break;
        }

        return false;
    }

    /// <summary>
    /// This function fires a raycast from the camera to the camera condition object to determine if there's any obstacles blocking the camera line of sight.
    /// </summary>
    /// <returns>Returns true or false depending if theres any obstacles in the way.</returns>
    private bool CheckRaycast()
    {
        // Fire raycast from camera in the direction of the viewobject
        if (Physics.Raycast(Camera.main.transform.position, viewConditionDirection.normalized, out viewConditionRaycastHit, viewConditionDirection.magnitude))
        {
            // If it hit something, inspect the returned data to set if it is the object we are checking that the camera can see
            if (viewConditionRaycastHit.transform == conditionObject.transform)
            {
                return true;
            }

            // If it hit something which wasn't the correct object then something is in the way and we return false
            return false;
        }

        // The raycast hit nothing at all so there's nothing in the way so return true
        return true;
    }

    /// <summary>
    /// This function checks to make sure the condition has been met for a certain amount of time.
    /// </summary>
    /// <returns>Returns true or false depending on if the condition has been met for a certain about of time.</returns>
    private bool CheckConditionTimer()
    {
        if (viewTimer >= conditionTime)
        {
            return true;
        }
        else
        {
            viewTimer += Time.fixedDeltaTime;
        }

        return false;
    }
}