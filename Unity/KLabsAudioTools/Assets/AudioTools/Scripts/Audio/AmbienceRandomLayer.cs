using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Random Settings")]
    public AudioClip[] randomSounds; // Random audioclips
    public bool spatializeRandomSource = false;

    [Range(-48.0f, 3.0f)] // Random volume
    [SerializeField]
    public float m_randomVolume = 0.0f;

    public bool m_muteRandom = false; // Random mute
    public float timeBetweenInst = 5.0f; // Time (in seconds) to wait between each instantiation
    public float timeRandom = 1.0f; // Random time (in seconds) added or removed from the instantiation time

    [Range(0, 100)] // Pan randomization (in percents)
    [SerializeField]
    public float panRandomization;

    [Range(0, 100)] // Volume randomization (in percents)
    [SerializeField]
    public float volRandomization;
    


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
            randTime = Random.Range(-1.0f, 1.0f) + timeBetweenInst;
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


        if (chronometer >= randTime && playing && randomSounds.Length > 0)
        {
            if (spatializeRandomSource)
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

    void spatialiseSoundSpawn(GameObject child)
    {


        int spatialRand = Random.Range(0, 30);
        child.transform.position = new Vector3(gameObject.transform.position.x + spatialRand, 1.5f, transform.position.z + spatialRand);
        child.transform.parent = gameObject.transform;
        Debug.Log("New");
        int randNb = Random.Range(0, randomSounds.Length);
        //spatialSoundSource.clip = randomSounds[randNb];
        spatialSoundSource.spatialBlend = 1.0f;
        spatialSoundSource.volume = Mathf.Pow(10, m_randomVolume/20.0f);
        spatialSoundSource.PlayOneShot(randomSounds[randNb]);
        chronometer = 0.0f;
    }

}
