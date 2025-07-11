using UnityEngine;

public class TutorialDisplay : MonoBehaviour
{
    [SerializeField] private TutorialDisplayTexts TutorialDisplayTextsComponent;
    private bool IsCollided = false;

    [SerializeField] float DisplayTime = 1f;
    private float Timer = 0f;
    private bool TimerStop = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TimerStop) return;
        if(!IsCollided) return;

        Timer += Time.deltaTime;

        if(Timer > DisplayTime)
        {
            TimerStop = true;
            TutorialDisplayTextsComponent.DisplayTutorialUI5();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsCollided && collision.gameObject.CompareTag("Player"))
        {
            IsCollided = true;
            //TutorialDisplayTextsComponent.DisplayTutorialUI5();
        }
    }
}
