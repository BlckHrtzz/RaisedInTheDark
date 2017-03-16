using UnityEngine;
using System.Collections;

public class CameraAlertSystem : MonoBehaviour
{

    private LastPlayerPosition lastPlayerPOsition;
    PlayerHealth playerHealth;


    void Awake()
    {
        lastPlayerPOsition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerPosition>();
        playerHealth = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerHealth>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.player && playerHealth.isDead == false)
        {
            Debug.Log("Player On Sight");
            Vector3 rayDirection = other.transform.position - transform.position;
            Debug.DrawLine(transform.position, other.transform.position);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit))
            {
                if (hit.collider.tag == Tags.player)
                {
                    Debug.Log("AlarmOn");
                    lastPlayerPOsition.playerPosition = other.transform.position;
                }
            }

        }
    }
}
