using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]

public class MusicBehavior : MonoBehaviour
{
    public AudioSource audioSource;
    public float userBpm = 120.0f;
    public bool transition = false;

    [Space]

    public AudioClip musicalSegmentA;
    public int lengthSegmentA = 4;
    public bool postExitA = true;
    
    [Space]

    public AudioClip musicalSegmentB;
    public int lengthSegmentB = 4;
    public bool postExitB = true;
    
    // PRIVATE
    float timer = 0.0f;
    float timerDup = 0.0f;
    int counter = 0;
    int segmentToPlay = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        float bpmInSeconds = 1/(userBpm/60.0f);
        //Debug.Log(timer);
        if(timer - timerDup >= bpmInSeconds)
        {
            timerDup += bpmInSeconds;
            counter += 1;

            if(transition == true)
            {
                segmentToPlay = 1;
            }
            else
            {
                segmentToPlay = 0;
            }

                if(counter == lengthSegmentA && segmentToPlay == 0){
                    
                    if(postExitA == false){
                        audioSource.Stop();
                    }
                    
                    audioSource.PlayOneShot(musicalSegmentA);
                    counter = 0;
                    Debug.Log("Played");
                }

                if(counter == lengthSegmentA && segmentToPlay == 1){
                    
                    if(postExitB == false){
                        audioSource.Stop();
                    }
                    
                    audioSource.PlayOneShot(musicalSegmentB);
                    counter = 0;
                    Debug.Log("Played");
                }
        }
    }
}
