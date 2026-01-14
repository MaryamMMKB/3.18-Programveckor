using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class LanternInteract : MonoBehaviour
{
    public Cutscene cutsceneManager;
    public GameObject lanternObject;
    public TextMeshProUGUI promptText;
        
    private bool playerInside = false;
    private bool pickedUp = false;


    void Start()
    {
        if(promptText != null)
        {
            promptText.gameObject.SetActive(false);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            playerInside = true;
            if (promptText != null)
            {
                promptText.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            if (promptText != null)
                promptText.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (!playerInside || pickedUp) { return; } 

        if(Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            PickUpLantern();
        }
    }
    public void PickUpLantern()
    {
        pickedUp = true;

        if (promptText != null)
        {
            promptText.gameObject.SetActive(false);
        }


        if (lanternObject != null)
        {
            cutsceneManager.TeleportAndFade();
            
            Debug.Log("Lantern picked up!");
        }
       
    }

}
