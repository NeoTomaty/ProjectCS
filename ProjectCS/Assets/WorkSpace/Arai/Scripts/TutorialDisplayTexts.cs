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

    [SerializeField] private GameObject[] TutorialUI;
    private int TutorialIndex = 0;

    [System.NonSerialized] public bool IsDisplayUI = false;

    [Header("�Q��")]
    [SerializeField] private PlayerStateManager PlayerStateManagerComponent;
    private bool IsLiftingPart = false; // ���t�e�B���O�p�[�g�ɂȂ�����

    private bool IsWarp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // �`���[�g���A��UI���\���ɂ���
        foreach (var UI in TutorialUI)
        {
            UI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDisplayUI)
        {
            if (Input.anyKeyDown)
            {
                TutorialUI[TutorialIndex].SetActive(false);
                TutorialIndex++;
                IsDisplayUI = false;
                Time.timeScale = 1f;


                switch(TutorialIndex)
                {
                    case 1:
                        break;
                    case 2:
                        break;
                    case 3:
                        DisplayTutorialUI4();
                        break;
                    case 4:
                        break;
                    case 5:
                        DisplayTutorialUI6();
                        break;
                    case 6:
                        DisplayTutorialUI7();
                        break;
                    default:
                        break;
                }

            }
        }

        if (!IsLiftingPart && PlayerStateManagerComponent.GetLiftingState() == PlayerStateManager.LiftingState.LiftingPart)
        {
            IsLiftingPart = true;
            DisplayTutorialUI3();
        }
    }

    public void DisplayTutorialUI1()
    {
        TutorialUI[0].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }

    public void DisplayTutorialUI2()
    {
        TutorialUI[1].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
    public void DisplayTutorialUI3()
    {
        TutorialUI[2].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
    public void DisplayTutorialUI4()
    {
        TutorialUI[3].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
    public void DisplayTutorialUI5()
    {
        if (IsWarp) return;
        IsWarp = true;
        TutorialUI[4].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
    public void DisplayTutorialUI6()
    {
        TutorialUI[5].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
    public void DisplayTutorialUI7()
    {
        TutorialUI[6].SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
}
