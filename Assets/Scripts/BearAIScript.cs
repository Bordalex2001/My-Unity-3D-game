using System;
using UnityEngine;
using UnityEngine.AI;

public class BearAIScript : MonoBehaviour
{
    [SerializeField]
    private Transform player;
    private float detectionRadius = 10f;
    private float attackRadius = 2f;
    private float patrolRadius = 5f;
    private Animator animator;

    private NavMeshAgent agent;
    private Vector3 spawnPoint;
    private bool isChasing;

    public Action OnBearDeath;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPoint = transform.position;
        InvokeRepeating("Patrol", 2f, 2f);
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackRadius)
        {
            agent.ResetPath();
            animator.SetTrigger("05_Attack");
        }
        else if (distance < detectionRadius)
        {
            isChasing = true;
            agent.SetDestination(player.position);
            animator.SetBool("02_Walk", true);
        }
        else if (isChasing)
        {
            isChasing = false;
            Patrol();
        }
    }

    private void Patrol()
    {
        if (isChasing)
            return;

        Vector3 randomPos = spawnPoint + UnityEngine.Random.insideUnitSphere * patrolRadius;
        randomPos.y = spawnPoint.y;

        if (NavMesh.SamplePosition(randomPos, out NavMeshHit hit, 2f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
            animator.SetBool("02_Walk", true);
        }
    }

    public void Die()
    {
        animator.SetTrigger("07_Die");
        agent.enabled = false;
        OnBearDeath?.Invoke();
        Destroy(gameObject, 5f);
    }
}