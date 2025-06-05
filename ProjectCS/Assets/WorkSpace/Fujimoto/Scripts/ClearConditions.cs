//======================================================
// ClearConditionsスクリプト
// 作成者：藤本
// 最終更新日：06/05
// 
// [Log]
// 04/29 藤本　クリア条件の追加
// 05/01 竹内　UIと連携してリフティング回数を参照できるように修正
// 05/08 荒井　クリア演出を実行できるように変更
// 05/10 荒井　クリア演出に関する例外処理を追加
// 06/05 荒井　シーン遷移時にフェードアウトする処理を追加
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearConditions : MonoBehaviour
{
    [Header("クリア条件")]
    public int clearCondition = 3; // クリア条件のリフティング回数

    [Header("遷移先シーン")]
    public string nextSceneName; // インスペクターで設定可能な遷移先シーン名

    [Header("UI")]
    public Text liftingCounterText; // Textを設定

    [SerializeField]
    private GameClearSequence GameClearSequence; // クリア演出のスクリプト


    private int currentCount; // 現在のリフティング回数（外部から受け取る）

    // 開始時処理
    private void Start()
    {
        // クリア回数を設定
        currentCount = clearCondition;
        UpdateLiftingCounterUI();
    }

    // 外部からリフティング回数を減らすメソッド
    public void CheckLiftingCount()
    {
        currentCount--;
        Debug.Log("Current Lifting Count: " + currentCount);

        UpdateLiftingCounterUI();

        // クリア条件を満たしているかチェック
        if (currentCount <= 0)
        {
            Debug.Log("ClearConditionsスクリプト：条件を満たしたためシーン遷移を行います");

            if(GameClearSequence == null)
            {
                TriggerSceneTransition();
            }
            else
            {
                bool Res = GameClearSequence.OnGameClear();

                // OnGameClear関数が正常に終了しなかった場合
                // 演出無しでシーン遷移
                if (!Res)
                {
                    Debug.LogError("ClearConditionsスクリプト：OnGameClear関数が正常に終了しませんでした");
                    TriggerSceneTransition();
                }
            }
        }
    }

    // リフティング回数の更新
    private void UpdateLiftingCounterUI()
    {
        if (liftingCounterText != null)
        {
            liftingCounterText.text = "Lift Count: " + currentCount.ToString();
        }
        else
        {
            Debug.LogWarning("liftingCounterText が設定されていません！");
        }
    }

    // シーン遷移を実行
    public void TriggerSceneTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // シーン名が設定されている場合のみ実行
        {
            Debug.Log("Loading Scene: " + nextSceneName);

            // フェードアウトしてシーン遷移
            FadeManager fade = FindFirstObjectByType<FadeManager>();
            if (fade != null)
            {
                fade.FadeToScene(nextSceneName);
            }
            else
            {
                SceneManager.LoadScene(nextSceneName);
            }
        }
        else
        {
            Debug.LogError("次のシーン名が設定されていません！");
        }
    }
}
