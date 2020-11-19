using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_PostPTest : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Rigidbody rb;
    public Animator anim;
    public Player_PostPTest player;
    float totalTimer = 3;
    float runningTimer;
    public GameObject crystalPB;
    [SerializeField]private float lifePoints;
    private float baseLifePoints;
    float baseDefense = 0.5f;
    float defenseValue;
    public Transform lifeBarContainer;
    public Transform lifeBar;
    public float range = 15.0f;
    public bool isTotem;
    public AudioSource audioSource;
    public AudioClip[] clips;
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

    private void Awake()
    {
        player = FindObjectOfType<Player_PostPTest>();
    }

    private void Start()
    {
        baseLifePoints = lifePoints;
        if (lifeBar != null)
        {
            lifeBar.localScale = new Vector2(lifePoints / baseLifePoints, lifeBar.localScale.y);
        }
    }

    private void Update()
    {
        if (NavMeshAgent != null)
        {
            if (player != null)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= 20)
                {
                    if (!shouldAvoidPlayer)
                    {
                        NavMeshAgent.SetDestination(player.transform.position);
                        anim.SetBool("isWalking", true);
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -7.5f * Time.deltaTime);
                        anim.SetBool("isWalking", false);
                    }
                    if (rb.velocity != Vector3.zero)
                    {
                        rb.velocity = Vector3.zero;
                    }
                    lifeBarContainer.LookAt(Camera.main.transform);
                }
                else
                {
                    if (runningTimer >= totalTimer)
                    {
                        //random patrol
                        Vector3 point;
                        if (RandomPoint(transform.position, range, out point))
                        {
                            NavMeshAgent.SetDestination(point);
                        }
                        rb.velocity = Vector3.zero;
                        runningTimer = 0;
                    }
                    else
                    {
                        runningTimer += Time.deltaTime;
                    }
                }
            }
        }
    }

    public GameObject furyOrbPB;

    int bleedCount;
    bool isBleeding;

    IEnumerator StartBleeding()
    {
        
        isBleeding = true;
        TakeDamage(Player.enemyBleedingDamage, false);
        //bleeding's feedback
        yield return new WaitForSeconds(1);
        bleedCount++;
        if(bleedCount < 5)
        {
            StartCoroutine(StartBleeding());
        }
        else
        {
            bleedCount = 0;
            isBleeding = false;
        }
    }


    public void TakeDamage(float damage, bool shouldBleed)
    {
        lifePoints -= (damage - defenseValue);
        if(lifeBar != null)
        {
            lifeBar.localScale = new Vector2(lifePoints/baseLifePoints, lifeBar.localScale.y);
        }

        if(!shouldAvoidPlayer && !cantAvoidPlayer)
        {
            StartCoroutine(AvoidPlayer());
        }
        if (lifePoints <= 0)
        {
            GameObject furyOrb = Instantiate(furyOrbPB, transform.position, Quaternion.identity);
            furyOrb.GetComponent<Orb_PostPTest>().StartFollowingPlayer(player);
            if (Random.Range(0, 3) == 0)
            {
                GameObject crystal = Instantiate(crystalPB, transform.position, Quaternion.identity);
                Destroy(crystal, 5);
            }
            Player.enemiesToDefeat -= 1;
            if(isTotem)
            {
                Destroy(gameObject);
            }
            else
                anim.SetTrigger("Death");
            // DESTROY CALLED AS ANIM EVENT
        }
        else
        {
            anim.SetTrigger("Hit");
            if(shouldBleed && !isBleeding)
            {
                StartCoroutine(StartBleeding());
            }
            if(damage == Player.strongDamageValue)
            {
                StartCoroutine(DefenseDrop());
            }
        }
    }

    public IEnumerator DefenseDrop()
    {
        defenseValue = 0;
        yield return new WaitForSeconds(7.5f);
        defenseValue = baseDefense;
    }

    bool shouldAvoidPlayer;
    bool cantAvoidPlayer;
    IEnumerator AvoidPlayer()
    {
        shouldAvoidPlayer = true;
        yield return new WaitForSeconds(2);
        shouldAvoidPlayer = false;
        StartCoroutine(CantAvoidPlayer());
    }

    IEnumerator CantAvoidPlayer()
    {
        cantAvoidPlayer = true;
        yield return new WaitForSeconds(2);
        cantAvoidPlayer = false;
    }




}
