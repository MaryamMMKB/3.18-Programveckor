using UnityEngine;

public class DiaryInteractable : Interactable
{
    [Header("First Interaction")]
    [TextArea(2, 4)]
    public string shortComment;   // Optional comment before main diary text

    [TextArea(6, 12)]
    public string diaryEntry;     // Full diary text

    [Header("After Reading")]
    [TextArea(2, 4)]
    public string alreadyReadComment = "I've already looked at this.";

    private bool hasBeenRead = false;

    public override void Interact()
    {
        if (!hasBeenRead)
        {
            hasBeenRead = true;

            // Write to diary (persistent pages)
            DiaryUIManager.Instance.WriteDiary(diaryEntry, shortComment);
        }
        else
        {
            // Show a short comment if already read
            DiaryUIManager.Instance.ShowShortComment(alreadyReadComment);
        }
    }
}
