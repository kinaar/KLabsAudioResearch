using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Delay_KLabs : MonoBehaviour
{

    System.Random rand = new System.Random();
    [Range(0.0f,1.0f)]
    public float masterVolume = 1.0f;
    int fs = 0;
    float w = 0; // angular frequency
    float d = 0.0f; // Q
    float beta;
    float gamma;
    float a0, a1, a2, b1, b2;
    float[] echoMemory = new float[20000];
    int writePoint = 0;
    int readPoint = 0;
    int delayLineSize = 20000;
    public int echoSize;
    float TapOutSample = 0;
    public float echoVolume = 0.5f;

    void Awake()
    {

        fs = AudioSettings.outputSampleRate;

        /*w = 2*Mathf.PI*(440.0f / fs);
        d = 1.0f / 0.707f;
        beta = ((1 - (d/2) * Mathf.Sin(w)) / (1 + (d/2) * Mathf.Sin(w))) / 2.0f;
        gamma = (0.5f + beta) * Mathf.Cos(w);

        // Coefficients
        a0 = (0.5f + beta - gamma) / 2.0f;
        a1 = 0.5f + beta - gamma;
        a2 = a0;
        b1 = -2.0f * gamma;
        b2 = 2.0f * beta;*/

        int size = echoSize;
        readPoint = delayLineSize - size;


        //http://creatingsound.com/2014/02/dsp-audio-programming-series-part-2/
    }

    void OnAudioFilterRead(float [] data, int channels)
    {

        for(int i = 0; i < data.Length; i++)
        {
            //data[i] = masterVolume*0.5f * (float)(rand.NextDouble() * 2.0 - 1.0);
            //data[i] = (a0* data[i]) + (a1 * data[i]) + (a2 * data[i]);

            //echoLine(data[i]);
            echoMemory[writePoint] = data[i] + (TapOutSample * echoVolume);
            writePoint++;
            if(writePoint >= delayLineSize)
            {
                writePoint = 0;
            }

            TapOutSample = echoMemory[readPoint];
            readPoint++;
            if(readPoint >= delayLineSize)
            {
                readPoint = 0;
            }

            data[i] = data[i] + TapOutSample * 0.5f;
            
            //data[i] = data[i] + echoMemory[i - 200];
            //Debug.Log(data.Length);
            
        }
    }
}
