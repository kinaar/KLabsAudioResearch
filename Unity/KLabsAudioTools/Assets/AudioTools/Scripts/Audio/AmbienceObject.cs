using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceObject : MonoBehaviour
{
    public GameObject ambienceManager;
    AmbienceManager managerScript;
    
    [HideInInspector]
    public AudioSource sourceBed;

    [Header("Audio Clips")] //// Audio clips
    public AudioClip m_bed;
    public AudioClip[] randomSounds;
    public float fadeTime = 1.0f;

    private bool fadeIn = false, fadeOut = false, done = true;

    void Start()
    {
        managerScript = ambienceManager.gameObject.GetComponent<AmbienceManager>();
    }

    // Update is called once per frame
    void Update()
    {

        if (sourceBed != null)
        {

            float generalVolume = Mathf.Pow(10, (managerScript.ambienceGeneralVol)/20.0f);

            if (fadeIn == true && done == false)
            {
                sourceBed.volume += Time.deltaTime / fadeTime;
                //randomSoundsSource.volume += Time.deltaTime / 1.0f;
                if (sourceBed.volume >= generalVolume)
                {
                    fadeIn = false;
                    print(generalVolume);
                }
            }
            else if (fadeOut == false && done == false)
            {
                sourceBed.volume = generalVolume;
                done = true;
                //randomSoundsSource.volume = randVol;
            }

            if (fadeOut == true && done == false)
            {
                sourceBed.volume -= Time.deltaTime / fadeTime;
                //randomSoundsSource.volume -= Time.deltaTime / 1.0f;

                if (sourceBed.volume <= 0.0f)
                {
                    fadeOut = false;
                    sourceBed.Stop();
                    Destroy(sourceBed);
                    done = true;
                    //randomSoundsSource.Stop();
                    print("false");
                }
            }
        }

    }

    void OnTriggerEnter(Collider collider)
    {
        sourceBed = ambienceManager.gameObject.AddComponent<AudioSource>();

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

}
