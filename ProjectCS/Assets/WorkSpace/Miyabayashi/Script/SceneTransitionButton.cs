//======================================================
// SceneTransitionButtonスクリプト
// 作成者：宮林
// 最終更新日：4/25
// 
// [Log]4/22 宮林　ボタンを押したときにシーン遷移する
//　　　4/25 宮林　フェード処理対応
//======================================================
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    // インスペクタビューから設定するシーン名
    public string sceneName;
 
    //FadeManager fade = Object.FindFirstObjectByType<FadeManager>();

    //// シーン遷移
    //public void LoadScene()
    //{
    //    if (fade != null)
    //    {
    //        StartCoroutine(fade.FadeAndLoadScene(sceneName)); // Fade処理を開始し、シーン遷移
    //    }
    //}



   public void StartSceneTransition()
    {
       

        FadeManager fade = Object.FindFirstObjectByType<FadeManager>();
        if (fade != null)
        {
            fade.FadeToScene(sceneName);
        }
        else
        {
            Debug.LogWarning("FadeManager が見つかりませんでした。CanvasにFadeManagerを付けたか確認してね！");
        }
    }
}
