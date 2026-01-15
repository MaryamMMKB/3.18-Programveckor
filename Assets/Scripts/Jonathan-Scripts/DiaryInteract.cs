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

    [Tooltip("Delay before this object disappears and next one shows (cutscene time)")]
    public float advanceDelay = 2f;

    private bool hasBeenRead = false;

    public override void Interact()
    {
        if (!hasBeenRead)
        {
            hasBeenRead = true;

            // Write diary
            DiaryUIManager.Instance.WriteDiary(diaryEntry, shortComment);

            // Advance after delay
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
