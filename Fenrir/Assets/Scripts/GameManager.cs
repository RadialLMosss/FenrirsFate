using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // progression markers (room index where things get different && HOW it get different)
    // maybe make it a list of classes and every time the player get to go to a new room we see if there is a marker there
    // difficulty boost, biome change, boss, dialogue box, shop...
    [SerializeField] Canvas levelCanvas = null;
    [SerializeField] Text levelCountText = null;
    [SerializeField] Text levelTypeText = null;
    int levelCount = 1;

    private void Awake()
    {
        GameManager inst = FindObjectOfType<GameManager>();
        if(inst != null && inst != this)
        {
            Destroy(inst.gameObject);
        }
        DontDestroyOnLoad(levelCanvas);
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(1);
    }

    public void NextLevel(int nextIndex)
    {
        levelCount++;
        levelCountText.text = "ROOM = " + levelCount.ToString();
        levelTypeText.text = "TYPE = " + levelCount.ToString();
        SceneManager.LoadScene(1);


        /*
         * Main Menu + story presentation
         * 
         * + paredes serem ramdomicas (pra permitir diferentes possibilidades)
        - inimigos seguindo e causando dano => ao morrer volta pro quarto 1, mas com fury + habilidades já adquiridas
         * 
         * 
         * resetlevel
         * change type (based on the level count && nextIndex) 
         *      -> Create a START_LEVEL type (Fury currency + Skill Tree)
         *      -> Develop Shop Type (crystals currency, if possible = craft system)
         * generatelevel + Spawn Enemies if combat type
         *  enemies = access list of levelsection.transform.position -> RandomPointOnNavMesh
         *          keep on patrolling until player get in radius, then 'combat mode'
         *          receive fury & crystals by defeating them
         * levelCount++
         * 
         * 
         */


    }

}
