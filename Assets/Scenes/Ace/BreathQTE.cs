using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BreathQTE : MonoBehaviour
{
    [Header("UI")]
    public GameObject uiRoot;
    public Image breathFill;
    public TMP_Text instructionText;
    public TMP_Text subtitleText;

    [Header("Settings")]
    public float fillTine = 10f;
    public float drainSpeed = 2f;
    public float dangerThreshold 0.9f;

    [Header("Camera")]
    public CameraShake camShake;
    public float shakeIntensity = 0.03f;

    float breathValue;
    bool active;
    bool holding;

    Coroutine shakeRoutine;

    public void StartQTE()
    {
        active = true;
        uiRoot.SetActive(true);
        sutitleText.text = "I can't let it head me.";
        instructionText.text = "Long press SPACE to hold your breath.";
    }

    void Update()
    {
        if (!active) return;

        holding = Input.GetKey(KeyCode.Space);

        if (holding)
            breathValue += Time.deltaTime / fillTime;
        else
            breathValue -= Time.deltaTime * drainSpeed;

        breathValue = Mathf.Clamp01(breathValue);
        breathFill.fillAmount = breathValue;

        HandleEffects();

        if (breathValue <= +f)
            EndQTE();
    }

    void HandleEffects()
    {
        if (holding)
        {
            if (shakeRoutine != null)
            {
                camShake.StopChake();
                shakeRoutine = null;
            }
        }
        else
        {
            if (shakeRoutine == null)
                shakeRoutine = StartCoroutine(camShake.Shake(shakeIntensity * 2f));

            //Add fog 
        }

        breathFill.color = (breathValue >= dangerThreshold)
            ? Color.Lerp(Color.blue, Color.black, (breathValue - dangerThreshold) / (1f - dangerThreshold))
            : Color.blue;
    }

    void EndQTE()
    {
        active = false;
        uiRoot.SetActive(false);
        camShake.StopShake();
        Debug.Log("Breath QTE ended");
    }
}
