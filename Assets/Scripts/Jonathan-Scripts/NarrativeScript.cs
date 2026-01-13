using UnityEngine;

public class NarrativeTrigger : MonoBehaviour
{
    [TextArea(2, 4)]
    public string dialogText;

    public float dialogDuration = 2.5f;

    [Header("Progression")]
    public bool blockBacktracking = false;
    public GameObject blockerPrefab;

    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag("Player")) return;

        hasTriggered = true;

        DiaryUIManager.Instance.ShowDialog(dialogText, dialogDuration);

        if (blockBacktracking && blockerPrefab != null)
        {
            SpawnBlocker(other.transform);
        }

        Destroy(gameObject);
    }

    void SpawnBlocker(Transform player)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.z -= 0.5f; // adjust per level layout

        Instantiate(blockerPrefab, spawnPos, Quaternion.identity);
    }
}
