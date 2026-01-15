using UnityEngine;

public class PlayerNarrativeTriggerDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        NarrativeTrigger trigger = other.GetComponent<NarrativeTrigger>();
        if (trigger == null || trigger.hasTriggered) return;

        trigger.hasTriggered = true;

        if (trigger.opensDiary)
        {
            DiaryUIManager.Instance.ShowDiaryDirect(trigger.diaryEntry);
        }
        else
        {
            DiaryUIManager.Instance.ShowDialog(
                trigger.dialogText,
                trigger.dialogDuration
            );
        }

        if (trigger.blockBacktracking && trigger.blockerPrefab != null)
        {
            Instantiate(
                trigger.blockerPrefab,
                trigger.transform.position,
                Quaternion.identity
            );
        }

        Destroy(trigger.gameObject);
    }
}
