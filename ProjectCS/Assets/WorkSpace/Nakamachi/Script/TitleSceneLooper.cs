//TitleSceneLooper.cs
//作成者:中町雷我
//最終更新日:2025/06/24
//アタッチ:各タイトルシーンにアタッチ
//[Log]
//06/24　中町　タイトルシーンの切り替え処理

using UnityEngine;
using UnityEngine.SceneManagement;

//タイトルシーンを一定時間ごとに切り替えてループさせるスクリプト
public class TitleSceneLooper : MonoBehaviour
{
    //シーンを切り替えるまでの待ち時間(秒)
    public float Delay = 10.0f;

    //タイトルシーンの名前を順番に格納する配列
    public string[] SceneNames;

    //ゲーム開始時に呼ばれる関数
    private void Start()
    {
        //指定した秒数後にLoadNextScene関数を呼び出す
        Invoke("LoadNextScene", Delay);
    }

    //次のシーンを読み込む処理
    void LoadNextScene()
    {
        //現在アクティブなシーンの名前を取得
        string CurrentScene = SceneManager.GetActiveScene().name;

        //現在のシーンがSceneNames配列の何番目かを取得
        int CurrentIndex = System.Array.IndexOf(SceneNames, CurrentScene);

        //次に読み込みシーンのインデックスを計算(最後のシーンの次は最初に戻る)
        int NextIndex = (CurrentIndex + 1) % SceneNames.Length;

        //次のシーンを読み込む
        SceneManager.LoadScene(SceneNames[NextIndex]);
    }
}