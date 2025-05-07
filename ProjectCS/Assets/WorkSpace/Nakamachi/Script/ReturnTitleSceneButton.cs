//ReturnTitleSceneButton.cs
//作成者:中町雷我
//最終更新日:2025/05/07
//アタッチ:ReturnTitleSceneButtonにアタッチ
//[Log]
//05/07　中町　ReturnTitleSceneButtonをクリックしたらオプションシーンに遷移する処理

using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnTitleSceneButton : MonoBehaviour
{
    //シーンの名前を設定する変数
    public string SceneName;

    //リターンボタンがクリックされたときの関数
    public void OnReturnTitleSceneButtonClicked()
    {
        Debug.Log("リターンボタンをクリックした");

        //シーンをロード
        SceneManager.LoadScene(SceneName);
    }
}