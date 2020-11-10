using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelTemplate - ", menuName = "Level Template - Biome")]
public class LevelTemplate : ScriptableObject
{
    [Header("Madatory:")]
    public GameObject firstSectionsPB;
    public GameObject lastSectionsPB;
    
    [Header("Optional:")]
    public GameObject combatSectionsPB;
    public GameObject shopSectionsPB;
    public GameObject bossSectionsPB;
    public GameObject puzzleSectionsPB;
}
