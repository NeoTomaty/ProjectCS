//SceneChanger.cs
//作成者:中町雷我
//最終更新日:2025/04/11
//[Log]
//04/11　中町　TestResultSceneからTestGameSceneに遷移する処理

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChanger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //XboxコントローラーのAボタンが押されたかどうかをチェック
        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            //TestGameSceneに移動
            SceneManager.LoadScene("TestGameScene");
        }
    }
}
