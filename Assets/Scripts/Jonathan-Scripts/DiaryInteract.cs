using UnityEngine;
using System.Collections;

public class DiaryInteractable : Interactable
{
    [Header("First Interaction")]
    [TextArea(2, 4)]
    public string shortComment;

    [TextArea(6, 12)]
    public string diaryEntry;

    [Header("After Reading")]
    [TextArea(2, 4)]
    public string alreadyReadComment = "I've already looked at this.";

    [Header("Sequence")]
    public InteractableSequenceManager sequenceManager;

    [Tooltip("Delay before advancing sequence (cutscene length)")]
    public float advanceDelay = 2f;

    [Header("Blocking")]
    [Tooltip("Collider that blocks the path and should be disabled after interaction")]
    public Collider blockingCollider;

    private bool hasBeenRead = false;

    public override void Interact()
    {
        if (!hasBeenRead)
        {
            hasBeenRead = true;

            // Diary logic
            DiaryUIManager.Instance.WriteDiary(diaryEntry, shortComment);

            // Disable blocking collider immediately (or after delay if you prefer)
            if (blockingCollider != null)
                blockingCollider.enabled = false;

            // Advance sequence after delay
            if (sequenceManager != null)
                StartCoroutine(AdvanceAfterDelay());
        }
        else
        {
            DiaryUIManager.Instance.ShowShortComment(alreadyReadComment);
        }
    }

    IEnumerator AdvanceAfterDelay()
    {
        yield return new WaitForSeconds(advanceDelay);
        sequenceManager.AdvanceSequence();
    }
}
