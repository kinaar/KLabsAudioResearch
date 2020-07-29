using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioLowPassFilter))]
public class OcclusionFilter : MonoBehaviour
{

    public GameObject listener;
    public float fqcWhenOccluded = 1000.0f;
    public float transitionTime = 1.0f;
    private AudioSource audioSource;
    private float maxDistance = 0.0f;

    private void Update()
    {
        RaycastHit hit;
        Vector3 soundSourceVector = listener.transform.position;
        Vector3 playerVector = transform.position;
        Vector3 destination = soundSourceVector - playerVector;
        

        audioSource = gameObject.GetComponent<AudioSource>();
        maxDistance = audioSource.maxDistance;

        if (destination.x < maxDistance && destination.z < maxDistance)
        {
            Debug.DrawRay(transform.position, destination, Color.blue);
            if (Physics.Raycast(transform.position, destination, out hit))
            {

                AudioLowPassFilter lowpass = gameObject.GetComponent<AudioLowPassFilter>();

                var rig = hit.collider.gameObject.tag;

                float cutofffreq = fqcWhenOccluded;

                if (rig == "OcclusionObject")
                {
                    Debug.Log("object");

                    if (lowpass.cutoffFrequency > cutofffreq)
                    {
                        lowpass.cutoffFrequency -= Time.deltaTime * 10000.0f * transitionTime;
                    }
                    else
                    {
                        lowpass.cutoffFrequency = cutofffreq;
                    }
                }
                else
                {
                    if (lowpass.cutoffFrequency < 20000.0f)
                    {
                        lowpass.cutoffFrequency += Time.deltaTime * 10000.0f * transitionTime;
                    }
                    else
                    {
                        lowpass.cutoffFrequency = 20000.0f;
                    }
                }
            }
        }
    }
}
