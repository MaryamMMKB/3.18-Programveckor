using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class SliderEffects : MonoBehaviour
{
    [Header("Reference")]
    public Press_Hold pressHoldScript;

    [Header("Camera")]
    public Transform cameraTransform;
    public Camera mainCamera;

    [Header("UI")]
    public Slider slider_Main;
    public CanvasGroup sliderCanvas; // for fade-in/out
    public Image blackScreen;
    public Image whiteFlash;
    public TMP_Text subtitlesText;

    [Header("Post Processing")]
    public Volume volume;

    private Vignette vignette;
    private ChromaticAberration chromatic;
    private FilmGrain filmGrain;

    private Quaternion originalCameraRot;
    private Vector3 originalSliderPos;
    private float baseFOV;

    [Header("Timing")]
    public float dizzyStartTime = 20f;
    public float maxValue = 25f;

    [Header("Camera Motion")]
    public float maxSway = 0.6f;
    public float swaySpeed = 1.5f;
    public float jerkChance = 0.15f;
    public float jerkStrength = 3f;
    public float snapChance = 0.06f;

    [Header("Slider Shake")]
    public float sliderShakeAmount = 8f;

    [Header("FOV Panic")]
    public float maxFOVPulse = 12f;

    [Header("Audio")]
    public AudioSource audioAlways;
    public AudioSource audioDizzy1;
    public AudioSource audioDizzy2;

    public float minDizzyVolume = 0.2f;
    public float maxDizzyVolume = 0.6f;
    public float maxDizzyPitch = 1.35f;

    [Header("Audio Timing")]
    public float dizzy1StartTime = 19f;
    public float alwaysAudioStopTime = 23f;

    [Header("Blackout")]
    public float blackoutFadeDuration = 1.5f;
    public float whiteFlashTime = 0.08f;
    public float cameraSnapTime = 24.8f;

    private bool fadeStarted = false;
    private bool audioCut = false;
    private bool cameraSnapped = false;
    private bool sliderShown = false;
    private bool sliderHidden = false;

    // Subtitle timing
    private bool subtitleSecondScheduled = false;

    void Start()
    {
        if (cameraTransform != null)
            originalCameraRot = cameraTransform.localRotation;

        if (slider_Main != null)
            originalSliderPos = slider_Main.transform.localPosition;

        if (mainCamera != null)
            baseFOV = mainCamera.fieldOfView;

        if (volume != null)
        {
            volume.profile.TryGet(out vignette);
            volume.profile.TryGet(out chromatic);
            volume.profile.TryGet(out filmGrain);
        }

        if (blackScreen != null)
        {
            blackScreen.gameObject.SetActive(false);
            blackScreen.color = new Color(0, 0, 0, 0);
        }

        if (whiteFlash != null)
        {
            whiteFlash.gameObject.SetActive(false);
            whiteFlash.color = new Color(1, 1, 1, 0);
        }

        if (audioAlways != null && !audioAlways.isPlaying)
            audioAlways.Play();

        if (subtitlesText != null)
            subtitlesText.text = "";

        if (sliderCanvas != null)
            sliderCanvas.alpha = 0f; // slider hidden initially

        // First subtitle at the very start (~5s reading time)
        StartCoroutine(ShowSubtitleTyping("It can hear me. I have to steady my breathing.", 5f, 0.05f, true));
    }

    void Update()
    {
        if (pressHoldScript == null) return;

        float currentValue = pressHoldScript.GetCurrentValue();

        // SECOND SUBTITLE: after space pressed, 1s delay
        if (sliderShown && !subtitleSecondScheduled && Input.GetKeyDown(KeyCode.Space))
        {
            subtitleSecondScheduled = true;
            StartCoroutine(DelayedSubtitleTyping("Calm. Stay calm.", 3f, 0.04f, 1f));
        }

        // Hide slider 2 seconds before total blackout
        if (!sliderHidden && currentValue >= maxValue - 2f)
        {
            sliderHidden = true;
            if (sliderCanvas != null)
                StartCoroutine(FadeCanvas(sliderCanvas, 0f, 0.3f));
        }

        // Subtitles at specific times
        if (currentValue >= 15f && subtitlesText != null && subtitlesText.text == "")
            StartCoroutine(ShowSubtitleTyping("It's almost gone now.", 2f, 0.04f));

        if (currentValue >= 20f && subtitlesText != null)
            StartCoroutine(ShowSubtitleTyping("Air. I need air.", 2f, 0.04f));

        // Clear all subtitles at ~23.5s
        if (currentValue >= 23.5f && subtitlesText != null)
            subtitlesText.text = "";

        // Early dizzy audio
        if (audioDizzy1 != null && !audioDizzy1.isPlaying && currentValue >= dizzy1StartTime)
            audioDizzy1.Play();

        // Hard audio cut
        if (!audioCut && currentValue >= alwaysAudioStopTime)
        {
            audioCut = true;
            if (audioAlways != null) audioAlways.Stop();
        }

        if (currentValue >= dizzyStartTime)
        {
            float t = Mathf.InverseLerp(dizzyStartTime, maxValue, Mathf.Min(currentValue, maxValue));
            float panicBoost = currentValue >= maxValue - 2f ? 1.6f : 1f;

            // Vignette pulse
            if (vignette != null)
            {
                float heart = Mathf.Abs(Mathf.Sin(Time.time * 4.5f));
                vignette.intensity.value = Mathf.Lerp(0.3f, 0.75f, t) + heart * 0.2f * t;
                vignette.smoothness.value = Mathf.Lerp(0.5f, 1f, t);
            }

            // Chromatic aberration
            if (chromatic != null)
                chromatic.intensity.value = Mathf.Abs(Mathf.Sin(Time.time * 3f)) * 0.35f * t;

            // Film grain
            if (filmGrain != null)
                filmGrain.intensity.value = Mathf.Lerp(0.2f, 0.8f, t);

            // Camera motion
            if (cameraTransform != null)
            {
                float swayZ = Mathf.Sin(Time.time * swaySpeed * panicBoost) * maxSway * t;
                float swayX = (Mathf.PerlinNoise(Time.time * 2f, 0f) - 0.5f) * maxSway * t;
                float swayY = (Mathf.PerlinNoise(0f, Time.time * 2f) - 0.5f) * maxSway * t;

                float jerk = Random.value < jerkChance * t
                    ? Random.Range(-jerkStrength, jerkStrength)
                    : 0f;

                cameraTransform.localRotation =
                    originalCameraRot * Quaternion.Euler(swayX, swayY, swayZ + jerk);
            }

            // Sudden camera snap near end
            if (!cameraSnapped && currentValue >= cameraSnapTime)
            {
                cameraSnapped = true;
                cameraTransform.localRotation =
                    originalCameraRot * Quaternion.Euler(0f, 0f, Random.Range(-40f, 40f));
            }

            // FOV
            if (mainCamera != null)
            {
                float choke = Mathf.Sin(Time.time * 6f) * maxFOVPulse * t * panicBoost;
                mainCamera.fieldOfView = baseFOV + choke;
            }

            // Slider shake
            if (slider_Main != null)
            {
                Vector2 shake =
                    new Vector2(Mathf.Sin(Time.time * 50f), Mathf.Cos(Time.time * 35f))
                    * sliderShakeAmount * t * panicBoost;

                slider_Main.transform.localPosition = originalSliderPos + (Vector3)shake;
            }

            // Time instability
            Time.timeScale = Mathf.Clamp(0.8f + Random.Range(-0.08f, 0.08f) * t, 0.6f, 1f);

            // Dizzy audio chaos
            if (audioDizzy2 != null && !audioDizzy2.isPlaying)
                audioDizzy2.Play();

            if (audioDizzy1 != null)
            {
                audioDizzy1.volume = Mathf.Lerp(minDizzyVolume, maxDizzyVolume, t);
                audioDizzy1.pitch = Mathf.Lerp(1f, maxDizzyPitch, t) + Random.Range(-0.1f, 0.1f) * t;
            }

            if (audioDizzy2 != null)
            {
                audioDizzy2.volume = Mathf.Lerp(minDizzyVolume, maxDizzyVolume, t);
                audioDizzy2.pitch = Mathf.Lerp(1f, maxDizzyPitch, t) - Random.Range(-0.1f, 0.1f) * t;
            }
        }

        if (currentValue >= maxValue && !fadeStarted)
        {
            fadeStarted = true;
            StartCoroutine(WhiteFlashThenBlackout());
        }
    }

    // Typing subtitle
    IEnumerator ShowSubtitleTyping(string text, float totalDuration, float typingDelay = 0.05f, bool revealSliderAfter = false)
    {
        if (subtitlesText == null) yield break;

        subtitlesText.text = "";
        float typingTime = Mathf.Max(0.01f, totalDuration * 0.5f); // typing takes half the display time
        float perCharDelay = typingTime / text.Length;

        for (int i = 0; i < text.Length; i++)
        {
            subtitlesText.text += text[i];
            yield return new WaitForSeconds(perCharDelay);
        }

        // Wait remaining display time
        yield return new WaitForSeconds(totalDuration - typingTime);
        subtitlesText.text = "";

        // Show slider after first subtitle
        if (revealSliderAfter && sliderCanvas != null && !sliderShown)
        {
            sliderShown = true;
            StartCoroutine(FadeCanvas(sliderCanvas, 1f, 0.5f));
        }
    }

    IEnumerator DelayedSubtitleTyping(string text, float totalDuration, float typingDelay, float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return ShowSubtitleTyping(text, totalDuration, typingDelay);
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float targetAlpha, float duration)
    {
        float startAlpha = cg.alpha;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cg.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            yield return null;
        }
        cg.alpha = targetAlpha;
    }

    IEnumerator WhiteFlashThenBlackout()
    {
        if (whiteFlash != null)
        {
            whiteFlash.gameObject.SetActive(true);
            whiteFlash.color = Color.white;
            yield return new WaitForSeconds(whiteFlashTime);
            whiteFlash.gameObject.SetActive(false);
        }

        blackScreen.gameObject.SetActive(true);
        Color c = blackScreen.color;
        float t = 0f;

        while (t < blackoutFadeDuration)
        {
            t += Time.deltaTime;
            blackScreen.color = new Color(c.r, c.g, c.b, t / blackoutFadeDuration);
            yield return null;
        }

        blackScreen.color = Color.black;
    }
}
