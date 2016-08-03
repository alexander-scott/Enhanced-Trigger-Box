using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Perform an action when a player walks into a box
/// </summary>
[ExecuteInEditMode]
public class TriggerBox : MonoBehaviour
{
    #region Variables

    #region Options

    /// <summary>
    /// This bool is used to store whether the base options tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showBaseOptions = true;

    /// <summary>
    /// A list of tags belonging to gameobjects which are able to trigger the trigger box
    /// </summary>
    [Tooltip("Only tags listed here are able to trigger the trigger box. By default the Player tag is used here.")]
    public List<string> triggerTags;

    /// <summary>
    /// If true the application will write to the console a message with the name of the trigger that was triggered
    /// </summary>
    [Tooltip("If true, the script will write to the console when certain events happen such as when the trigger box is triggered.")]
    public bool debugTriggerBox;

    /// <summary>
    /// If true then only a wireframe will be displayed instead of a coloured box
    /// </summary>
    [Tooltip("If true, the trigger box will no longer have a fill colour in the editor and only the edges will be visible.")]
    public bool drawWire;

    /// <summary>
    /// The colour of the trigger box
    /// </summary>
    [Tooltip("This is the colour the trigger box and it's edges will have in the editor.")]
    public Color triggerboxColour;

    /// <summary>
    /// A set of options for when the trigger box has been trigged. Nothing does nothing. Trigger box destroys trigger box. Parent destroys parent.
    /// </summary>
    [Tooltip("This allows you to choose what happens to this gameobject after the trigger box has been triggered. Set Inactive will set this gameobject as inactive. Destroy trigger box will destroy this gameobject. Destroy parent will destroy this gameobject's parent.")]
    public AfterTriggerOptions afterTrigger;

    /// <summary>
    /// An Enum allowing users to choose whether the trigger box follows the main camera or a selected transform
    /// </summary>
    [Tooltip("This allows you to choose if you want your trigger box to stay positioned on a moving transform or the main camera. If you pick Follow Transform a field will appear to set which transform you want the trigger box to follow. Or if you pick Follow Main Camera the trigger box will stay positioned on wherever the main camera currently is.")]
    public TriggerFollow triggerFollow;

    /// <summary>
    /// This transform is used when trigger follow is set to transform. The trigger boxes position will be set to this transforms position every frame.
    /// </summary>
    [Tooltip("This is used when Trigger Follow is set to Follow Transform. The trigger box will stay positioned on wherever this transform is currently positioned.")]
    public Transform followTransform;

    /// <summary>
    /// This is used when a transform reference for followTransform is unavailable. An object with this named is searched for and used as the followTransform object
    /// </summary>
    [Tooltip("This is can be used if you cannot get a reference for the above Follow Transform transform. GameObject.Find() will be used to find the gameobject and transform with the name you enter.")]
    public string followTransformName;

    /// <summary>
    /// If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.
    /// </summary>
    [Tooltip("If this is true then the condition checks will continue taking place if the user leaves the trigger box area. If this is false then if the user leaves the trigger box and all conditions haven't been met then it will stop doing condition checks.")]
    public bool canWander;

    /// <summary>
    /// This is set to true when the trigger box is triggered. Once this is true we can start checking if the conditions have been met.
    /// </summary>
    private bool triggered = false;

    /// <summary>
    /// This is set to true when all the conditions have been met.
    /// </summary>
    private bool conditionMet = false;
    #endregion 

    #region Camera Conditions

    /// <summary>
    /// This bool is used to store whether the camera conditions tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showCameraConditions = false;

    /// <summary>
    /// The type of condition the user wants, either Looking At an object or Looking Away from an object.
    /// </summary>
    [Tooltip("The type of condition you want. The Looking At condition only passes when the user can see a specific transform or gameobject. The Looking Away condition only passes when a transform or gameobject is out of the users camera frustum.")]
    public LookType cameraConditionType;

    /// <summary>
    /// A reference for the object that the condition is based upon
    /// </summary>
    [Tooltip("This is the object that the condition is based upon.")]
    public GameObject conditionObject;

    /// <summary>
    /// The type of condition the object will be checked with, either transform (a vector3 in frame), minimum box collider (any part of a box collider can be in frame) or full box collider (the whole box collider must be in frame).
    /// </summary>
    [Tooltip("The type of component the condition will be checked against.  Either transform (a single point in space), minimum box collider (any part of a box collider) or full box collider (the entire box collider). For example with the Looking At condition and Minimum Box Collider, if any part of the box collider were to enter the camera's view, the condition would be met.")]
    public CameraConditionComponentParameters componentParameter;

    /// <summary>
    /// If this is true, when checking if the user is looking at an object no raycast checks will be performed to check if there is something preventing the line of sight. This means that as long as the objects position is within the camera frustum the condition will pass.
    /// </summary>
    [Tooltip("If this is true, when checking if the user is looking at an object no raycast checks will be performed to check if there is something preventing the line of sight. This means that as long as the objects position is within the camera frustum the condition will pass.")]
    public bool ignoreObstacles;

    /// <summary>
    /// The time that this camera condition must be met for in seconds. E.g. camera must be looking at object for 2 seconds for condition to pass.
    /// </summary>
    [Tooltip("This is the time that this camera condition must be met for in seconds. E.g. camera must be looking at object for 2 seconds for the condition to pass.")]
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
    /// The view planes of the camera
    /// </summary>
    private Plane[] viewConditionCameraPlane;

    /// <summary>
    /// This is a timer used to make sure the time the condition has been met for is longer than conditionTime
    /// </summary>
    private float viewTimer = 0f;

    #endregion

    #region Player Prefs Conditions

    /// <summary>
    /// This bool is used to store whether the player pref conditions tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showPPrefConditions = false;

    /// <summary>
    /// The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to, less than
    /// </summary>
    [Tooltip("The type of condition the user wants. Options are greater than, greater than or equal to, equal to, less than or equal to, and less than.")]
    public PlayerPrefCondition playerPrefCondition;

    /// <summary>
    /// The value that will be used to check the condition
    /// </summary>
    [Tooltip("The value that will be used to compare against the value stored in the player pref.")]
    public string playerPrefVal;

    /// <summary>
    /// The key of the player pref which will be inspected in the condition
    /// </summary>
    [Tooltip("The key (ID) of the player pref that will be compared against the above value.")]
    public string playerPrefKey;

    /// <summary>
    /// The type of the player pref. Options are int, float or string
    /// </summary>
    [Tooltip("The data type stored within the player pref.")]
    public ParameterType playerPrefType;

    /// <summary>
    /// Holds the value stored in the player pref as a float
    /// </summary>
    private float playerPrefFloat;

    /// <summary>
    /// Holds the value stored in the player pref as a int
    /// </summary>
    private int playerPrefInt;

    /// <summary>
    /// Holds the value stored in the player pref as a string
    /// </summary>
    private string playerPrefString;

    /// <summary>
    /// This is a conversion of playerPrefVal to a float
    /// </summary>
    private float playerPrefValFloat;

    /// <summary>
    /// This is a conversion of playerPrefVal to a int
    /// </summary>
    private int playerPrefValInt;

    #endregion

    #region Animation

    /// <summary>
    /// This bool is used to store whether the animation responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showAnimResponses = false;

    /// <summary>
    /// The gameobject to apply the animation to
    /// </summary>
    [Tooltip("The gameobject to apply the animation to.")]
    public GameObject animationTarget;

    /// <summary>
    /// The mecanim trigger string
    /// </summary>
    [Tooltip("The name of the trigger in the animator that you want to trigger.")]
    public string setMecanimTrigger;

    /// <summary>
    /// Stops the current animation on the animation target
    /// </summary>
    [Tooltip("Stops the current animation on the animation target")]
    public bool stopAnim;

    /// <summary>
    /// The animation clip to play
    /// </summary>
    [Tooltip("Fades the animation in on the animation target over 0.3 seconds and fades other animations out.")]
    public AnimationClip playLegacyAnimation;

    #endregion

    #region Audio

    /// <summary>
    /// This bool is used to store whether the audio responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showAudioResponses = false;

    /// <summary>
    /// If true, all audio is stopped
    /// </summary>
    [Tooltip("Stops the current audio clip being played on the main camera.")]
    public bool muteAllAudio;

    /// <summary>
    /// The audio clip to play on the Main Camera
    /// </summary>
    [Tooltip("This is the audio clip that will be played on the main camera.")]
    public AudioClip playMusic;

    /// <summary>
    /// If true, the audio clip is looped
    /// </summary>
    [Tooltip("If this is true, the above audio clip will loop when played.")]
    public bool loopMusic;

    /// <summary>
    /// The volume of the music. Default is 1.
    /// </summary>
    [Tooltip("The volume of the audio clip. Default is 1.")]
    public float musicVolume = 1f;

    /// <summary>
    /// This is an audio clip, played at the position of this trigger box
    /// </summary>
    [Tooltip("This is an audio clip which is played at the position of this trigger box.")]
    public AudioClip playSoundEffect;

    /// <summary>
    /// The volume of the soundEffect
    /// </summary>
    public float soundEffectVolume = 1f;

    #endregion

    #region Call Function

    /// <summary>
    /// This bool is used to store whether the call function responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showCallFResponses = false;

    /// <summary>
    /// The gameobject on which messageMethodName is called.
    /// </summary>
    [Tooltip("This is the gameobject on which the below function is called on.")]
    public GameObject messageTarget;

    /// <summary>
    /// The name of the method which is called on the messageTarget gameobject.
    /// </summary>
    [Tooltip("This is the function which is called on the above gameobject.")]
    public string messageMethodName;

    /// <summary>
    /// The type of parameter that is being sent
    /// </summary>
    [Tooltip("This is the type of parameter that will be sent to the function. Options are int, float and string.")]
    public ParameterType parameterType;

    /// <summary>
    /// The value of the parameter that is being sent
    /// </summary>
    [Tooltip("This is the value of the parameter that will be sent to the function.")]
    public string parameterValue;

    #endregion

    #region Player Prefs

    /// <summary>
    /// This bool is used to store whether the player prefs responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showPPrefResponses = false;

    /// <summary>
    /// The player pref key which will hold the value set by the user
    /// </summary>
    [Tooltip("This is the key (ID) of the player pref which will have its value set.")]
    public string setPlayerPrefKey;

    /// <summary>
    /// The type of the player pref. Options are int, float and string
    /// </summary>
    [Tooltip("This is the type of data stored within the player pref.")]
    public ParameterType setPlayerPrefType;

    /// <summary>
    /// The value being set in the player pref
    /// </summary>
    [Tooltip("This is the value that will be stored in the player pref.")]
    public string setPlayerPrefVal;

    #endregion

    #region Spawn Gameobject

    /// <summary>
    /// This bool is used to store whether the spawn gameobject responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showSpawnResponses = false;

    /// <summary>
    /// The gameobject or prefab to instanstiate
    /// </summary>
    [Tooltip("This is the prefab which will be instanstiated (spawned).")]
    public GameObject prefabToSpawn;

    /// <summary>
    /// If this isn't empty, the name of the instanciated object will be called this
    /// </summary>
    [Tooltip("This field is used to set the name of the newly instantiated object. If left blank the name will remain as the prefab's saved name.")]
    public string newInstanceName;

    /// <summary>
    /// Position to spawn the gameobject on
    /// </summary>
    [Tooltip("This is the position which the prefab will be spawned on. If left blank it will use the prefab's saved position.")]
    public Vector3 spawnPosition;

    /// <summary>
    /// Used to ensure it is only spawned once
    /// </summary>
    private bool onetime;

    #endregion

    #region Destroy Gameobject

    /// <summary>
    /// This bool is used to store whether the destroy gameobject responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showDestroyResponses = false;

    /// <summary>
    /// This is a list storing object references that will be destroyed when the trigger box is triggered
    /// </summary>
    [Tooltip("All gameobjects stored in this list will be destroyed.")]
    public List<GameObject> destroyGameobjects;

    /// <summary>
    /// A list storing object names that will be destroyed when the trigger box is triggered
    /// </summary>
    [Tooltip("If you are unable to provide references to gameobjects you can enter their names here. They will be found using GameObject.Find() and will be destroyed.")]
    public List<string> destroyObjectNames;

    #endregion

    #region Enable object

    /// <summary>
    /// This bool is used to store whether the enable gameobject responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showEnableResponses = false;

    /// <summary>
    /// A list storing gameobject references that will be set to active when this trigger box is triggered
    /// </summary>
    [Tooltip("This is a list storing gameobject references that will be set to active.")]
    public List<GameObject> enableGameObject;

    #endregion

    #region Disable gameobject

    /// <summary>
    /// This bool is used to store whether the disable gameobject responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showDisableResponses = false;

    /// <summary>
    /// A list storing gameobject references that will be set to inactive when this trigger box is triggered
    /// </summary>
    [Tooltip("This is a list storing gameobject references that will be set to inactive.")]
    public List<GameObject> disableGameObject;

    /// <summary>
    /// A list storing gameobject names that will be set to inactive when this trigger box is triggered
    /// </summary>
    [Tooltip("If you are unable to provide references to gameobjects you can enter their names here. They will be found using GameObject.Find() and will be set to inactive.")]
    public List<string> disableGameObjectName;

    #endregion

    #region Load Level

    /// <summary>
    /// This bool is used to store whether the level responses tab is open in the inspector so it can persist across sessions
    /// </summary>
    public bool showLevelResponses = false;

    /// <summary>
    /// The name of the level to be loaded
    /// </summary>
    [Tooltip("This is the scene name you want to be loaded.")]
    public string loadLevelName;

    /// <summary>
    /// A delay that will take place before the level gets loaded
    /// </summary>
    [Tooltip("This is an optional delay that will take place before loading the scene in seconds.")]
    public float loadDelay = 2;

    #endregion

    #endregion

    #region Enums and structs

    /// <summary>
    /// Used in various things to determine the datatype of a parameter.
    /// </summary>
    public enum ParameterType
    {
        Int,
        Float,
        String,
    }

    /// <summary>
    /// Options that are available to the user after the trigger box has been triggered.
    /// </summary>
    public enum AfterTriggerOptions
    {
        SetInactive,
        DestroyTriggerBox,
        DestroyParent,
    }

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
    /// The available component parameters that can be used in the camera condition.
    /// </summary>
    public enum CameraConditionComponentParameters
    {
        Transform,
        FullBoxCollider,
        MinimumBoxCollider,
    }

    /// <summary>
    /// The available types of player pref conditions such as greater than and less than.
    /// </summary>
    public enum PlayerPrefCondition
    {
        None,
        GreaterThan,
        GreaterThanOrEqualTo,
        EqualTo,
        LessThanOrEqualTo,
        LessThan,
    }

    /// <summary>
    /// The user can select if they want the trigger box to follow an object or the main camera.
    /// </summary>
    public enum TriggerFollow
    {
        Static,
        FollowMainCamera,
        FollowTransform,
    }

    #endregion

    void Start()
    {
        if (cameraConditionType != LookType.None)
        {
            viewConditionObjectCollider = conditionObject.GetComponent<BoxCollider>();
            if (viewConditionObjectCollider == null)
            {
                componentParameter = CameraConditionComponentParameters.Transform;
            }
        }

        if (playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.Float:
                    float.TryParse(playerPrefVal, out playerPrefValFloat);
                    break;

                case ParameterType.Int:
                    int.TryParse(playerPrefVal, out playerPrefValInt);
                    break;
            }
        }

        if (!string.IsNullOrEmpty(followTransformName))
        {
            GameObject gObj = GameObject.Find(followTransformName);
            followTransform = gObj.transform;
        }
    }

    /// <summary>
    /// Update loop called every frame
    /// </summary>
    void FixedUpdate()
    {
        if (prefabToSpawn && !onetime && !Application.isPlaying)
        {
            onetime = true;
            spawnPosition = transform.position + Vector3.forward;
        }

        if (triggerFollow == TriggerFollow.FollowTransform)
        {
            transform.position = followTransform.position;
        }
        else if (triggerFollow == TriggerFollow.FollowMainCamera)
        {
            transform.position = Camera.main.transform.position;
        }

        if (triggered)
        {
            if (cameraConditionType == LookType.None && playerPrefCondition == PlayerPrefCondition.None)
            {
                conditionMet = true;
            }

            if (!conditionMet && cameraConditionType != LookType.None)
            {
                conditionMet = CheckCameraConditions();
            }

            if (conditionMet && playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
            {
                conditionMet = CheckPlayerPrefConditions();
            }

            if (conditionMet)
            {
                ConditionMet();
            }
        }
    }

    /// <summary>
    /// Called when this box intersects with another gameobject
    /// </summary>
    /// <param name="other">The collider that this object has collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Count >= 0 && (triggerTags.Contains(other.gameObject.tag)))
        {
            triggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!canWander)
        {
            if (triggerTags.Count >= 0 && (triggerTags.Contains(other.gameObject.tag)))
            {
                triggered = false;
            }
        }
    }

    private bool CheckCameraConditions()
    {
        // Is this necessary? TODO: Find out if this is necessary
        if (Vector3.Distance(Camera.main.transform.position, conditionObject.transform.position) < 2f)
        {
            return false;
        }

        if (cameraConditionType == LookType.LookingAt)
        {
            if (componentParameter == CameraConditionComponentParameters.Transform)
            {
                // Get the viewport point of the object
                viewConditionScreenPoint = Camera.main.WorldToViewportPoint(conditionObject.transform.position);

                // This checks that the object is in our screen
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
            else if (componentParameter == CameraConditionComponentParameters.MinimumBoxCollider)
            {
                viewConditionCameraPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main);
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
            }
            else
            {
                // Get the viewport point of the object
                viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.min);

                // This checks that the object is in our screen
                if (viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                    viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1)
                {
                    viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.max);

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
            }
        }
        else if (cameraConditionType == LookType.LookingAway)
        {
            if (componentParameter == CameraConditionComponentParameters.Transform)
            {
                // Get the viewport point of the object
                viewConditionScreenPoint = Camera.main.WorldToViewportPoint(conditionObject.transform.position);

                // This checks that the object is in our screen
                if (!(viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                    viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1))
                {
                    return CheckConditionTimer();
                }
            }
            else if (componentParameter == CameraConditionComponentParameters.FullBoxCollider)
            {
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
            }
            else
            {
                // Get the viewport point of the object
                viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewConditionObjectCollider.bounds.min);

                // This checks that the object is in our screen
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
            }
        }
        return false;
    }

    private void GetUpdatedPlayerPrefs()
    {
        if (playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.Float:
                    playerPrefFloat = PlayerPrefs.GetFloat(playerPrefKey);
                    break;

                case ParameterType.Int:
                    playerPrefInt = PlayerPrefs.GetInt(playerPrefKey);
                    break;

                case ParameterType.String:
                    playerPrefString = PlayerPrefs.GetString(playerPrefString);
                    break;
            }
        }
    }

    private bool CheckPlayerPrefConditions()
    {
        GetUpdatedPlayerPrefs();
        if (playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.String:
                    switch (playerPrefCondition)
                    {
                        case PlayerPrefCondition.EqualTo:
                            if (playerPrefVal == playerPrefString)
                                return true;
                            else
                                return false;
                        default:
                            Debug.Log("You can only use Equal To with strings");
                            return false;
                    }

                case ParameterType.Float:
                    switch (playerPrefCondition)
                    {
                        case PlayerPrefCondition.EqualTo:
                            if (playerPrefValFloat == playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.GreaterThan:
                            if (playerPrefValFloat > playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.GreaterThanOrEqualTo:
                            if (playerPrefValFloat >= playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.LessThan:
                            if (playerPrefValFloat < playerPrefFloat)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.LessThanOrEqualTo:
                            if (playerPrefValFloat <= playerPrefFloat)
                                return true;
                            else
                                return false;
                    }
                    break;

                case ParameterType.Int:
                    switch (playerPrefCondition)
                    {
                        case PlayerPrefCondition.EqualTo:
                            if (playerPrefValInt == playerPrefInt)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.GreaterThan:
                            if (playerPrefValInt > playerPrefInt)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.GreaterThanOrEqualTo:
                            if (playerPrefValInt >= playerPrefInt)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.LessThan:
                            if (playerPrefValInt < playerPrefInt)
                                return true;
                            else
                                return false;

                        case PlayerPrefCondition.LessThanOrEqualTo:
                            if (playerPrefValInt <= playerPrefInt)
                                return true;
                            else
                                return false;
                    }
                    break;
            }
        }

        return false;
    }

    private bool CheckRaycast()
    {
        // This checks that there's no obstacles in the way
        if (Physics.Raycast(Camera.main.transform.position, viewConditionDirection.normalized, out viewConditionRaycastHit, viewConditionDirection.magnitude))
        {
            if (viewConditionRaycastHit.transform.position == conditionObject.transform.position)
            {
                if (viewTimer >= conditionTime)
                {
                    return true;
                }
                else
                {
                    viewTimer += Time.fixedDeltaTime;
                    return false;
                }
            }
            return false;
        }
        return true;
    }

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

    private void ConditionMet()
    {
        if (debugTriggerBox)
        {
            Debug.Log(gameObject.name + " has been triggered!");
        }

        if (muteAllAudio)
        {
            Camera.main.GetComponent<AudioSource>().Stop();
        }

        if (playMusic)
        {
            Camera.main.GetComponent<AudioSource>().loop = loopMusic;
            Camera.main.GetComponent<AudioSource>().clip = playMusic;
            Camera.main.GetComponent<AudioSource>().volume = musicVolume;
            Camera.main.GetComponent<AudioSource>().Play();
        }

        if (playSoundEffect)
        {
            AudioSource.PlayClipAtPoint(playSoundEffect, transform.position, soundEffectVolume);
        }

        if (messageMethodName != "" && messageTarget)
        {
            if (parameterValue != "")
            {
                switch (parameterType)
                {
                    case ParameterType.Int:
                        messageTarget.SendMessage(messageMethodName, int.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
                        break;
                    case ParameterType.Float:
                        messageTarget.SendMessage(messageMethodName, float.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
                        break;
                    case ParameterType.String:
                        messageTarget.SendMessage(messageMethodName, parameterValue, SendMessageOptions.DontRequireReceiver);
                        break;
                }
            }
            else
            {
                messageTarget.SendMessage(messageMethodName, SendMessageOptions.DontRequireReceiver);
            }
        }

        if (stopAnim && animationTarget)
        {
            animationTarget.GetComponent<Animation>().Stop();
        }


        if (playLegacyAnimation && animationTarget)
        {
            animationTarget.GetComponent<Animation>().CrossFade(playLegacyAnimation.name, 0.3f, PlayMode.StopAll);
        }

        if (setMecanimTrigger != "")
        {
            animationTarget.GetComponent<Animator>().SetTrigger(setMecanimTrigger);
        }

        if (prefabToSpawn)
        {
            if (!string.IsNullOrEmpty(newInstanceName))
            {
                // Is spawnrotation ever null?
                var newobj = Instantiate(prefabToSpawn, (spawnPosition != Vector3.zero) ? spawnPosition : prefabToSpawn.transform.position, prefabToSpawn.transform.rotation);
                newobj.name = newInstanceName;
            }
            else
            {
                Instantiate(prefabToSpawn, (spawnPosition != Vector3.zero) ? spawnPosition : prefabToSpawn.transform.position, prefabToSpawn.transform.rotation);
            }
        }

        for (int i = 0; i < enableGameObject.Count; i++)
        {
            if (enableGameObject[i])
            {
                enableGameObject[i].SetActive(true);
            }
        }

        for (int i = 0; i < disableGameObject.Count; i++)
        {
            if (disableGameObject[i])
            {
                disableGameObject[i].SetActive(false);
            }
        }

        for (int i = 0; i < disableGameObjectName.Count; i++)
        {
            if (!string.IsNullOrEmpty(disableGameObjectName[i]))
            {
                GameObject gameobj = GameObject.Find(disableGameObjectName[i]);
                gameobj.SetActive(false);
            }
        }

        for (int i = 0; i < destroyGameobjects.Count; i++)
        {
            Destroy(destroyGameobjects[i]);
        }

        for (int i = 0; i < destroyObjectNames.Count; i++)
        {
            if (!string.IsNullOrEmpty(destroyObjectNames[i]))
            {
                GameObject gameobj = GameObject.Find(destroyObjectNames[i]);
                Destroy(gameobj);
            }
        }

        if (!string.IsNullOrEmpty(setPlayerPrefKey))
        {
            switch (setPlayerPrefType)
            {
                case ParameterType.String:
                    PlayerPrefs.SetString(setPlayerPrefKey, setPlayerPrefVal);
                    break;

                case ParameterType.Int:
                    PlayerPrefs.SetInt(setPlayerPrefKey, Convert.ToInt32(setPlayerPrefVal));
                    break;

                case ParameterType.Float:
                    PlayerPrefs.SetFloat(setPlayerPrefKey, (float)Convert.ToInt32(setPlayerPrefVal));
                    break;
            }
        }

        if (loadLevelName != "")
        {
            StartCoroutine("LoadScene");
        }

        switch (afterTrigger)
        {
            case AfterTriggerOptions.SetInactive:
                gameObject.SetActive(false);
                break;

            case AfterTriggerOptions.DestroyTriggerBox:
                Destroy(gameObject);
                break;

            case AfterTriggerOptions.DestroyParent:
                Destroy(transform.parent.gameObject);
                break;
        }
    }

    /// <summary>
    /// Loads the specified scene
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(loadDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadLevelName);
    }

    /// <summary>
    /// Draws the trigger box
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = triggerboxColour;

        if (!drawWire)
        {
            Gizmos.DrawCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                            new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z));
        }
        else
        {
            Gizmos.DrawWireCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                           new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z));
        }
    }
}