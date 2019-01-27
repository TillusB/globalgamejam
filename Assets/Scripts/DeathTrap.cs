using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathTrap : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerBehaviour playerHit = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (playerHit)
        {
            playerHit.SetPlayerState(PlayerState.Dead);
            playerHit.transform.SetParent(transform);
        }
    }
}
