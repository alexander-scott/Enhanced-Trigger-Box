using UnityEngine;
using UnityEditor;
using System;

public class AudioResponse : EnhancedTriggerBoxComponent
{
    /// <summary>
    /// Stops the current audio clip being played on the main camera.
    /// </summary>
    public bool muteAllAudio;

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

    public override void DrawInspectorGUI()
    {
        muteAllAudio = EditorGUILayout.Toggle(new GUIContent("Mute all audio",
            "Stops the current audio clip being played on the main camera."), muteAllAudio);

        playMusic = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Play Music",
            "This is the audio clip that will be played on the main camera."), playMusic, typeof(AudioClip), true);

        loopMusic = EditorGUILayout.Toggle(new GUIContent("Loop Music",
            "If this is true, the above audio clip will loop when played."), loopMusic);

        musicVolume = EditorGUILayout.FloatField(new GUIContent("Music Volume",
                "The volume of the audio clip. Default is 1."), musicVolume);

        playSoundEffect = (AudioClip)EditorGUILayout.ObjectField(new GUIContent("Play Sound Effect",
            "This is an audio clip, played at a certain position in world space as defined below."), playSoundEffect, typeof(AudioClip), true);

        soundEffectPosition = (Transform)EditorGUILayout.ObjectField(new GUIContent("Sound Effect Position",
            "The position the sound effect will be played at."), soundEffectPosition, typeof(Transform), true);
    }

    public override void Validation()
    {
        // If we're playing a sound effect a position must be specified
        if (playSoundEffect)
        {
            if (!soundEffectPosition)
            {
                ShowWarningMessage("You have chosen to play a sound effect but haven't set a position for it to play at!");
            }
        }
    }

    public override bool ExecuteAction()
    {
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
            // This will play the audio clip at soundEffectPosition's position with the volume of musicVolume
            AudioSource.PlayClipAtPoint(playSoundEffect, soundEffectPosition.position, musicVolume);
        }

        return true;
    }
}
