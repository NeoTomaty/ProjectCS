//======================================================
// SceneChangeButton
// 作成者：森脇
// 最終更新日：4/17
//
// [Log]4/16 森脇　シーンチェンジのボタンスクリプト作成
//======================================================

using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeButton : MonoBehaviour
{
    // 遷移先のシーン名（Inspectorで設定可能）
    [SerializeField] private string sceneName;

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}