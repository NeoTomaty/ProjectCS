//====================================================
// スクリプト名：GameStartCountdown
// 作成者：藤本
//
// [Log]
// 05/07 藤本　カウントダウン処理実装
// 05/29　宮林　ポーズ画面表示ボタンの停止
// 06/06　森脇　カウント時の待機Animation制御
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
    public PlayerInput PauseInput;

<<<<<<< HEAD
    [Header("アニメーション制御用")]
    [SerializeField] private PlayerAnimationController playerAnimController;

    [Header("カウントダウン中に再生するアニメーショントリガー名")]
    [SerializeField] private string countdownAnimTrigger = "CountdownIdle";

    private void Start()
    {
        // カウントダウン中に特定のアニメーションを再生
        if (playerAnimController != null)
        {
            playerAnimController.PlaySpecificAnimation(countdownAnimTrigger);
        }

=======

    private bool isCountingDown = false;
    public bool IsCountingDown => isCountingDown;

    
    void Start()
    {
      
>>>>>>> origin/miyabayashi
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

<<<<<<< HEAD
        // model非表示 or 通常状態に戻す
        if (playerAnimController != null)
        {
            playerAnimController.SetUseNormalModel(false);  // rotationModel に戻すなど
        }

        // Snackの打ち上げ実行
=======
        isCountingDown = false; // カウントダウン終了

>>>>>>> origin/miyabayashi
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