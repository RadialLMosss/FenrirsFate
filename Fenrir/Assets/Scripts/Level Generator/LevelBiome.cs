using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBiome - ", menuName = "Level Biome")]
public class LevelBiome : ScriptableObject
{
    [Header("Madatory:")]
    public GameObject firstSectionsPB;
    public GameObject lastSectionsPB;
    
    [Header("Optional:")]
    public GameObject combatSectionsPB;
    public GameObject shopSectionsPB;
    public GameObject bossSectionsPB;
    public GameObject puzzleSectionsPB;
    public GameObject startSectionsPB; //access to skill tree
}
