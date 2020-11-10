using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SectionSensor : MonoBehaviour
{
    public GameObject wall;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Finish"))
        {
            wall.SetActive(false);
        }
        //this.gameObject.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
    }
}
