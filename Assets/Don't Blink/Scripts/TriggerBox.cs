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
    public msgtype parameterType;

    /// <summary>
    /// The value of the parameter that is being sent
    /// </summary>
    public string parameterValue;
    #endregion

    #region Spawn Gameobject
    /// <summary>
    /// The gameobject or prefab to instanstiate
    /// </summary>
    public GameObject spawnGameobject;

    /// <summary>
    /// Used to ensure it is only spawned once
    /// </summary>
    private bool onetime;

    /// <summary>
    /// Position to spawn the gameobject on
    /// </summary>
    public Vector3 spawnPosition;
    #endregion

    #region Enable / Disable gameobject
    public bool isEnabled;
    public GameObject targetgameObject;
    #endregion

    #region Load Level
    public string loadLevelName;
    public float loadDelay = 2;
    #endregion

    #endregion

    /// <summary>
    /// The type of message that will be sent to the recieving gameobject
    /// </summary>
    public enum msgtype
    {
        Int,
        Float,
        String,
    }

    /// <summary>
    /// Update loop called every frame
    /// </summary>
    void Update()
    {
        if (spawnGameobject && !onetime && !Application.isPlaying)
        {
            onetime = true;
            spawnPosition = transform.position + Vector3.forward;
        }
    }

    /// <summary>
    /// Called when this box intersects with another gameobject that has a collider
    /// </summary>
    /// <param name="other">The collider that has been collided with</param>
    private void OnTriggerEnter(Collider other)
    {
        if (triggerTags.Count >= 0 && (triggerTags.Contains(other.gameObject.tag)))
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
                        case msgtype.Int:
                            messageTarget.SendMessage(messageMethodName, int.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
                            break;
                        case msgtype.Float:
                            messageTarget.SendMessage(messageMethodName, float.Parse(parameterValue), SendMessageOptions.DontRequireReceiver);
                            break;
                        case msgtype.String:
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
                Instantiate(spawnGameobject, spawnPosition, Quaternion.identity);
            }

            if (targetgameObject)
            {
                if (isEnabled)
                {
                    targetgameObject.SetActive(true);
                }
                else
                {
                    targetgameObject.SetActive(false);
                }
            }

            if (loadLevelName != "")
            {
                StartCoroutine("LoadScene");
            }
        }
        else
        {
            Destroy(gameObject);
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