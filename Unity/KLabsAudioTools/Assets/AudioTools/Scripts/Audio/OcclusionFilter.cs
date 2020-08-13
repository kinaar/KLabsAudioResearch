﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("KLabsAudioTools/OcclusionObject")]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class OcclusionFilter : MonoBehaviour
{

    public GameObject m_listener;
    float fqcWhenOccluded = 20000.0f;
    public float m_fqHalfOccluded = 3000.0f;
    public float m_fqFullOccluded = 2000.0f;
    public float m_freqFadeTime = 1.0f;
    public float interEarDistance = 0.75f;
    private AudioSource audioSource;
    private float maxDistance = 0.0f;
    float audioSourceCopy;
    public float m_volFadeTime = 1.0f;
    public float m_occludedVol = -12.0f;

    private GameObject earL;
    private GameObject earR;

    bool earLOccluded = false;
    bool earROccluded = false;

    float audioVolumeVar;

    void Start()
    {
        audioSourceCopy = gameObject.GetComponent<AudioSource>().volume;
        earL = new GameObject("Ear L");
        earR = new GameObject("Ear R");
        
        earL.transform.position = new Vector3(m_listener.transform.position.x + interEarDistance, m_listener.transform.position.y, m_listener.transform.position.z);
        earL.transform.parent = m_listener.transform;

        earR.transform.position = new Vector3(m_listener.transform.position.x - interEarDistance, m_listener.transform.position.y, m_listener.transform.position.z);
        earR.transform.parent = m_listener.transform;
    }

    private void Update()
    {
        RaycastHit hitL;
        RaycastHit hitR;

        Vector3 earPosition = m_listener.transform.position + m_listener.transform.forward; //m_listener.transform.position

        Vector3 sourceVectorL = earL.transform.position; //new Vector3(earPosition.x + 0.5f, m_listener.transform.position.y, m_listener.transform.position.z + m_listener.transform.forward.z);
        Vector3 sourceVectorR = earR.transform.position; //new Vector3(earPosition.x - 0.5f, m_listener.transform.position.y, m_listener.transform.position.z + m_listener.transform.forward.z);
        Vector3 playerVector = transform.position;
        Vector3 destinationL = sourceVectorL - playerVector;
        Vector3 destinationR = sourceVectorR - playerVector;
        

        audioSource = gameObject.GetComponent<AudioSource>();
        maxDistance = audioSource.maxDistance;

        if (destinationL.x < maxDistance && destinationL.z < maxDistance)
        {
            Debug.DrawRay(transform.position, destinationL, Color.red);
            Debug.DrawRay(transform.position, destinationR, Color.white);

            if (Physics.Raycast(transform.position, destinationL, out hitL))
            {

                var rigL = hitL.collider.gameObject.tag;

                if (rigL == "OcclusionObject")
                {
                    //Debug.Log("ear L occluded");
                    earLOccluded = true;
                }
                else
                {
                    earLOccluded = false;
                }
            }

            if (Physics.Raycast(transform.position, destinationR, out hitR))
            {
                var rigR = hitR.collider.gameObject.tag;

                if (rigR == "OcclusionObject")
                {
                    //Debug.Log("ear R occluded");
                    earROccluded = true;
                }
                else
                {
                    earROccluded = false;
                }
            }

            if(earLOccluded || earROccluded)
            {
                if (earLOccluded && earROccluded)
                {
                    Debug.Log("both occluded");
                    fqcWhenOccluded = m_fqFullOccluded;
                    audioVolumeVar = Mathf.Pow(10, m_occludedVol / 20.0f);
                    audioSource.spread = 90.0f;
                }
                else
                {
                    fqcWhenOccluded = m_fqHalfOccluded;
                    float vol = (m_occludedVol + audioSourceCopy) / 2;
                    audioVolumeVar = Mathf.Pow(10, vol / 20.0f);
                    audioSource.spread = 45.0f;
                }
            }
            else
            {
                fqcWhenOccluded = 20000.0f;
                audioVolumeVar = 0.6f;
                audioSource.spread = 0.0f;
            }
            
            occluded(fqcWhenOccluded);
            occludedVolume(audioVolumeVar);

        }
    }

    void occluded(float fqc)
    {

        AudioLowPassFilter lowpass = gameObject.GetComponent<AudioLowPassFilter>();

        if (lowpass.cutoffFrequency > fqc)
        {
            lowpass.cutoffFrequency -= 300.0f * (1.0f / m_freqFadeTime);
        }
        else if(lowpass.cutoffFrequency < fqc)
        {
            lowpass.cutoffFrequency = fqc - 1.0f;
        }

        //lowpass.cutoffFrequency = fqc;
    }

    void occludedVolume(float volume)
    {
        if (audioSource.volume != volume)
        {
            if (audioSource.volume >= volume)
            {
                audioSource.volume -= Time.deltaTime / m_volFadeTime;
            }
            
            if (audioSource.volume <= volume)
            {
                audioSource.volume += Time.deltaTime / m_volFadeTime;
            }
        }
    }
}
