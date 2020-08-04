using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioFilter : MonoBehaviour
{

    System.Random rand = new System.Random();
    [Range(-96.0f, 0.0f)]
    public float masterVolume = -12.0f;

    public float cutoffFrequency = 440.0f;
    public float qFactor = 0.707f;
    private float cutoffFrequencyCopy = 440.0f;

    int sampleRate = 48000;
    float w = 2*Mathf.PI, d = 1/0.707f, beta, gamma, a0, a1, a2, b1, b2, outSample;

    void Awake()
    {
        w = (w * cutoffFrequency) / sampleRate;
        d = 1.0f/ qFactor;
        beta = ((1.0f - (d / 2.0f) * Mathf.Sin(w)) / (1.0f + (d / 2.0f) * Mathf.Sin(w))) / 2.0f;
        gamma = (0.5f + beta) * Mathf.Cos(w);

        a0 = (0.5f + beta - gamma) / 2.0f;
        a1 = 0.5f + beta - gamma;
        a2 = a0;
        b1 = -2.0f * gamma;
        b2 = 2.0f * beta;
        cutoffFrequencyCopy = cutoffFrequency;
        outSample = 0.0f;
    }

    void Update()
    {

    }

    void OnAudioFilterRead(float [] data, int channels)
    {

        if (cutoffFrequency != cutoffFrequencyCopy)
        {
            w = (w * cutoffFrequency) / sampleRate;
            beta = ((1.0f - (d / 2.0f) * Mathf.Sin(w)) / (1.0f + (d / 2.0f) * Mathf.Sin(w))) / 2.0f;
            gamma = (0.5f + beta) * Mathf.Cos(w);

            a0 = (0.5f + beta - gamma) / 2.0f;
            a1 = 0.5f + beta - gamma;
            a2 = a0;
            b1 = -2.0f * gamma;
            b2 = 2.0f * beta;
            outSample = 0.0f;
            cutoffFrequencyCopy = cutoffFrequency;
        }

        for(int i = 0; i < data.Length; i++)
        {
            data[i] = (float)(rand.NextDouble() * 2.0 - 1.0) * Mathf.Pow(10, 0.05f*masterVolume);//*masterVolume*0.5f;
            float inSample = data[i];
            data[i] = (a0 * inSample) + (a1 * inSample) + (a2 * inSample) - (b1 * outSample) - (b2 * outSample);
            if(i > 0)
            {
                outSample = data[i-1];
            }
            else
            {
                outSample = data[data.Length - 1];
            }
        }
    }
}
