using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicScript : MonoBehaviour
{
    public AudioMixerGroup m_outputMixerGroup; // Audio Mixer for audio output
    private float m_generalVolume = 0.0f;
    public bool m_muteGeneral = false; // General Mute
    public float m_userBpm = 120.0f;


    public playType m_playType;
    public enum playType { OnTriggerEnter, onAwake }
    public GameObject m_triggerObject;
    public AudioClip m_musicalSegment;
    public int m_segmentBarLength = 4;
    public float m_fadeInTime = 1.0f;

    AudioSource segmentSourceA, segmentSourceB;
    double dspTime = 0.0f, dspCopy = 0.0f;
    [HideInInspector]
    public double nextEventTime = 0.0f;
    bool faded = false;
    int counter = 0;
    [HideInInspector]
    public int loopNumber = 1;
    bool playing = false, toFade = false;
    float crossTime = 1.0f;

    AudioSource sourceA, sourceB, sourceCopy;
    List<AudioClip> audioList = new List<AudioClip>();
    List<AudioSource> audioSourceList = new List<AudioSource>();
    int nbTrigger = 0;
    bool triggerEnter, triggerEnterCopy;

    void Start()
    {
        segmentSourceA = gameObject.AddComponent<AudioSource>();
        //segmentSourceB = gameObject.AddComponent<AudioSource>();

        segmentSourceA.clip = m_musicalSegment;
        segmentSourceA.volume = 0.0f;
        segmentSourceA.loop = true;
        
        triggerEnterCopy = triggerEnter;

    }

    // Update is called once per frame
    void Update()
    {

        if (m_fadeInTime == 0.0f)
        {
            segmentSourceA.volume = 1.0f;
        }
        else if (m_fadeInTime != 0.0f && !faded)
        {
            fadeIn();
        }

        if (m_playType == playType.onAwake && !segmentSourceA.isPlaying && !playing)
        {

            dspTime = AudioSettings.dspTime;
            dspCopy = dspTime;
            segmentSourceA.loop = true;
            segmentSourceA.volume = 0.0f;
            segmentSourceA.PlayScheduled(dspTime);
            nextEventTime = dspTime;
            sourceB = segmentSourceA;
            playing = true;
        }

        if(m_playType == playType.OnTriggerEnter && !segmentSourceA.isPlaying && !playing)
        {
            triggerEnter = m_triggerObject.GetComponent<MusicObject>().triggerEntered;

            if (triggerEnterCopy != triggerEnter)
            {
                dspTime = AudioSettings.dspTime;
                dspCopy = dspTime;
                segmentSourceA.loop = true;
                segmentSourceA.volume = 0.0f;
                segmentSourceA.PlayScheduled(dspTime);
                nextEventTime = dspTime;
                sourceB = segmentSourceA;

                audioSourceList.Add(segmentSourceA);
                nbTrigger++;

                playing = true;
                triggerEnterCopy = triggerEnter;
            }
        }

        if(playing)
        {
            dspTime = AudioSettings.dspTime;
            bpmCounter();
        }

        if(toFade && AudioSettings.dspTime > nextEventTime)
        {
            crossFade(sourceA, sourceB);
        }

    }

    void fadeIn()
    {
        segmentSourceA.volume += Time.deltaTime / m_fadeInTime;

        if (segmentSourceA.volume >= 1.0f)
        {
            faded = true;
        }
    }


    void bpmCounter()
    {

        double beatInterval = 60.0f / m_userBpm;

        if (dspTime - dspCopy >= beatInterval)
        {
            dspCopy -= dspTime;
            counter += 1;
            //Debug.Log(counter);
            if (counter == m_segmentBarLength)
            {
                counter = 0;
                loopNumber += 1;
                //Debug.Log(loopNumber);
            }
            dspCopy = dspTime;
        }
    }

    public void musicTrigger(bool trigger, GameObject objTrigger, AudioClip clip, int barLength)
    {
        Debug.Log(nbTrigger);

        if (trigger)
        {
            playing = true;

            if(loopNumber == 0) loopNumber = 1;

            nextEventTime += (60.0f / m_userBpm * barLength) * (loopNumber);

            sourceA = gameObject.AddComponent<AudioSource>();
            sourceA.loop = true;
            sourceA.clip = clip;

            audioSourceList.Add(sourceA);

            sourceA.PlayScheduled(nextEventTime);
            sourceB.SetScheduledEndTime(nextEventTime);
            sourceCopy = sourceB;
            sourceB = sourceA;

            loopNumber = 0;
        }
        else
        {
            nbTrigger -= 1;

            sourceCopy = audioSourceList[nbTrigger-1];
            sourceA = audioSourceList[nbTrigger];

            if(loopNumber == 0) loopNumber = 1;

            nextEventTime += (60.0f / m_userBpm * barLength) * (loopNumber);

            sourceCopy.PlayScheduled(nextEventTime);
            sourceA.SetScheduledEndTime(nextEventTime);
            audioSourceList.Remove(sourceA);
            sourceB = sourceCopy;

            loopNumber = 0;
        }

        nbTrigger++;
    }

    public void fade(bool trigger,  GameObject objTrigger, AudioClip clip, int barLength, float crossfadeTime)
    {
        if(trigger)
        {
            nbTrigger++;
            sourceA = gameObject.AddComponent<AudioSource>();
            sourceA.loop = true;
            sourceA.clip = clip;
            sourceA.volume = 0.0f;
            audioSourceList.Add(sourceA);

            crossTime = crossfadeTime;

            if(loopNumber == 0) loopNumber = 1;

            nextEventTime += (60.0f / m_userBpm * barLength) * (loopNumber);

            sourceA.PlayScheduled(nextEventTime);
            toFade = true;
            //sourceB.SetScheduledEndTime(nextEventTime);

            loopNumber = 0;
        }
        else
        {
            nbTrigger -= 1;

            sourceCopy = audioSourceList[nbTrigger-1];
            sourceA = audioSourceList[nbTrigger];
            //sourceCopy.volume = 1.0f;

            if(loopNumber == 0) loopNumber = 1;

            nextEventTime += (60.0f / m_userBpm * barLength) * (loopNumber);

            sourceCopy.PlayScheduled(nextEventTime);
            audioSourceList.Remove(sourceA);
            sourceB = sourceA;
            sourceA = sourceCopy;
            toFade = true;

            loopNumber = 0;
        }
    }

    void crossFade(AudioSource audioA, AudioSource audioB)
    {
        audioA.volume += Time.deltaTime / crossTime;
        audioB.volume -= Time.deltaTime / crossTime;
        Debug.Log("crossfading..");

        if(audioB.volume <= 0)
        {
            //audioB.Stop();
            toFade = false;
            sourceCopy = sourceB;
            sourceB = sourceA;
            Debug.Log("audioBstopped");
        }
    }

    public void stop(bool trigger,  GameObject objTrigger, AudioClip clip, int barLength, float stopTime)
    {
        sourceA = gameObject.AddComponent<AudioSource>();
        crossTime = stopTime;
        toFade = true;
    }

}
