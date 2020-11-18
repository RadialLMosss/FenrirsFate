using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Rigidbody rb;
    public Player player;
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
        player = FindObjectOfType<Player>();
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
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -7.5f * Time.deltaTime);
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
            furyOrb.GetComponent<FuryOrb>().StartFollowingPlayer(player);
            if (Random.Range(0, 3) == 0)
            {
                GameObject crystal = Instantiate(crystalPB, transform.position, Quaternion.identity);
                Destroy(crystal, 5);
            }
            Player.enemiesToDefeat -= 1;
            Destroy(gameObject);
        }
        else
        {
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
