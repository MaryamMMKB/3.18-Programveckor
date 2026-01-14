using UnityEngine;
using UnityEngine.InputSystem;

public class LanternInteract : MonoBehaviour
{
    public Cutscene cutsceneManager;
    public GameObject lanternObject;
    private bool playerInside = false;
    private bool pickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
        playerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
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

        if(lanternObject != null)
        {
            cutsceneManager.TeleportAndFade();
            
            Debug.Log("Lantern picked up!");
        }
       
    }

}
