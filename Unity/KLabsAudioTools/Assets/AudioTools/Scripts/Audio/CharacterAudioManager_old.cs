using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityEditor;

public class CharacterAudioManager_old : MonoBehaviour
{
    private AudioSource[] audioSources = new AudioSource[10];
    //public AudioMixerGroup m_audioMixer;

    [System.Serializable]
    public class CharaAudioClips
    {
        public enum playOn { fire1, fire2, spacebar, custom }
        public playOn _playOnTrigger;
        public AudioClip[] soundToTrig;
        public string customKey;

        [Range (-48.0f, 3.0f)]
        public float _volume;

        [Range (0.0f, 100.0f)]
        public float _randomPitch;

        public bool bypassReverb = true;
        public AudioMixerGroup m_audioMixer;
    }
    public CharaAudioClips[] charaAudioClipsArray;

    void Start()
    {
        for (int i = 0; i < charaAudioClipsArray.Length; i++)
        {
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSources[i] = child.AddComponent<AudioSource>();
            audioSources[i].clip = charaAudioClipsArray[i].soundToTrig[0];
            audioSources[i].outputAudioMixerGroup = charaAudioClipsArray[i].m_audioMixer;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            for (int i = 0; i < charaAudioClipsArray.Length; i++)
            {
                if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.fire1)
                {
                    playSound(i);
                }
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < charaAudioClipsArray.Length; i++)
            {
                if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.fire2)
                {
                    playSound(i);
                }
            }
        }

        for (int i = 0; i < charaAudioClipsArray.Length; i++)
        {
            if (charaAudioClipsArray[i].customKey != "")
            {
                if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.custom)
                {
                    if (Input.GetKeyDown(charaAudioClipsArray[i].customKey) || Input.GetButtonDown("Fart"))
                    {
                        playSound(i);
                    }
                }
            }

            if (charaAudioClipsArray[i]._playOnTrigger == CharaAudioClips.playOn.spacebar)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    playSound(i);
                }
            }
        }
    }

    void playSound(int i)
    {
        float pitchRand = Random.Range(-1.0f, 1.0f);
        int audioRand = Random.Range(0, charaAudioClipsArray[i].soundToTrig.Length);
        audioSources[i].pitch = 1 + (pitchRand * (charaAudioClipsArray[i]._randomPitch / 100.0f));
        audioSources[i].PlayOneShot(charaAudioClipsArray[i].soundToTrig[audioRand]);
        audioSources[i].bypassReverbZones = charaAudioClipsArray[i].bypassReverb;
        audioSources[i].bypassEffects = charaAudioClipsArray[i].bypassReverb;
        audioSources[i].bypassListenerEffects = charaAudioClipsArray[i].bypassReverb;
        audioSources[i].volume = Mathf.Pow(10, charaAudioClipsArray[i]._volume / 20.0f);
        Debug.Log("trig");
    }
}
