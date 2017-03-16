using UnityEngine;
using System.Collections;

public class AlarmSystem : MonoBehaviour
{

    public float lightFadeSpeed = 0.2f;                             //The speed at which the Alarm Light will fade
    public float highIntensity = 2f;                                //The maximum intensity of alarm light 
    public float lowIntensity = 0.5f;                               //The Lowest intensity of alarmlight 
    public float changeMargin = 0.2f;                               //for reverting the direction of light intensity

    [HideInInspector]
    public bool alarmOn = false;                                    //This will be modified by any of the alarm sysytems

    private Light alarmLight;                                       //Referecing the alarm light.

    private float targetIntensity;                                  //
    private AudioSource sirens;                                     //



    void Awake()
    {
        alarmLight = GetComponent<Light>();
        targetIntensity = highIntensity;
        sirens = GameObject.FindGameObjectWithTag(Tags.siren).GetComponent<AudioSource>();
    }

    void Update()
    {
        if (alarmOn)
        {
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, targetIntensity, lightFadeSpeed * Time.deltaTime);      //For blinking the light when player gets detected.
            BlinkAlarmLights();
        }
        else
        {
            alarmLight.intensity = Mathf.Lerp(alarmLight.intensity, 0f, lightFadeSpeed * Time.deltaTime);       //Light Goes back to normal if player Becomes incognito.
        }

       AlarmSound();

    }


    //this function will revert the intensity direction.
    void BlinkAlarmLights()
    {
        if (Mathf.Abs(targetIntensity - alarmLight.intensity) < changeMargin)
        {
            if (targetIntensity == highIntensity)
            {
                targetIntensity = lowIntensity;
            }
            else
            {
                targetIntensity = highIntensity;
            }
        }
    }
    void AlarmSound()
    {
        if (alarmOn && !sirens.isPlaying)
        {
            sirens.Play();
        }
        else if (!alarmOn)
        {
            sirens.Stop();
        }
    }
}



