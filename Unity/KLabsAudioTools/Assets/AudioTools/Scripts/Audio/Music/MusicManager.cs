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

    bool done = false;
    int musicPlayingID = 0;

    // Start is called before the first frame update
    void Start()
    {
        //beatTimer = Time.deltaTime;
        //musicSource.PlayOneShot(musicalSegment);
        //managerscript.isPlaying = true;
        for(int i = 0; i<m_musicalSegments.Length; i++)
        {
            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;
            //triggerEntered[i] = objectScript[i].triggerEntered;
            if(i >= m_musicalSegments.Length-1)
            {
                Debug.Log("Hello");
            }
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < m_musicalSegments.Length; i++)
        {
            triggerEntered[i] = m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered;

            if (m_musicalSegments[i].m_playType == musicalSegments.playType.onAwake && done == false)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musSeg = m_musicalSegments[i].musicalSegment;
                musicSource.clip = musSeg;
                //bpmCounter();
                musicSource.Play();
                //Debug.Log("onAwake");
                done = true;
                m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
            }
            
            if (i >= m_musicalSegments.Length - 1)
            {
                
                Debug.Log("Hello");
            }
        }

        //triggerEntered = objectScript.triggerEntered;
        
        bpmCounter();

    }

    void TriggerEnter(int n)
    {
        if (m_musicalSegments[n].m_playType != musicalSegments.playType.onAwake )//&& triggerEntered == true)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musSeg = m_musicalSegments[n].musicalSegment;
            musicSource.clip = musSeg;
            //triggerEntered = true;
            Debug.Log("OnTriggerEnter");
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    void bpmCounter()
    {
        beatTimer += Time.deltaTime;
        float beatInterval = 60.0f / userBpm;

        if (beatTimer >= beatInterval)
        {
            beatTimer -= beatInterval;
            counter += 1;
            if(counter == m_musicalSegments[0].m_segmentBarLength)
            {
                counter = 0;
                Debug.Log("Boom");
                //musicSource.loop = false;
            }
        }

        for (int i = 0; i < m_musicalSegments.Length; i++)
        {
            if (triggerEntered[i] && musicSource != null && i != musicPlayingID)
            {
                musicSource.loop = false;
                if(musicSource.isPlaying != true)
                {
                    triggerEntered[musicPlayingID] = false;
                    m_musicalSegments[musicPlayingID].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                    done = false;
                    Destroy(musicSource);
                }
            }
            
            if (triggerEntered[i] && done == false && i != musicPlayingID)
            {
                TriggerEnter(i);
                beatTimer = 0.0f;
                counter = 0;
                musicPlayingID = i;
                done = true;
                
                //m_musicalSegments[i].triggerObject.gameObject.GetComponent<MusicObject>().triggerEntered = false;
                //Debug.Log("Hi");
            }


        }
    }
}