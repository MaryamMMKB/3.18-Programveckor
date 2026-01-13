using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Press_Hold : MonoBehaviour
{
    public Slider slider_Main;
    public TMP_Text slider_Counter;
    public TMP_Text slider_Status;
    public Image slider_Fill;

    [Header("Colors")]
    public Color mainColor = new Color(0.141f, 0.239f, 0.306f); // 233D4E
    public Color finishColor = new Color(0.588f, 0.710f, 0.800f); // 96B5CC

    public float fillSpeed = 1.5f;
    public float drainSpeed = 4f;

    private float currentValue;
    private float maxValue;
    private float dangerStartTime = 20f; // danger zone starts at 20s

    void Start()
    {
        currentValue = 0f;
        maxValue = 25f;

        slider_Main.minValue = 0f;
        slider_Main.maxValue = maxValue;
        slider_Main.value = currentValue;

        slider_Counter.text = "0";

        slider_Fill.color = mainColor;
    }

    void Update()
    {
        // Fill or drain
        if (Input.GetKey(KeyCode.Space))
        {
            currentValue = Mathf.MoveTowards(currentValue, maxValue, fillSpeed * Time.deltaTime);
            slider_Status.text = "SPACE is in Hold State";
        }
        else
        {
            currentValue = Mathf.MoveTowards(currentValue, 0f, drainSpeed * Time.deltaTime);
            slider_Status.text = "The Button is Released";
        }

        slider_Main.value = currentValue;
        slider_Counter.text = Mathf.RoundToInt(currentValue).ToString();

        // ===== Danger zone effect for the last portion =====
        float normalizedValue = currentValue / maxValue;

        if (currentValue <= dangerStartTime)
        {
            // Regular fill, no danger zone
            slider_Fill.color = mainColor;
        }
        else
        {
            // Calculate danger portion (0 → 1 over last 5s)
            float dangerT = (currentValue - dangerStartTime) / (maxValue - dangerStartTime);

            // Base color stays mainColor
            slider_Fill.color = mainColor;

            // Overlay finishColor for the danger portion
            // Trick: use a Material with fill, or simply lerp the color based on normalizedValue of danger
            slider_Fill.color = Color.Lerp(mainColor, finishColor, dangerT);
        }
    }

    public float GetCurrentValue()
    {
        return currentValue;
    }
}
