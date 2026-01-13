using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;
    public Transform cameraTransform;
    public TextMeshProUGUI interactText;
    public Cutscene cutsceneManager;



    private Interactable currentInteractable;

    void Start()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
       CheckForInteractable();
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame) 
        { 
            if (currentInteractable != null) 
            { 
                cutsceneManager.PlayOF();          //since objects dont have any interaction related script attached, they can only play one custcene that the player performs IYKWIM
                currentInteractable.Interact();
            } 
        }
    }

    void CheckForInteractable()
    {
        currentInteractable = null;

        if (interactText != null)
            interactText.gameObject.SetActive(false);

        Ray ray = new Ray(cameraTransform.position, cameraTransform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                currentInteractable = interactable;

                if (interactText != null)
                    interactText.gameObject.SetActive(true);
            }
        }
    }
}
