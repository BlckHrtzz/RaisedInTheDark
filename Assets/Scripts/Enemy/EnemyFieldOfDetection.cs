using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFieldOfDetection : MonoBehaviour
{
    public float fieldOfViewAngle = 110;
    public bool playerInSight;
    [HideInInspector]
    public Vector3 localPlayerLastSighting;

    NavMeshAgent nav;
    GameObject player;
    Animator playerAnimator;
    SphereCollider fieldOfDetection;
    Animator enemyAnimator;
    LastPlayerPosition lastPlayerPosition;
    HashIDs hashIds;
    PlayerHealth playerHealth;
    EnemyShooting enemyShooting;
    Vector3 previousSightingPosition;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerAnimator = player.GetComponent<Animator>();
        playerHealth = player.GetComponent<PlayerHealth>();
        fieldOfDetection = GetComponent<SphereCollider>();
        enemyAnimator = GetComponent<Animator>();
        enemyShooting = GetComponent<EnemyShooting>();
        lastPlayerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerPosition>();
        hashIds = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        localPlayerLastSighting = lastPlayerPosition.playerIncognitoPosition;
        previousSightingPosition = lastPlayerPosition.playerIncognitoPosition;
    }

    private void Update()
    {
        if (lastPlayerPosition.playerPosition != previousSightingPosition)
            localPlayerLastSighting = lastPlayerPosition.playerPosition;

        previousSightingPosition = lastPlayerPosition.playerPosition;
        //previousSightingPosition = localPlayerLastSighting;


        if (!playerHealth.isDead)
        {
            enemyAnimator.SetBool(hashIds.playerInSightBool, playerInSight);
        }
        else enemyAnimator.SetBool(hashIds.playerInSightBool, false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            // Debug.Log("Player is in Enemy Range");
            playerInSight = false;
            Vector3 direction = player.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle <= (fieldOfViewAngle / 2) && !playerHealth.isDead)
            {
                //Debug.Log("Player Inside FieldOfView");
                RaycastHit hit;
                if (Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, fieldOfDetection.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                        playerInSight = true;
                        lastPlayerPosition.playerPosition = player.transform.position;
                        enemyShooting.ShootEnemy();
                    }
                }
            }

            int playerStateHash = playerAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;

            if (playerStateHash == hashIds.runningState || playerStateHash == hashIds.walkState)
            {
                if (CalculatePathLength(player.transform.position) <= fieldOfDetection.radius)
                {
                    localPlayerLastSighting = player.transform.position;
                    //Debug.Log(localPlayerLastSighting);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            playerInSight = false;
            //  Debug.Log("Player is Out Of Enemies Range");
        }
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (nav.enabled)
            nav.CalculatePath(targetPosition, path);

        Vector3[] wayPoints = new Vector3[path.corners.Length];
        //  Debug.Log("Length of WayPoint Array " + wayPoints.Length);

        wayPoints[0] = transform.position;
        wayPoints[wayPoints.Length - 1] = targetPosition;

        for (int i = 0; i < path.corners.Length; i++)
        {
            wayPoints[i] = path.corners[i];
        }

        float pathLength = 0;
        for (int i = 0; i < wayPoints.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
            pathLength += Vector3.Distance(wayPoints[i], wayPoints[i + 1]);
        }

        // Debug.Log("The Length Of the Path is " + pathLength);
        return pathLength;
    }
}

