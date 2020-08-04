using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[AddComponentMenu("KLabsAudioTools/ReverbZone")]
public class ReverbZone : MonoBehaviour
{
    
    public AudioMixer audioMixer;
    public string parameterName;
    public float reverbVolume;
    bool inside = false;
    float timerVol = 0.0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (parameterName != null && audioMixer != null)
        {
            if (inside)
            {
                audioMixer.SetFloat(parameterName, timerVol);

                if(timerVol < 0.0f)
                {
                    timerVol += Time.deltaTime*100.0f;
                }
                else
                {
                    timerVol = 0.0f;
                }
            }
            else
            {
                timerVol -= Time.deltaTime*30.0f;
                audioMixer.SetFloat(parameterName, timerVol);
                if(timerVol <= -80.0f)
                {
                    audioMixer.SetFloat(parameterName, -80.0f);
                }
            }
        }
        else
        {
            Debug.Log("You need to set the AudioMixer and the Paramater Name !");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            inside = true;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            inside = false;
        }
    }
}
