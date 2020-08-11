using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// AmbienceManager.cs by KLabsAudio is free to use and is made to create complex ambience systems in your games.
// If needed, please contact me at kinaarmusic@gmail.com

[AddComponentMenu("KLabsAudioTools/Ambience/AmbienceManager")]
public class AmbienceManager : MonoBehaviour
{
    [Header("(Please use the mixer for general output volume)")]
    [Space]
    [Header("General Settings")]
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    public float ambienceGeneralVol = 0.0f;
    [Space]
    public bool mute = false;
    private bool muteCopy;
    private float generalVolCopy = 0.0f;
    [HideInInspector]
    //public AudioSource ambienceAudioSource;
    GameObject[] ambienceObjects;

    void Start()
    {
        muteCopy = mute;
        generalVolCopy = ambienceGeneralVol;
    }

    // Update is called once per frame
    void Update()
    {
        if (muteCopy != mute)
        {
            isMuted();
            muteCopy = mute;
        }

        if(generalVolCopy != ambienceGeneralVol)
        {
            volumeChanged();
            generalVolCopy = ambienceGeneralVol;
        }

    }

    void isMuted()
    {
        Component[] audioSourcestoMute;
        audioSourcestoMute = GetComponents(typeof(AudioSource));
        if (mute == true)
        {
            foreach (AudioSource sources in audioSourcestoMute)
            {
                sources.mute = true;
            }

        }
        else
        {
            foreach (AudioSource sources in audioSourcestoMute)
            {
                sources.mute = false;
            }
        }
    }

    void volumeChanged()
    {
        Component[] audioSourcestoMute;
        audioSourcestoMute = GetComponents(typeof(AudioSource));
        float genVol = Mathf.Pow(10, ambienceGeneralVol/20);
        foreach (AudioSource sources in audioSourcestoMute)
        {
            sources.volume = genVol;
        }
    }
}
