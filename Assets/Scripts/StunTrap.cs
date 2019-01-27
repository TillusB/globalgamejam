using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class StunTrap : MonoBehaviour
{
    public float stunTime;
    public bool despawns;
    
    private void OnCollisionEnter(Collision collision)
    {
        PlayerBehaviour playerHit = collision.gameObject.GetComponent<PlayerBehaviour>();
        if (playerHit)
        {
            playerHit.State = PlayerState.Stunned;
            if (despawns)
            {
                StartCoroutine(IsPlayerStuck(playerHit));
            }
        }
    }

    private IEnumerator IsPlayerStuck(PlayerBehaviour player)
    {
        while(player.State == PlayerState.Stunned)
        {
            yield return null;
        }
        if(despawns && player.State != PlayerState.Stunned)
        {
            gameObject.SetActive(false);
        }
    }
}
