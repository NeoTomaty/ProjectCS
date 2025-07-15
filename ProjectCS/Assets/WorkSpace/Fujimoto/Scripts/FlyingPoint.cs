
//======================================================
// FlyingPointスクリプト
// 作成者：藤本
// 最終更新日：6/20
//
// [Log]
// 4/22 藤本　飛距離に応じたポイント計算実装
// 5/09 藤本　スコアを計算を（チャージジャンプ＋QTEゲージ）×プレイヤーの速度＝スコアに変更
// 5/24 荒井　コンボボーナスを追加
// 6/19 荒井　スコア表示の更新処理を追加
// 6/20 荒井　スコア減少処理を追加
// 7/14 高下　スコアの計算の仕方を変更
//======================================================
using UnityEngine;
using UnityEngine.UI;

public class FlyingPoint : MonoBehaviour
{
    [Header("必要な参照")]
    [SerializeField] private LiftingJump LiftingJump;
    [SerializeField] private PlayerSpeedManager PlayerSpeed;
    [SerializeField] private Text ScoreUI;

    [Header("スコア出力")]
    [SerializeField] private float Score;

    [Header("最終スコア（加算式）")]
    [SerializeField] private float totalScore;
    public float TotalScore => totalScore;

    public float GetScore() => Score;

    [Header("コンボボーナス設定")]
    [SerializeField] private float ComboBonusUpValue = 0.2f; // コンボボーナスの増加量
    private float ComboBonusRate = 1.0f; // コンボボーナス倍率
    [Header("ベースとなるスコア増加量")]
    [SerializeField] private float BaseScore = 100;

    public void ResetComboBonus()
    {
        ComboBonusRate = 1.0f; // コンボボーナス倍率をリセット
        Debug.Log("コンボボーナスをリセットしました");
    }

    public void DecreaseScore(float score)
    {
        totalScore -= score;
        if(totalScore < 0)
        {
            totalScore = 0;
        }
        if (ScoreUI != null)
        {
            ScoreUI.text = "Score: " + totalScore.ToString();
        }
        Debug.Log($"PlayerScore：放置ペナルティによりスコア -{score}");
    }

    // スコアを計算する関数
    public void CalculateScore()
    {
        if (LiftingJump == null|| PlayerSpeed == null)
        {
            Debug.LogWarning("PlayerScore：参照が不足しています");
            return;
        }

        float jumpPower = LiftingJump.GetJumpPower;
        float speedRatio = PlayerSpeed.GetSpeedRatio();

        // スコア計算
        float rawScore = jumpPower * (speedRatio * BaseScore);
        rawScore *= ComboBonusRate; // コンボボーナス
        Score = Mathf.Floor(rawScore); // 小数点以下を切り捨て
        totalScore += Score;

        if (totalScore > 99999)
        {
            totalScore = 99999;
        }

        // テキストUI更新
        if (ScoreUI != null)
        {
            ScoreUI.text = "Score: " + totalScore.ToString();
        }

        Debug.Log($"スコア加算: {jumpPower} × ({speedRatio} × {BaseScore}) = {Score} → 合計: {totalScore}");
        Debug.Log($"コンボボーナス: {ComboBonusRate}");

        // コンボボーナスの更新
        ComboBonusRate += ComboBonusUpValue;
    }
}
