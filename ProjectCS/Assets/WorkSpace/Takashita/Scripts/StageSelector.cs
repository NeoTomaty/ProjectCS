//====================================================
// スクリプト名：StageSelector
// 作成者：高下
// 内容：選択中のステージの状態を保持する
// 最終更新日：06/01
// 
// [Log]
// 06/01 高下 スクリプト作成
//====================================================
using UnityEngine;

// 現在選択中のステージ番号を管理するシングルトン
public class StageSelector : MonoBehaviour
{
    // ステージの番号（列挙型）
    public enum SelectStageNumber
    {
        Tutorial,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6,
        Stage7,
        Stage8,
        Stage9,
    }

    // 現在選択中のステージ（static：どのシーンからでも参照可能）
    private static SelectStageNumber SelectStage = SelectStageNumber.Tutorial;

    // 現在のステージ番号（0～5）を int で取得
    private int CurrentIndex => (int)SelectStage;

    // ステージの最大インデックス値（列挙型の要素数 - 1）
    private int MaxIndex => System.Enum.GetValues(typeof(SelectStageNumber)).Length - 1;

    // シングルトンインスタンス（唯一のStageSelector）
    public static StageSelector Instance { get; private set; }

    // シングルトン初期化処理
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // シーンをまたいでも保持される
        }
        else
        {
            Destroy(gameObject); // すでに存在する場合は自動的に破棄
        }
    }

    // ステージ番号を変更する（+1, -1などの相対指定）
    /// <param name="delta">変更する値（例: +1で次のステージ）</param>
    public void SetStageNumber(int delta)
    {
        // 現在のインデックスに delta を加算し、0～最大値の範囲に制限
        int newIndex = Mathf.Clamp(CurrentIndex + delta, 0, MaxIndex);
        SelectStage = (SelectStageNumber)newIndex;

        // デバッグ出力
        Debug.Log("現在のステージ: " + SelectStage);
    }

    // 現在のステージ番号（int）を取得
    /// <returns>ステージ番号（0～5）</returns>
    public int GetStageNumber()
    {
        return (int)SelectStage;
    }
}
