using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [Header("Diary Trigger")]
    public bool opensDiary = false;

    [TextArea(6, 12)]
    public string diaryEntry;

    [Header("Dialog Trigger")]
    [TextArea(2, 4)]
    public string dialogText;
    public float dialogDuration = 2.5f; // Not used anymore, kept for reference

    [Header("Progression")]
    public bool blockBacktracking = false;
    public GameObject blockerPrefab;

    [HideInInspector]
    public bool hasTriggered = false;
}
