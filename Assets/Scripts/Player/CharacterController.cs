using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]

public class CharacterController : MonoBehaviour
{
    public float turnSmoothing = 15f;
    public float speedDampTime = 0.1f;

    Animator anim;
    HashIDs hashId;
    Rigidbody rBody;
    Quaternion finalRotation;

    public Quaternion FinalRotation
    {
        get { return finalRotation; }
    }


    void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        rBody.constraints = RigidbodyConstraints.FreezeRotation;
        anim = GetComponent<Animator>();
        hashId = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
        anim.SetLayerWeight(1, 1f);

    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool sneak = Input.GetButton("Sneak");

        MovePlayer(horizontal, vertical, sneak);

    }

    void MovePlayer(float h, float v, bool sneaking)
    {
        anim.SetBool(hashId.sneakingBool, sneaking);
        if (h != 0f || v != 0)
        {
            Rotate(h, v);
            anim.SetFloat(hashId.speedFloat, 3.4f, speedDampTime, Time.deltaTime);
        }
        else
        {
            anim.SetFloat(hashId.speedFloat, 0f);
        }
    }

    void Rotate(float horizontal, float vertical)
    {
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        finalRotation = Quaternion.Lerp(rBody.rotation, targetRotation, turnSmoothing * Time.deltaTime);
        //transform.rotation = finalRotation;
        rBody.MoveRotation(finalRotation);

    }
}
