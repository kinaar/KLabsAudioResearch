using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class UnityExampleMusic : MonoBehaviour
{
    [Header("General Settings")]
    //public GameObject musicManager; // Audio Manager (to instantiate AudioSources)
    public AudioMixerGroup m_outputMixerGroup; // Audio Mixer for audio output
    
    public float userBpm = 120.0f;

    [System.Serializable]
    public class musicalSegments
    {
        public enum playType { OnTriggerEnter, onAwake }
        public playType m_playType;
        public GameObject triggerObject;

        public enum transitionType { fading, playNextSegment, stop }
        public transitionType m_transition;

        public AudioClip musicalSegment;
        public int m_segmentBarLength = 4;
    }
    public musicalSegments[] m_musicalSegments;

    //public AudioClip[] clips = new AudioClip[2];

    private double nextEventTime;
    private int flip = 0;
    private AudioSource[] audioSources = new AudioSource[2];
    private bool running = false;
    MusicObject objectscript;
    List<bool> triggerEntered = new List<bool>();
    bool playing = false, done = false, countBpm = false;
    int musicPlayingID = 0;
    double time = 0;
    double dspCopy = 0;
    int nbLoops = 1;
    int counter = 0;
    double beatTimer = 0;

    void Start()
    {
        for (int i = 0; i < m_musicalSegments.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].outputAudioMixerGroup = m_outputMixerGroup;
            audioSources[i].loop = true;
            triggerEntered.Add(m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered);
        }

        nextEventTime = AudioSettings.dspTime + 2.0f;
        running = true;
    }

    void Update()
    {
        if (!running)
        {
            return;
        }

        for (int i = 0; i < m_musicalSegments.Length; i++)
        {
            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;
            
            if (triggerEntered[i] == true && playing == false)
            {
                musicPlayingID = i;
                audioSources[i].clip = m_musicalSegments[i].musicalSegment;
                audioSources[i].Play();
                playing = true;
                nextEventTime = AudioSettings.dspTime;

                time = AudioSettings.dspTime;
                dspCopy = time;
                countBpm = true;
            }

            if(i != musicPlayingID && triggerEntered[i] == true && done == false)
            {
                nextEventTime += (60.0f / userBpm * m_musicalSegments[i].m_segmentBarLength)*(nbLoops);
                audioSources[i].clip = m_musicalSegments[i].musicalSegment;
                audioSources[musicPlayingID].SetScheduledEndTime(nextEventTime);
                audioSources[i].PlayScheduled(nextEventTime);
                m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                musicPlayingID = i;
                nbLoops = 1;
                done = true;
            }

            if(i != musicPlayingID && triggerEntered[i] == true && done == true)
            {
                nbLoops -= 1;
                done = false;
            }

        }

        if(countBpm)
        {
            bpmCounter();
        }


        time = AudioSettings.dspTime;

        /*if (time + 1.0f > nextEventTime)
        {
            audioSources[flip].clip = m_musicalSegments[flip].musicalSegment;
            audioSources[flip].PlayScheduled(nextEventTime);

            Debug.Log("Scheduled source " + flip + " to start at time " + nextEventTime);

            // Place the next event 16 beats from here at a rate of 140 beats per minute
            nextEventTime += 60.0f / userBpm * m_musicalSegments[0].m_segmentBarLength;

            // Flip between two audio sources so that the loading process of one does not interfere with the one that's playing out
            flip = 1 - flip;
        }*/
    }

    void bpmCounter()
    {
        beatTimer += Time.deltaTime;
        float beatInterval = 60.0f / userBpm;

        if (beatTimer >= beatInterval)
        {
            dspCopy = time;
            counter += 1;
            if (counter == m_musicalSegments[0].m_segmentBarLength)
            {
                counter = 0;
                nbLoops++;
                Debug.Log(nbLoops);
            }
            beatTimer = 0;
        }
    }

}