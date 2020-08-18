using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSegment : MonoBehaviour
{

    public GameObject m_triggerObject;
    public AudioClip m_musicalSegment;
    public int m_segmentBarLength = 4;
    public transitionType m_transition;
    public enum transitionType {nextExitCue, fade, stop};
    public float m_crossfadeTime = 1.0f;
    public float m_fadeOutTime = 1.0f;

    float userBpm;
    AudioSource segmentSource;
    bool triggerEnter = false, triggerEnterCopy = false;
    double nextEventTime = 0.0f;
    int loopNumber;

    void Start()
    {
        userBpm = gameObject.GetComponent<MusicScript>().m_userBpm;
        segmentSource = m_triggerObject.AddComponent<AudioSource>();
        segmentSource.clip = m_musicalSegment;
        segmentSource.loop = true;
        triggerEnter = m_triggerObject.GetComponent<MusicObject>().triggerEntered;
        triggerEnterCopy = triggerEnter;

        nextEventTime = gameObject.GetComponent<MusicScript>().nextEventTime;
        loopNumber = gameObject.GetComponent<MusicScript>().loopNumber;
    }

    // Update is called once per frame
    void Update()
    {
        nextEventTime = gameObject.GetComponent<MusicScript>().nextEventTime;
        loopNumber = gameObject.GetComponent<MusicScript>().loopNumber;
        triggerEnter = m_triggerObject.GetComponent<MusicObject>().triggerEntered;

        if(triggerEnterCopy != triggerEnter)
        {
            if(m_transition == transitionType.nextExitCue)
            {
                gameObject.GetComponent<MusicScript>().musicTrigger(triggerEnter, m_triggerObject, m_musicalSegment, m_segmentBarLength);
            }
            if(m_transition == transitionType.fade)
            {
                gameObject.GetComponent<MusicScript>().fade(triggerEnter, m_triggerObject, m_musicalSegment, m_segmentBarLength, m_crossfadeTime);
            }

            if(m_transition == transitionType.stop)
            {
                gameObject.GetComponent<MusicScript>().stop(triggerEnter, m_triggerObject, m_musicalSegment, m_segmentBarLength, m_fadeOutTime);
            }

            triggerEnterCopy = triggerEnter;
        }
        
    }
}
