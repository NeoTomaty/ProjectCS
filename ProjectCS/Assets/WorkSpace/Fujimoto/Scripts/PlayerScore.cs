//======================================================
// PlayerScoreスクリプト
// 作成者：森脇
// 最終更新日：4/22
//
// [Log]4/22 藤本　プレイヤーのスコア管理とUIへの反映を行う
//======================================================
using UnityEngine;
using TMPro; // TextMeshPro用の名前空間

public class PlayerScore : MonoBehaviour
{
    // 現在のスコア
    public int Score = 0;

    // スコアを表示するTextMeshProUGUI（インスペクターで設定）
    public TextMeshProUGUI ScoreText;

    void Start()
    {
        // ゲーム開始時にスコア表示を初期化
        UpdateScoreUI();
    }


    // 指定したポイントをスコアに加算
    public void AddScore(int amount)
    {
        Score += amount;

        // スコアのUIを更新
        UpdateScoreUI();

        // デバッグ用ログ
        Debug.Log($"{gameObject.name} の現在スコア: {Score}");
    }

    // UIテキストを最新のスコアに更新
    private void UpdateScoreUI()
    {
        if (ScoreText != null)
        {
            ScoreText.text = $"Score: {Score}";
        }
    }
}
