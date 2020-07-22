using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AmbienceManager : MonoBehaviour
{
    [Header("General Settings")]
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    //[HideInInspector]
    public float ambienceGeneralVol = 0.0f;
    public bool mute = false;
    private bool muteCopy;
    private float generalVolCopy = 0.0f;
    [HideInInspector]
    //public AudioSource ambienceAudioSource;
    public List<int> ids;
    GameObject[] ambienceObjects;

    void Start()
    {
        muteCopy = mute;
        generalVolCopy = ambienceGeneralVol;
    }

    // Update is called once per frame
    void Update()
    {
        if (muteCopy != mute)
        {
            isMuted();
            muteCopy = mute;
        }

        if(generalVolCopy != ambienceGeneralVol)
        {
            volumeChanged();
            generalVolCopy = ambienceGeneralVol;
        }

    }

    void isMuted()
    {
        Component[] audioSourcestoMute;
        audioSourcestoMute = GetComponents(typeof(AudioSource));
        if (mute == true)
        {
            foreach (AudioSource sources in audioSourcestoMute)
            {
                sources.mute = true;
            }

        }
        else
        {
            foreach (AudioSource sources in audioSourcestoMute)
            {
                sources.mute = false;
            }
        }
    }

    void volumeChanged()
    {
        Component[] audioSourcestoMute;
        audioSourcestoMute = GetComponents(typeof(AudioSource));
        float genVol = Mathf.Pow(10, ambienceGeneralVol/20);
        foreach (AudioSource sources in audioSourcestoMute)
        {
            sources.volume = genVol;
        }
    }
}
