using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public static int levelCount = -1;
    [SerializeField] LevelGenerator levelGenerator = null;
    bool hasRepeatedCombat;
    bool hasRepeatedPuzzle;

    private void Start()
    {
        NextLevel(1);
    }

    public void NextLevel(int nextIndex)
    {
        levelGenerator.ResetGenerator();
        if ((levelCount == 4 || levelCount == 7) && nextIndex == 1) //Shop Type
        {
            LevelGenerator.levelType = LevelGenerator.LevelType.shop;
        }
        else if(levelCount == 9) // Boss Type
        {
            LevelGenerator.levelType = LevelGenerator.LevelType.boss;
        }
        else if(levelCount == -1)
        {
            LevelGenerator.levelType = LevelGenerator.LevelType.start;
        }
        else if(levelCount == 0)
        {
            LevelGenerator.levelType = LevelGenerator.LevelType.combat;
        }
        else
        {
            if(Random.Range(0, 2) == 0)
            {
                if (nextIndex == 1) //combat
                {
                    if (LevelGenerator.levelType != LevelGenerator.LevelType.combat)
                    {
                        LevelGenerator.levelType = LevelGenerator.LevelType.combat;
                    }
                    else
                    {
                        if (!hasRepeatedCombat)
                        {
                            hasRepeatedCombat = true;
                        }
                        else
                        {
                            LevelGenerator.levelType = LevelGenerator.LevelType.puzzle;
                            hasRepeatedCombat = false;
                        }
                    }
                }

                else //puzzle
                {
                    if (LevelGenerator.levelType != LevelGenerator.LevelType.puzzle)
                    {
                        LevelGenerator.levelType = LevelGenerator.LevelType.puzzle;
                    }
                    else
                    {
                        if (!hasRepeatedPuzzle)
                        {
                            hasRepeatedPuzzle = true;
                        }
                        else
                        {
                            LevelGenerator.levelType = LevelGenerator.LevelType.combat;
                            hasRepeatedPuzzle = false;
                        }
                    }
                }
            }
            else
            {
                if (nextIndex == 1) //puzzle
                {
                    if(LevelGenerator.levelType != LevelGenerator.LevelType.puzzle)
                    {
                        LevelGenerator.levelType = LevelGenerator.LevelType.puzzle;
                    }
                    else
                    {
                        if (!hasRepeatedPuzzle)
                        {
                            hasRepeatedPuzzle = true;
                        }
                        else
                        {
                            LevelGenerator.levelType = LevelGenerator.LevelType.combat;
                            hasRepeatedPuzzle = false;
                        }
                    }
                }

                else //combat
                {
                    if (LevelGenerator.levelType != LevelGenerator.LevelType.combat)
                    {
                        LevelGenerator.levelType = LevelGenerator.LevelType.combat;
                    }
                    else
                    {
                        if (!hasRepeatedCombat)
                        {
                            hasRepeatedCombat = true;
                        }
                        else
                        {
                            LevelGenerator.levelType = LevelGenerator.LevelType.puzzle;
                            hasRepeatedCombat = false;
                        }
                    }
                }
            }
        }
        // DECIDE ENEMY GROUP || Sections Counts
        if (LevelGenerator.levelType == LevelGenerator.LevelType.combat || LevelGenerator.levelType == LevelGenerator.LevelType.puzzle)
        {
            if (levelCount < 3) //1 && 2
            {
                levelGenerator.enemyGroupIndex = 0;
                levelGenerator.sectionsCount = 4;
            }
            else if (levelCount < 5 && levelCount > 2) // 3 && 4
            {
                levelGenerator.enemyGroupIndex = 1;
                levelGenerator.sectionsCount = 6;
            }
            else if (levelCount > 4 && levelCount < 7) // 5 && 6
            {
                levelGenerator.enemyGroupIndex = 2;
                levelGenerator.sectionsCount = 8;
            }
            else // 7 && 8
            {
                levelGenerator.enemyGroupIndex = 3;
                levelGenerator.sectionsCount = 10;
            }
        }

        levelCount++;
        levelGenerator.StartGenerator();
    }
}
