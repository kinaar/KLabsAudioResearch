using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CharacterAudioManager : MonoBehaviour
{
    private AudioSource[] audioSources = new AudioSource[10];

    [System.Serializable]
    public class CharaAudioClips
    {
        public enum playOn { fire1, fire2, spacebar, custom }
        public playOn _playOnTrigger;
        public AudioClip soundToTrig;
        public string customKey;
    }
    public CharaAudioClips[] charaAudioClipsArray;

    void Start()
    {
        for (int i = 0; i < charaAudioClipsArray.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].clip = charaAudioClipsArray[i].soundToTrig;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < charaAudioClipsArray.Length; i++)
            {
                if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.fire1)
                {
                    audioSources[i].PlayOneShot(charaAudioClipsArray[i].soundToTrig);
                    Debug.Log("trig");
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
            for (int i = 0; i < charaAudioClipsArray.Length; i++)
            {
                if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.fire2)
                {
                    audioSources[i].PlayOneShot(charaAudioClipsArray[i].soundToTrig);
                    Debug.Log("trig");
                }
            }

        if (Input.GetKeyDown(charaAudioClipsArray[0].customKey))
        {
            for (int i = 0; i < charaAudioClipsArray.Length; i++)
            {
                if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.custom)
                {
                    audioSources[i].PlayOneShot(charaAudioClipsArray[i].soundToTrig);
                    Debug.Log("trig");
                }
            }
        }
    }
}
