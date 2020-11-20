using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCircleRotate : MonoBehaviour
{
    Player player;
    
    private void Start()
    {
        player = FindObjectOfType<Player>();
    }
    void Update()
    {
        if(Vector3.Distance(transform.position, player.transform.position) <= 15)
        {
            transform.Rotate(Vector3.up * 25f * Time.deltaTime);
        }
    }
}
