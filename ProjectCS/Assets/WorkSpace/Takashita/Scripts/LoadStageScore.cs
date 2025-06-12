//====================================================
// スクリプト名：LoadStageScore
// 作成者：高下
// 内容：CSVからスコアを読み取り、保持する
// 最終更新日：06/12
// 
// [Log]
// 06/12 高下 スクリプト作成
//====================================================
using UnityEngine;

// CSVファイルからステージごとのスコア（6個分）を読み込んで格納するコンポーネント
public class LoadStageScore : MonoBehaviour
{
    // ステージスコアを格納する配列（最大6個）
    [SerializeField] private int[] StageScores = new int[6];

    // インスペクターからアタッチするCSVファイル（TextAsset）
    [SerializeField] private TextAsset ScoreCSV;

    // ゲーム開始時にCSVを読み込む
    void Start()
    {
        LoadCSV();
    }

    // CSVファイルを読み込み、スコアを StageScores 配列に格納する
    private void LoadCSV()
    {
        // CSVが設定されていない場合はエラーログを出して終了
        if (ScoreCSV == null)
        {
            Debug.LogError("CSVファイルがアタッチされていません。");
            return;
        }

        // 改行で分割して行単位に取り出す（空行は除外）
        string[] lines = ScoreCSV.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

        // 最大で6つまで読み込み
        for (int i = 0; i < Mathf.Min(StageScores.Length, lines.Length); i++)
        {
            // 数値にパースできたら格納（最大値99999まで制限）
            if (int.TryParse(lines[i], out int score))
            {
                StageScores[i] = Mathf.Clamp(score, 0, 99999);
                Debug.Log(StageScores[i]); // デバッグ表示（必要に応じて削除）
            }
            else
            {
                // 数値でない行があった場合は警告ログ
                Debug.LogWarning($"スコアの読み取り失敗: 行{i + 1} = {lines[i]}");
            }
        }
    }

    // 指定されたインデックスのステージスコアを取得
    /// <param name="arrayNum">ステージ番号（0～5）</param>
    /// <returns>スコア（範囲外の場合は -1）</returns>
    public int GetStageScore(int arrayNum)
    {
        if (arrayNum >= StageScores.Length)
            return -1;

        return StageScores[arrayNum];
    }
}
