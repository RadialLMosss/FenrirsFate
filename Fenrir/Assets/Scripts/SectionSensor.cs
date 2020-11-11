using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SectionSensor : MonoBehaviour
{
    //public GameObject wall;
    [HideInInspector] public bool isConnected;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            //wall.SetActive(false);
            isConnected = true;
        }
    }
}
