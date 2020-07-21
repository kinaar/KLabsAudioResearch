using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioManager : MonoBehaviour
{
    AudioSource characterAudio;
    [Header("General Settings")]
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float characterVolume = 0.0f;

    public CharaAudioClips[] charaAudioClipsArray;
    [System.Serializable]
    public class CharaAudioClips{
        public enum playOn{footstep, fire1, fire2, spacebar}
        public playOn _playOnTrigger;
        public AudioClip[] soundsToTrigger;
    }

    void Start()
    {
        characterAudio = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
