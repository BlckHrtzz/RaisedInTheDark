using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public float maxDamage = 120f;
    public float minDamage = 40f;
    [Tooltip("The Position On Players Body Where The ENemy will Aim")]
    public Transform aimPoint;
    public AudioClip gunShotClip;


    Animator enemyAnimator;
    LineRenderer shootRay;
    GameObject player;

    EnemyFieldOfDetection enemyFOD;
    PlayerHealth playerHealthScript;
    HashIDs hashIds;

    SphereCollider rangeCollider;

    bool shooting = false;
    float scaledDamage;


    private void Awake()
    {
        enemyFOD = GetComponent<EnemyFieldOfDetection>();
        enemyAnimator = GetComponent<Animator>();
        shootRay = GetComponentInChildren<LineRenderer>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        playerHealthScript = player.GetComponent<PlayerHealth>();
        hashIds = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();

        rangeCollider = GetComponent<SphereCollider>();

        shootRay.enabled = false;
        shootRay.startWidth = 0.03f;
        shootRay.endWidth = 0.01f;
        scaledDamage = maxDamage - minDamage;

        if (aimPoint == null)              //To Check if Enemy has a target to shoot
        {
            Debug.LogError("Please Attach Aim Assist Target To Enemy Shoot Script");
        }
        if (gunShotClip==null)
        {
            Debug.LogError("Please Attach Gunshot Clip");
        }

    }

    //For Applying Inverse Kinematic to Enemy Shoot Animation
    private void OnAnimatorIK(int layerIndex)
    {
        float aimWeight = enemyAnimator.GetFloat(hashIds.aimWeightFloat);

        if (enemyFOD.playerInSight)
        {
            enemyAnimator.SetIKPosition(AvatarIKGoal.RightHand, aimPoint.position);         //For Setting the Hands Toward the Traget Position
            enemyAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, aimWeight);           //For Setting the Hands Toward the Traget Position

            enemyAnimator.SetLookAtPosition(aimPoint.position);                             //For Making The Head Look at Traget
            enemyAnimator.SetLookAtWeight(aimWeight);                                       // For Maikng The Head Look at Target
        }
    }

    //Function For Shooting The Player
    public void ShootEnemy()
    {
        float shot = enemyAnimator.GetFloat(hashIds.shotFloat);

        if (shot >= 0.5f && !shooting)              //Applies Damage After Shot Animation is Finished.
        {
            ApplyDamageToPlayer();
        }

        if (shot < 0.5f)
        {
            shootRay.enabled = false;
            shooting = false;
        }

    }

    //Function for applying Damage to Player after the shooying Animation Finishes
    void ApplyDamageToPlayer()
    {
        shooting = true;
        float fractionalDamage = (rangeCollider.radius - Vector3.Distance(transform.position, player.transform.position)) / rangeCollider.radius;
        float damageToPlayer = scaledDamage * fractionalDamage + minDamage;
        playerHealthScript.DamageTaken(damageToPlayer);

        ShotFx();
    }

    //Function For Creating Shot Effects
    void ShotFx()
    {
        shootRay.SetPosition(0, shootRay.transform.position);
        shootRay.SetPosition(1, aimPoint.position);
        shootRay.enabled = true;
        AudioSource.PlayClipAtPoint(gunShotClip, shootRay.transform.position);
    }

}
