using UnityEngine;
using System.Collections;

public class LastPlayerPosition : MonoBehaviour
{
    public Vector3 playerPosition = new Vector3(1000, 1000, 1000);                      //Arbitary position when enemies are not aware of player position.
    public Vector3 playerIncognitoPosition = new Vector3(1000, 1000, 1000);


    private AlarmSystem alarmSystem;
    
    void Awake()
    {
        alarmSystem = GameObject.FindGameObjectWithTag(Tags.alarmLight).GetComponent<AlarmSystem>();
       
    }
    void Update()
    {
        SwitchAlarm();
    }

    //Function to change the Alarm state.
    void SwitchAlarm()
    {
        alarmSystem.alarmOn = (playerPosition != playerIncognitoPosition);
    }

   
}
