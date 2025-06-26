//====================================================
// スクリプト名：ScoreManager
// 作成者：高下
// 内容：ステージ全体のスコアを管理するスクリプト
// 最終更新日：06/20
// 
// [Log]
// 06/20 高下 スクリプト作成
//====================================================
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    // ステージスコアを格納する配列
    [SerializeField] private int[] StageScores = new int[9];



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadCSV();
    }

    public void SetStageScore(int arrayNum, int score)
    {
        if (arrayNum >= StageScores.Length)
        {
            Debug.LogError("範囲外の数値が渡されました");
            return;
        }

        StageScores[arrayNum] = score;
    }

    public int GetStageScore(int arrayNum)
    {
        if (arrayNum >= StageScores.Length)
            return -1;

        return StageScores[arrayNum];
    }

    private void OnApplicationQuit()
    {
        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("エディタ再生中のため、CSVファイル保存はスキップされました");
            return;
        }
        SaveCSV();
    }

    private void LoadCSV()
    {
#if UNITY_STANDALONE
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, "scores.csv");

        // エディタ実行中ならCSV生成をスキップ
        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("エディタ再生中のため、CSVファイル生成はスキップされました");
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("scores.csv が存在しません。新しく作成して初期化します。");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                for (int i = 0; i < StageScores.Length; i++)
                {
                    StageScores[i] = 0;
                    writer.WriteLine(0);
                }
            }

            Debug.Log($"scores.csv を {filePath} に初期化しました");
            return;
        }

        // 読み込み処理
        string[] lines = File.ReadAllLines(filePath);
        for (int i = 0; i < Mathf.Min(StageScores.Length, lines.Length); i++)
        {
            if (int.TryParse(lines[i], out int score))
            {
                StageScores[i] = Mathf.Clamp(score, 0, 99999);
            }
            else
            {
                StageScores[i] = 0;
                Debug.LogWarning($"行{i + 1} 読み取り失敗 → 0で初期化");
            }
        }

        Debug.Log("スコアを読み込みました");
#else
    Debug.LogWarning("この機能はスタンドアロン（PCビルド）専用です。");
#endif
    }

    private void SaveCSV()
    {
#if UNITY_STANDALONE
        // YourGame_Data フォルダの親が exe のある場所
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, "scores.csv");

        using (StreamWriter writer = new StreamWriter(filePath, false))
        {
            foreach (int score in StageScores)
            {
                writer.WriteLine(score);
            }
        }

        Debug.Log($"exeと同じ場所にスコアを保存しました: {filePath}");
#else
    Debug.LogWarning("この機能はスタンドアロン（PCビルド）専用です。");
#endif
    }
}
