//======================================================
// CheckClearCountスクリプト
// 作成者：宮林
// 最終更新日：4/11
// 
// [Log]4/11 宮林　クリア条件の追加
//======================================================
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckClearCount : MonoBehaviour
{
    [Header("クリア条件")]
    public int clearCondition = 10; // クリア条件のリフティング回数

    [Header("遷移先シーン")]
    public string nextSceneName; // インスペクターで設定可能な遷移先シーン名

    private int currentCount; // 現在のリフティング回数（外部から受け取る）

    // 外部からリフティング回数を設定するメソッド
    public void SetLiftingCount(int count)
    {
        currentCount = count;
        Debug.Log("Current Lifting Count: " + currentCount);

        // クリア条件を満たしているかチェック
        if (currentCount >= clearCondition)
        {
            TriggerSceneTransition();
        }
    }

    // シーン遷移を実行
    private void TriggerSceneTransition()
    {
        if (!string.IsNullOrEmpty(nextSceneName)) // シーン名が設定されている場合のみ実行
        {
            Debug.Log("Loading Scene: " + nextSceneName);
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("次のシーン名が設定されていません！");
        }
    }
}
