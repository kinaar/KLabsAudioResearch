using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSegment : MonoBehaviour
{

    public GameObject triggerObject;
    public AudioClip musicalSegment;
    public int m_segmentBarLength = 4;
    public enum transitionType {nextExitCue, fade, stop};
    public transitionType transition;
    public float crossFadeTime = 1.0f;
    public float fadeOutTime = 1.0f;

    float userBpm;
    AudioSource segmentSource;
    bool triggerEnter = false, triggerEnterCopy = false;
    double nextEventTime = 0.0f;
    int loopNumber;

    void Start()
    {
        userBpm = gameObject.GetComponent<MusicScript>().userBpm;
        segmentSource = triggerObject.AddComponent<AudioSource>();
        segmentSource.clip = musicalSegment;
        segmentSource.loop = true;
        triggerEnter = triggerObject.GetComponent<MusicObject>().triggerEntered;
        triggerEnterCopy = triggerEnter;

        nextEventTime = gameObject.GetComponent<MusicScript>().nextEventTime;
        loopNumber = gameObject.GetComponent<MusicScript>().loopNumber;
    }

    // Update is called once per frame
    void Update()
    {
        nextEventTime = gameObject.GetComponent<MusicScript>().nextEventTime;
        loopNumber = gameObject.GetComponent<MusicScript>().loopNumber;
        triggerEnter = triggerObject.GetComponent<MusicObject>().triggerEntered;

        if(triggerEnterCopy != triggerEnter)
        {
            if(transition == transitionType.nextExitCue)
            {
                gameObject.GetComponent<MusicScript>().musicTrigger(triggerEnter, triggerObject, musicalSegment, m_segmentBarLength);
            }
            if(transition == transitionType.fade)
            {
                gameObject.GetComponent<MusicScript>().fade(triggerEnter, triggerObject, musicalSegment, m_segmentBarLength, crossFadeTime);
            }

            if(transition == transitionType.stop)
            {
                gameObject.GetComponent<MusicScript>().stop(triggerEnter, triggerObject, musicalSegment, m_segmentBarLength, fadeOutTime);
            }

            triggerEnterCopy = triggerEnter;
        }
        
    }
}
