using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_Maker_KLabs : MonoBehaviour
{

    System.Random rand = new System.Random();
    [Range(0.0f,1.0f)]
    public float masterVolume = 1.0f;

    void OnAudioFilterRead(float [] data, int channels)
    {
        for(int i = 0; i < data.Length; i++)
        {
            data[i] = masterVolume*0.5f * (float)(rand.NextDouble() * 2.0 - 1.0);
        }
    }
}
