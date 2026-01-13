using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Animator Cutscenes;
    public PlayerMovement PlayerMovement;
    public GameObject cutsceneCamera;
    public GameObject playerCamera;
    


    void Awake()
    {
        
        if (Cutscenes == null)
        {
            Cutscenes = GetComponent<Animator>();
        }
    }
    //wanna add a new cutscene? copy the methods below with a new (shortened)name and trigger, add that trigger to the "Cutscenes" animator controller and make a new state with the name of ur cutscene in the animator controller.
    //make a new clip for ur state and animate away!
    //at last frame of ur animation, add an event that calls method "EndCutscene" in the dropdown menu
    public void PlayJVV()                                      
    {
        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = false;
        }
        if (playerCamera != null)
        {
            playerCamera.SetActive(false);
        }
        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(true);
        }
        if (Cutscenes != null)
            Debug.Log("Playing JVV Cutscene");
            Cutscenes.SetTrigger("JVV");
    }
    public void PlayBTC() 
    {
        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = false;
        }
        if (playerCamera != null)
        {
            playerCamera.SetActive(false);
        }
        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(true);
        }
        if (Cutscenes != null)
            Debug.Log("Playing BTC Cutscene");
            Cutscenes.SetTrigger("BTC");
    }
    public void PlayAE()
    {
        if (PlayerMovement != null) 
        {
            PlayerMovement.enabled = false; 
        }
        if (playerCamera != null) 
        {
            playerCamera.SetActive(false);               
        }
        if (cutsceneCamera != null)
        { 
            cutsceneCamera.SetActive(true); 
        }
        if (Cutscenes != null)
        Debug.Log("Playing AE Cutscene");
        Cutscenes.SetTrigger("AE");
    }
    public void PlayOF() 
    {
        Debug.Log("Playing OF Cutscene");

        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = false;
        }
        if (cutsceneCamera != null) 
        { 
            cutsceneCamera.SetActive(true);  // enable camera first
        }
        if (Cutscenes != null)
        {
            Cutscenes.enabled = true; // ensure Animator is active
            Cutscenes.SetTrigger("OF");
        }

        if (playerCamera != null)
        {
            playerCamera.SetActive(false);   // hide player camera
        }
    }
   
    public void EndCutscene() //DONT FORGET TO ADD AS EVENT IN CLIP >:|
    {
        if (cutsceneCamera != null)
        {
            cutsceneCamera.SetActive(false);
        }

        if (playerCamera != null)
        {
            playerCamera.SetActive(true);
        }

        if (PlayerMovement != null)
        {
            PlayerMovement.enabled = true;
        }
    }
}
