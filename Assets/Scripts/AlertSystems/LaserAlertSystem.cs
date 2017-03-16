using UnityEngine;
using System.Collections;

public class LaserAlertSystem : MonoBehaviour
{
    public float onTime = 5f;
    public float offTime = 5f;

    float timer;

    private LastPlayerPosition lastPlayerPosition;
    //private AlarmSystem alarmSystem;
    Renderer render;
    Light pointlight;
    AudioSource laserSound;


    void Awake()
    {
        lastPlayerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerPosition>();
       // alarmSystem = GameObject.FindGameObjectWithTag(Tags.alarmLight).GetComponent<AlarmSystem>();
        render = GetComponent<Renderer>();
        laserSound = GetComponent<AudioSource>();
        pointlight = GetComponent<Light>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (render.enabled && timer >= onTime)
        {
            SwitchLaserBeams();
            laserSound.Stop();
        }
        else if (!render.enabled && timer >= offTime)
        {
            SwitchLaserBeams();
            laserSound.Play();
        }
    }

    void SwitchLaserBeams()
    {
        timer = 0;
        render.enabled = !render.enabled;
        pointlight.enabled = !pointlight.enabled;
    }

    void OnTriggerStay(Collider other)
    {
        if (render.enabled && other.tag == Tags.player)
        {
            lastPlayerPosition.playerPosition = other.transform.position;
        }
    }
}
