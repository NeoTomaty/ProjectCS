//======================================================
// GameOverConditionsスクリプト
// 作成者：森脇
// 最終更新日：4/15
//
// [Log]4/15 森脇　ゲームオーバー条件の実装
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverConditions : MonoBehaviour
{
    [Header("ゲームオーバー判定に必要なリフティング回数")]
    public int gameOverThreshold = 5; // この回数以上で落とすとゲームオーバー

    [Header("ゲームオーバー時の遷移先シーン")]
    public string gameOverSceneName;

    private int currentCount;

    // リフティング回数を外部から設定
    public void SetLiftingCount(int count)
    {
        currentCount = count;
        Debug.Log("Current Lifting Count: " + currentCount);
    }

    // 地面との衝突を検出（地面にぶつかったら呼び出す）
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Ground hit detected.");

            if (currentCount >= gameOverThreshold)
            {
                TriggerGameOver();
            }
            else
            {
                Debug.Log("セーフ：リフティング回数が少ないためゲームオーバーにはなりません。");
            }
        }
    }

    // ゲームオーバー処理
    private void TriggerGameOver()
    {
        if (!string.IsNullOrEmpty(gameOverSceneName))
        {
            Debug.Log("Game Over! Loading scene: " + gameOverSceneName);
            SceneManager.LoadScene(gameOverSceneName);
        }
        else
        {
            Debug.LogError("ゲームオーバーシーンが設定されていません！");
        }
    }
}