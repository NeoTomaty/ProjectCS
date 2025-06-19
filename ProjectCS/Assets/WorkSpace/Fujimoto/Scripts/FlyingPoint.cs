
//======================================================
// FlyingPointスクリプト
// 作成者：藤本
// 最終更新日：6/19
//
// [Log]
// 4/22 藤本　飛距離に応じたポイント計算実装
// 5/09 藤本　スコアを計算を（チャージジャンプ＋QTEゲージ）×プレイヤーの速度＝スコアに変更
// 5/24 荒井　コンボボーナスを追加
// 6/19 荒井　スコア表示の更新処理を追加
//======================================================
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyingPoint : MonoBehaviour
{
    [Header("必要な参照")]
    [SerializeField] private LiftingJump LiftingJump;
    [SerializeField] private GaugeController GaugeController;
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

    public void ResetComboBonus()
    {
        ComboBonusRate = 1.0f; // コンボボーナス倍率をリセット
        Debug.Log("コンボボーナスをリセットしました");
    }

    // スコアを計算する関数
    public void CalculateScore()
    {
        if (LiftingJump == null || GaugeController == null || PlayerSpeed == null)
        {
            Debug.LogWarning("PlayerScore：参照が不足しています");
            return;
        }

        float jumpPower = LiftingJump.GetJumpPower;
        float rawValue = GaugeController.GetGaugeValue;
        int gaugeScore = Mathf.Clamp(Mathf.CeilToInt(rawValue * 10f), 1, 10); // ゲージのポイントを1～10にする
        float speed = PlayerSpeed.GetPlayerSpeed;

        // スコア計算
        float rawScore = jumpPower * speed;
        rawScore *= ComboBonusRate; // コンボボーナス
        Score = Mathf.Floor(rawScore); // 小数点以下を切り捨て
        totalScore += Score;

        // テキストUI更新
        if (ScoreUI != null)
        {
            ScoreUI.text = "Score: " + totalScore.ToString();
        }

        Debug.Log($"スコア加算: {jumpPower} × {speed} = {Score} → 合計: {totalScore}");
        Debug.Log($"コンボボーナス: {ComboBonusRate}");

        // コンボボーナスの更新
        ComboBonusRate += ComboBonusUpValue;
    }
}
