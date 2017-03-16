using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDeactivation : MonoBehaviour
{

    public GameObject laser;
    public Material unlockedMaterial;

    private void Awake()
    {
        if (laser == null || laser.tag != Tags.laserGrid)
        {
            Debug.LogError("Laser Grid is not attached to the switch");
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == Tags.player)
        {
            if (Input.GetButton("LaserSwitch"))
            {
                SwitchLaser();
            }
        }
    }

    private void SwitchLaser()
    {
        Renderer screen = transform.Find("prop_switchUnit_screen").GetComponentInChildren<Renderer>();
        laser.SetActive(false);
        screen.material = unlockedMaterial;
    }
}
