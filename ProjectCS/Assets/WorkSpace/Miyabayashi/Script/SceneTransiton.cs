//======================================================
// SceneTransitionスクリプト
// 作成者：宮林
// 最終更新日：4/22
// 
// [Log]4/22 宮林　コントローラーのボタンを押したときに遷移する
//
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneTransiton : MonoBehaviour
{

    // インスペクタビューから設定するシーン名
    public string sceneName;
    // Update is called once per frame
    void Update()
    {

        //XboxコントローラーのAボタンが押されたかどうかをチェック
        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }

        //Enterキーが押されたかどうかをチェック
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
