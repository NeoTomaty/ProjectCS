//TitleSceneLooper.cs
//作成者:中町雷我
//最終更新日:2025/06/24
//アタッチ:各タイトルシーンにアタッチ
//[Log]
//06/24　中町　タイトルシーンの切り替え処理

using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleSceneLooper : MonoBehaviour
{
    public float Delay = 10.0f;             // シーンを切り替えるまでの待ち時間
    public string[] SceneNames;            // タイトルシーンの名前配列

    [SerializeField]
    private GameObject optionUI;           // オプションUI（アクティブ状態を見る）

    private void Start()
    {
        // 指定秒後にLoadNextSceneを実行
        Invoke("LoadNextScene", Delay);
    }

    private void LoadNextScene()
    {
        // オプションが開いている場合はシーン遷移しない（再スケジュール）
        if (optionUI != null && optionUI.activeSelf)
        {
            // オプションを閉じるまで再チェックを待機（再Invoke）
            Invoke("LoadNextScene", 1.0f); // 1秒後にもう一度確認
            return;
        }

        // 現在のシーン名を取得
        string currentScene = SceneManager.GetActiveScene().name;

        // 現在のシーンが配列の何番目か
        int currentIndex = System.Array.IndexOf(SceneNames, currentScene);

        // 次のインデックスを計算（最後なら最初に戻る）
        int nextIndex = (currentIndex + 1) % SceneNames.Length;

        // 次のシーンを読み込む
        SceneManager.LoadScene(SceneNames[nextIndex]);
    }
}