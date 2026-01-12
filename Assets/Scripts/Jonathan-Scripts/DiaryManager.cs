using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;

public class DiaryUIManager : MonoBehaviour
{
    public static DiaryUIManager Instance;

    [Header("Short Comment UI")]
    public GameObject shortCommentPanel;
    public TextMeshProUGUI shortCommentText;

    [Header("Diary UI")]
    public GameObject diaryPanel;
    public TextMeshProUGUI diaryText;
    public Animator diaryAnimator;

    [Header("Timing")]
    public float shortCommentDuration = 2.5f;
    public float typeSpeed = 0.03f;
    public float closeAnimDuration = 0.4f;

    private bool diaryOpen;
    private System.Action onDiaryClosedCallback;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        shortCommentPanel.SetActive(false);
        diaryPanel.SetActive(false);
    }

    void Update()
    {
        if (!diaryOpen) return;

        if (Keyboard.current != null &&
            (Keyboard.current.eKey.wasPressedThisFrame ||
             Keyboard.current.escapeKey.wasPressedThisFrame))
        {
            CloseDiary();
        }
    }

    // =========================
    // PUBLIC API
    // =========================

    public void ShowDiary(string shortText, string diaryTextFull, System.Action onClosed = null)
    {
        if (diaryOpen) return;

        onDiaryClosedCallback = onClosed;
        StartCoroutine(DiarySequence(shortText, diaryTextFull));
    }


    public void ShowShortComment(string text)
    {
        StartCoroutine(ShortCommentOnly(text));
    }

    // =========================
    // SEQUENCES
    // =========================

    IEnumerator DiarySequence(string shortText, string diaryTextFull)
    {
        // Short comment
        shortCommentPanel.SetActive(true);
        shortCommentText.text = "";
        yield return StartCoroutine(TypeText(shortCommentText, shortText));

        yield return new WaitForSeconds(shortCommentDuration);
        shortCommentPanel.SetActive(false);

        // Diary
        diaryPanel.SetActive(true);
        diaryAnimator.ResetTrigger("Close");
        diaryAnimator.SetTrigger("Open");

        diaryText.text = "";
        diaryOpen = true;
        yield return StartCoroutine(TypeText(diaryText, diaryTextFull));
    }

    IEnumerator ShortCommentOnly(string text)
    {
        shortCommentPanel.SetActive(true);
        shortCommentText.text = "";
        yield return StartCoroutine(TypeText(shortCommentText, text));
        yield return new WaitForSeconds(2f);
        shortCommentPanel.SetActive(false);
    }

    // =========================
    // CLOSE LOGIC
    // =========================

    void CloseDiary()
    {
        diaryAnimator.ResetTrigger("Open");
        diaryAnimator.SetTrigger("Close");
        diaryOpen = false;
        StartCoroutine(DisableDiaryAfterAnim());
    }

    IEnumerator DisableDiaryAfterAnim()
    {
        yield return new WaitForSeconds(closeAnimDuration);

        diaryAnimator.ResetTrigger("Close");
        diaryAnimator.Play(0, 0, 0f); // reset animator state

        diaryPanel.SetActive(false);

        onDiaryClosedCallback?.Invoke();
        onDiaryClosedCallback = null;
    }


    // =========================
    // TYPEWRITER
    // =========================

    IEnumerator TypeText(TextMeshProUGUI textComponent, string text)
    {
        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
    }
}
