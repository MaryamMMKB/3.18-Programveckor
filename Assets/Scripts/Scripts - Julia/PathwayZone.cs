using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// UnityEvent that passes the enemy GameObject entering or exiting the path.
/// </summary>
[System.Serializable]
public class EnemyZoneEvent : UnityEvent<GameObject>
{
}

/// <summary>
/// Emits events when an enemy enters or exits the pathway trigger.
/// </summary>
public class PathwayZone : MonoBehaviour
{
    [Tooltip("Tag used to identify the enemy entering the path.")]
    public string enemyTag = "Enemy";

    [Header("Integration Events")]
    // Connect breath-hold function here: fail -> kill player, success -> despawn enemy.
    [Tooltip("Hook your breath-hold start logic here (enemy enters the path).")]
    public EnemyZoneEvent onEnemyEnterPath;
    [Tooltip("Hook your breath-hold end logic here (enemy exits the path). On success, despawn the enemy.")]
    public EnemyZoneEvent onEnemyExitPath;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(enemyTag))
        {
            return;
        }

        onEnemyEnterPath?.Invoke(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(enemyTag))
        {
            return;
        }

        onEnemyExitPath?.Invoke(other.gameObject);
    }
}
