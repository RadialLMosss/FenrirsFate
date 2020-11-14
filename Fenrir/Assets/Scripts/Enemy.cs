using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent NavMeshAgent;
    public Player player;
    float totalTimer = 3;
    float runningTimer;
    public GameObject crystalPB;

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
    private void Update()
    {
        if(player != null)
        {
            if (Vector3.Distance(player.transform.position, transform.position) <= 20)
            {
                NavMeshAgent.SetDestination(player.transform.position);
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
                    runningTimer = 0;
                }
                else
                {
                    runningTimer += Time.deltaTime;
                }
            }
        }

    }

    public void EnemyDeath()
    {
        player.GetFuryCurrency();
        if(Random.Range(0, 3) == 0)
        {
            GameObject cystal = Instantiate(crystalPB, transform.position, Quaternion.identity);
        }
        Player.enemiesToDefeat -= 1;
        Destroy(gameObject);
    }



}
