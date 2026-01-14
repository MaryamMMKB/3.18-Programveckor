using UnityEngine;

/// <summary>
/// Centralized tuning values for EnemyPatrolAI behavior.
/// </summary>
public class EnemyAISettings : MonoBehaviour
{
    [Header("Start Condition")]
    [Tooltip("If true, the enemy starts moving immediately on Awake.")]
    public bool startOnAwake = false;
    [Tooltip("Set true to allow the enemy to leave Point A.")]
    public bool startConditionMet = false;

    [Header("Wait At B")]
    [Tooltip("Seconds to wait at Point B before moving to Point C.")]
    [Min(0f)]
    public float waitAtBSeconds = 6f;

    [Header("NavMesh Agent")]
    [Tooltip("Movement speed used by the NavMeshAgent.")]
    public float agentSpeed = 3.5f;
    [Tooltip("Acceleration used by the NavMeshAgent.")]
    public float agentAcceleration = 8f;
    [Tooltip("Rotation speed used by the NavMeshAgent.")]
    public float agentAngularSpeed = 120f;
    [Tooltip("Stopping distance used by the NavMeshAgent.")]
    public float agentStoppingDistance = 0.2f;

    private void Awake()
    {
        if (startOnAwake)
        {
            startConditionMet = true;
        }
    }

    /// <summary>
    /// Call this to allow the enemy to leave its start position.
    /// </summary>
    public void SetStartCondition(bool value)
    {
        startConditionMet = value;
    }
}
