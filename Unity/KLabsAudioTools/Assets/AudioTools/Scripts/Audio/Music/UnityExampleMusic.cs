using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

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

        public enum transitionType { playAtNextBeat, playNextBar, playAtExitCue, fade, stop }
        public transitionType m_whenTriggered;

        public AudioClip musicalSegment;
        public int m_segmentBeatLength = 4;
    }
    public musicalSegments[] m_musicalSegments;

    //public AudioClip[] clips = new AudioClip[2];

    private double nextEventTime;
    private int flip = 0;
    private AudioSource[] audioSources = new AudioSource[2];
    private bool running = false;
    MusicObject objectscript;
    List<bool> triggerEntered = new List<bool>();
    List<bool> triggerEnteredCopy = new List<bool>();
    bool playing = false, done = false, countBpm = false;
    int musicPlayingID = 0;
    double time = 0;
    double dspCopy = 0;
    int nbLoops = 1;
    int counter = 0;
    double beatTimer = 0;
    bool toReset = false , fading = false;
    int fadingID = 0;

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
            triggerEnteredCopy.Add(triggerEntered[i]);
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
        
        for(int i = 1; i<m_musicalSegments.Length; i++)
        {
            m_musicalSegments[i].m_playType = musicalSegments.playType.OnTriggerEnter;
        }

        for (int i = 0; i < m_musicalSegments.Length; i++)
        {
            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;

            if (triggerEntered[0] != triggerEnteredCopy[0] && playing == false && m_musicalSegments[i].m_playType == musicalSegments.playType.OnTriggerEnter)
            {
                firstPlay(i);
                triggerEnteredCopy[0] = triggerEntered[0];
            }

            if (playing == false && m_musicalSegments[i].m_playType == musicalSegments.playType.onAwake)
            {
                firstPlay(i);
            }



            if (triggerEntered[i] != triggerEnteredCopy[i] && done == false && playing == true) //i != musicPlayingID
            {
                if (i == 0 && triggerEntered[0] == false)
                {
                    audioSources[0].Stop();
                    Debug.Log("ending");
                    triggerEnteredCopy[0] = triggerEntered[0];
                    playing = false;
                    countBpm = false;
                }
                else
                {
                    Debug.Log("entered or exit");
                    if (triggerEntered[i] == true)
                    {
                        playingNext(i);
                    }
                    else
                    {
                        playingNext(i - 1);
                        triggerEnteredCopy[i] = triggerEntered[i];
                    }
                }
            }

            /*if (triggerEntered[musicPlayingID] && done == false)
            {
                Debug.Log("oui");
                if (musicPlayingID > 0 && musicPlayingID < m_musicalSegments.Length)
                {
                    playingNext(musicPlayingID - 1);
                }
                m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                // if(i != musicPlayingID && triggerEntered[i] == true && done == false)
            }*/

            if (i != musicPlayingID && triggerEntered[i] == true && done == true)
            {
                nbLoops -= 1;
                done = false;
            }

            if (fading)
            {
                isFading(musicPlayingID, fadingID);
            }

        }

        if(toReset && AudioSettings.dspTime >= nextEventTime)
        {
            Debug.Log("prout");
            counter = 0;
            nbLoops = 2;
            toReset = false;
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
            nextEventTime += 60.0f / userBpm * m_musicalSegments[0].m_segmentBeatLength;

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
            Debug.Log(counter);
            if (counter == m_musicalSegments[0].m_segmentBeatLength)
            {
                nbLoops++;
                counter = 0;
                Debug.Log(nbLoops);
            }
            
            beatTimer = 0;
        }
    }

    void firstPlay(int i)
    {
        musicPlayingID = i;
        audioSources[i].clip = m_musicalSegments[i].musicalSegment;
        audioSources[i].Play();

        playing = true;
        nextEventTime = AudioSettings.dspTime;

        if (i + 1 <= m_musicalSegments.Length && m_musicalSegments[i + 1].m_whenTriggered == musicalSegments.transitionType.fade)
        {
            audioSources[i+1].clip = m_musicalSegments[i+1].musicalSegment;
            audioSources[i+1].Play();
            audioSources[i+1].volume = 0.0f;
            Debug.Log("Next is fade");
        }

        time = AudioSettings.dspTime;
        dspCopy = time;
        countBpm = true;
        toReset = true;
        //m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
    }

    void isFading(int i, int g)
    {
        audioSources[i].volume += Time.deltaTime / 2.0f;
        audioSources[g].volume -= Time.deltaTime / 2.0f;

        if (audioSources[g].volume <= 0.0f)
        {
            fading = false;
        }
    }

    void playingNext(int i)
    {
        if (m_musicalSegments[i].m_whenTriggered == musicalSegments.transitionType.playAtExitCue)
        {
            nextEventTime += (60.0f / userBpm * m_musicalSegments[i].m_segmentBeatLength) * (nbLoops);
        }
        if (m_musicalSegments[i].m_whenTriggered == musicalSegments.transitionType.playNextBar)
        {
            nextEventTime += (60.0f / userBpm * m_musicalSegments[i].m_segmentBeatLength) * (nbLoops);
            if (counter < 4)
            {
                nextEventTime -= 4 * (60.0f / userBpm);
            }
            toReset = true;
        }
        if (m_musicalSegments[i].m_whenTriggered == musicalSegments.transitionType.playAtNextBeat)
        {
            nextEventTime += (60.0f / userBpm * m_musicalSegments[i].m_segmentBeatLength) * (nbLoops) - (8 - counter - 1) * (60.0f / userBpm);
            toReset = true;
        }
        if (m_musicalSegments[i].m_whenTriggered == musicalSegments.transitionType.stop)
        {
            nextEventTime += (60.0f / userBpm * m_musicalSegments[i].m_segmentBeatLength) * (nbLoops);
        }
        if (m_musicalSegments[i].m_whenTriggered == musicalSegments.transitionType.fade)
        {
            nextEventTime += (60.0f / userBpm * m_musicalSegments[i].m_segmentBeatLength) * (nbLoops);
        }

        audioSources[i].clip = m_musicalSegments[i].musicalSegment;

        audioSources[musicPlayingID].SetScheduledEndTime(nextEventTime);

        if (m_musicalSegments[i].m_whenTriggered != musicalSegments.transitionType.stop && m_musicalSegments[i].m_whenTriggered != musicalSegments.transitionType.fade)
        {
            //if(i != musicPlayingID)
            audioSources[i].PlayScheduled(nextEventTime);
            audioSources[i].volume = 1.0f;
        }

        if (m_musicalSegments[i].m_whenTriggered == musicalSegments.transitionType.fade)
        {
            //audioSources[i].volume = 0.8f;
            //audioSources[musicPlayingID].volume = 0.0f;
            fading = true;
            fadingID = musicPlayingID;
        }

        //m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
        //m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
        triggerEnteredCopy[i] = triggerEntered[i];
        triggerEnteredCopy[musicPlayingID] = triggerEntered[musicPlayingID];
        musicPlayingID = i;
        nbLoops = 1;
        done = true;
    }

}