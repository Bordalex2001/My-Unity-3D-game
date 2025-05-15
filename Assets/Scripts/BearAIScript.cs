using System;
using UnityEngine;

public class BearAIScript : MonoBehaviour
{
    private Transform player;
    private float chaseRadius = 8f;
    private float attackRadius = 2f;
    private float patrolRadius = 5f;
    private float moveSpeed = 2.5f;
    private Animator animator;
    private Vector3 spawnPoint;
    private Vector3 targetPoint;
    private bool isChasing;
    private BearMoveStates prevMoveState = BearMoveStates.Idle;
    private Rigidbody rb;

    public Action OnDeath;

    public void Init(Transform playerTarget, float speed, float patrolRadiusValue)
    {
        player = playerTarget;
        moveSpeed = speed;
        patrolRadius = patrolRadiusValue;

        spawnPoint = transform.position;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        if (!animator)
        {
            animator = GetComponentInChildren<Animator>();
        }

        SetNewPatrolPoint();
        SetAnim(BearMoveStates.Idle);
    }

    void Update()
    {
        GroundSnap();

        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < attackRadius)
        {
            SetAnim(BearMoveStates.Attack);
            return;
        }
        else if (distance < chaseRadius)
        {
            isChasing = true;
            MoveTo(player.position);
            SetAnim(BearMoveStates.Run);
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
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
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
        SetAnim(BearMoveStates.Walk);
    }

    private void MoveTo(Vector3 destination) 
    { 
        Vector3 direction = (destination - transform.position).normalized;
        
        if (Physics.Raycast(transform.position + Vector3.up, direction, 1f))
        {
            direction = Quaternion.Euler(0f, 30f, 0f) * direction; // Reverse direction if blocked
        }

        Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * direction;
        rb.MovePosition(newPosition);
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private void SetNewPatrolPoint()
    {
        for (int i = 0; i < 10; i++)
        {
            Vector2 offset = UnityEngine.Random.insideUnitCircle * patrolRadius;
            Vector3 rawPoint = spawnPoint + new Vector3(offset.x, 0f, offset.y);

            if (Physics.Raycast(rawPoint + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f))
            {
                targetPoint = hit.point;
                return;
            }
        }

        targetPoint = spawnPoint; // Fallback to spawn point if no valid patrol point found
    }

    private void SetAnim(BearMoveStates moveState)
    {
        if (animator && prevMoveState != moveState)
        {
            animator.SetInteger("MoveState", (int)moveState);
            prevMoveState = moveState;
        }
    }

    public void Die()
    {
        SetAnim(BearMoveStates.Die);
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