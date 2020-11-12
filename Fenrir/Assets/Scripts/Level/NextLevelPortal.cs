using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPortal : MonoBehaviour
{
    void Start()
    {
        if(GameManager.levelCount == 4 || GameManager.levelCount == 7)
        {
            if(gameObject.CompareTag("Finish"))
            {
                //Show that the next level will be of type "Shop"
                GetComponent<MeshRenderer>().material.color = Color.cyan;
            }
        }
    }
}
