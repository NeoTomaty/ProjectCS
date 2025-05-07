//OptionButton.cs
//作成者:中町雷我
//最終更新日:2025/05/07
//アタッチ:OptionButtonにアタッチ
//[Log]
//05/07　中町　OptionButtonをクリックしたらオプションシーンに遷移する処理

using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionButton : MonoBehaviour
{
    //シーンの名前を設定する変数
    public string SceneName;

    //オプションボタンがクリックされたときの関数
    public void OnOptionButtonClicked()
    {
        Debug.Log("オプションボタンをクリックした");

        //シーンをロード
        SceneManager.LoadScene(SceneName);
    }
}