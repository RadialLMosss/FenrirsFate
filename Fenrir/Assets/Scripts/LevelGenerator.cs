using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public enum LevelType { combat, puzzle, shop, boss }
    public LevelType levelType;

    enum SectionDirection { east, north, south}
    SectionDirection sectionDirection;

    [SerializeField] LevelTemplate[] levelTemplates = null; // 1 for each 'biome'

    private LevelTemplate currentLevelTemplate;
    private GameObject currentLevelPB;

    private GameObject currentFirstSection; // first section made
    private GameObject currentLastSection; // last section made (with door to next room)
    private GameObject currentNormalSection;

    private int sectionsCount;
    private int sectionsMade;
    private List<LevelSection> levelSections = new List<LevelSection>();

    private void Start()
    {
        StartGenerator();
    }

    void StartGenerator()
    {
        ResetGenerator();

        currentLevelTemplate = levelTemplates[Random.Range(0, levelTemplates.Length)];
        currentFirstSection = currentLevelTemplate.firstSectionsPB;
        currentLastSection = currentLevelTemplate.lastSectionsPB;

        switch(levelType)
        {
            case LevelType.combat:
                sectionsCount = Random.Range(5, 10);
                currentNormalSection = currentLevelTemplate.combatSectionsPB;
                break;

            case LevelType.puzzle:
                sectionsCount = Random.Range(3, 6);
                currentNormalSection = currentLevelTemplate.puzzleSectionsPB;
                break;

            case LevelType.shop:
                sectionsCount = 3;
                currentNormalSection = currentLevelTemplate.shopSectionsPB;
                break;

            case LevelType.boss:
                sectionsCount = 1;
                currentNormalSection = currentLevelTemplate.bossSectionsPB;
                break;
        }

        GenerateLevel();
    }

    void GenerateLevel()
    {
        if(levelType != LevelType.boss)
        {
            if (sectionsMade < sectionsCount)
            {
                if (sectionsMade == 0)
                {
                    currentLevelPB = currentFirstSection;
                }
                else if (sectionsMade == sectionsCount-1)
                {
                    currentLevelPB = currentLastSection;
                }
                else
                {
                    currentLevelPB = currentNormalSection;
                }

                GameObject levelSection = Instantiate(currentLevelPB, transform.position, Quaternion.identity);
                levelSections.Add(levelSection.GetComponent<LevelSection>());

                sectionsMade++;
                if (sectionsMade != sectionsCount)
                {
                    // Move to spawn the next section at another location
                    transform.position = RandomizeSectionDirection();
                    GenerateLevel();
                }
                else
                {
                    GenerateLevelSections();
                }
            }
        }
        else
        {
            //instantiate boss section
            currentLevelPB = currentNormalSection;
            GameObject levelSection = Instantiate(currentLevelPB, transform.position, Quaternion.identity);
            levelSections.Add(levelSection.GetComponent<LevelSection>());
            GenerateLevelSections();
        }
    }

    Vector3 RandomizeSectionDirection()
    {
        int r = Random.Range(0, 3);
        switch(r)
        {
            case 0:
                sectionDirection = SectionDirection.east;
                break;

            case 1:
                if(sectionDirection == SectionDirection.south)
                {
                    int r2 = Random.Range(0, 2);
                    if(r2 == 0)
                    {
                        sectionDirection = SectionDirection.south;
                    }
                    else
                    {
                        sectionDirection = SectionDirection.east;
                    }
                }
                else
                {
                    sectionDirection = SectionDirection.north;
                }
                break;

            case 2:
                if (sectionDirection == SectionDirection.north)
                {
                    int r2 = Random.Range(0, 2);
                    if (r2 == 0)
                    {
                        sectionDirection = SectionDirection.north;
                    }
                    else
                    {
                        sectionDirection = SectionDirection.east;
                    }
                }
                else
                {
                    sectionDirection = SectionDirection.south;
                }
                break;
        }

        switch(sectionDirection)
        {
            case SectionDirection.east:
                return new Vector3(transform.position.x+30, transform.position.y, transform.position.z);

            case SectionDirection.north:
                return new Vector3(transform.position.x, transform.position.y, transform.position.z+30);

            case SectionDirection.south:
                return new Vector3(transform.position.x, transform.position.y, transform.position.z-30);

            default: // just because it is mandatory to have a default. (any direction will do)
                return new Vector3(transform.position.x+30, transform.position.y, transform.position.z);
        }
    }

    private void ResetGenerator()
    {
        sectionsMade = 0;
        transform.position = Vector3.zero;
        foreach (LevelSection section in levelSections)
        {
            Destroy(section.gameObject);
        }
        levelSections.Clear();
    }

    private void GenerateLevelSections()
    {
        foreach (LevelSection section in levelSections)
        {
            section.GenerateSection();
        }
    }
}
