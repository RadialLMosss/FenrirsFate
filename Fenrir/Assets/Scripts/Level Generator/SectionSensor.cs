using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SectionSensor : MonoBehaviour
{
    public GameObject sectionWall;
    [HideInInspector] public bool isConnected;


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("SectionSensor"))
        {
            sectionWall.SetActive(false);
            isConnected = true;
        }
    }
}
