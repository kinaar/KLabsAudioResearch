using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceSequencer : MonoBehaviour
{

    AudioSource ambianceBedSource;
    AudioSource randomSoundsSource;
    public AudioClip ambianceBed;
    public AudioClip[] randomSounds;
    
    public float fadeTime = 1.0f;

    [Range(0, 100)]
    [SerializeField]
    private int panRandomization;
    private float chronometer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
        //ambianceBedSource.Play();
        //ambianceBedSource.loop = true;
        ambianceBedSource = gameObject.AddComponent<AudioSource>();
        randomSoundsSource = gameObject.AddComponent<AudioSource>();

        ambianceBedSource.clip = ambianceBed;
        ambianceBedSource.playOnAwake = false;
        ambianceBedSource.loop = true;
        ambianceBedSource.Play();

        randomSoundsSource.playOnAwake = false;

    }

    // Update is called once per frame
    void Update()
    {
        
        chronometer += Time.deltaTime;
        //randomSoundsSource.clip = randomSounds[Random.Range(0, randomSounds.Length)];
        //ambianceSource.PlayOneShot(randomSounds);
        
    }
}
