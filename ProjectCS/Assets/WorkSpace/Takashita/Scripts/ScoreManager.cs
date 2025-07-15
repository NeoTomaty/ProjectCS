//====================================================
// スクリプト名：ScoreManager
// 作成者：高下
// 内容：ステージ全体のスコアを管理するスクリプト
// 最終更新日：07/15
// 
// [Log]
// 06/20 高下 スクリプト作成
// 07/15 高下 保存形式をJSONに変更
//====================================================
using System.IO;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int[] StageScores = new int[9];

    private const string FileName = "scores.json";

    [System.Serializable]
    private class ScoreData
    {
        public int[] StageScores;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadJSON();
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
            Debug.Log("エディタ再生中のため、JSON保存はスキップされました");
            return;
        }

        SaveJSON();
    }

    private void LoadJSON()
    {
#if UNITY_STANDALONE
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, FileName);

        if (Application.isEditor && Application.isPlaying)
        {
            Debug.Log("エディタ再生中のため、JSON読み込みはスキップされました");
            return;
        }

        if (!File.Exists(filePath))
        {
            Debug.LogWarning("scores.json が存在しません。新しく作成して初期化します。");
            SaveJSON(); // 初回保存
            return;
        }

        string json = File.ReadAllText(filePath);
        ScoreData data = JsonUtility.FromJson<ScoreData>(json);

        if (data != null && data.StageScores != null)
        {
            int count = Mathf.Min(StageScores.Length, data.StageScores.Length);
            for (int i = 0; i < count; i++)
            {
                StageScores[i] = Mathf.Clamp(data.StageScores[i], 0, 99999);
            }

            Debug.Log("スコアをJSONから読み込みました");
        }
        else
        {
            Debug.LogWarning("JSONの読み込みに失敗しました。初期化します。");
            SaveJSON();
        }
#else
        Debug.LogWarning("この機能はスタンドアロン（PCビルド）専用です。");
#endif
    }

    private void SaveJSON()
    {
#if UNITY_STANDALONE
        string exeFolderPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
        string filePath = Path.Combine(exeFolderPath, FileName);

        ScoreData data = new ScoreData { StageScores = StageScores };
        string json = JsonUtility.ToJson(data, true); // trueで整形（見やすい）

        File.WriteAllText(filePath, json);

        Debug.Log($"スコアをJSONで保存しました: {filePath}");
#else
        Debug.LogWarning("この機能はスタンドアロン（PCビルド）専用です。");
#endif
    }
}
