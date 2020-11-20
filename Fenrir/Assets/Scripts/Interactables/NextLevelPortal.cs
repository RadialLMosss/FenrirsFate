using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevelPortal : MonoBehaviour
{
    public static bool canUpdate;
    public Material[] magicCircleMaterials;
    public MeshRenderer meshRender;
    public GameObject swordOnTop;
    public GameObject shopIconOnTop;
    public MagicCircleRotate rotateScript;

    void Start()
    {
        meshRender.material = magicCircleMaterials[2];
        GetComponent<Collider>().enabled = false;
        shopIconOnTop.SetActive(false);
        swordOnTop.SetActive(false);
        rotateScript.enabled = false;
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
                        meshRender.material = magicCircleMaterials[1];
                        shopIconOnTop.SetActive(true);
                    }
                    else
                    {
                        meshRender.material = magicCircleMaterials[0];
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
                        meshRender.material = magicCircleMaterials[0];
                        swordOnTop.SetActive(true);
                    }
                    else
                    {
                        meshRender.material = magicCircleMaterials[0];
                    }
                }
                else
                {
                    meshRender.material = magicCircleMaterials[0];
                }
                rotateScript.enabled = true;
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}
