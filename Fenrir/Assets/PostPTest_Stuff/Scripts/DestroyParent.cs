using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParent : MonoBehaviour
{
    public void DwarfDeath()
    {
        Destroy(transform.parent.gameObject);
    }

    public void FenrirDeath()
    {
        transform.GetComponentInParent<Player_PostPTest>().PlayerDeath();
    }

    public void FenrirPlaying()
    {
        transform.GetComponentInParent<Player_PostPTest>().anim.SetBool("Playing", true);
        transform.GetComponentInParent<Player_PostPTest>().playing = true;
    }
}
