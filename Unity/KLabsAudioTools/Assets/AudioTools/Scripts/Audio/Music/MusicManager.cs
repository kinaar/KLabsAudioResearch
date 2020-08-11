using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [Header("General Settings")]
    //public GameObject musicManager; // Audio Manager (to instantiate AudioSources)
    public AudioMixerGroup m_outputMixerGroup; // Audio Mixer for audio output

    [Range(-48.0f, 3.0f)] // General Output Volume
    [SerializeField]
    private float m_generalVolume = 0.0f;

    public bool m_muteGeneral = false; // General Mute
    public float userBpm = 120.0f;
    public float m_fadeInTime = 1.0f; // Fade in time
    //public float m_fadeOutTime = 1.0f; // Fade out time

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

    //// _Private Variables_ ////
    float beatTimer = 0.0f;
    float timerDup = 0.0f;
    int counter = 0;
    bool[] triggerEntered = {false, false, false, false, false, false, false};

    AudioSource musicSource;
    AudioClip musSeg;
    MusicObject[] objectScript;
    List<AudioSource> segmentSource = new List<AudioSource>();

    bool done = false;
    int musicPlayingID = 0;
    double dsptime = 0.0f, dspCopy = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<m_musicalSegments.Length; i++)
        {
            segmentSource.Add(gameObject.AddComponent<AudioSource>());
            musSeg = m_musicalSegments[i].musicalSegment;
            segmentSource[i].clip = musSeg;
            segmentSource[i].playOnAwake = false;
            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;
        }
        dsptime = AudioSettings.dspTime;
        dspCopy = dsptime;
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_musicalSegments.Length; i++)
        {
            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;

            if (m_musicalSegments[i].m_playType == musicalSegments.playType.onAwake && done == false)
            {
                segmentSource[i].volume = Mathf.Pow(10, (m_generalVolume/20.0f));
                segmentSource[i].outputAudioMixerGroup = m_outputMixerGroup;
                segmentSource[i].Play();
                done = true;
                m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
            }
        }
        //triggerEntered = objectScript.triggerEntered;
        
        transition();
        bpmCounter();

    }

    void TriggerEnter(int n)
    {
        if (m_musicalSegments[n].m_playType != musicalSegments.playType.onAwake )
        {
            Debug.Log("OnTriggerEnter");
            segmentSource[n].volume = Mathf.Pow(10, (m_generalVolume/20.0f));
            segmentSource[n].loop = true;
            segmentSource[n].PlayScheduled(AudioSettings.dspTime + 2.0f);
        }
    }

    void bpmCounter()
    {
        beatTimer += Time.deltaTime;
        float beatInterval = 60.0f / userBpm;
        

        if (dsptime - dspCopy >= 0.48f)
        {
            /*dspCopy -= dsptime;
            counter += 1;
            dspCopy = dsptime;
            if (counter == m_musicalSegments[0].m_segmentBarLength)
            {
                counter = 0;
                Debug.Log("Boom");
                Debug.Log(dsptime - dspCopy);
                Debug.Log(beatInterval * m_musicalSegments[0].m_segmentBarLength);
                Debug.Log(beatTimer);
            }*/
            Debug.Log(dsptime - dspCopy);
            Debug.Log(beatInterval);
            dspCopy = dsptime;
            
        }
    }

    void transition()
    {
        for (int i = 0; i < m_musicalSegments.Length; i++)
        {

            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;

            //// playNextSegment ////
            if (triggerEntered[i] && segmentSource[i] != null && i != musicPlayingID && m_musicalSegments[i].m_transition == musicalSegments.transitionType.playNextSegment)
            {
                segmentSource[musicPlayingID].loop = false;
                if(counter == 0 && segmentSource[musicPlayingID].isPlaying != true)
                {
                    segmentSource[musicPlayingID].Stop();
                    triggerEntered[musicPlayingID] = false;
                    m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                    done = false;
                }
            }

            //// stop the music ////
            if (triggerEntered[i] && segmentSource[i] != null && i != musicPlayingID && m_musicalSegments[i].m_transition == musicalSegments.transitionType.stop)
            {
                //segmentSource[i].loop = false;
                segmentSource[musicPlayingID].Stop();
                if(segmentSource[i].isPlaying != true)
                {
                    triggerEntered[musicPlayingID] = false;
                    m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                    done = false;
                }
            }

            //// Fading ////
            if (triggerEntered[i] && segmentSource[i] != null && i != musicPlayingID && m_musicalSegments[i].m_transition == musicalSegments.transitionType.fading)
            {
                float sourceVolume = segmentSource[i].volume;
                sourceVolume -= Time.deltaTime / 5.0f;
                segmentSource[i].volume = sourceVolume;
                if (segmentSource[i].volume <= 0.0f)
                {
                    triggerEntered[musicPlayingID] = false;
                    m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                    done = true;
                }
            }
            
            if (triggerEntered[i] && done == false && i != musicPlayingID)
            {
                TriggerEnter(i);
                if (segmentSource[musicPlayingID].isPlaying != true)
                {
                    beatTimer = 0.0f;
                    counter = 0;
                }
                musicPlayingID = i;
                done = true;
            }
        }
    }

}