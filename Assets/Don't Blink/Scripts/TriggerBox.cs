using System.Collections;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

[ExecuteInEditMode]
public class TriggerBox : MonoBehaviour
{
    public GameObject animationTarget;
    public float musicVolume = 1f;
    public bool isEnabled;
    public GameObject messageTarget;
    public AnimationClip playAnimation;
    public AudioClip playAudio;
    public string sendMessage;
    public string showText = "Subtitle text here...";
    public GameObject spawnGameobject;
    public Vector3 spawnPosition;
    private GUIStyle style;
    public GameObject targetgameObject;
    public Color textColor;
    public Font textFont;
    public FontStyle textFontStyle;
    public int textSize = 18;
    public float textTime;
    public string triggerByTag1 = "Player";
    public string triggerByTag2 = "";
    public int triggerCount = 1;
    public Color triggerboxColor;
    private bool onetime;
    private Vector3 last;
    public bool mute;
    public bool loop;
    public string loadLevelName;
    public float loadDelay = 2;
    public AudioClip sfx;
    public float soundEffectVolume = 1f;
    public bool drawWire;
    public string setMecanimTrigger;
        

        public enum msgtype
        {
           Int,
           Float,
           String,
        }

        public msgtype parameterType;
        public string parameterValue;
        public bool stopAnim;

    private void Start()
    {


        style = new GUIStyle
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = textSize,
            normal = { textColor = textColor },
            fontStyle = textFontStyle,
            font = textFont
        };
    }


    void Update()
    {

        if (spawnGameobject && !onetime && !Application.isPlaying)
        {
            onetime = true;
            spawnPosition = transform.position + Vector3.forward;
        }
 
    }
       

    private void OnTriggerEnter(Collider other)
    {
        triggerCount -= 1;
        if (triggerCount >= 0 && (other.gameObject.tag == triggerByTag1 || other.gameObject.tag == triggerByTag2))
        {
            if (mute)
                Camera.main.GetComponent<AudioSource>().Stop();

            if (playAudio)
            {
                Camera.main.GetComponent<AudioSource>().loop = loop;
                Camera.main.GetComponent<AudioSource>().clip = playAudio;
                Camera.main.GetComponent<AudioSource>().volume = musicVolume;
                Camera.main.GetComponent<AudioSource>().Play();
            }

            if (sfx) AudioSource.PlayClipAtPoint(sfx, transform.position,soundEffectVolume);

            if (sendMessage != "" && messageTarget)
            {
                if (parameterValue != "")
                {
                    switch (parameterType)
                    {

                        case msgtype.Int:
                            messageTarget.SendMessage(sendMessage, int.Parse(parameterValue),SendMessageOptions.DontRequireReceiver);
                            break;
                        case msgtype.Float:
                            messageTarget.SendMessage(sendMessage, float.Parse(parameterValue),SendMessageOptions.DontRequireReceiver);
                            break;
                        case msgtype.String:
                            messageTarget.SendMessage(sendMessage, parameterValue,SendMessageOptions.DontRequireReceiver);
                            break;

                    }
                }
                else
                {
                    messageTarget.SendMessage(sendMessage,SendMessageOptions.DontRequireReceiver);
                }
            }
            if (stopAnim && animationTarget) animationTarget.GetComponent<Animation>().Stop(); 
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
                    targetgameObject.SetActive(true);
                else targetgameObject.SetActive(false);
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

  

        IEnumerator LoadScene()
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMotor>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<HeadBob>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<MouseLook>().enabled = false;
            Camera.main.GetComponent<MouseLook>().enabled = false;
            GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>().enabled = false;


            yield return new WaitForSeconds(loadDelay);
            Application.LoadLevel(loadLevelName);
        }

  

    private void OnDrawGizmos()
    {
       
        Gizmos.color = triggerboxColor;
        if (!drawWire)
        Gizmos.DrawCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                            new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z));
        else
        {
            Gizmos.DrawWireCube(new Vector3(GetComponent<Collider>().bounds.center.x, GetComponent<Collider>().bounds.center.y, GetComponent<Collider>().bounds.center.z),
                           new Vector3(GetComponent<Collider>().bounds.size.x, GetComponent<Collider>().bounds.size.y, GetComponent<Collider>().bounds.size.z)); 
        }
    }
}