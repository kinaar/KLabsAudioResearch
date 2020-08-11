using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicScript : MonoBehaviour
{
    
    [Header("General Settings")]
    public AudioMixerGroup m_outputMixerGroup; // Audio Mixer for audio output
    private float m_generalVolume = 0.0f;
    public bool m_muteGeneral = false; // General Mute
    public float userBpm = 120.0f;


    [Header("First Segment Settings")]
    public playType m_playType;
    public enum playType { OnTriggerEnter, onAwake }
    public GameObject triggerObject;
    public AudioClip musicalSegment;
    public int m_segmentBarLength = 4;
    public float fadeInTime = 1.0f;

    AudioSource segmentSourceA, segmentSourceB;
    double dspTime = 0.0f, dspCopy = 0.0f;
    [HideInInspector]
    public double nextEventTime = 0.0f;
    bool faded = false;
    int counter = 0;
    [HideInInspector]
    public int loopNumber = 1;
    bool playing = false;

    AudioSource sourceA, sourceB, sourceCopy;

    void Start()
    {
        segmentSourceA = gameObject.AddComponent<AudioSource>();
        //segmentSourceB = gameObject.AddComponent<AudioSource>();

        segmentSourceA.clip = musicalSegment;
        segmentSourceA.volume = 0.0f;
        segmentSourceA.loop = true;
        

    }

    // Update is called once per frame
    void Update()
    {

        if (fadeInTime == 0.0f)
        {
            segmentSourceA.volume = 1.0f;
        }
        else if (fadeInTime != 0.0f && !faded)
        {
            fadeIn();
        }

        if (m_playType == playType.onAwake && !segmentSourceA.isPlaying && !playing)
        {

            dspTime = AudioSettings.dspTime;
            dspCopy = dspTime;
            segmentSourceA.loop = true;
            segmentSourceA.PlayScheduled(dspTime);
            nextEventTime = dspTime;
            sourceB = segmentSourceA;
            playing = true;
        }

        if(playing)
        {
            dspTime = AudioSettings.dspTime;
            bpmCounter();
        }

    }

    void fadeIn()
    {
        segmentSourceA.volume += Time.deltaTime / fadeInTime;

        if (segmentSourceA.volume >= 1.0f)
        {
            faded = true;
        }
    }


    void bpmCounter()
    {

        double beatInterval = 60.0f / userBpm;

        if (dspTime - dspCopy >= beatInterval)
        {
            dspCopy -= dspTime;
            counter += 1;
            //Debug.Log(counter);
            dspCopy = dspTime;
            if (counter == m_segmentBarLength)
            {
                counter = 0;
                loopNumber += 1;
                Debug.Log(loopNumber);
            }
            dspCopy = dspTime;
        }
    }

    public void musicTrigger(bool trigger, GameObject objTrigger, AudioClip clip, int barLength)
    {

        if (trigger)
        {
            playing = true;
            Debug.Log("Enter in " + objTrigger);

            if(loopNumber == 0) loopNumber = 1;

            nextEventTime += (60.0f / userBpm * barLength) * (loopNumber);

            if(sourceA != null)
            {
                //Destroy(sourceA);
            }

            sourceA = gameObject.AddComponent<AudioSource>();
            sourceA.loop = true;
            sourceA.clip = clip;
            sourceA.PlayScheduled(nextEventTime);
            sourceB.SetScheduledEndTime(nextEventTime);
            sourceCopy = sourceB;
            sourceB = sourceA;

            loopNumber = 0;
        }
        else
        {
            Debug.Log("Exit " + objTrigger);
            if(loopNumber == 0) loopNumber = 1;

            nextEventTime += (60.0f / userBpm * barLength) * (loopNumber);

            sourceCopy.PlayScheduled(nextEventTime);
            sourceA.SetScheduledEndTime(nextEventTime);
            sourceB = sourceCopy;

            loopNumber = 0;
        }
    }

}
