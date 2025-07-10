using UnityEngine;
using System.Collections;
using static UnityEngine.GraphicsBuffer;

public class CreateSnackAnnounce : MonoBehaviour
{
    [SerializeField] private GameObject BlackPanel;
    [SerializeField] private GameObject TextObject;

    private bool isDisplayMessage = false;
    private bool isPaused = false;
    private bool hasStoppedAtZero = false;
    private RectTransform TextRectTransform;

    [SerializeField] private float TextMoveSpeed = 100f;
    [SerializeField] private float TextStopTime = 1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip AnnounceSE;
    private float leftX = -1500f;
    private float rightX = 1500f;
    private int direction = -1;

    void Start()
    {
        BlackPanel.SetActive(false);
        TextObject.SetActive(false);

        TextRectTransform = TextObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!isDisplayMessage) return;

        if (isPaused) return;

        Vector2 pos = TextRectTransform.anchoredPosition;
        pos.x += direction * TextMoveSpeed * Time.deltaTime;

        // XÇ™0Çí âﬂÇµÇΩÇÁí‚é~Åi1âÒÇæÇØÅj
        if (!hasStoppedAtZero && pos.x <= 0f)
        {
            pos.x = 0f;
            TextRectTransform.anchoredPosition = pos;
            hasStoppedAtZero = true;
            StartCoroutine(PauseThenContinue());
            return;
        }

        TextRectTransform.anchoredPosition = pos;

        if (pos.x < leftX)
        {
            isDisplayMessage = false;
            BlackPanel.SetActive(false);
            TextObject.SetActive(false);
        }
        
    }

    IEnumerator PauseThenContinue()
    {
        isPaused = true;
        yield return new WaitForSeconds(TextStopTime);
        isPaused = false;
    }

    public void DisplayMessage()
    {
        isDisplayMessage = true;
        hasStoppedAtZero = false;
        BlackPanel.SetActive(true);
        TextObject.SetActive(true);

        Vector2 pos = TextRectTransform.anchoredPosition;
        pos.x = rightX;
        TextRectTransform.anchoredPosition = pos;

        if (AnnounceSE) audioSource.PlayOneShot(AnnounceSE);

    }
}
