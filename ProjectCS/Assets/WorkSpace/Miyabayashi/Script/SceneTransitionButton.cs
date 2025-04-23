//======================================================
// SceneTransitionButtonスクリプト
// 作成者：宮林
// 最終更新日：4/22
// 
// [Log]4/22 宮林　ボタンを押したときにシーン遷移する
//
//======================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    // インスペクタビューから設定するシーン名
    public string sceneName;

    
    //シーン遷移
    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }



}
