using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPortal : MonoBehaviour
{
    public static bool canUpdate;
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        GetComponent<Collider>().enabled = false;
    }

    private void Update()
    {
        if(canUpdate)
        {
            if (Player.enemiesToDefeat == 0)
            {
                if (GameManager.levelCount == 4 || GameManager.levelCount == 7)
                {
                    if (gameObject.CompareTag("Finish"))
                    {
                        //Show that the next level will be of type "Shop"
                        GetComponent<MeshRenderer>().material.color = Color.cyan;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                }
                else if (GameManager.levelCount == 0 || GameManager.levelCount == 9)
                {
                    if (gameObject.CompareTag("Finish"))
                    {
                        gameObject.SetActive(false);
                    }
                    else if (gameObject.CompareTag("Finish2") && GameManager.levelCount == 9)
                    {
                        GetComponent<MeshRenderer>().material.color = Color.magenta;
                    }
                    else
                    {
                        GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                }
                else
                {
                    GetComponent<MeshRenderer>().material.color = Color.white;
                }
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}
