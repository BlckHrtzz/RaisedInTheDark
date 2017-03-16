using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public float cameraSmoothTime = 0.09f;                               //The Time taken by Camera to reach the Destination. Keep Default for best Results.
    public Vector3 offsetFromTarget = new Vector3(-0.4f, 1.9f, -2f);    //The offset of the camera from the player. Keepn default for the best Results.
    public float xTilt = 15f;                                           // How the camara will be rotated on X-axis.
    public Transform target;

    Vector3 destination = Vector3.zero;
    CharacterController characterController;
    float rotateVel;

    void Awake()
    {

        //Initialize the values
        SetTarget(target);
    }

    void LateUpdate()
    {
        MoveCamera();
        LookAtTarget();
    }

    void SetTarget(Transform t)
    {
        target = t;
        if (target != null)
        {
            if (target.GetComponent<CharacterController>())
            {
                characterController = target.GetComponent<CharacterController>();
            }
            else
                Debug.LogError("Your Target is Missing a character controller");
        }
        else
            Debug.LogError("Camera Needs a target");
    }


    void MoveCamera()
    {
        destination = characterController.FinalRotation * offsetFromTarget;
        destination += target.position;
        transform.position = destination;
    }

    void LookAtTarget()
    {
        float eulerYAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target.eulerAngles.y, ref rotateVel, cameraSmoothTime);
        transform.rotation = Quaternion.Euler(xTilt, eulerYAngle, 0f);
    }

}
