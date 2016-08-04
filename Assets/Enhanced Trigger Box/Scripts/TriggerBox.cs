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
    /// If this is true, the script won't perform checks when the scene is run to notify you if you're missing any required references.
    /// </summary>
    [Tooltip("If this is true, the script won't perform checks when the scene is run to notify you if you're missing any required references.")]
    public bool disableStartupChecks;

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
    [Tooltip("The type of component the condition will be checked against.  Either transform (a single point in space), minimum box collider (any part of a box collider), full box collider (the entire box collider) or mesh renderer (any part of a mesh). For example with the Looking At condition and Minimum Box Collider, if any part of the box collider were to enter the camera's view, the condition would be met.")]
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

    private MeshRenderer viewConditionObjectMeshRenderer;

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
        MeshRenderer,
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
        // Perform startup checks
        if (!disableStartupChecks)
        {
            // If destroy parent is enable, check if a parent exists
            if (afterTrigger == AfterTriggerOptions.DestroyParent)
            {
                if (transform.parent == null)
                {
                    Debug.Log("You have selected Destroy Parent after the trigger but this trigger box has no parent! The option has been set to Destroy Trigger Box whilst the scene is running to prevent errors.");
                    afterTrigger = AfterTriggerOptions.DestroyTriggerBox;
                }
            }

            // If follow transform is enabled, check if the user specified a transform
            if (triggerFollow == TriggerFollow.FollowTransform)
            {
                if (followTransform == null && string.IsNullOrEmpty(followTransformName))
                {
                    Debug.Log("You have selected Follow Transform but you have not specified either a transform reference or a gameobject name! The option has been set to Follow Main Camera whilst the scene is running to prevent errors.");
                    triggerFollow = TriggerFollow.FollowMainCamera;
                }
                else 
                {
                    // If the user has entered both a gameobject name and transform reference
                    if (followTransform != null && !string.IsNullOrEmpty(followTransformName))
                    {
                        Debug.Log("You have selected Follow Transform and have entered both a transform reference and a gameobject name! The transform reference will be ignored and the gameobject name will take preference and you should remove one of them.");
                    }
                    if (!string.IsNullOrEmpty(followTransformName))
                    {
                        GameObject gObj = GameObject.Find(followTransformName);
                        // If the gameobject will the specified name cannot be found
                        if (gObj == null)
                        {
                            Debug.Log("The gameobject name you have entered for Follow Transform cannot be found. The option has been set to Follow Main Camera whilst the scene is running to prevent errors.");
                            triggerFollow = TriggerFollow.FollowMainCamera;
                        }
                        else
                        {
                            followTransform = gObj.transform;
                        }
                    }
                }
            }

            // If there's a camera condition
            if (cameraConditionType != LookType.None)
            {
                // Check if the user specified a gameobject to focus on
                if (conditionObject == null)
                {
                    Debug.Log("You have selected the " + ((cameraConditionType == LookType.LookingAt) ? "Looking At" : "Looking Away") + " camera condition but have not specified a gameobject reference! The option has been set to None whilst the scene is running to prevent errors.");
                    cameraConditionType = LookType.None;
                }
                else
                {
                    // If the user has selected full box collider check the object has a box collider
                    if (componentParameter == CameraConditionComponentParameters.FullBoxCollider || componentParameter == CameraConditionComponentParameters.MinimumBoxCollider)
                    {
                        viewConditionObjectCollider = conditionObject.GetComponent<BoxCollider>();
                        if (viewConditionObjectCollider == null)
                        {
                            Debug.Log("You have selected the Component Parameter for the camera condition to be " + ((componentParameter == CameraConditionComponentParameters.FullBoxCollider) ? "Full Box Collider" : "Minimum Box Collider") + " but the object doesn't have a Box Collider component! The option has been set to Transform whilst the scene is running to prevent errors.");
                            componentParameter = CameraConditionComponentParameters.Transform;
                        }
                    } // Else if the user selected mesh render check the object has mesh renderer
                    else if (componentParameter == CameraConditionComponentParameters.MeshRenderer)
                    {
                        viewConditionObjectMeshRenderer = conditionObject.GetComponent<MeshRenderer>();
                        if (viewConditionObjectMeshRenderer == null)
                        {
                            Debug.Log("You have selected the Component Parameter for the camera condition to be Mesh Renderer but the object doesn't have a Mesh Renderer component! The option has been set to Transform whilst the scene is running to prevent errors.");
                            componentParameter = CameraConditionComponentParameters.Transform;
                        }
                    }
                }
            }

            // Check that condition time is above 0
            if (conditionTime < 0f)
            {
                Debug.Log("You have set the camera condition timer to be less than 0 which isn't possible! This has been changed to 0 whilst the scene is running to prevent errors.");
                conditionTime = 0f;
            }

            // If there is a player pref condition check that there is a value for the condition
            if (playerPrefCondition != PlayerPrefCondition.None)
            {
                if (string.IsNullOrEmpty(playerPrefVal))
                {
                    Debug.Log("You have set up a player pref condition but haven't entered a value to be compared against the player pref! This option has been changed to None whilst the scene is running to prevent errors.");
                    playerPrefCondition = PlayerPrefCondition.None;
                }
                else
                {
                    // If there's any parsing required do this now
                    switch (playerPrefType)
                    {
                        case ParameterType.Float:
                            float.TryParse(playerPrefVal, out playerPrefValFloat);
                            break;

                        case ParameterType.Int:
                            int.TryParse(playerPrefVal, out playerPrefValInt);
                            break;
                    }

                    if (string.IsNullOrEmpty(playerPrefKey))
                    {
                        Debug.Log("You have set up a player pref condition but haven't entered a player pref key! This option has been changed to None whilst the scene is running to prevent errors.");
                        playerPrefCondition = PlayerPrefCondition.None;
                    }
                }
            }

            // If there is a mecanim trigger check there is a target for it
            if (!string.IsNullOrEmpty((setMecanimTrigger)))
            {
                if (animationTarget == null)
                {
                    Debug.Log("You have set a Mecanim Trigger as an Animation Response but haven't set an Animation Target to apply it to! This has been removed whilst the scene is running to prevent errors.");
                    setMecanimTrigger = "";
                }
            }

            // If stop anim is set check there is a target for it
            if (stopAnim)
            {
                if (animationTarget == null)
                {
                    Debug.Log("You have set Stop Animation as an Animation Response but haven't set an Animation Target to apply it to! This has been disabled whilst the scene is running to prevent errors.");
                    setMecanimTrigger = "";
                }
            }

            // If legacy animat is set to play check there is a target for it
            if (playLegacyAnimation != null)
            {
                if (animationTarget == null)
                {
                    Debug.Log("You have chosen to play a legacy animation as an Animation Response but haven't set an Animation Target to apply it to! This has been disabled whilst the scene is running to prevent errors.");
                    playLegacyAnimation = null;
                }
            }

            // TODO: Continue validation from MUTE ALL AUDIO
        }
    }

    /// <summary>
    /// Update loop called every frame
    /// </summary>
    void FixedUpdate()
    {
        // TODO: What happens in this if statement?
        if (prefabToSpawn && !onetime && !Application.isPlaying)
        {
            onetime = true;
            spawnPosition = transform.position + Vector3.forward;
        }

        // This if statement updates the trigger boxes position to either stay on a transform or on the main camera
        if (triggerFollow == TriggerFollow.FollowTransform)
        {
            transform.position = followTransform.position;
        }
        else if (triggerFollow == TriggerFollow.FollowMainCamera)
        {
            transform.position = Camera.main.transform.position;
        }

        // If the player has walked inside the trigger box
        if (triggered)
        {
            // If there is neither a camera condition or a player pref condition, set conditionMet to true
            if (cameraConditionType == LookType.None && playerPrefCondition == PlayerPrefCondition.None)
            {
                conditionMet = true;
            }
            else
            {
                // Perform the camera condition checks
                if (cameraConditionType != LookType.None)
                {
                    conditionMet = CheckCameraConditions();
                }

                // If the camera condition passed then check to see if the player pref condition passes as well.
                // But of course only if the player pref condition has been set
                if (conditionMet && playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
                {
                    conditionMet = CheckPlayerPrefConditions();
                }
            }

            // If all conditions have been met, start executing the responses
            if (conditionMet)
            {
                ConditionMet();
            }
        }
    }

    /// <summary>
    /// Called when this box intersects with another gameobject that has one of the specified triggerTags
    /// </summary>
    /// <param name="other">The collider that this object has collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Count >= 0 && (triggerTags.Contains(other.gameObject.tag)))
        {
            triggered = true;
        }
    }

    /// <summary>
    /// Called when a collider exits this collider. Only serves a purpose if the player cannot wander as it forces the condition checks to stop until the user re-enters the trigger box.
    /// </summary>
    /// <param name="other">The collider leaving this collider</param>
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

    /// <summary>
    /// This function performs the camera condition checks
    /// </summary>
    /// <returns>Returns true or false depending on if the conditions pass or not</returns>
    private bool CheckCameraConditions()
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
    /// This function gets the values set in the player pref
    /// </summary>
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

    /// <summary>
    /// This function determines if the player pref conditions have been met
    /// </summary>
    /// <returns>Returns true or false depending on if the player pref conditions have been met.</returns>
    private bool CheckPlayerPrefConditions()
    {
        // Get the player pref values. We need to do this regularly in case they change at runtime.
        GetUpdatedPlayerPrefs();

        if (playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.String:
                    if (playerPrefVal == playerPrefString)
                        return true;
                    else
                        return false;

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

    /// <summary>
    /// This function executes all the responses and only happens after all the conditions have been met
    /// </summary>
    private void ConditionMet()
    {
        // If debugTriggerBox is selected, write to the console saying the trigger box has successfully been triggered
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
            // This will play the audio clip at the trigger boxes current position
            AudioSource.PlayClipAtPoint(playSoundEffect, transform.position, soundEffectVolume);
        }

        // This will send the messages to the selected gameobjects
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
            // Plays an animation clip on the target animation over 0.3 seconds and fades other animations out
            animationTarget.GetComponent<Animation>().CrossFade(playLegacyAnimation.name, 0.3f, PlayMode.StopAll);
        }

        if (!string.IsNullOrEmpty(setMecanimTrigger))
        {
            animationTarget.GetComponent<Animator>().SetTrigger(setMecanimTrigger);
        }

        if (prefabToSpawn)
        {
            // If a newinstancename has been set then we will re-name the instance after it has been created
            if (!string.IsNullOrEmpty(newInstanceName))
            {
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
                if (gameobj == null)
                {
                    Debug.Log("Unable to find and disable the gameobject with the name " + disableGameObjectName[i]);
                }
                else
                {
                    gameobj.SetActive(false);
                }
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
                if (gameobj == null)
                {
                    Debug.Log("Unable to find and destroy the gameobject with the name " + disableGameObjectName[i]);
                }
                else
                {
                    Destroy(gameobj);
                }
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
    /// <returns>IEnumerator to load the scene after the loadDelay</returns>
    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(loadDelay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(loadLevelName);
    }

    /// <summary>
    /// Draws the trigger box in the editor
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