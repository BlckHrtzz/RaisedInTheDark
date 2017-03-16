using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class ThirdPersonCharacterController : MonoBehaviour
{
    public float moveVelocity;
    public float rotateVelocity;
    Quaternion targetRotation;

    Animator anim;
    HashIDs hashIds;
    Rigidbody rBody;
    float forwardInput, turnInput;
    bool sneak, run, moving;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    void Awake()
    {
        targetRotation = transform.rotation;
        sneak = run = false;
        forwardInput = turnInput = 0;
        anim = GetComponent<Animator>();
        hashIds = GameObject.FindGameObjectWithTag("GameController").GetComponent<HashIDs>();
        rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        GetInput();
        TurnPlayer(turnInput);
        MovePlayer(forwardInput, sneak, run);
    }

    void FixedUpdate()
    {
        
    }

    void GetInput()
    {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");

        if (run == false)
        {
            sneak = Input.GetButton("Sneak");
        }

        if (sneak == false)
        {
            run = Input.GetButton("Run");
        }

    }

    void MovePlayer(float _forwardInput, bool _sneak, bool _run)
    {
        anim.SetBool(hashIds.sneakingBool, _sneak);
        anim.SetBool(hashIds.runningBool, _run);

        //rBody.velocity = transform.forward * moveVelocity * _forwardInput;
        if (_forwardInput != 0)
        {
            anim.SetFloat(hashIds.speedFloat, 1f);
        }
        else
        {
            anim.SetFloat(hashIds.speedFloat, 0f);
        }
    }

    void TurnPlayer(float _turnInput)
    {
        targetRotation *= Quaternion.AngleAxis(rotateVelocity * _turnInput * Time.deltaTime, Vector3.up);
        transform.rotation = targetRotation;

    }

}
