using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Patrol,
    Chasing,
    Stunned
}

public class BossController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 2f;
    public List<Transform> waypoints;
    public float waypointStopDistance = 0.1f;

    [Header("Detection")]
    public float chaseRange = 3f;
    public float loseRange = 4.5f;

    [Header("References")]
    public Animator animator;
    public Rigidbody2D rb;

    private int currentWaypoint;
    private Transform player;
    private BossState state = BossState.Patrol;
    private Vector2 direction;
    private bool isPaused;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (isPaused) return;

        switch (state)
        {
            case BossState.Patrol:
                Patrol();
                DetectPlayer();
                break;

            case BossState.Chasing:
                Chase();
                DetectLosePlayer();
                break;

            case BossState.Stunned:
                StopMovement();
                break;
        }

        UpdateAnimator();
    }

    // ---------------- MOVEMENT ----------------

    void Patrol()
    {
        if (waypoints.Count == 0) return;

        Transform target = waypoints[currentWaypoint];
        MoveTowards(target.position);

        if (Vector2.Distance(transform.position, target.position) < waypointStopDistance)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Count;
        }
    }

    void Chase()
    {
        if (player == null) return;
        MoveTowards(player.position);
    }

    void MoveTowards(Vector2 target)
    {
        direction = (target - (Vector2)transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        direction = Vector2.zero;
    }

    // ---------------- DETECTION ----------------

    void DetectPlayer()
    {
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) <= chaseRange)
        {
            state = BossState.Chasing;
        }
    }

    void DetectLosePlayer()
    {
        if (player == null) return;

        if (Vector2.Distance(transform.position, player.position) > loseRange)
        {
            state = BossState.Patrol;
        }
    }

    // ---------------- TRAPS ----------------

    public void PauseByTrap(float time, string animationTrigger)
    {
        StartCoroutine(TrapPause(time, animationTrigger));
    }

    IEnumerator TrapPause(float time, string trigger)
    {
        isPaused = true;
        state = BossState.Stunned;

        if (!string.IsNullOrEmpty(trigger))
            animator.SetTrigger(trigger);

        yield return new WaitForSeconds(time);

        isPaused = false;
        state = BossState.Patrol;
    }

    // ---------------- ANIMATOR ----------------

    void UpdateAnimator()
    {
        bool moving = direction.magnitude > 0.01f;

        animator.SetBool("IsMoving", moving);
        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
    }
}
