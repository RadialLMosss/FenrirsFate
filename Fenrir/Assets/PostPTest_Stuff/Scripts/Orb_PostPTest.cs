using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb_PostPTest : MonoBehaviour
{
    void Update()
    {
        if(canFollow)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, 10 * Time.deltaTime);
            if(Vector3.Distance(transform.position, playerTransform.position) < 0.5f)
            {
                playerInst.UpdateFuryCurrency(5);
                Destroy(gameObject);
            }
        }
    }

    Player_PostPTest playerInst;
    Transform playerTransform;
    bool canFollow;

    public void StartFollowingPlayer(Player_PostPTest player)
    {
        playerInst = player;
        playerTransform = player.transform;
        canFollow = true;
    }

}
