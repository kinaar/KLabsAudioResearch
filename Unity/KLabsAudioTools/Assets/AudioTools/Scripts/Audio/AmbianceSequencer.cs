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
        ambianceBedSource.Play();

        randomSoundsSource.playOnAwake = false;

        randTime = timeBetweenInst;

        bedVol = Mathf.Pow(10, bedVolume/20);
        randVol = Mathf.Pow(10, randomVolume/20);

    }

    // Update is called once per frame
    void Update()
    {
        
        chronometer += Time.deltaTime;

        bedVol = Mathf.Pow(10, bedVolume/20);
        randVol = Mathf.Pow(10, randomVolume/20);

        ambianceBedSource.volume = bedVol;

        if(chronometer >= randTime)
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
}
