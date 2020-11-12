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

    [SerializeField] Text levelCountText = null;
    [SerializeField] Text levelTypeText = null;
    [HideInInspector] public static int levelCount = 0;
    [SerializeField] LevelGenerator levelGenerator = null;

    private void Start()
    {
        NextLevel(1);
    }

    public void NextLevel(int nextIndex)
    {
        levelGenerator.ResetGenerator();

        if ((levelCount == 4 || levelCount == 8) && nextIndex == 1) //Shop Type
        {
            levelGenerator.levelType = LevelGenerator.LevelType.shop;
        }
        else if(levelCount == 9) // Boss Type
        {
            levelGenerator.levelType = LevelGenerator.LevelType.boss;
        }
        else if(levelCount == 0)
        {
            levelGenerator.levelType = LevelGenerator.LevelType.start;
        }
        else if(levelCount == 1)
        {
            levelGenerator.levelType = LevelGenerator.LevelType.combat;
        }
        else
        {
            if(Random.Range(0, 2) == 0)
            {
                if (nextIndex == 1)
                {
                    levelGenerator.levelType = LevelGenerator.LevelType.combat;
                }
                else
                {
                    levelGenerator.levelType = LevelGenerator.LevelType.puzzle;
                }
            }
            else
            {
                if (nextIndex == 1)
                {
                    levelGenerator.levelType = LevelGenerator.LevelType.puzzle;
                }
                else
                {
                    levelGenerator.levelType = LevelGenerator.LevelType.combat;
                }
            }

            // DECIDE ENEMY GROUP
            if(levelGenerator.levelType == LevelGenerator.LevelType.combat)
            {
                if(levelCount == 2)
                {
                    levelGenerator.enemyGroupIndex = 0;
                    levelGenerator.sectionsCount = 4;
                }
                else if(levelCount < 5)
                {
                    levelGenerator.enemyGroupIndex = 1;
                    levelGenerator.sectionsCount = 6;
                }
                else if(levelGenerator.enemyGroupIndex >= 5 && levelGenerator.enemyGroupIndex < 7)
                {
                    levelGenerator.enemyGroupIndex = 2;
                    levelGenerator.sectionsCount = 8;
                }
                else
                {
                    levelGenerator.enemyGroupIndex = 3;
                    levelGenerator.sectionsCount = 10;
                }
            }
        }

        levelCount++;
        levelCountText.text = levelCount.ToString();
        levelTypeText.text = levelGenerator.levelType.ToString();
        levelGenerator.StartGenerator();
    }
}
