using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPath : MonoBehaviour
{
    public Vector3[] patrolPoints;
    [HideInInspector]public Vector3[] globalPatrolWaypoints;

    private void Awake()
    {
        globalPatrolWaypoints = new Vector3[patrolPoints.Length];

        for (int i = 0; i < globalPatrolWaypoints.Length; i++)
        {
            globalPatrolWaypoints[i] = patrolPoints[i] + transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Vector3 wayPointPostion = (Application.isPlaying) ? globalPatrolWaypoints[i] : patrolPoints[i] + transform.position;
            Gizmos.DrawWireSphere(wayPointPostion, 0.1f);
            
        }
    }

}
