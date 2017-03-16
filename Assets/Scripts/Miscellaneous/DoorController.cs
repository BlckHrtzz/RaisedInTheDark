using UnityEngine;
using System.Collections;

public class DoorController : MonoBehaviour
{
    Animator animator;
    bool doorOpen;

    void Start()
    {
        animator = GetComponent<Animator>();
        doorOpen = false;
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.tag == "Player")
        {
            Debug.Log("Player is in the range");
            doorOpen = true;
            DoorState("Open");

        }

    }
    void OnTriggerExit(Collider coll)
    {
        if (coll.tag == "Player")
        {
            Debug.Log("Player is out of range");
            doorOpen = false;
            DoorState("Close");
        }
    }

    void DoorState(string state)
    {
        animator.SetTrigger(state);
    }
}
