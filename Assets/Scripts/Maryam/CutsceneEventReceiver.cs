using UnityEngine;
using UnityEngine.UI;

    


public class CutsceneEventReceiver : MonoBehaviour
{
    public Cutscene cutsceneManager;
    public GameObject BorderJVV; //border gameobject to activate after OF cutscene
    public GameObject BorderI;
    public PlayerMovement PlayerMovement;
   
 
  

    public void EndCutsceneEvent() 
    {
     if(cutsceneManager != null)
     {
         cutsceneManager.EndCutscene();
     }

    }
    public void AfterI() 
    {
    cutsceneManager.AfterI();
        BorderI.SetActive(true);
    }
    public void Teleport() 
    {
     cutsceneManager.TeleportPlayer();

    }
    public void FadeEnded() 
    {
    
    
     cutsceneManager.EndFade();
    
    }
    public void ActivateBorderJVV() //add as event in JVV
    {
        BorderJVV.SetActive(true);
    }
    public void TeleportAndFade()
    {
        cutsceneManager.TeleportAndFade();

    }
    public void ShakeCall() 
    {
     
    cutsceneManager.ShakeCall();
    }

}