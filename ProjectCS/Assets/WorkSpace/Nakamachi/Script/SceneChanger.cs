using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneChanger : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //Xbox�R���g���[���[��A�{�^���������ꂽ���ǂ������`�F�b�N
        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            //TestGameScene�Ɉړ�
            SceneManager.LoadScene("TestGameScene");
        }
    }
}
