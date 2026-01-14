using UnityEngine;

public class CutsceneEventReceiver : MonoBehaviour
{
    public Cutscene cutsceneManager;
    public GameObject BorderJVV; //border gameobject to activate after OF cutscene
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

}