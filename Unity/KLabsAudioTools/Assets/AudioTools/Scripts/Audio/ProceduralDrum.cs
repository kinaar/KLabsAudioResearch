using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProceduralDrum : MonoBehaviour
{

    System.Random rand = new System.Random();
    [Range(0.0f,1.0f)]
    private float masterVolume = 0.0f;
    bool triggered = false;
    bool attack = false;

    void Awake()
    {

    }

    void OnAudioFilterRead(float [] data, int channels)
    {

        for(int i = 0; i < data.Length; i++)
        {
            data[i] = masterVolume*0.5f * (float)(rand.NextDouble() * 2.0 - 1.0);
        }
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("trig");
            triggered = true;
        }

        if(triggered)
        {
            adsr();
        }
        else
        {
            masterVolume = 0.0f;
        }
    }

    void adsr()
    {
        if (masterVolume <= 1.0f && attack == false)
        {
            masterVolume += Time.deltaTime * 50.0f;
        }


        if(masterVolume >= 1.0f)
        {
            attack = true;
        }
        
        if(attack == true)
        {
            masterVolume -= Time.deltaTime*3.0f;
            if(masterVolume <= 0.0f)
            {
                attack = false;
                triggered = false;
            }
        }
    }
}
