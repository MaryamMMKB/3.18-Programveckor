using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

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
    public int charactersPerPage = 350; // per page

    [Header("Timing")]
    public float shortCommentDuration = 2.5f;
    public float closeAnimDuration = 0.4f;

    [Header("Typewriter")]
    public float diaryTypeSpeed = 0.02f;

    private bool diaryOpen;
    private bool isTyping;
    private Coroutine typingRoutine;

    // Persistent history
    private string leftHistory = "";
    private string rightHistory = "";

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

        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            // Only close diary if NOT typing
            if (!isTyping)
            {
                CloseDiary();
            }
            // If typing, ignore space (no skipping)
        }
    }


    // =========================
    // PUBLIC API
    // =========================

    /// <summary>
    /// Write a new diary entry with optional short comment.
    /// Maintains previous history.
    /// </summary>
    public void WriteDiary(string text, string shortComment = "")
    {
        if (!string.IsNullOrEmpty(shortComment))
        {
            StartCoroutine(ShowShortCommentThenWrite(shortComment, text));
        }
        else
        {
            StartCoroutine(WriteDiaryRoutine(text));
        }
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

        // Typewriter effect
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
        yield return WriteDiaryRoutine(diaryText);
    }

    private IEnumerator ShortCommentOnly(string text, float duration)
    {
        shortCommentPanel.SetActive(true);
        shortCommentText.text = text;
        yield return new WaitForSeconds(duration);
        shortCommentPanel.SetActive(false);
    }

    // =========================
    // DIARY WRITING
    // =========================

    private IEnumerator WriteDiaryRoutine(string text)
    {
        diaryPanel.SetActive(true);
        diaryAnimator.ResetTrigger("Close");
        diaryAnimator.SetTrigger("Open");
        diaryOpen = true;

        int index = 0;

        // Pre-fill pages with history
        leftPageText.text = leftHistory;
        rightPageText.text = rightHistory;

        while (index < text.Length)
        {
            int leftRemaining = charactersPerPage - leftPageText.text.Length;
            int rightRemaining = charactersPerPage - rightPageText.text.Length;

            // If both pages full, flip spread
            if (leftRemaining <= 0 && rightRemaining <= 0)
            {
                yield return FlipSpread();
                leftRemaining = charactersPerPage;
                rightRemaining = charactersPerPage;
            }

            // Decide where to start writing
            if (leftPageText.text.Length == 0 && leftRemaining > 0)
            {
                // Left page is empty ? write here
                int take = Mathf.Min(leftRemaining, text.Length - index);
                string chunk = text.Substring(index, take);
                yield return TypeText(leftPageText, chunk);
                index += take;
                leftHistory = leftPageText.text;
            }
            else if (rightPageText.text.Length == 0 && rightRemaining > 0)
            {
                // Left page has text ? write on right
                int take = Mathf.Min(rightRemaining, text.Length - index);
                string chunk = text.Substring(index, take);
                yield return TypeText(rightPageText, chunk);
                index += take;
                rightHistory = rightPageText.text;
            }
            else
            {
                // Both pages have some text, flip spread
                yield return FlipSpread();
            }
        }
    }




    private IEnumerator TypeText(TextMeshProUGUI textComponent, string text)
    {
        isTyping = true;

        foreach (char c in text)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(diaryTypeSpeed);
        }

        isTyping = false;
    }


    private void ShowFullCurrentText()
    {
        // Stops typewriter instantly, nothing more needed
        isTyping = false;
    }

    // =========================
    // FLIP SPREAD
    // =========================

    private IEnumerator FlipSpread()
    {
        diaryAnimator.ResetTrigger("Open");
        diaryAnimator.SetTrigger("Close");
        yield return new WaitForSeconds(closeAnimDuration);

        // Clear pages and history for next spread
        leftPageText.text = "";
        rightPageText.text = "";
        leftHistory = "";
        rightHistory = "";

        diaryAnimator.ResetTrigger("Close");
        diaryAnimator.SetTrigger("Open");
    }

    // =========================
    // CLOSE DIARY
    // =========================

    public void CloseDiary()
    {
        diaryAnimator.ResetTrigger("Open");
        diaryAnimator.SetTrigger("Close");
        diaryOpen = false;

        diaryPanel.SetActive(false);
    }

    private void ClearPages()
    {
        leftPageText.text = "";
        rightPageText.text = "";
        leftHistory = "";
        rightHistory = "";
    }
}
