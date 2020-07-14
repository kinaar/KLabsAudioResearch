using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class AmbianceSequencer : MonoBehaviour
{

    public AudioSource ambianceSource;
    public AudioClip ambianceLoop;
    public List<AudioClip> randomSounds;
    
    [Range(0, 100)]
    [SerializeField]
    private int panRandomization;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
