using UnityEngine;

public class HitboxCutscene : MonoBehaviour //script for making cutscenes play when hitbox is triggered, add cutscene here AND in Cutscene.cs, attach to hitbox gameobject and pick anim
{
   public Cutscene cutsceneManager; //refference to gameobject w script Cutscene
   public enum CutsceneType //enums for current custcenes, add new cutscene here
    {
     OF,
     BTC,
     AE,
     JVV,
    
    }
    public CutsceneType cutsceneToPlay;
    public bool playOnce = true;
    private bool hasPlayed = false;

    
   

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HitboxCutscene triggered");
        if (hasPlayed && playOnce) 
        {
         return; 
        }//prevents multiple triggers <----::
        if (!other.CompareTag("Player"))
        {
         return;
        }


        switch (cutsceneToPlay)
        {
            case CutsceneType.OF:
                cutsceneManager.PlayOF();
                break;


            case CutsceneType.BTC:
                cutsceneManager.PlayBTC();
                break;


            case CutsceneType.AE:
                cutsceneManager.PlayAE();
                break;


            case CutsceneType.JVV:
                cutsceneManager.PlayJVV();
                break;

        }
        hasPlayed = true;
    }     
    
}
