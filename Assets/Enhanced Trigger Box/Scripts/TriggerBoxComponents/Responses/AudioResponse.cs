using System.Collections;
using UnityEngine;

namespace EnhancedTriggerbox.Component
{
    /// <summary>
    /// This response allows you to play music on an audio source, stop music or play a audio clip at a specified position
    /// </summary>
    [AddComponentMenu("")]
    public class AudioResponse : ResponseComponent
    {
        /// <summary>
        /// The audio source for the music
        /// </summary>
        public AudioSource audioSource;

        /// <summary>
        /// This is the audio clip that will be played on the main camera.
        /// </summary>
        public AudioClip playMusic;

        /// <summary>
        /// If this is true, the above audio clip will loop when played.
        /// </summary>
        public bool loopMusic;

        /// <summary>
        /// The volume of the audio clip. Default is 1.
        /// </summary>
        public float musicVolume = 1f;

        /// <summary>
        /// This is an audio clip, played at a certain position in world space as defined below.
        /// </summary>
        public AudioClip playSoundEffect;

        /// <summary>
        /// The position the sound effect will be played at.
        /// </summary>
        public Transform soundEffectPosition;

        /// <summary>
        /// The type of response that this component will be using. AudioSource allows you to modify an audio source and SoundEffect allows you to play positional sound effects
        /// </summary>
        public ResponseType responseType;

        /// <summary>
        /// The type of action to be performed on the audio source. Play allows you to play an Audio Clip, Stop stops an audio source's currently playing clip,
        /// Restart put the time of the audio clip back to 0 and ChangeVolume sets the volume of the audio source.
        /// </summary>
        public AudioSourceAction audioSourceAction;

        public enum ResponseType
        {
            AudioSource,
            SoundEffect,
        }

        public enum AudioSourceAction
        {
            Play,
            Stop,
            Restart,
            ChangeVolume
        }

#if UNITY_EDITOR
        public override void DrawInspectorGUI()
        {
            responseType = (ResponseType)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Response Type",
                "The type of response that this component will be using. AudioSource allows you to modify an audio source and SoundEffect allows you to play positional sound effects"), responseType);

            if (responseType == ResponseType.AudioSource)
            {
                audioSource = (AudioSource)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Audio Source",
               "The audio source."), audioSource, typeof(AudioSource), true);

                audioSourceAction = (AudioSourceAction)UnityEditor.EditorGUILayout.EnumPopup(new GUIContent("Audio Source Action",
                "The type of action to be performed on the audio source. Play allows you to play an Audio Clip, Stop stops an audio source's currently playing clip, Restart put the time of the audio clip back to 0 and ChangeVolume sets the volume of the audio source."), audioSourceAction);

                if (audioSourceAction == AudioSourceAction.Play)
                {
                    playMusic = (AudioClip)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Audio Clip",
                    "This is the audio clip that will be played on the audio source."), playMusic, typeof(AudioClip), true);

                    loopMusic = UnityEditor.EditorGUILayout.Toggle(new GUIContent("Loop Music",
                    "If this is true, the above audio clip will loop when played."), loopMusic);

                    musicVolume = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Music Volume",
                        "The volume of the audio clip. Default is 1."), musicVolume);
                }    
                else if (audioSourceAction == AudioSourceAction.ChangeVolume)
                {
                    musicVolume = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Music Volume",
                        "The volume of the audio clip. Default is 1."), musicVolume);

                    duration = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Change Duration",
                    "The duration that the volume change will happen over in seconds. If you leave it as 0 it will perform the changes instantly."), duration);
                }
            }
            else if (responseType == ResponseType.SoundEffect)
            {
                playSoundEffect = (AudioClip)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Play Sound Effect",
                "This is an audio clip, played at a certain position in world space as defined below."), playSoundEffect, typeof(AudioClip), true);

                soundEffectPosition = (Transform)UnityEditor.EditorGUILayout.ObjectField(new GUIContent("Sound Effect Position",
                    "The position the sound effect will be played at."), soundEffectPosition, typeof(Transform), true);

                musicVolume = UnityEditor.EditorGUILayout.FloatField(new GUIContent("Sound Effect Volume",
                        "The volume of the sound effect. Default is 1."), musicVolume);
            }
        }
#endif

        public override void Validation()
        {
            if (responseType == ResponseType.AudioSource)
            {
                switch (audioSourceAction)
                {
                    case AudioSourceAction.Stop:
                        if (!audioSource)
                            ShowWarningMessage("You have chosen to stop an audio source but haven't specified an Audio Source.");
                        break;

                    case AudioSourceAction.ChangeVolume:
                        if (!audioSource)
                            ShowWarningMessage("You have chosen to change the volume on an audio source but haven't specified an Audio Source.");
                        break;

                    case AudioSourceAction.Play:
                        if (!playMusic)
                            ShowWarningMessage("You have chosen to play an audio source but haven't specified an Audio Clip.");
                        if (!audioSource)
                            ShowWarningMessage("You have chosen to play an audio source but haven't specified an Audio Source.");
                        break;

                    case AudioSourceAction.Restart:
                        if (!audioSource)
                            ShowWarningMessage("You have chosen to restart an audio source but haven't specified an Audio Source.");
                        break;
                }
            }
            else
            {
                if (!soundEffectPosition)
                {
                    ShowWarningMessage("You have chosen to play a sound effect but haven't set a position for it to play at!");
                }
            }
        }

        public override bool ExecuteAction()
        {
            if (responseType == ResponseType.AudioSource)
            {
                switch (audioSourceAction)
                {
                    case AudioSourceAction.Stop:
                        audioSource.Stop();
                        break;

                    case AudioSourceAction.ChangeVolume:
                        // If duration isn't 0 then we'll apply the changes over a set duration using a coroutine
                        if (duration != 0f)
                        {
                            activeCoroutines.Add(StartCoroutine(ChangeVolumeOverTime()));
                        }
                        else // Else we'll instantly apply the changes
                        {
                            audioSource.volume = musicVolume;
                        }   
                        break;

                    case AudioSourceAction.Play:
                        audioSource.loop = loopMusic;  
                        audioSource.volume = musicVolume; 

                        if (playMusic)
                            audioSource.clip = playMusic;
                         
                        audioSource.Play();
                        break;

                    case AudioSourceAction.Restart:
                        audioSource.time = 0;
                        break;
                }
            }
            else
            {
                // This will play the audio clip at soundEffectPosition's position with the volume of musicVolume
                AudioSource.PlayClipAtPoint(playSoundEffect, soundEffectPosition.position, musicVolume);
            }

            return false;
        }

        private IEnumerator ChangeVolumeOverTime()
        {
            float smoothness = 0.02f; // Should the user be able to set this?
            float progress = 0; // This float will serve as the 3rd parameter of the lerp function.
            float increment = smoothness / duration; // The amount of change to apply.

            float startVolume = audioSource.volume;

            while (progress < 1)
            {
                audioSource.volume = Mathf.Lerp(startVolume, musicVolume, progress); 

                progress += increment;
                yield return new WaitForSeconds(smoothness);
            }
        }
    }
}