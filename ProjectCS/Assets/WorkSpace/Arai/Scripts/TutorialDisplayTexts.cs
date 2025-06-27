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
    [SerializeField] private GameObject OnStartUI;
    [SerializeField] private GameObject OnCollideUI;

    public bool IsDisplayUI = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OnStartUI.SetActive(false);
        OnCollideUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDisplayUI)
        {
            if (Input.anyKeyDown)
            {
                OnStartUI.SetActive(false);
                OnCollideUI.SetActive(false);
                IsDisplayUI = false;
                Time.timeScale = 1f;
            }
        }
    }

    public void DisplayTutorialUI1()
    {
        OnStartUI.SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }

    public void DisplayTutorialUI2()
    {
        OnCollideUI.SetActive(true);
        IsDisplayUI = true;
        Time.timeScale = 0f;
    }
}
