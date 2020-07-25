using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTest : MonoBehaviour
{

    MusicObject musicScript;
    public GameObject[] musicTrigger;
    bool[] triggerEnter = new bool[10];
    bool[] triggerEnterCopy = new bool[10];
    bool playing = false;
    public AudioClip[] audioClip;
    private AudioSource[] audioSource = new AudioSource[10];
    public float userBpm = 120.0f;
    float beatTimer = 0.0f;
    int counter = 0;
    int nbLoops = 1;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < musicTrigger.Length; i++)
        {
            musicScript = musicTrigger[i].gameObject.GetComponent<MusicObject>();
            triggerEnter[i] = musicScript.triggerEntered;
            triggerEnterCopy[i] = triggerEnter[i];
            audioSource[i] = gameObject.AddComponent<AudioSource>();
            audioSource[i].clip = audioClip[0];
            audioSource[i].loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < musicTrigger.Length; i++)
        {
        triggerEnter[i] = musicTrigger[i].gameObject.GetComponent<MusicObject>().triggerEntered;
        if(triggerEnter[i] != triggerEnterCopy[i])
        {
            Debug.Log("PROUT");

            if(triggerEnter[i] == true)
            {
                audioSource[i].Play();
                audioSource[i].loop = true;
                playing = true;
            }
            else
            {
                audioSource[i].loop = false;
                if(!audioSource[i].isPlaying)
                {
                    playing = false;
                }
            }

            triggerEnterCopy[i] = triggerEnter[i];
        }}

        if(playing)
        {
            bpmCounter();
        }
    }

    void bpmCounter()
    {
        beatTimer += Time.deltaTime;
        float beatInterval = 60.0f / userBpm;
        if (beatTimer >= beatInterval)
        {
            //dspCopy = time;
            counter += 1;
            Debug.Log(counter);
            if (counter == 8)
            {
                nbLoops++;
                counter = 0;
                Debug.Log(nbLoops);
            }
            
            beatTimer = 0;
        }
    }
}
