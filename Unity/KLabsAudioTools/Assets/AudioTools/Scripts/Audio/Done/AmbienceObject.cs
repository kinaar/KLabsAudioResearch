using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// AmbienceObject.cs by KLabsAudio is free to use and is made to create complex ambience systems in your games.
// If needed, please contact me at kinaarmusic@gmail.com

[AddComponentMenu("KLabsAudioTools/Ambience/AmbienceObject")]
public class AmbienceObject : MonoBehaviour
{   
    //// _General Audio Settings_ /////

    public GameObject m_ambienceManager; // Audio Manager (to instantiate AudioSources)
    public AudioMixerGroup m_outputMixerGroup; // Audio Mixer for audio output
    
    public float m_generalVolume = 0.0f;
    
    public bool m_muteGeneral = false; // General Mute
    public float m_fadeInTime = 1.0f; // Fade in time
    public float m_fadeOutTime = 1.0f; // Fade out time
    AmbienceManager managerScript; // Get Components from the AmbienceManager
    
    [HideInInspector]
    public AudioSource sourceBed; // Bed audio source

    //// _Bed Audio Settings_ ////

    public AudioClip m_bed; // Bed audioclip (needs to be perfectly looping)
    
    public float m_bedVolume = 0.0f;
    public bool m_muteBed = false; // Bed mute

    //// _Private Variables_ ////

    private bool fadeIn = false, fadeOut = false, done = true, generalMute = false, objectCreated = false;
    private float generalVolume = 0.0f, bedVol, bedVolCopy = 0.0f, randVol, randVolCopy = 0.0f; /// Volume Floats
    private float chronometer = 0.0f, randTime;
    AudioSource spatialSoundSource;
    private GameObject child;

    void Start()
    {
        managerScript = m_ambienceManager.gameObject.GetComponent<AmbienceManager>();
        generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol)/20.0f);
        bedVol = Mathf.Pow(10, m_bedVolume/20.0f);
        bedVolCopy = bedVol;
    }

    // Update is called once per frame
    void Update()
    {

        if (sourceBed != null)
        {

            generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol) / 20.0f);
            bedVol = Mathf.Pow(10, m_bedVolume / 20.0f);
            //generalMute = managerScript.mute;

            fadingIn();
            fadingOut();
            
            if(m_muteGeneral) muted(m_muteGeneral, sourceBed);
            else muted(m_muteBed, sourceBed);

            if (fadeOut == false && fadeIn == false && done == true)
            {
                setAudioVolume(bedVolCopy, bedVol, sourceBed);
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        sourceBed = m_ambienceManager.gameObject.AddComponent<AudioSource>();
        sourceBed.outputAudioMixerGroup = m_outputMixerGroup;

        if (collider.gameObject.tag == "Player")
        {
            sourceBed.clip = m_bed;
            sourceBed.playOnAwake = false;
            sourceBed.loop = true;
            sourceBed.volume = 0.0f;
            done = false;
            fadeIn = true;
            sourceBed.Play();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            done = false;
            fadeOut = true;
        }
    }

    void fadingIn()
    {
        if (fadeIn == true && done == false)
            {
                sourceBed.volume += Time.deltaTime / m_fadeInTime;
                if (sourceBed.volume >= bedVol)
                {
                    fadeIn = false;
                }
            }
            else if (fadeOut == false && done == false)
            {
                sourceBed.volume = bedVol;
                done = true;
            }
    }

    void fadingOut()
    {
        if (fadeOut == true && done == false)
        {
            sourceBed.volume -= Time.deltaTime / m_fadeOutTime;

            if (sourceBed.volume <= 0.0f)
            {
                sourceBed.Stop();
                Destroy(sourceBed);
                fadeOut = false;
                done = true;
                //randomSoundsSource.Stop();
            }
        }
    }

    void muted(bool m_muted, AudioSource audioSourceToMute)
    {
        if(m_muted)
        {
            audioSourceToMute.mute = true;
        }
        else
        {
            audioSourceToMute.mute = false;
        }
    }

    void setAudioVolume(float volCopy, float volOg, AudioSource audioSource)
    {
        if(volCopy != volOg)
        {
            audioSource.volume = volOg;
            volCopy = volOg;
        }
    }
}
