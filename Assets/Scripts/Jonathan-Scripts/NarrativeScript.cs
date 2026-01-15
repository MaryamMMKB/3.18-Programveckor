using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [Header("Trigger State")]
    [HideInInspector]
    public bool hasTriggered = false;

    [Header("Diary")]
    public bool opensDiary = false;

    [TextArea(6, 12)]
    public string diaryEntry;

    [Header("Dialog")]
    [TextArea(2, 4)]
    public string dialogText;

    [Header("Backtracking Blocker")]
    public bool blockBacktracking = false;
    public GameObject blockerPrefab;

    [Header("Spawn Extra Collider")]
    public bool spawnExtraCollider = false;
    public GameObject extraColliderPrefab;
    public Transform extraColliderSpawnPoint;
}
