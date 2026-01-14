using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolAI : MonoBehaviour
{
    public EnemyAISettings settings;
    public Transform pointB;
    public Transform pointC;
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
            agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        ApplySettings();

        // ?? TVINGA RESET VID SPELSTART
        if (settings != null)
        {
            settings.SetStartCondition(false);
        }

        agent.isStopped = true;
    }


    private void Update()
    {
        if (state == State.Done || settings == null || agent == null)
            return;

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
        agent.isStopped = false;   // ?? släpp agenten
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
            return;

        agent.SetDestination(target.position);
    }

    private void ApplySettings()
    {
        if (settings == null || agent == null)
            return;

        agent.speed = settings.agentSpeed;
        agent.acceleration = settings.agentAcceleration;
        agent.angularSpeed = settings.agentAngularSpeed;
        agent.stoppingDistance = settings.agentStoppingDistance;
    }

    private bool ReachedDestination()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        return !agent.hasPath || agent.velocity.sqrMagnitude < 0.01f;
    }

    public void DespawnFromBreathHold()
    {
        state = State.Done;
        Destroy(gameObject);
    }
}
