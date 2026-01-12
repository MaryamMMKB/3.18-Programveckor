using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Press_Hold : MonoBehaviour
{

    public Slider slider_Main;
    public Text slider_Counter;
    public Text slider_Status;
    public Text slider_ColorStatus;
    public Gradient slider_GradientColor;
    public Gradient slider_PlainColor;
    public Image slider_Fill;

    public float slider_StartingValue;
    public float slider_EndValue;
    public float slider_MinValue;

    public bool slider_BGFill;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider_EndValue = slider_StartingValue * 10f;
        slider_MinValue = 0;

        slider_Main.minValue = slider_MinValue;
        slider_Main.maxValue = slider_EndValue;

        slider_Counter.text = slider_MinValue.ToString();
        slider_Fill.color = slider_GradientColor.Evaluate(slider_Main.normalizedValue);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            slider_StartingValue = Mathf.MoveTowards(
                slider_StartingValue,
                slider_EndValue,
                1.5f * Time.deltaTime
            );
            slider_Status.text = "The Button E is in Hold State";
        }
        else
        {
            slider_StartingValue = Mathf.MoveTowards(
                slider_StartingValue,
                slider_MinValue,
                4f * Time.deltaTime
            );
            slider_Status.text = "The Button is Released or No Action Performed";
        }

        slider_Main.value = slider_StartingValue;
        slider_Counter.text = Mathf.RoundToInt(slider_StartingValue).ToString();

        if (Input.GetKeyDown(KeyCode.C))
        {
            slider_BGFill = !slider_BGFill;
        }

        if (slider_BGFill)
        {
            slider_Fill.color = slider_GradientColor.Evaluate(slider_Main.normalizedValue);
            slider_ColorStatus.text = "BG Fill is in Gradient";
        }
        else
        {
            slider_Fill.color = slider_PlainColor.Evaluate(slider_Main.normalizedValue);
            slider_ColorStatus.text = "BG is in Plain Color";
        }
    }
}