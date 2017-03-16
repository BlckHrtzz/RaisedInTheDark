using UnityEngine;
using System.Collections;

public class HashIDs : MonoBehaviour
{

    public int dyingState;
    public int isDeadBool;
    public int walkState;
    public int speedFloat;
    public int sneakState;
    public int sneakingBool;
    public int runningState;
    public int runningBool;
    public int angularSpeedFloat;
    public int playerInSightBool;
    public int shotFloat;
    public int aimWeightFloat;


    void Awake()
    {
        dyingState = Animator.StringToHash("Base Layer.Dying");
        isDeadBool = Animator.StringToHash("IsDead");
        walkState = Animator.StringToHash("Base Layer.Walk");
        speedFloat = Animator.StringToHash("Speed");
        sneakState = Animator.StringToHash("Base Layer.Sneak");
        sneakingBool = Animator.StringToHash("Sneaking");
        runningState = Animator.StringToHash("Base Layer.Running");
        runningBool = Animator.StringToHash("Running");
        angularSpeedFloat = Animator.StringToHash("AngularSpeed");
        playerInSightBool = Animator.StringToHash("PlayerInSight");
        shotFloat = Animator.StringToHash("Shot");
        aimWeightFloat = Animator.StringToHash("AimWeight");

    }
}
