using UnityEngine;

/// <summary>
/// Sets the EnemyAISettings start condition when the player enters a trigger.
/// </summary>
public class StartConditionTrigger : MonoBehaviour
{
    [Tooltip("Settings object to unlock the patrol.")]
    public EnemyAISettings settings;
    [Tooltip("Tag used to identify the player.")]
    public string playerTag = "Player";
    [Tooltip("If true, the trigger disables itself after firing once.")]
    public bool oneShot = true;

    private void OnTriggerEnter(Collider other)
    {
        if (settings == null)
        {
            return;
        }

        if (!other.CompareTag(playerTag))
        {
            return;
        }

        settings.SetStartCondition(true);

        if (oneShot)
        {
            gameObject.SetActive(false);
        }
    }
}
