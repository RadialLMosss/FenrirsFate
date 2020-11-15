using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpikeTrap : MonoBehaviour
{
    float delay;
    public Transform spikesTransform;

    public enum TrapType { FlipFlop, IfTouched}
    [HideInInspector] public TrapType trapType;
    [HideInInspector] public bool canActivate;

    void Start()
    {
        if(Random.Range(0, 2) == 0)
        {
            trapType = TrapType.FlipFlop;
        }
        else
        {
            trapType = TrapType.IfTouched;
        }

        //============================================================================
        
        if(trapType == TrapType.FlipFlop)
        {
            if (Random.Range(2, 4) == 2)
            {
                delay = 0.75f;
            }
            else
            {
                delay = 1.5f;
            }
            StartCoroutine(FlipFlopSpikes());
        }
        else
        {
            canActivate = true;
            delay = 1f;
        }

    }

    IEnumerator FlipFlopSpikes()
    {
        yield return new WaitForSeconds(delay);
        if(spikesTransform != null)
        spikesTransform.DOMoveY(spikesTransform.position.y + 2f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(delay);
        if(spikesTransform != null)
        spikesTransform.DOMoveY(spikesTransform.position.y - 2f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        if(spikesTransform != null)
        StartCoroutine(FlipFlopSpikes());
    }


    public IEnumerator DelayedSpikes()
    {
        if(canActivate)
        {
            canActivate = false;
            yield return new WaitForSeconds(delay);
            if(spikesTransform != null)
            spikesTransform.DOMoveY(spikesTransform.position.y + 2f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            if(spikesTransform != null)
            spikesTransform.DOMoveY(spikesTransform.position.y - 2f, 0.5f);
            yield return new WaitForSeconds(0.5f);
            canActivate = true;
        }
    }
}
