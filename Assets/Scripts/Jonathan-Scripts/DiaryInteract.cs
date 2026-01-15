using UnityEngine;

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

    private bool hasBeenRead = false;

    public override void Interact()
    {
        if (!hasBeenRead)
        {
            hasBeenRead = true;

            DiaryUIManager.Instance.ShowDiary(
                shortComment,
                diaryEntry
            );
        }
        else
        {
            DiaryUIManager.Instance.ShowShortComment(alreadyReadComment);
        }
    }
}
