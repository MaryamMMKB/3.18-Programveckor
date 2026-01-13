using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitTimeAtPoint = 1.5f;

    [Header("Visual")]
    [SerializeField] private Transform visual; // child som flippar

    private NavMeshAgent agent;
    private int currentPointIndex;
    private float waitTimer;
    private bool agentReady;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        // KRITISKT för 2.5D
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Start()
    {
        PlaceAgentOnNavMesh();
    }

    private void Update()
    {
        Debug.Log("OnNavMesh: " + agent.isOnNavMesh);

        if (!agentReady || patrolPoints.Length == 0)
            return;

        // Vänta vid waypoint
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                GoToNextPoint();
                waitTimer = 0f;
            }
        }

        FlipVisual();
    }

    // ----------------------

    private void PlaceAgentOnNavMesh()
    {
        if (agent.isOnNavMesh)
        {
            agentReady = true;
            agent.SetDestination(patrolPoints[currentPointIndex].position);
            return;
        }

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 3f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position);
            agentReady = true;
            agent.SetDestination(patrolPoints[currentPointIndex].position);
        }
        else
        {
            Debug.LogError("EnemyPatrol: No NavMesh found near enemy!");
        }
    }

    private void GoToNextPoint()
    {
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        agent.SetDestination(patrolPoints[currentPointIndex].position);
    }

    private void FlipVisual()
    {
        if (visual == null)
            return;

        if (agent.velocity.x > 0.1f)
            visual.localScale = Vector3.one;
        else if (agent.velocity.x < -0.1f)
            visual.localScale = new Vector3(-1, 1, 1);
    }
}
