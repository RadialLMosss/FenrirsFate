using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Rigidbody rb;
    public Player player;
    public Animator anim;
    float totalTimer = 3;
    float runningTimer;
    [SerializeField] private float lifePoints;
    private float baseLifePoints;
    public Image lifeBar;
    public float range = 15.0f;
    public AudioSource audioSource;
    public AudioClip[] clips;

    public GameObject swordAttack1;
    public Animator[] swordsAttack1;
    int swordAttack1MoveCount;

    public GameObject swordAttack2;
    public Animator[] swordsAttack2;
    int swordAttack2MoveCount;

    IEnumerator SwordAttack1()
    {
        if (swordAttack1MoveCount == 0)
        {
            isAttacking = true;
            anim.SetTrigger("Attack1");
            audioSource.PlayOneShot(clips[2]);
        }
        for (int i = 0; i < swordsAttack1.Length; i++)
        {
            swordsAttack1[i].Play("SwordUP");
        }
        yield return new WaitForSeconds(3f);
        swordAttack1.transform.position = new Vector3(swordAttack1.transform.position.x, swordAttack1.transform.position.y, swordAttack1.transform.position.z - 3.30f);
        swordAttack1MoveCount++;
        if(swordAttack1MoveCount < 2)
        {
            StartCoroutine(SwordAttack1());
        }
        else
        {
            swordAttack1.transform.position = new Vector3(swordAttack1.transform.position.x, swordAttack1.transform.position.y, 13.30f);
            swordAttack1MoveCount = 0;
            isAttacking = false;
        }
    }


    IEnumerator SwordAttack2()
    {
        if(swordAttack2MoveCount == 0)
        {
            isAttacking = true;
            anim.SetTrigger("Attack2");
            audioSource.PlayOneShot(clips[1]);            
        }
        for (int i = 0; i < swordsAttack2.Length; i++)
        {
            swordsAttack2[i].Play("SwordUP");
        }
        yield return new WaitForSeconds(3f);
        swordAttack2.transform.position = new Vector3(swordAttack2.transform.position.x, swordAttack2.transform.position.y, swordAttack2.transform.position.z - 3.30f);
        swordAttack2MoveCount++;
        if (swordAttack2MoveCount < 5)
        {
            StartCoroutine(SwordAttack2());
        }
        else
        {
            swordAttack2.transform.position = new Vector3(swordAttack2.transform.position.x, swordAttack2.transform.position.y, 13.30f);
            swordAttack2MoveCount = 0;
            isAttacking = false;
        }
    }

    bool isAttacking;
    IEnumerator Attack()
    {
        isAttacking = true;
        anim.SetTrigger("Attack");
        audioSource.PlayOneShot(clips[0]);
        yield return new WaitForSeconds(0.75f);
        isAttacking = false;
    }

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
            lifeBar.fillAmount = lifePoints / baseLifePoints;
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
                        if(Vector3.Distance(player.transform.position, transform.position) > 3 && !isAttacking)
                        {
                            if(Random.Range(0, 2) == 0)
                            {
                                StartCoroutine(SwordAttack1());
                            }
                            else
                            {
                                StartCoroutine(SwordAttack2());
                            }
                        }
                        if (Vector3.Distance(player.transform.position, transform.position) <= 3 && !isAttacking)
                        {
                            StartCoroutine(Attack());
                        }
                    }
                    else
                    {
                        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, -7.5f * Time.deltaTime);
                    }
                    if (rb.velocity != Vector3.zero)
                    {
                        rb.velocity = Vector3.zero;
                    }
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
            if (NavMeshAgent.speed != 0 && !isWalking)
            {
                isWalking = true;
                anim.SetBool("isWalking", true);
            }
            else if (NavMeshAgent.speed == 0 && isWalking)
            {
                isWalking = false;
                anim.SetBool("isWalking", false);
            }
        }
    }

    bool isWalking;




    public void TakeDamage(float damage, bool shouldBleed)
    {
        if (lifePoints < 0)
        {
            lifePoints = 0;
        }
        if (lifeBar != null)
        {
            lifeBar.fillAmount = lifePoints / baseLifePoints;
        }

        if (!shouldAvoidPlayer && !cantAvoidPlayer)
        {
            StartCoroutine(AvoidPlayer());
        }
        if (lifePoints <= 0)
        {
            Player.enemiesToDefeat -= 1;
            BossDeath();
        }
    }

    void BossDeath()
    {
        anim.Play("Death_Tyr");
        Destroy(gameObject, 1f);
        //Cutscene Final
        //Tela de créditos
        //menu inicial
    }
    //==================================================


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
