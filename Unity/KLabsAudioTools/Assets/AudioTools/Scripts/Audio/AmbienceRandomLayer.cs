using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KLabsAudioTools/Ambience/AmbienceRandomLayer")]
[RequireComponent(typeof(AmbienceObject))]
public class AmbienceRandomLayer : MonoBehaviour
{
//// _General Audio Settings_ /////

    [Header("General Settings")]
    private GameObject ambienceManager; // Audio Manager (to instantiate AudioSources)
    
    //public bool m_muteGeneral = false; // General Mute
    private float m_fadeInTime = 1.0f; // Fade in time
    private float m_fadeOutTime = 1.0f; // Fade out time
    AmbienceManager managerScript; // Get Components from the AmbienceManager
    
    AudioSource sourceBed; // Bed audio source
    AudioSource sourceRand; // Random audio Source

    //// _Random Settings_ ////

    //[Header("Random Settings")]
    public AudioClip[] m_randomClips; // Random audioclips

    public bool m_spatialize = false;
    public Vector3 m_randomZoneMin = new Vector3(5.0f, 0.5f, 5.0f);
    public Vector3 m_randomZoneMax = new Vector3(20.0f, 1.5f, 20.0f);
    public float m_minAttenuationDistance = 1.0f;
    public float m_maxAttenuationDistance = 100.0f;
    public float m_spread = 0.0f;
    public m_volumeRolloff m_Rolloff;
    public enum m_volumeRolloff { logarithmicRolloff, linearRolloff };

    public float m_randomVolume = 0.0f;
    public bool m_muteRandom = false; // Random mute

    public float m_triggerTime = 5.0f; // Time (in seconds) to wait between each instantiation
    public float m_randomTime = 1.0f; // Random time (in seconds) added or removed from the instantiation time

    public float m_panRandomization;

    public float m_volRandomization;
    


    //// _Private Variables_ ////

    private bool fadeIn = false, fadeOut = false, done = true, generalMute = false, objectCreated = false;
    private float generalVolume = 0.0f, bedVol, bedVolCopy = 0.0f, randVol, randVolCopy = 0.0f; /// Volume Floats
    private float chronometer = 0.0f, randTime;
    private AudioSource spatialSoundSource;
    private GameObject child;
    private bool playing = false;

    void Start()
    {
        ambienceManager = gameObject.GetComponent<AmbienceObject>().ambienceManager;
        managerScript = ambienceManager.gameObject.GetComponent<AmbienceManager>();
        m_fadeInTime = gameObject.GetComponent<AmbienceObject>().m_fadeInTime;
        m_fadeOutTime = gameObject.GetComponent<AmbienceObject>().m_fadeOutTime;

        //generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol)/20.0f);
        //bedVol = Mathf.Pow(10, m_bedVolume/20.0f);
        //bedVolCopy = bedVol;
    }

    // Update is called once per frame
    void Update()
    {
        if (sourceRand != null)
        {
            chronometer += Time.deltaTime;
            randTime = Random.Range(-1.0f, 1.0f) + m_triggerTime;
        }

        //generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol) / 20.0f);
        //bedVol = Mathf.Pow(10, m_bedVolume / 20.0f);
        randVol = Mathf.Pow(10, m_randomVolume / 20.0f);
        //generalMute = managerScript.mute;

        fadingIn();
        fadingOut();

        if (fadeOut == false && fadeIn == false && done == true)
        {
            //setAudioVolume(bedVolCopy, bedVol, sourceBed);
        }


        if (chronometer >= randTime && playing && m_randomClips.Length > 0)
        {
            if (m_spatialize)
            {
                if (objectCreated == false)
                {
                    child = new GameObject("Player");
                    spatialSoundSource = child.AddComponent<AudioSource>();
                    objectCreated = true;
                }
                spatialiseSoundSpawn(child);
            }
            else
            {
                randomSoundPicking();
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        sourceRand = ambienceManager.gameObject.AddComponent<AudioSource>();
        sourceRand.outputAudioMixerGroup = gameObject.GetComponent<AmbienceObject>().m_outputMixerGroup;

        if (collider.gameObject.tag == "Player")
        {
            done = false;
            fadeIn = true;
            playing = true;
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
            sourceRand.volume += Time.deltaTime / m_fadeInTime;
            if (sourceRand.volume >= bedVol)
            {
                fadeIn = false;
            }
        }
        else if (fadeOut == false && done == false)
        {
            done = true;
            sourceRand.volume = randVol;
        }
    }

    void fadingOut()
    {
        if (fadeOut == true && done == false)
        {
            sourceRand.volume -= Time.deltaTime / m_fadeOutTime;

            if (sourceRand.volume <= 0.0f)
            {
                Destroy(sourceRand);
                fadeOut = false;
                done = true;
                playing = false;
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
        int randNb = Random.Range(0, m_randomClips.Length);
        sourceRand.clip = m_randomClips[randNb];
        sourceRand.Play();

        randTime = Random.Range(-1.0f, 1.0f) + m_triggerTime;

        float panRandom = Random.Range(-1.0f, 1.0f) * (m_panRandomization / 100);
        float volRandom = randVol - (Random.Range(0.0f, 1.0f) / 100);
        sourceRand.panStereo = panRandom;
        sourceRand.volume = volRandom;

        chronometer = 0.0f;
    }

    void spatialiseSoundSpawn(GameObject child)
    {


        float spatialRandX = Random.Range(m_randomZoneMin.x, m_randomZoneMax.x);
        float spatialRandY = Random.Range(m_randomZoneMin.y, m_randomZoneMax.y);
        float spatialRandZ = Random.Range(m_randomZoneMin.z, m_randomZoneMax.z);
        int randomSign = Random.Range(0,1);

        if(randomSign == 0)
        {
            child.transform.position = new Vector3(gameObject.transform.position.x + spatialRandX, spatialRandY, transform.position.z + spatialRandZ);
        }
        else
        {
            child.transform.position = new Vector3(gameObject.transform.position.x - spatialRandX, spatialRandY, transform.position.z - spatialRandZ);
        }

        child.transform.parent = gameObject.transform;
        Debug.Log("New");
        int randNb = Random.Range(0, m_randomClips.Length);
        //spatialSoundSource.clip = m_randomClips[randNb];
        spatialSoundSource.spatialBlend = 1.0f;
        spatialSoundSource.minDistance = m_minAttenuationDistance;
        spatialSoundSource.maxDistance = m_maxAttenuationDistance;
        spatialSoundSource.spread = m_spread;

        if(m_Rolloff == m_volumeRolloff.logarithmicRolloff)
        {
            spatialSoundSource.rolloffMode = AudioRolloffMode.Logarithmic;
        }
        else if(m_Rolloff == m_volumeRolloff.linearRolloff)
        {
            spatialSoundSource.rolloffMode = AudioRolloffMode.Linear;
        }

        spatialSoundSource.volume = Mathf.Pow(10, m_randomVolume/20.0f);
        spatialSoundSource.PlayOneShot(m_randomClips[randNb]);
        chronometer = 0.0f;
    }

}
