using UnityEngine;

public class PlayerNarrativeTriggerDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NarrativeTrigger trigger = other.GetComponent<NarrativeTrigger>();
        if (trigger == null || trigger.hasTriggered) return;

        trigger.hasTriggered = true;

        // Diary or dialog
        if (trigger.opensDiary)
        {
            DiaryUIManager.Instance.WriteDiary(trigger.diaryEntry);
        }
        else
        {
            DiaryUIManager.Instance.ShowShortComment(trigger.dialogText);
        }

        // Optional backtracking blocker
        if (trigger.blockBacktracking && trigger.blockerPrefab != null)
        {
            Instantiate(
                trigger.blockerPrefab,
                trigger.transform.position,
                Quaternion.identity
            );
        }

        // Optional extra invisible collider spawn
        if (trigger.spawnExtraCollider &&
            trigger.extraColliderPrefab != null &&
            trigger.extraColliderSpawnPoint != null)
        {
            Instantiate(
                trigger.extraColliderPrefab,
                trigger.extraColliderSpawnPoint.position,
                trigger.extraColliderSpawnPoint.rotation
            );
        }

        // Remove trigger
        Destroy(trigger.gameObject);
    }
}
    