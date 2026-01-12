using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Animator Cutscenes;
    public PlayerMovement PlayerMovement;
    void Start()
    {
        
    }
    void Awake()
    {
        if (Cutscenes == null)
        {
            Cutscenes = GetComponent<Animator>();
        }
    }

    public void PlayJVV() 
    {
     PlayerMovement.enabled = false;
        Cutscenes.SetTrigger("JVV");
    }
    public void PlayBTC() 
    {
        PlayerMovement.enabled = false;
        Cutscenes.SetTrigger("BTC");
    }
    public void PlayAE() 
    {
        PlayerMovement.enabled = false;
        Cutscenes.SetTrigger("AE");
    }
    public void PlayOF() 
    {
        PlayerMovement.enabled = false;
        Cutscenes.SetTrigger("OF");
    }
    public void EndCutscene()
    {
        PlayerMovement.enabled = true;
    }
}
