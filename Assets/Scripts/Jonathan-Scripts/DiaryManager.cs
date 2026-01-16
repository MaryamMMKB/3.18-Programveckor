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
    public Animator diaryAnimator;

    [Header("Diary Pages")]
    public TextMeshProUGUI leftPageText;
    public TextMeshProUGUI rightPageText;
    public int charactersPerPage = 350;

    [Header("Timing")]
    public float shortCommentDuration = 2.5f;
    public float closeAnimDuration = 0.4f;

    [Header("Typewriter")]
    public float diaryTypeSpeed = 0.02f;

    private bool diaryOpen;
    private bool isTyping;

    // Writing state
    private string activeText;
    private int activeIndex;
    private Coroutine writingRoutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        shortCommentPanel.SetActive(false);
        diaryPanel.SetActive(false);
        ClearPages();
    }

    void Update()
    {
        if (!diaryOpen) return;

        var keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Finish typing instantly
        if (keyboard.spaceKey.wasPressedThisFrame && isTyping)
        {
            FinishTypingInstant();
            return;
        }

        // Flip page with D
        if (keyboard.dKey.wasPressedThisFrame && !isTyping)
        {
            StartCoroutine(FlipAndContinue());
        }

        // Close diary if no text left
        if (keyboard.spaceKey.wasPressedThisFrame &&
            !isTyping &&
            activeIndex >= activeText.Length)
        {
            CloseDiary();
        }
    }

    // =========================
    // PUBLIC API
    // =========================

    public void WriteDiary(string text, string shortComment = "")
    {
        if (!string.IsNullOrEmpty(shortComment))
            StartCoroutine(ShowShortCommentThenWrite(shortComment, text));
        else
            StartWriting(text);
    }

    public void ShowShortComment(string text)
    {
        StartCoroutine(ShortCommentTyped(text, shortCommentDuration));
    }

    // =========================
    // SHORT COMMENT
    // =========================

    IEnumerator ShortCommentTyped(string text, float duration)
    {
        shortCommentPanel.SetActive(true);
        shortCommentText.text = "";

        foreach (char c in text)
        {
            shortCommentText.text += c;
            yield return new WaitForSeconds(diaryTypeSpeed);
        }

        yield return new WaitForSeconds(duration);
        shortCommentPanel.SetActive(false);
    }

    private IEnumerator ShowShortCommentThenWrite(string shortComment, string diaryText)
    {
        yield return ShortCommentOnly(shortComment, shortCommentDuration);
        StartWriting(diaryText);
    }

    private IEnumerator ShortCommentOnly(string text, float duration)
    {
        shortCommentPanel.SetActive(true);
        shortCommentText.text = text;
        yield return new WaitForSeconds(duration);
        shortCommentPanel.SetActive(false);
    }

    // =========================
    // DIARY SYSTEM
    // =========================

    void StartWriting(string text)
    {
        // If diary already has content, append instead of reset
        if (!string.IsNullOrEmpty(activeText))
        {
            activeText += "\n\n" + text; // spacing between entries
        }
        else
        {
            activeText = text;
            activeIndex = 0;
        }

        diaryPanel.SetActive(true);
        diaryAnimator.ResetTrigger("Close");
        diaryAnimator.SetTrigger("Open");
        diaryOpen = true;

        ResumeWriting();
    }


    void ResumeWriting()
    {
        if (writingRoutine != null)
            StopCoroutine(writingRoutine);

        writingRoutine = StartCoroutine(WriteRoutine());
    }

    IEnumerator WriteRoutine()
    {
        isTyping = true;

        while (activeIndex < activeText.Length)
        {
            // Left page
            while (leftPageText.text.Length < charactersPerPage &&
                   activeIndex < activeText.Length)
            {
                leftPageText.text += activeText[activeIndex++];
                yield return new WaitForSeconds(diaryTypeSpeed);
            }

            // Right page
            while (rightPageText.text.Length < charactersPerPage &&
                   activeIndex < activeText.Length)
            {
                rightPageText.text += activeText[activeIndex++];
                yield return new WaitForSeconds(diaryTypeSpeed);
            }

            // Pages full ? wait for D
            if (activeIndex < activeText.Length)
            {
                isTyping = false;
                yield break;
            }
        }

        isTyping = false;
    }

    void FinishTypingInstant()
    {
        StopCoroutine(writingRoutine);

        while (leftPageText.text.Length < charactersPerPage &&
               activeIndex < activeText.Length)
        {
            leftPageText.text += activeText[activeIndex++];
        }

        while (rightPageText.text.Length < charactersPerPage &&
               activeIndex < activeText.Length)
        {
            rightPageText.text += activeText[activeIndex++];
        }

        isTyping = false;
    }

    // =========================
    // PAGE FLIP
    // =========================

    IEnumerator FlipAndContinue()
    {
        diaryAnimator.ResetTrigger("Open");
        diaryAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(closeAnimDuration);

        ClearPages();

        diaryAnimator.ResetTrigger("Close");
        diaryAnimator.SetTrigger("Open");

        ResumeWriting();
    }

    // =========================
    // CLOSE
    // =========================

    public void CloseDiary()
    {
        diaryAnimator.ResetTrigger("Open");
        diaryAnimator.SetTrigger("Close");

        diaryOpen = false;
        diaryPanel.SetActive(false);
    }

    void ClearPages()
    {
        leftPageText.text = "";
        rightPageText.text = "";
    }
}
