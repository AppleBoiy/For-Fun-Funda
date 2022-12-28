using System;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable()]
public struct SoundParameters
{
    [FormerlySerializedAs("Volume")] [Range(0, 1)]
    public float volume;
    [FormerlySerializedAs("Pitch")] [Range(-3, 3)]
    public float pitch;
    [FormerlySerializedAs("Loop")] public bool loop;
}
[System.Serializable()]
public class Sound
{
    #region Variables

    [SerializeField]    String              name            = String.Empty;
    public              String              Name            { get { return name; } }

    [SerializeField]    AudioClip           clip            = null;
    public              AudioClip           Clip            { get { return clip; } }

    [SerializeField]    SoundParameters     parameters      = new SoundParameters();
    public              SoundParameters     Parameters      { get { return parameters; } }

    [FormerlySerializedAs("Source")] [HideInInspector]
    public              AudioSource         source          = null;

    #endregion

    public void Play ()
    {
        source.clip = Clip;

        source.volume = Parameters.volume;
        source.pitch = Parameters.pitch;
        source.loop = Parameters.loop;

        source.Play();
    }
    public void Stop ()
    {
        source.Stop();
    }
}
public class AudioManager : MonoBehaviour {

    #region Variables

    public static       AudioManager    Instance        = null;

    [SerializeField]    Sound[]         sounds          = null;
    [SerializeField]    AudioSource     sourcePrefab    = null;

    [SerializeField]    String          startupTrack    = String.Empty;

    #endregion

    #region Default Unity methods

    /// <summary>
    /// Function that is called on the frame when a script is enabled just before any of the Update methods are called the first time.
    /// </summary>
    void Awake()
    {
        if (Instance != null)
        { Destroy(gameObject); }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        InitSounds();
    }
    /// <summary>
    /// Function that is called when the script instance is being loaded.
    /// </summary>
    void Start()
    {
        if (string.IsNullOrEmpty(startupTrack) != true)
        {
            PlaySound(startupTrack);
        }
    }

    #endregion

    /// <summary>
    /// Function that is called to initializes sounds.
    /// </summary>
    void InitSounds()
    {
        foreach (var sound in sounds)
        {
            AudioSource source = (AudioSource)Instantiate(sourcePrefab, gameObject.transform);
            source.name = sound.Name;

            sound.source = source;
        }
    }

    /// <summary>
    /// Function that is called to play a sound.
    /// </summary>
    public void PlaySound(string name)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            sound.Play();
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found! Issues occured at AudioManager.PlaySound()");
        }
    }
    /// <summary>
    /// Function that is called to stop a playing sound.
    /// </summary>
    public void StopSound(string name)
    {
        var sound = GetSound(name);
        if (sound != null)
        {
            sound.Stop();
        }
        else
        {
            Debug.LogWarning("Sound by the name " + name + " is not found! Issues occured at AudioManager.StopSound()");
        }
    }

    #region Getters

    Sound GetSound(string name)
    {
        foreach (var sound in sounds)
        {
            if (sound.Name == name)
            {
                return sound;
            }
        }
        return null;
    }

    #endregion
}