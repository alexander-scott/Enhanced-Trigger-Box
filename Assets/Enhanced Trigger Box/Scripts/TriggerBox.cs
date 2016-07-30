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
    /// A list of tags belonging to gameobjects which are able to trigger the trigger box
    /// </summary>
    public List<string> triggerTags;

    /// <summary>
    /// The colour of the trigger box
    /// </summary>
    public Color triggerboxColour;

    /// <summary>
    /// If true then only a wireframe will be displayed instead of a coloured box
    /// </summary>
    public bool drawWire;

    /// <summary>
    /// If true the application will write to the console a message with the name of the trigger that was triggered
    /// </summary>
    public bool debugTriggerBox;

    /// <summary>
    /// A set of options for when the trigger box has been trigged. Nothing does nothing. Trigger box destroys trigger box. Parent destroys parent.
    /// </summary>
    public DestroyTriggerBox destroyOnTrigger;

    public TriggerFollow triggerFollow;

    public Transform followTransform;

    public string followTransformName;

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

    public LookType viewConditionType;

    public GameObject viewObject;

    public LookObjectCondition lookObjectCondition;

    public float conditionTime = 0f;

    public bool ignoreObstacles;

    public bool canWander;

    private Vector3 viewConditionScreenPoint = new Vector3();

    private Vector3 viewConditionDirection = new Vector3();

    private RaycastHit viewConditionRaycastHit = new RaycastHit();

    private BoxCollider viewConditionObjectCollider;

    private Plane[] viewConditionCameraPlane;

    private float viewTimer = 0f;

    #endregion

    #region Player Prefs Conditions

    public string playerPrefKey;

    public string playerPrefVal;

    public ParameterType playerPrefType;

    public PlayerPrefCondition playerPrefCondition;

    private float playerPrefFloat;

    private int playerPrefInt;

    private string playerPrefString;

    private float playerPrefValFloat;

    private int playerPrefValInt;

    #endregion

    #region Animation

    /// <summary>
    /// The gameobject to apply the animation to
    /// </summary>
    public GameObject animationTarget;

    /// <summary>
    /// The animation clip to play
    /// </summary>
    public AnimationClip playAnimation;

    /// <summary>
    /// The mecanim trigger string
    /// </summary>
    public string setMecanimTrigger;

    /// <summary>
    /// Stops the current animation
    /// </summary>
    public bool stopAnim;
    #endregion

    #region Audio
    /// <summary>
    /// The volume of the music. Default is 1.
    /// </summary>
    public float musicVolume = 1f;

    /// <summary>
    /// The audio clip to play on the Main Camera
    /// </summary>
    public AudioClip playAudio;

    /// <summary>
    /// If true, all audio is stopped
    /// </summary>
    public bool mute;

    /// <summary>
    /// If true, the audio clip is looped
    /// </summary>
    public bool loop;

    /// <summary>
    /// This is an audio clip, played at the position of this trigger box
    /// </summary>
    public AudioClip soundEffect;

    /// <summary>
    /// The volume of the soundEffect
    /// </summary>
    public float soundEffectVolume = 1f;
    #endregion

    #region Call Function
    /// <summary>
    /// The gameobject on which messageMethodName is called on every MonoBehaviour belonging to it.
    /// </summary>
    public GameObject messageTarget;

    /// <summary>
    /// The name of the method which is called on the messageTarget gameobject.
    /// </summary>
    public string messageMethodName;

    /// <summary>
    /// The type of parameter that is being sent
    /// </summary>
    public ParameterType parameterType;

    /// <summary>
    /// The value of the parameter that is being sent
    /// </summary>
    public string parameterValue;
    #endregion

    #region Player Prefs

    public string setPlayerPrefKey;

    public ParameterType setPlayerPrefType;

    public string setPlayerPrefVal;

    #endregion

    #region Spawn Gameobject
    /// <summary>
    /// The gameobject or prefab to instanstiate
    /// </summary>
    public GameObject spawnGameobject;

    public string spawnedObjectName;

    /// <summary>
    /// Position to spawn the gameobject on
    /// </summary>
    public Vector3 spawnPosition;

    /// <summary>
    /// Used to ensure it is only spawned once
    /// </summary>
    private bool onetime;
    #endregion

    #region Destroy Gameobject
    /// <summary>
    /// The gameobject or prefab to instanstiate
    /// </summary>
    public List<GameObject> destroyGameobjects;

    public List<string> destroyObjectNames;
    #endregion

    #region Enable object

    public List<GameObject> enableGameObject;

    #endregion

    #region Disable gameobject

    public List<GameObject> disableGameObject;

    public List<string> disableGameObjectName;

    #endregion

    #region Load Level
    public string loadLevelName;
    public float loadDelay = 2;
    #endregion

    #endregion

    #region Enums and structs

    /// <summary>
    /// The type of message that will be sent to the recieving gameobject
    /// </summary>
    public enum ParameterType
    {
        Int,
        Float,
        String,
    }

    public enum DestroyTriggerBox
    {
        Nothing,
        TriggerBox,
        Parent,
    }

    public enum LookType
    {
        None,
        LookingAt,
        LookingAway,
    }

    public enum LookObjectCondition
    {
        Transform,
        FullBoxCollider,
        MinimumBoxCollider,
    }

    public enum PlayerPrefCondition
    {
        None,
        GreaterThan,
        GreaterThanOrEqualTo,
        EqualTo,
        LessThanOrEqualTo,
        LessThan,
    }

    public enum TriggerFollow
    {
        Static,
        FollowMainCamera,
        FollowTransform,
    }

    #endregion

    void Start()
    {
        if (viewConditionType != LookType.None)
        {
            viewConditionObjectCollider = viewObject.GetComponent<BoxCollider>();
            if (viewConditionObjectCollider == null)
            {
                lookObjectCondition = LookObjectCondition.Transform;
            }
        }

        if (playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
        {
            switch (playerPrefType)
            {
                case ParameterType.Float:
                    playerPrefFloat = PlayerPrefs.GetFloat(playerPrefKey);
                    break;

                case ParameterType.Int:
                    playerPrefInt = PlayerPrefs.GetInt(playerPrefKey);
                    float.TryParse(playerPrefVal, out playerPrefValFloat);
                    break;

                case ParameterType.String:
                    playerPrefString = PlayerPrefs.GetString(playerPrefString);
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
        if (spawnGameobject && !onetime && !Application.isPlaying)
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
            if (viewConditionType == LookType.None && playerPrefCondition == PlayerPrefCondition.None)
            {
                conditionMet = true;
            }

            if (!conditionMet && viewConditionType != LookType.None)
            {
                conditionMet = CheckCameraConditions();
            }

            if (!conditionMet && playerPrefCondition != PlayerPrefCondition.None && !string.IsNullOrEmpty(playerPrefVal))
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
        if (Vector3.Distance(Camera.main.transform.position, viewObject.transform.position) < 2f)
        {
            return false;
        }

        if (viewConditionType == LookType.LookingAt)
        {
            if (lookObjectCondition == LookObjectCondition.Transform)
            {
                // Get the viewport point of the object
                viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewObject.transform.position);

                // This checks that the object is in our screen
                if (viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                    viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1)
                {
                    if (ignoreObstacles)
                    {
                        return CheckConditionTimer();
                    }
                    viewConditionDirection = (viewObject.transform.position - Camera.main.transform.position);

                    if (CheckRaycast())
                    {
                        return CheckConditionTimer();
                    }
                }
            }
            else if (lookObjectCondition == LookObjectCondition.MinimumBoxCollider)
            {
                viewConditionCameraPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main);
                if (GeometryUtility.TestPlanesAABB(viewConditionCameraPlane, viewConditionObjectCollider.bounds))
                {
                    if (ignoreObstacles)
                    {
                        return CheckConditionTimer();
                    }

                    viewConditionDirection = (viewObject.transform.position - Camera.main.transform.position);

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

                        viewConditionDirection = (viewObject.transform.position - Camera.main.transform.position);

                        if (CheckRaycast())
                        {
                            return CheckConditionTimer();
                        }
                    }
                }
            }
        }
        else if (viewConditionType == LookType.LookingAway)
        {
            if (lookObjectCondition == LookObjectCondition.Transform)
            {
                // Get the viewport point of the object
                viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewObject.transform.position);

                // This checks that the object is in our screen
                if (!(viewConditionScreenPoint.z > 0 && viewConditionScreenPoint.x > 0 &&
                    viewConditionScreenPoint.x < 1 && viewConditionScreenPoint.y > 0 && viewConditionScreenPoint.y < 1))
                {
                    return CheckConditionTimer();
                }
            }
            else if (lookObjectCondition == LookObjectCondition.FullBoxCollider)
            {
                viewConditionCameraPlane = GeometryUtility.CalculateFrustumPlanes(Camera.main);
                if (!GeometryUtility.TestPlanesAABB(viewConditionCameraPlane, viewConditionObjectCollider.bounds))
                {
                    viewConditionScreenPoint = Camera.main.WorldToViewportPoint(viewObject.transform.position);

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

    private bool CheckPlayerPrefConditions()
    {
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
            if (viewConditionRaycastHit.transform.position == viewObject.transform.position)
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

        if (mute)
        {
            Camera.main.GetComponent<AudioSource>().Stop();
        }

        if (playAudio)
        {
            Camera.main.GetComponent<AudioSource>().loop = loop;
            Camera.main.GetComponent<AudioSource>().clip = playAudio;
            Camera.main.GetComponent<AudioSource>().volume = musicVolume;
            Camera.main.GetComponent<AudioSource>().Play();
        }

        if (soundEffect)
        {
            AudioSource.PlayClipAtPoint(soundEffect, transform.position, soundEffectVolume);
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


        if (playAnimation && animationTarget)
        {
            animationTarget.GetComponent<Animation>().CrossFade(playAnimation.name, 0.3f, PlayMode.StopAll);
        }

        if (setMecanimTrigger != "")
        {
            animationTarget.GetComponent<Animator>().SetTrigger(setMecanimTrigger);
        }

        if (spawnGameobject)
        {
            if (!string.IsNullOrEmpty(spawnedObjectName))
            {
                // Is spawnrotation ever null?
                var newobj = Instantiate(spawnGameobject, (spawnPosition != Vector3.zero) ? spawnPosition : spawnGameobject.transform.position, spawnGameobject.transform.rotation);
                newobj.name = spawnedObjectName;
            }
            else
            {
                Instantiate(spawnGameobject, (spawnPosition != Vector3.zero) ? spawnPosition : spawnGameobject.transform.position, spawnGameobject.transform.rotation);
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

        switch (destroyOnTrigger)
        {
            case DestroyTriggerBox.Nothing:
                gameObject.SetActive(false);
                break;

            case DestroyTriggerBox.TriggerBox:
                Destroy(gameObject);
                break;

            case DestroyTriggerBox.Parent:
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