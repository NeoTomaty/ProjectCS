//using UnityEditor.SceneManagement;
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    public enum SelectStageNumber
    {
        Stage1,
        Stage2,
        Stage3
    }

    private SelectStageNumber SelectStage = SelectStageNumber.Stage1;

    // 現在のステージを数値として取得
    private int CurrentIndex => (int)SelectStage;

    // ステージの最大数（最後のindex）
    private int MaxIndex => System.Enum.GetValues(typeof(SelectStageNumber)).Length - 1;

    public static StageSelector Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 最初の1つだけ残す
        }
        else
        {
            Destroy(gameObject); // 2つ目以降は破棄
        }
    }

    public void SetStageNumber(int delta)
    {
        int newIndex = Mathf.Clamp(CurrentIndex + delta, 0, MaxIndex);
        SelectStage = (SelectStageNumber)newIndex;
        Debug.Log("現在のステージ: " + SelectStage);
    }

    public int GetStageNumber()
    {
        return (int)SelectStage;
    }
}
