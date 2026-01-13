using UnityEngine;

public class CutsceneEventReceiver : MonoBehaviour
{
    public Cutscene cutsceneManager;
    public GameObject BorderJVV; //border gameobject to activate after OF cutscene

    public void EndCutsceneEvent() 
    {
     if(cutsceneManager != null)
     {
         cutsceneManager.EndCutscene();
        }

    }
    public void ActivateBorderJVV() //add as event in JVV
    {
        BorderJVV.SetActive(true);
    }
}
