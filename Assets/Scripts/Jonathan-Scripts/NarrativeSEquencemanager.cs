using UnityEngine;

public class InteractableSequenceManager : MonoBehaviour
{
    [Header("Sequence Order")]
    public GameObject[] interactableObjects;

    private int currentIndex = 0;

    void Start()
    {
        // Hide all at start
        foreach (GameObject obj in interactableObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Show first one
        if (interactableObjects.Length > 0 && interactableObjects[0] != null)
            interactableObjects[0].SetActive(true);
    }

    public void AdvanceSequence()
    {
        // Hide current
        if (currentIndex < interactableObjects.Length &&
            interactableObjects[currentIndex] != null)
        {
            interactableObjects[currentIndex].SetActive(false);
        }

        currentIndex++;

        // Show next
        if (currentIndex < interactableObjects.Length &&
            interactableObjects[currentIndex] != null)
        {
            interactableObjects[currentIndex].SetActive(true);
        }
    }
}
