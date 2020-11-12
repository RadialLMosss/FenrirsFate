using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensor : MonoBehaviour
{
    public GameObject wall;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SensorWall"))
        {
            wall.SetActive(false);
        }
    }
}
