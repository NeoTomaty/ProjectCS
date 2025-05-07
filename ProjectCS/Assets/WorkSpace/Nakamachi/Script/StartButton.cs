//StartButton.cs
//作成者:中町雷我
//最終更新日:2025/05/06
//アタッチ:StartButtonにアタッチ
//[Log]
//05/06　中町　StartButtonをクリックしたらステージセレクトシーンに遷移する処理

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    //シーンの名前を設定する変数
    public string SceneName;

    //スタートボタンがクリックされたときの関数
    public void OnStartButtonClicked()
    {
        Debug.Log("スタートボタンをクリックした");

        //シーンをロード
        SceneManager.LoadScene(SceneName);
    }
}