//====================================================
// スクリプト名：GameStartCountdown
// 作成者：藤本
// 
// [Log]
// 05/07 藤本　カウントダウン処理実装
// 05/29　宮林　ポーズ画面表示ボタンの停止
//====================================================

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.InputSystem;

public class GameStartCountdown : MonoBehaviour
{
    [Header("カウントダウン用のText")]
    [SerializeField] private Text countdownText;

    [Header("開始時に表示されるCanvas")]
    [SerializeField] private Canvas countdownCanvas;

    [Header("GO!!表示後に非表示にするまでの時間")]
    [SerializeField] private float goDisplayTime = 0.5f;

    [Header("Snack打ち上げ対象")]
    [SerializeField] private SnackLauncher snackLauncher;

    [Header("操作を止めるinput")]
    public PlayerInput PauseInput;                              //ポーズ画面の操作受け取り


    private bool isCountingDown = false;
    public bool IsCountingDown => isCountingDown;

    
    void Start()
    {
      
        StartCoroutine(CountdownCoroutine());
   
    }

    private IEnumerator CountdownCoroutine()
    {
        isCountingDown = true;  // カウントダウン開始

        Time.timeScale = 0f;
        countdownCanvas.gameObject.SetActive(true);

        yield return StartCoroutine(ShowCount("3"));
        yield return StartCoroutine(ShowCount("2"));
        yield return StartCoroutine(ShowCount("1"));
        yield return StartCoroutine(ShowCount("GO!!", goDisplayTime));

        countdownCanvas.gameObject.SetActive(false);

        Time.timeScale = 1f;

        isCountingDown = false; // カウントダウン終了

        if (snackLauncher != null)
        {
            snackLauncher.Launch();
        }
    }

    private IEnumerator ShowCount(string message, float customDuration = 1f)
    {
        countdownText.text = message;
        yield return WaitForRealtimeSeconds(customDuration);
    }

    // Time.timeScale = 0 でも待機できる関数
    private IEnumerator WaitForRealtimeSeconds(float seconds)
    {
        float endTime = Time.realtimeSinceStartup + seconds;
        while (Time.realtimeSinceStartup < endTime)
        {
            yield return null;
        }
    }
}
