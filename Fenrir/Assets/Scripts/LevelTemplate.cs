using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTemplate : MonoBehaviour
{
    public GameObject[] children;
    private void OnEnable()
    {
        if(children != null)
        {
            children[Random.Range(0, children.Length)].gameObject.SetActive(true);
        }
    }
}
