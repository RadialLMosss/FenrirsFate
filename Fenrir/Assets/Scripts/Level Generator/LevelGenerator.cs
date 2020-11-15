using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public GameObject water; // should activate only after baking the navmesh
    public enum LevelType { start, combat, puzzle, shop, boss }
    public static LevelType levelType;

    [HideInInspector] public int enemyGroupIndex; //each number represents a different group of enemies (types && quantity)
    public EnemyGroup[] enemyGroups;
    private List<GameObject> enemiesSpawned = new List<GameObject>();
    enum SectionDirection { east, north, south}
    SectionDirection sectionDirection;

    [SerializeField] LevelBiome[] levelTemplates = null; // 1 for each 'biome'

    private LevelBiome currentLevelTemplate;
    private GameObject currentLevelPB;

    private GameObject currentFirstSection; // first section made
    private GameObject currentLastSection; // last section made (with door to next room)
    private GameObject currentNormalSection;

    [HideInInspector] public int sectionsCount;
    private int sectionsMade;
    private List<LevelSection> levelSections = new List<LevelSection>();

    [SerializeField] private NavMeshSurface navMeshParent = null;
    public Transform playerTransform;
    public Collider playerCollider;

    public Image levelTransitionScreen;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            ResetGenerator();
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            StartGenerator();
        }
    }

    public void StartGenerator()
    {
        levelTransitionScreen.gameObject.SetActive(true);
        ResetGenerator();

        currentLevelTemplate = levelTemplates[Random.Range(0, levelTemplates.Length)];
        currentFirstSection = currentLevelTemplate.firstSectionsPB;
        currentLastSection = currentLevelTemplate.lastSectionsPB;

        switch(levelType)
        {
            case LevelType.combat:
                currentNormalSection = currentLevelTemplate.combatSectionsPB;
                break;

            case LevelType.puzzle:
                currentNormalSection = currentLevelTemplate.puzzleSectionsPB;
                break;

            case LevelType.start:
                sectionsCount = 3;
                currentNormalSection = currentLevelTemplate.startSectionsPB;
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
                levelSection.transform.SetParent(navMeshParent.transform);
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
            levelSection.transform.SetParent(navMeshParent.transform);
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

    public void ResetGenerator()
    {
        // ficar instanciando e destruindo não é uma boa ideia, depois melhorar isso!
        //   -> Talvez usar Addressables ou tentar limpar GarbageCollector de outra forma
        sectionsMade = 0;
        transform.position = Vector3.zero;
        foreach (LevelSection section in levelSections)
        {
            Destroy(section.gameObject);
        }
        levelSections.Clear();

        foreach (GameObject enemie in enemiesSpawned)
        {
            Destroy(enemie);
        }
        enemiesSpawned.Clear();
        water.SetActive(false);
        NextLevelPortal.canUpdate = false;
        Player.enemiesToDefeat = 0;
    }


    private void GenerateLevelSections()
    {
        playerCollider.enabled = false;
        foreach (LevelSection section in levelSections)
        {
            section.GenerateSection();
        }
        StartCoroutine(BakeNavMesh());
    }

    private IEnumerator BakeNavMesh()
    {
        yield return new WaitForSeconds(5);
        navMeshParent.BuildNavMesh();
        yield return new WaitForSeconds(1);
        if(levelType == LevelType.combat)
        {
            SpawnEnemies();
        }
        else if(levelType == LevelType.puzzle)
        {
            SpawnPuzzleTotems();
        }
        ResetPlayerPosition();
    }

    private void ResetPlayerPosition()
    {
        playerTransform.position = new Vector3(levelSections[0].transform.position.x, 1, levelSections[0].transform.position.z);
        playerCollider.enabled = true;
        levelTransitionScreen.gameObject.SetActive(false);
        water.SetActive(true);
        Player.enemiesToDefeat = enemiesSpawned.Count;
        NextLevelPortal.canUpdate = true;
    }

    public GameObject puzzleTotemPB;
    void SpawnPuzzleTotems()
    {
        Vector3 point;
        for (int i = 0; i < levelSections.Count; i++)
        {
            if(i!=0 && i< levelSections.Count-1)
            {
                if (RandomPoint(levelSections[i].transform.position, range, out point))
                {
                    GameObject puzzleTotem = Instantiate(puzzleTotemPB, point, Quaternion.identity);
                    enemiesSpawned.Add(puzzleTotem);
                }
            }
        }
    }

    void SpawnEnemies()
    {
        int levelIndex = 1;
        for (int i = 0; i < enemyGroups[enemyGroupIndex].enemies.Length; i++)
        {
            if (i < 2)
            {
                levelIndex = 1;
            }
            else if (i > 1 && i < 4)
            {
                levelIndex = 2;
            }
            else if (i > 3 && i < 6)
            {
                levelIndex = 3;
            }
            else if (i > 5 && i < 8)
            {
                levelIndex = 4;
            }
            else if (i > 7 && i < 10)
            {
                levelIndex = 5;
            }
            else if (i > 9 && i < 12)
            {
                levelIndex = 6;
            }
            else if(i > 11)
            {
                levelIndex = 7;
            }

            Vector3 point;
            if (RandomPoint(levelSections[levelIndex].transform.position, range, out point))
            {
                GameObject enemie = Instantiate(enemyGroups[enemyGroupIndex].enemies[i], point, Quaternion.identity);
                enemiesSpawned.Add(enemie);
            }
        }
    }

    private float range = 8.0f;
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }
}
