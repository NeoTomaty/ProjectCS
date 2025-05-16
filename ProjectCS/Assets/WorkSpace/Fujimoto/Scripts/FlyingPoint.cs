
//======================================================
// FlyingPointスクリプト
// 作成者：藤本
// 最終更新日：4/22
//
// [Log]
// 4/22 藤本　飛距離に応じたポイント計算実装
// 5/09 藤本　スコアを計算を（チャージジャンプ＋QTEゲージ）×プレイヤーの速度＝スコアに変更
//======================================================
using System.Collections.Generic;
using UnityEngine;

public class FlyingPoint : MonoBehaviour
{
    [Header("必要な参照")]
    [SerializeField] private LiftingJump LiftingJump;
    [SerializeField] private GaugeController GaugeController;
    [SerializeField] private PlayerSpeedManager PlayerSpeed;

    [Header("スコア出力")]
    [SerializeField] private float Score;

    [Header("最終スコア（加算式）")]
    [SerializeField] private float totalScore;
    public float TotalScore => totalScore;

    public float GetScore() => Score;

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
        Score = Mathf.Floor(rawScore); // 小数点以下を切り捨て
        totalScore += Score;

        Debug.Log($"スコア加算: {jumpPower} +  × {speed} = {Score} → 合計: {totalScore}");
    }
}
