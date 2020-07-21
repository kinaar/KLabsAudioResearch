using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    [Header("General Settings")]
    [Range(-48.0f, 3.0f)]
    [SerializeField]
    private float ambienceGeneralVol = 0.0f;
    public bool mute = false;
    public AudioSource ambienceAudioSource;
    public List<int> ids;
    void Start()
    {
        //ambienceAudioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
