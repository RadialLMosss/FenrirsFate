using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSection : MonoBehaviour
{
    public SectionSensor[] sectionSensors;
    public GameObject[] sectionTemplates;

    enum DirectionsCombining { NS, NE, NW, SE, SW, NSE, NSW, NSWE}
    DirectionsCombining directionsCombining;

    public bool isNotRandomized;
    /*
     * north = 0
     * south = 1
     * east = 2
     * west = 3
     */


    IEnumerator Generate()
    {
        yield return new WaitForSeconds(3);
        if (!isNotRandomized)
        {
            //verificar pelos sensores quais caminhos estão ligados com outros
            if (sectionSensors[0].isConnected && sectionSensors[1].isConnected && !sectionSensors[2].isConnected && !sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.NS;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[2].isConnected && !sectionSensors[1].isConnected && !sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.NE;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[3].isConnected && !sectionSensors[1].isConnected && !sectionSensors[2].isConnected)
            {
                directionsCombining = DirectionsCombining.NW;
            }
            else if (sectionSensors[1].isConnected && sectionSensors[2].isConnected && !sectionSensors[0].isConnected && !sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.SE;
            }
            else if (sectionSensors[1].isConnected && sectionSensors[3].isConnected && !sectionSensors[0].isConnected && !sectionSensors[2].isConnected)
            {
                directionsCombining = DirectionsCombining.SW;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[1].isConnected && sectionSensors[2].isConnected && !sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.NSE;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[1].isConnected && sectionSensors[3].isConnected && !sectionSensors[2].isConnected)
            {
                directionsCombining = DirectionsCombining.NSW;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[1].isConnected && sectionSensors[2].isConnected && sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.NSWE;
            }


            // com base na verificação dos sensores, ativar o template correto
            switch (directionsCombining)
            {
                case DirectionsCombining.NS:
                    sectionTemplates[0].SetActive(true);
                    break;

                case DirectionsCombining.NE:
                    sectionTemplates[1].SetActive(true);
                    break;

                case DirectionsCombining.NW:
                    sectionTemplates[2].SetActive(true);
                    break;

                case DirectionsCombining.SE:
                    sectionTemplates[3].SetActive(true);
                    break;

                case DirectionsCombining.SW:
                    sectionTemplates[4].SetActive(true);
                    break;

                case DirectionsCombining.NSE:
                    sectionTemplates[5].SetActive(true);
                    break;

                case DirectionsCombining.NSW:
                    sectionTemplates[6].SetActive(true);
                    break;

                case DirectionsCombining.NSWE:
                    sectionTemplates[7].SetActive(true);
                    break;
            }


        }

    }

    public void GenerateSection()
    {
        StartCoroutine(Generate());
    }
}
