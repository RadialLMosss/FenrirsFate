using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBlock : MonoBehaviour
{
    [SerializeField] GameObject[] blockOptions = null;
    [SerializeField] GameObject[] floorOptions = null;

    void Start()
    {
        if(blockOptions != null && blockOptions.Length > 0)
        {
            int rBlock = Random.Range(0, blockOptions.Length);
            if (blockOptions[rBlock] != null)
            {
                GameObject blockInst = Instantiate(blockOptions[rBlock], transform.position, Quaternion.identity);
                blockInst.transform.SetParent(this.transform);
            }
        }

        if(floorOptions != null && floorOptions.Length > 0)
        {
            int rFloor = Random.Range(0, floorOptions.Length);
            if (floorOptions[rFloor] != null)
            {
                GameObject floorOption = Instantiate(floorOptions[rFloor], transform.position, Quaternion.identity);
                floorOption.transform.SetParent(this.transform);
            }
        }
    }

}
