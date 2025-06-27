//======================================================
// [TutorialDisplayTexts]
// �쐬�ҁF�r��C
// �ŏI�X�V���F06/27
// 
// [Log]
// 06/27�@�r��@�`���[�g���A���p��UI��\��������悤�Ɏ���
//======================================================
using UnityEngine;

public class TutorialDisplayTexts : MonoBehaviour
{
    [Header("�`���[�g���A���p��UI")]
    [SerializeField] private GameObject OnSnackGanarateUI;
    [SerializeField] private GameObject OnJumpUI;

    private bool IsDisplayUI = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnSnackGanarateUI.SetActive(false);
        OnJumpUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDisplayUI)
        {
            if (Input.anyKeyDown)
            {
                OnSnackGanarateUI.SetActive(false);
                OnJumpUI.SetActive(false);
                IsDisplayUI = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void DisplayTutorialUI1()
    {
        OnSnackGanarateUI.SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }

    public void DisplayTutorialUI2()
    {
        OnJumpUI.SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
}
