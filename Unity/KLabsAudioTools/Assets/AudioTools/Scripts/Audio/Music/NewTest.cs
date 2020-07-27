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
            GameObject child = new GameObject("Player");
            child.transform.parent = gameObject.transform;
            audioSource[i] = child.AddComponent<AudioSource>();
            
            musicScript = musicTrigger[i].gameObject.GetComponent<MusicObject>();
            triggerEnter[i] = musicScript.triggerEntered;
            triggerEnterCopy[i] = triggerEnter[i];
            //audioSource[i] = gameObject.AddComponent<AudioSource>();
            audioSource[i].clip = audioClip[i];
            audioSource[i].loop = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < musicTrigger.Length; i++)
        {
            triggerEnter[i] = musicTrigger[i].gameObject.GetComponent<MusicObject>().triggerEntered;
            firstTrigger();

            if (i != 0 && playing && triggerEnter[i] != triggerEnterCopy[i])
            {
                Debug.Log("other trig");
                
                if (triggerEnter[i] == true)
                {
                    audioSource[0].loop = false;
                    audioSource[i].Play();
                    audioSource[i].loop = true;
                }
                else
                {
                    audioSource[i].loop = false;
                    audioSource[i-1].Play();
                    audioSource[i-1].loop = true;
                }
                triggerEnterCopy[i] = triggerEnter[i];
            }
        }

        if (playing)
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

    void firstTrigger()
    {
        if (triggerEnter[0] != triggerEnterCopy[0])
        {

            if (triggerEnter[0] == true)
            {
                audioSource[0].Play();
                audioSource[0].loop = true;
                playing = true;
            }
            else
            {
                audioSource[0].loop = false;
                if (!audioSource[0].isPlaying)
                {
                    playing = false;
                }
            }
            Debug.Log("1st trig");

            triggerEnterCopy[0] = triggerEnter[0];
        }
    }
}
