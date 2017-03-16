using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetup
{
    public float speedDampTime = 0.1f;
    public float angularSpeedDampTime = 0.7f;
    public float angleResponseTime = 0.6f;

    Animator enemyAimator;
    HashIDs hashIds;

    public AnimatorSetup(Animator animator, HashIDs hash)
    {
        enemyAimator = animator;
        hashIds = hash;
    }
    public void Setup(float speed, float angle)
    {
        float angularSpeed = angle / angleResponseTime;
        enemyAimator.SetFloat(hashIds.angularSpeedFloat, angularSpeed, angularSpeedDampTime, Time.deltaTime);
        enemyAimator.SetFloat(hashIds.speedFloat, speed, speedDampTime, Time.deltaTime);
    }

}
