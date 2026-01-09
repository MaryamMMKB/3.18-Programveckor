using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    // Screenshake
    public bool start = false;
    public float ShakeDuration = 1f; // duration of shake
    public AnimationCurve curve; // modify shake intensity

    // ViewBobbing
    public float bobFrequency = 5f; // speed of bobbing
    public float bobAmount = 0.05f; // amount of bobbing

    public enum ShakeMode
    {
        ScreenShake,
        ViewBobbing
    }

    public ShakeMode mode = ShakeMode.ScreenShake; // default mode
    public bool loop = false;

    public bool useLocalPosition = true;
    Vector3 initialPosition;
    Coroutine activeCoroutine;

    void Awake()
    {
        initialPosition = useLocalPosition ? transform.localPosition : transform.position;
    }

    void Update()
    {
        if (start)
        {
            start = false;
            StartEffect();
        }
    }

    void StartEffect()
    {
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = StartCoroutine(Shaking());
    }

    IEnumerator Shaking()
    {
        Vector3 startPosition = useLocalPosition ? transform.localPosition : transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < ShakeDuration || loop)
        {
            elapsedTime += Time.deltaTime;
            float t = (ShakeDuration > 0f) ? Mathf.Clamp01(elapsedTime / ShakeDuration) : 1f;
            float strength = (curve != null) ? curve.Evaluate(t) : 1f;
            Vector3 offset;

            if (mode == ShakeMode.ScreenShake)
            {
                offset = Random.insideUnitSphere * strength;
            }
            else
            {
                float phase = elapsedTime * bobFrequency;
                float horizontal = Mathf.Sin(phase) * bobAmount * strength;
                float vertical = Mathf.Abs(Mathf.Cos(phase)) * bobAmount * strength * 0.5f;
                offset = new Vector3(horizontal, vertical, 0f);
            }

            if (useLocalPosition)
                transform.localPosition = startPosition + offset;
            else
                transform.position = startPosition + offset;

            yield return null;
        }

        // restore exact start position when finished
        if (useLocalPosition)
            transform.localPosition = startPosition;
        else
            transform.position = startPosition;

        activeCoroutine = null;
    }

    // Triggers
    public void TriggerShake()
    {
        mode = ShakeMode.ScreenShake;
        loop = false;
        StartEffect();
    }

    public void TriggerViewBobbing(bool continuous = true)
    {
        mode = ShakeMode.ViewBobbing;
        loop = continuous;
        StartEffect();
    }

    public void StopEffects()
    {
        loop = false;
        if (activeCoroutine != null) StopCoroutine(activeCoroutine);
        activeCoroutine = null;

        if (useLocalPosition)
            transform.localPosition = initialPosition;
        else
            transform.position = initialPosition;
    }
}

