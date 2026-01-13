using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Moves an enemy from its start position to Point B, waits, then moves to Point C and despawns.
/// </summary>
public class EnemyPatrolAI : MonoBehaviour
{
    [Tooltip("Central settings object for this enemy.")]
    public EnemyAISettings settings;
    [Tooltip("Intermediate position to reach before waiting.")]
    public Transform pointB;
    [Tooltip("Final position. Enemy despawns after reaching this point.")]
    public Transform pointC;
    [Tooltip("NavMeshAgent used for movement.")]
    public NavMeshAgent agent;

    private enum State
    {
        IdleAtA,
        MoveToB,
        WaitAtB,
        MoveToC,
        Done
    }

    private State state = State.IdleAtA;
    private float waitTimer;

    private void Awake()
    {
        if (agent == null)
        {
            agent = GetComponent<NavMeshAgent>();
        }
    }

    private void Start()
    {
        ApplySettings();

        if (settings != null && settings.startConditionMet)
        {
            StartPatrol();
        }
    }

    private void Update()
    {
        if (state == State.Done)
        {
            return;
        }

        if (settings == null || agent == null)
        {
            return;
        }

        switch (state)
        {
            case State.IdleAtA:
                if (settings.startConditionMet)
                {
                    StartPatrol();
                }
                break;
            case State.MoveToB:
                if (ReachedDestination())
                {
                    BeginWait();
                }
                break;
            case State.WaitAtB:
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    MoveTo(pointC);
                    state = State.MoveToC;
                }
                break;
            case State.MoveToC:
                if (ReachedDestination())
                {
                    state = State.Done;
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void StartPatrol()
    {
        MoveTo(pointB);
        state = State.MoveToB;
    }

    private void BeginWait()
    {
        waitTimer = Mathf.Max(0f, settings.waitAtBSeconds);
        state = State.WaitAtB;
    }

    private void MoveTo(Transform target)
    {
        if (target == null)
        {
            return;
        }

        agent.SetDestination(target.position);
    }

    private void ApplySettings()
    {
        if (settings == null || agent == null)
        {
            return;
        }

        agent.speed = settings.agentSpeed;
        agent.acceleration = settings.agentAcceleration;
        agent.angularSpeed = settings.agentAngularSpeed;
        agent.stoppingDistance = settings.agentStoppingDistance;
    }

    private bool ReachedDestination()
    {
        if (agent.pathPending)
        {
            return false;
        }

        if (agent.remainingDistance > agent.stoppingDistance)
        {
            return false;
        }

        return !agent.hasPath || agent.velocity.sqrMagnitude <= 0.01f;
    }

    /// <summary>
    /// Call this from your breath-hold system when the player succeeds.
    /// </summary>
    public void DespawnFromBreathHold()
    {
        state = State.Done;
        Destroy(gameObject);
    }
}
