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

    public void GenerateSection()
    {
        if(!isNotRandomized)
        {
            //verificar pelos sensores quais caminhos estão ligados com outros
            print(sectionSensors.Length);
            if (sectionSensors[0].isConnected && sectionSensors[1].isConnected)
            {
                directionsCombining = DirectionsCombining.NS;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[2].isConnected)
            {
                directionsCombining = DirectionsCombining.NE;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.NW;
            }
            else if (sectionSensors[1].isConnected && sectionSensors[2].isConnected)
            {
                directionsCombining = DirectionsCombining.SE;
            }
            else if (sectionSensors[1].isConnected && sectionSensors[3].isConnected)
            {
                directionsCombining = DirectionsCombining.SW;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[1].isConnected && sectionSensors[2].isConnected)
            {
                directionsCombining = DirectionsCombining.NSE;
            }
            else if (sectionSensors[0].isConnected && sectionSensors[1].isConnected && sectionSensors[3].isConnected)
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



            // Generate Walls
            /*
             * 
             * free sensors:
             * west & north gererate big walls (depends on the biome, if surrounded by water it doesn't need big walls)
             * east & south generate small walls
             * 
            */
            // && foreGround Parallax

            // section design && poppulation + exits & enterings combining (use sensors to do that)
        }
    }
}
