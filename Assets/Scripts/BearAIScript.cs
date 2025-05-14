using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BearAIScript : MonoBehaviour
{
    private Transform player;
    private float chaseRadius = 8f;
    private float attackRadius = 2f;
    private float patrolRadius = 5f;
    private float speed;
    private Animator animator;

    private Vector3 spawnPoint;
    private Vector3 targetPoint;
    private bool isChasing;

    public Action OnDeath;

    public void Init(Transform playerTarget, float moveSpeed, float patrolRadiusValue)
    {
        player = playerTarget;
        speed = moveSpeed;
        patrolRadius = patrolRadiusValue;
        spawnPoint = transform.position;

        SetNewPatrolPoint();
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("02_Walk", true);
    }

    void Update()
    {
        GroundSnap();

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackRadius)
        {
            animator.SetTrigger("06_Attack");
            return;
        }
        else if (distance < chaseRadius)
        {
            isChasing = true;
            MoveTo(player.position);
            return;
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    private void GroundSnap()
    {
        if (Physics.Raycast(transform.position + Vector3.up * 2f, Vector3.down, out RaycastHit hit, 5f))
        {
            Vector3 position = transform.position;
            position.y = hit.point.y; // Adjust height to be above the ground
            transform.position = position;
        }
    }

    private void Patrol()
    {
        if (Vector3.Distance(transform.position, targetPoint) < 1f)
        {
            SetNewPatrolPoint();
        }
        MoveTo(targetPoint);
    }

    private void MoveTo(Vector3 destination) 
    { 
        Vector3 direction = (destination - transform.position).normalized;
        
        transform.position += direction * speed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);

        animator.SetBool("02_Walk", true);
    }

    private void SetNewPatrolPoint()
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle * patrolRadius;
        targetPoint = spawnPoint + new Vector3(offset.x, 0, offset.y);
    }

    public void Die()
    {
        animator.SetTrigger("07_Die");
        OnDeath?.Invoke();
        Destroy(gameObject, 3f);
    }
}

enum BearMoveStates
{
    Idle = 1,
    Walk = 2,
    Run = 3,
    Roar = 4,
    Attack = 5,
    TakeDamage = 6,
    Die = 7
}