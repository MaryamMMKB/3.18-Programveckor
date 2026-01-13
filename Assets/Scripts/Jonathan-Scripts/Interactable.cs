using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Cutscene manager; // Reference to the CutsceneManager
   
    
    public virtual void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
        if(manager != null)
        {
            manager.PlayOF();
        }
    }
}
