using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FilterTest : MonoBehaviour
{
    System.Random rand = new System.Random();
    [Range(0.1f, 5.0f)]
    public float reso = 1.0f;
    [Range(200.0f, 20000.0f)]
    public float cutoffFreq = 1000.0f;
    int sampleRate;

    float mX1, mX2, mY1, mY2, pi;
    float k, c1, c2, c3;
    float mA0, mA1, mA2, mB1, mB2;
    float cutoff, cutoffCopy;
    float resocopy;


    void Awake()
    {
        sampleRate = AudioSettings.outputSampleRate;
        cutoff = cutoffFreq*2.0f / sampleRate;
        cutoffCopy = cutoffFreq;
        resocopy = reso;
        mX1 = 0;
        mX2 = 0;
        mY1 = 0;
        mY2 = 0;
        pi = Mathf.PI;


        k = 0.5f * reso * Mathf.Sin(pi * cutoff);
        c1 = 0.5f * (1.0f - k) / (1.0f + k);
        c2 = (0.5f + c1) * Mathf.Cos(pi * cutoff);
        c3 = (0.5f + c1 - c2) * 0.25f;

        mA0 = 2.0f * c3;
        mA1 = 2.0f * 2.0f * c3;
        mA2 = 2.0f * c3;
        mB1 = 2.0f * -c2;
        mB2 = 2.0f * c1;
    }

    void OnAudioFilterRead(float [] data, int channels)
    {
        for(int i = 0; i<data.Length; i++)
        {
            
            if(cutoffCopy != cutoffFreq || resocopy != reso)
            {
                coefUpdate();
            }

            float[] inputValue = data;
            data[i] = mA0 * data[i] + mA1 * mX1 + mA2 * mX2 - mB1 * mY1 - mB2 * mY2;

            mX2 = mX1;
            mX1 = inputValue[i];
            mY2 = mY1;
            mY1 = data[i];
        }
    }

    void coefUpdate()
    {
        cutoff = cutoffFreq*2.0f / sampleRate;
        k = 0.5f * reso * Mathf.Sin(pi * cutoff);
        c1 = 0.5f * (1.0f - k) / (1.0f + k);
        c2 = (0.5f + c1) * Mathf.Cos(pi * cutoff);
        c3 = (0.5f + c1 - c2) * 0.25f;

        mA0 = 2.0f * c3;
        mA1 = 2.0f * 2.0f * c3;
        mA2 = 2.0f * c3;
        mB1 = 2.0f * -c2;
        mB2 = 2.0f * c1;
        cutoffCopy = cutoffFreq;
        resocopy = reso;
    }
}
