using UnityEngine;

public class PlayerNarrativeTriggerDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NarrativeTrigger trigger = other.GetComponent<NarrativeTrigger>();
        if (trigger == null || trigger.hasTriggered) return;

        trigger.hasTriggered = true;

        // If this trigger is a diary trigger, write to persistent diary pages
        if (trigger.opensDiary)
        {
            DiaryUIManager.Instance.WriteDiary(trigger.diaryEntry);
        }
        else
        {
            // Otherwise show a temporary dialog comment
            DiaryUIManager.Instance.ShowShortComment(trigger.dialogText);
        }

        // Handle optional backtracking blocker
        if (trigger.blockBacktracking && trigger.blockerPrefab != null)
        {
            Instantiate(trigger.blockerPrefab, trigger.transform.position, Quaternion.identity);
        }

        // Remove the trigger object
        Destroy(trigger.gameObject);
    }
}
