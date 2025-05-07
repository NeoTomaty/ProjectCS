//QuitButton.cs
//作成者:中町雷我
//最終更新日:2025/05/06
//アタッチ:QuitButtonにアタッチ
//[Log]
//05/06　中町　QuitButtonをクリックしたらゲームを終了する処理

using UnityEngine;

public class QuitButton : MonoBehaviour
{
    //終了ボタンがクリックされたときの関数
    public void OnQuitButtonClicked()
    {
        Debug.Log("終了ボタンがクリックされた");

        //Unityエディタで実行中かどうかを確認
        #if UNITY_EDITOR

        //Unityエディタで実行中のとき、プレイモードを終了
        UnityEditor.EditorApplication.isPlaying = false;
        #else

        //ビルドされたアプリケーションのとき、アプリケーション終了
        Application.Quit();
        #endif
    }
}