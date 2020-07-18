using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSequencer : MonoBehaviour
{
    AudioSource ambianceBedSource;
    AudioSource randomSoundsSource;
    AudioLowPassFilter lowpassRandom;

    [Header("General Settings")]
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float bedVolume = 0.0f;

    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float randomVolume = 0.0f;


    [Header("Audio Clips")] //// Audio clips
    public AudioClip ambianceBed;
    public AudioClip[] randomSounds;
    

    [Header("Settings")] //// Settings for the random
    public float timeBetweenInst = 5.0f;
    public float timeRandom = 1.0f;
    public bool lowpassFilterOn = false;

    [Range(0, 100)]
    [SerializeField]
    private float panRandomization;

    [Range(0, 100)]
    [SerializeField]
    private float volRandomization;

    private float chronometer = 0.0f;
    private float randTime;
    private float bedVol = 0.0f;
    private float randVol = 0.0f;
    private float volFade = 0.0f;

    private bool fadeIn = false, fadeOut = false;

    // Start is called before the first frame update
    void Start()
    {
        
        //ambianceBedSource.Play();
        //ambianceBedSource.loop = true;
        ambianceBedSource = gameObject.AddComponent<AudioSource>();
        
        if(lowpassFilterOn == true)
        {
            lowpassRandom = gameObject.AddComponent<AudioLowPassFilter>();
            lowpassRandom.cutoffFrequency = 400.0f;
        }
        randomSoundsSource = gameObject.AddComponent<AudioSource>();

        ambianceBedSource.clip = ambianceBed;
        ambianceBedSource.playOnAwake = false;
        ambianceBedSource.loop = true;
        //ambianceBedSource.Play();

        randomSoundsSource.playOnAwake = false;

        randTime = timeBetweenInst;

        bedVol = Mathf.Pow(10, bedVolume/20);
        randVol = Mathf.Pow(10, randomVolume/20);

        volFade = 0.0f;

    }

    // Update is called once per frame
    void Update()
    {
        
        chronometer += Time.deltaTime;

        bedVol = Mathf.Pow(10, bedVolume/20);
        randVol = Mathf.Pow(10, randomVolume/20);

        if(fadeIn)
        {
            ambianceBedSource.volume += Time.deltaTime / 1.0f;
            randomSoundsSource.volume += Time.deltaTime / 1.0f;
            if(ambianceBedSource.volume >= bedVol)
            {
                fadeIn = false;
                print("false");
            }
        }
        else if(fadeOut == false)
        {
            ambianceBedSource.volume = bedVol;
            randomSoundsSource.volume = randVol;
        }

        if(fadeOut)
        {
            ambianceBedSource.volume -= Time.deltaTime / 1.0f;
            randomSoundsSource.volume -= Time.deltaTime / 1.0f;
            
            if(ambianceBedSource.volume <= 0.0f)
            {
                fadeOut = false;
                ambianceBedSource.Stop();
                randomSoundsSource.Stop();
                print("false");
            }
        }
        

        if(chronometer >= randTime && ambianceBedSource.isPlaying)
        {
            int randNb = Random.Range(0, randomSounds.Length);
            randomSoundsSource.clip = randomSounds[randNb];
            randomSoundsSource.Play();
            
            randTime = Random.Range(-1.0f, 1.0f) + timeBetweenInst;
            
            float panRandom = Random.Range(-1.0f, 1.0f)*(panRandomization/100);
            float volRandom = randVol - (Random.Range(0.0f, 1.0f)/100);
            randomSoundsSource.panStereo = panRandom;
            randomSoundsSource.volume = volRandom;

            chronometer = 0.0f;
        }
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            ambianceBedSource.volume = 0.0f;
            fadeIn = true;
            ambianceBedSource.Play();
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            //volFade = 0.0f;
            //ambianceBedSource.Stop();
            fadeOut = true;
        }
    }
}
