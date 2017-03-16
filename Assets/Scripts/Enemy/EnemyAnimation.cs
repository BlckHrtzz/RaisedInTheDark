using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAnimation : MonoBehaviour
{
    public float deadZone = 5f;

    AnimatorSetup animatorSetup;
    EnemyFieldOfDetection enemyFieldOfDetection;
    Transform target;
    Animator enemyAnimator;
    HashIDs hashIds;
    NavMeshAgent navAgent;

    private void Awake()
    {
        enemyFieldOfDetection = GetComponent<EnemyFieldOfDetection>();
        target = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Transform>();
        enemyAnimator = GetComponent<Animator>();
        hashIds = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        navAgent = GetComponent<NavMeshAgent>();

        navAgent.updateRotation = false;
        animatorSetup = new AnimatorSetup(enemyAnimator, hashIds);

        enemyAnimator.SetLayerWeight(1, 1f);
        enemyAnimator.SetLayerWeight(2, 1f);

        deadZone *= Mathf.Deg2Rad;
    }

    private void Update()
    {
        NavigationAnimSetup();
    }

    private void OnAnimatorMove()
    {
        navAgent.velocity = enemyAnimator.deltaPosition / Time.deltaTime;
        transform.rotation = enemyAnimator.rootRotation;
    }

    void NavigationAnimSetup()
    {
        float speed;
        float angle;

        if (enemyFieldOfDetection.playerInSight)
        {
            speed = 0f;
            angle = CalcAngle(transform.forward, target.position - transform.position, transform.up);
        }

        else
        {
            speed = Vector3.Project(navAgent.desiredVelocity, transform.forward).magnitude;
            angle = CalcAngle(transform.forward, navAgent.desiredVelocity, transform.up);
            Debug.DrawRay(transform.position, navAgent.desiredVelocity);

            if (Mathf.Abs(angle) < deadZone)
            {
                transform.LookAt(transform.position + navAgent.desiredVelocity);
                angle = 0f;
            }
        }
        animatorSetup.Setup(speed, angle);
    }

    float CalcAngle(Vector3 fromVector, Vector3 toVector, Vector3 upVector)
    {
        if (toVector == Vector3.zero)
            return 0f;

        float angle = Vector3.Angle(fromVector, toVector);
        Vector3 normal = Vector3.Cross(fromVector, toVector);
        angle *= Mathf.Sign(Vector3.Dot(normal, upVector));
        angle *= Mathf.Deg2Rad;


        return angle;


    }

}
