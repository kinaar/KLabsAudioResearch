using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbienceObject : MonoBehaviour
{
    [Header("General Settings")]
    public GameObject ambienceManager;
    public AudioMixerGroup m_outputMixerGroup;
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float m_generalVolume = 0.0f;
    public bool m_muteGeneral = false;
    public float m_fadeInTime = 1.0f;
    public float m_fadeOutTime = 1.0f;
    AmbienceManager managerScript;
    
    [HideInInspector]
    public AudioSource sourceBed;
    AudioSource sourceRand;

    [Header("Bed Settings")] //// Bed Audio Settings
    public AudioClip m_bed;
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float m_bedVolume = 0.0f;
    public bool m_muteBed = false;
    
    
    [Header("Random Settings")] ///// Random Settings
    public AudioClip[] randomSounds;
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float m_randomVolume = 0.0f;
    public bool m_muteRandom = false;
    public float timeBetweenInst = 5.0f;
    public float timeRandom = 1.0f;
    [Range(0, 100)]
    [SerializeField]
    private float panRandomization;
    [Range(0, 100)]
    [SerializeField]
    private float volRandomization;


    ////// Private Variables
    private bool fadeIn = false, fadeOut = false, done = true, generalMute = false;
    private float generalVolume = 0.0f, bedVol, bedVolCopy = 0.0f, randVol, randVolCopy = 0.0f; /// Volume Floats
    private float chronometer = 0.0f, randTime;

    void Start()
    {
        managerScript = ambienceManager.gameObject.GetComponent<AmbienceManager>();
        generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol)/20.0f);
        bedVol = Mathf.Pow(10, m_bedVolume/20.0f);
        bedVolCopy = bedVol;
    }

    // Update is called once per frame
    void Update()
    {

        if (sourceBed != null)
        {
            chronometer += Time.deltaTime;

            generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol)/20.0f);
            bedVol = Mathf.Pow(10, m_bedVolume/20.0f);
            randVol = Mathf.Pow(10, m_randomVolume/20.0f);
            generalMute = managerScript.mute;

            fadingIn();
            fadingOut();

            if (generalMute != true)
            {
                if (m_muteGeneral != true)
                {
                    muted(m_muteBed, sourceBed);
                }
                else muted(m_muteGeneral, sourceBed);
            }

            if (fadeOut == false && fadeIn == false && done == true)
            {
                setAudioVolume(bedVolCopy, bedVol, sourceBed);
            }


            if (chronometer >= randTime && sourceBed.isPlaying && randomSounds.Length > 0)
            {
                randomSoundPicking();
            }

        }

    }

    void OnTriggerEnter(Collider collider)
    {
        sourceBed = ambienceManager.gameObject.AddComponent<AudioSource>();
        sourceBed.outputAudioMixerGroup = m_outputMixerGroup;
        sourceRand = ambienceManager.gameObject.AddComponent<AudioSource>();
        sourceRand.outputAudioMixerGroup = m_outputMixerGroup;

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
                sourceRand.volume += Time.deltaTime / m_fadeInTime;
                if (sourceBed.volume >= bedVol)
                {
                    fadeIn = false;
                }
            }
            else if (fadeOut == false && done == false)
            {
                sourceBed.volume = bedVol;
                done = true;
                sourceRand.volume = randVol;
            }
    }

    void fadingOut()
    {
        if (fadeOut == true && done == false)
        {
            sourceBed.volume -= Time.deltaTime / m_fadeOutTime;
            sourceRand.volume -= Time.deltaTime / m_fadeOutTime;

            if (sourceBed.volume <= 0.0f)
            {
                sourceBed.Stop();
                Destroy(sourceRand);
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

    void randomSoundPicking()
    {
        int randNb = Random.Range(0, randomSounds.Length);
        sourceRand.clip = randomSounds[randNb];
        sourceRand.Play();

        randTime = Random.Range(-1.0f, 1.0f) + timeBetweenInst;

        float panRandom = Random.Range(-1.0f, 1.0f) * (panRandomization / 100);
        float volRandom = randVol - (Random.Range(0.0f, 1.0f) / 100);
        sourceRand.panStereo = panRandom;
        sourceRand.volume = volRandom;

        chronometer = 0.0f;
    }

}
