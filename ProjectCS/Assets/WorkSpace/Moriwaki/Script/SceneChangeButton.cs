//======================================================
// SceneChangeButton
// 作成者：森脇
// 最終更新日：5/07
//
// [Log]4/16 森脇　シーンチェンジのボタンスクリプト作成
// [Log]5/07 森脇　シーンチェンジのボタンスクリプトフェード対応
//======================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    [SerializeField] private string sceneName;      // 遷移先シーン名
    [SerializeField] private FadeController fadeController;  // FadeControllerを参照

    public void ChangeScene()
    {
        // フェードアウトを開始し、完了後にシーンをロード
        fadeController.FadeOut(() =>
        {
            SceneManager.LoadScene(sceneName);
        });
    }
}