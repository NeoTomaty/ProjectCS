//======================================================
// [TutorialDisplayTexts]
// 作成者：荒井修
// 最終更新日：06/27
// 
// [Log]
// 06/27　荒井　チュートリアル用のUIを表示させるように実装
//======================================================
using UnityEngine;

public class TutorialDisplayTexts : MonoBehaviour
{
    [Header("チュートリアル用のUI")]

    [SerializeField] private GameObject[] TutorialUI;
    private int TutorialIndex = 0;

    [System.NonSerialized] public bool IsDisplayUI = false;

    [Header("参照")]
    [SerializeField] private PlayerStateManager PlayerStateManagerComponent;
    private bool IsLiftingPart = false; // リフティングパートになったか

    private bool IsWarp = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // チュートリアルUIを非表示にする
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
