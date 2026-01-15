using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class LanternInteract : MonoBehaviour
{
    public Cutscene cutsceneManager;
    public GameObject lanternObject;
    public GameObject Image;
    public GameObject PointLight;
    
        
    private bool playerInside = false;
    private bool pickedUp = false;


   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !pickedUp)
        {
            playerInside = true;
            Image.gameObject.SetActive(false);
            PointLight.gameObject.SetActive(false);

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            Image.gameObject.SetActive(true);
            PointLight.gameObject.SetActive(true);

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
        Image.gameObject.SetActive(true);
        PointLight.gameObject.SetActive(true);

        if (lanternObject != null)
        {
            cutsceneManager.TeleportAndFade();
            
            Debug.Log("Lantern picked up!");
        }
        lanternObject.SetActive(false);

    }

}
