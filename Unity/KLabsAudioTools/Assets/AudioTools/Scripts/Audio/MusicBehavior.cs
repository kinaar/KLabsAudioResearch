using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Audio;

public class MusicBehavior : MonoBehaviour
{
    [Header("General Settings")]
    public AudioSource audioSource;
    public float userBpm = 120.0f;
    public bool transition = false;

    [Space] /////// Music A

    [Header("Musical Segment A")]

    public AudioClip musicalSegmentA;
    public int lengthSegmentA = 4;

    public enum transitionChoicesA
    {
        onTriggerEnter,
        onStart
    };
    public transitionChoicesA transitionChoiceA;

    public bool postExitA = true;
    
    [Space] /////// Music B

    [Header("Musical Segment B")]

    public AudioClip musicalSegmentB;
    public int lengthSegmentB = 4;

    public enum transitionChoicesB
    {
        onTriggerEnter,
        onStart
    };
    public transitionChoicesB transitionChoiceB;
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
        
        if (transitionChoiceA == transitionChoicesA.onTriggerEnter)
        {
            print("Hello");
        }

        if (timer - timerDup >= bpmInSeconds)
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
