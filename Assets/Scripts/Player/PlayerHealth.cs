using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health = 100f;
    public bool isDead = false;

    Animator playerAnimator;
    HashIDs hashIds;
    LastPlayerPosition lastPlayerPosition;
    ThirdPersonCharacterController playerMovement;

    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        hashIds = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<HashIDs>();
        lastPlayerPosition = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerPosition>();
        playerMovement = GetComponent<ThirdPersonCharacterController>();
    }

    private void Update()
    {
        if (health <= 0)
        {
            PlayerDying();
            lastPlayerPosition.playerPosition = lastPlayerPosition.playerIncognitoPosition;
        }
    }

    void PlayerDying()
    {
        isDead = true;
        playerAnimator.SetBool(hashIds.isDeadBool, isDead);

        playerMovement.enabled = false;

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == hashIds.dyingState)
        {
            playerAnimator.SetBool(hashIds.isDeadBool, false);
        }
    }

   public void DamageTaken(float damage)
    {
        health -= damage;
    }
}
