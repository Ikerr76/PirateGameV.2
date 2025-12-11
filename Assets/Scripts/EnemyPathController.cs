using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Patrol,
    Chasing,
    Returning,
    Idle,
    Paused
}

public class EnemyPathController : MonoBehaviour
{
    [Header("Ruta del Boss")]
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool loopPath = true;
    [SerializeField] private float stopDistance = 0.1f;

    [Header("Detección del jugador")]
    [SerializeField] private float chaseRange = 3f;
    [SerializeField] private float loseRange = 5f;

    [Header("Evitar Paredes")]
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float avoidStrength = 3f;
    [SerializeField] private float avoidDistance = 1f;

    private int currentTargetIndex = 0;
    private bool isPaused = false;
    private Vector2 direction;

    private Transform player { get; set; }
    private BossState state = BossState.Patrol;

    public event System.Action onPointReach;
    public Vector2 Direction => direction;

    // -----------------------------------------------------
    public void SetPlayer(Transform p)
    {
        player = p;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("EnemyPathController: No se encontró el jugador con tag 'Player'");
        }
        else
        {
            GameObject[] managers = GameObject.FindGameObjectsWithTag("GameManager");
            if (managers.Length > 0)
            {
                player = managers[0].transform;
            }
        }

        if (waypoints.Count > 0)
            direction = (waypoints[0].position - transform.position).normalized;
    }
    

    void Update()
    {
        if (isPaused) return;

        CheckPlayerDetection();

        switch (state)
        {
            case BossState.Patrol:
                PatrolMovement();
                break;

            case BossState.Chasing:
                ChasePlayer();
                break;

            case BossState.Returning:
                ReturnMovement();
                break;

            case BossState.Idle:
            case BossState.Paused:
                break;
        }
    }

    // -----------------------------------------------------
    // SISTEMA DE EVITACIÓN DE PAREDES
    // -----------------------------------------------------
    private Vector2 AvoidObstacles(Vector2 currentDir)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, currentDir, avoidDistance, obstacleMask);

        if (hit.collider != null)
        {
            Vector2 hitNormal = hit.normal;
            Vector2 newDir = currentDir + hitNormal * avoidStrength;
            return newDir.normalized;
        }

        return currentDir;
    }

    // -----------------------------------------------------
    // Movimiento en Patrulla
    // -----------------------------------------------------
    void PatrolMovement()
    {
        if (waypoints.Count == 0) return;

        Transform target = waypoints[currentTargetIndex];

        Vector2 moveDir = (target.position - transform.position).normalized;
        moveDir = AvoidObstacles(moveDir);

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);

        direction = moveDir;

        if (Vector2.Distance(transform.position, target.position) < stopDistance)
        {
            currentTargetIndex++;
            if (currentTargetIndex >= waypoints.Count)
            {
                if (loopPath) currentTargetIndex = 0;
                else { enabled = false; return; }
            }

            direction = (waypoints[currentTargetIndex].position - transform.position).normalized;
            onPointReach?.Invoke();
        }
    }

    // -----------------------------------------------------
    // Movimiento Persiguiendo al Jugador
    // -----------------------------------------------------
    void ChasePlayer()
    {
        if (player == null) return;

        Vector2 moveDir = (player.position - transform.position).normalized;
        moveDir = AvoidObstacles(moveDir);

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
        direction = moveDir;
    }

    // -----------------------------------------------------
    // DETECCIÓN DE JUGADOR
    // -----------------------------------------------------
    void CheckPlayerDetection()
    {
        if (player == null) return;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= chaseRange)
        {
            state = BossState.Chasing;
            return;
        }

        if (state == BossState.Chasing && dist > loseRange)
        {
            BeginReturnToPath();
        }
    }

    // -----------------------------------------------------
    // VOLVER A LA RUTA DE PATRULLA (INTELIGENTE)
    // -----------------------------------------------------

    private void BeginReturnToPath()
    {
        currentTargetIndex = GetBestRejoinIndex();
        state = BossState.Returning;
    }

    private void ReturnMovement()
    {
        Transform target = waypoints[currentTargetIndex];

        Vector2 moveDir = (target.position - transform.position).normalized;
        moveDir = AvoidObstacles(moveDir);

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
        direction = moveDir;

        if (Vector2.Distance(transform.position, target.position) < stopDistance)
        {
            state = BossState.Patrol;
        }
    }

    // ELEGIR el waypoint MÁS CERCANO entre ANTERIOR o SIGUIENTE
    private int GetBestRejoinIndex()
    {
        int next = currentTargetIndex;
        int prev = currentTargetIndex - 1;

        if (prev < 0) prev = waypoints.Count - 1;

        float dPrev = Vector2.Distance(transform.position, waypoints[prev].position);
        float dNext = Vector2.Distance(transform.position, waypoints[next].position);

        return (dPrev < dNext) ? prev : next;
    }

    // -----------------------------------------------------
    // PAUSA POR TRAMPA
    // -----------------------------------------------------
    public void PausarPorTrampa(float duracion)
    {
        if (!isPaused)
        {
            StartCoroutine(PausaTemporal(duracion));
        }
    }

    private IEnumerator PausaTemporal(float duracion)
    {
        isPaused = true;
        BossState prev = state;
        state = BossState.Paused;

        yield return new WaitForSeconds(duracion);

        isPaused = false;

        if (prev == BossState.Chasing)
            BeginReturnToPath();
        else
            state = BossState.Patrol;
    }

    // -----------------------------------------------------
    // GIZMOS
    // -----------------------------------------------------
    private void OnDrawGizmosSelected()
    {
        if (waypoints == null || waypoints.Count < 2) return;

        Gizmos.color = Color.red;
        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            Gizmos.DrawSphere(waypoints[i].position, 0.1f);
        }

        if (loopPath)
            Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
    }
}
