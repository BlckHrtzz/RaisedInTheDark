using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemyFieldOfDetection))]
[RequireComponent(typeof(EnemyPath))]
[RequireComponent(typeof(EnemyShooting))]
[RequireComponent(typeof(EnemyAnimation))]

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float patrolWaitTime = 5f;
    public float chaseSpeed = 5f;
    public float chaseWaitTime = 5f;
    //public Transform[] wayPoint;

    EnemyPath enemyPathScript;
    EnemyFieldOfDetection enemyFODScript;
    PlayerHealth playerHealthScript;
    LastPlayerPosition lastPlayerPosition;

    NavMeshAgent navAgent;
    GameObject player;

    float patrolTimer;
    float chaseTimer;

    int targetWaypointIndex;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(Tags.player);

        navAgent = GetComponent<NavMeshAgent>();

        enemyPathScript = GetComponent<EnemyPath>();
        enemyFODScript = GetComponent<EnemyFieldOfDetection>();
        playerHealthScript = player.GetComponent<PlayerHealth>();
        lastPlayerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerPosition>();

        patrolTimer = 0f;
        chaseTimer = 0f;
    }

    private void Update()
    {
        if (enemyFODScript.playerInSight && !playerHealthScript.isDead)
        {
            Shooting();
        }
        else if (enemyFODScript.localPlayerLastSighting != lastPlayerPosition.playerIncognitoPosition && playerHealthScript.health > 0f)
        {
            Chasing();
        }
        else
            Patrolling();
    }

    void Shooting()
    {
        navAgent.Stop();
    }

    void Chasing()
    {
        navAgent.Resume();
        Vector3 localSightDistance = enemyFODScript.localPlayerLastSighting - transform.position;
        if (localSightDistance.sqrMagnitude > 4f)
        {
            navAgent.destination = enemyFODScript.localPlayerLastSighting;
            Debug.Log("The Destination Has Changed");
        }

        navAgent.speed = chaseSpeed;
        if (navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            chaseTimer += Time.deltaTime;

            if (chaseTimer > chaseWaitTime)
            {
                enemyFODScript.localPlayerLastSighting = lastPlayerPosition.playerIncognitoPosition;
                lastPlayerPosition.playerPosition = lastPlayerPosition.playerIncognitoPosition;
                chaseTimer = 0f;
            }
        }
        else
            chaseTimer = 0f;
    }

    void Patrolling()
    {
        navAgent.Resume();
        navAgent.speed = patrolSpeed;

        if (navAgent.destination == lastPlayerPosition.playerIncognitoPosition || navAgent.remainingDistance < navAgent.stoppingDistance)
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                if (targetWaypointIndex == enemyPathScript.globalPatrolWaypoints.Length - 1)
                {
                    targetWaypointIndex = 0;
                }
                else
                    targetWaypointIndex++;
                patrolTimer = 0f;
            }
        }
        else
            patrolTimer = 0f;


        navAgent.destination = enemyPathScript.globalPatrolWaypoints[targetWaypointIndex];
        Debug.Log(targetWaypointIndex);
    }
}
