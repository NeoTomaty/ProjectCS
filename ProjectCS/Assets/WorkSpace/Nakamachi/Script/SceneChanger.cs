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
