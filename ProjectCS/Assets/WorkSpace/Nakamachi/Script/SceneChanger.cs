//SceneChanger.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/11
//[Log]
//04/11�@�����@TestResultScene����TestGameScene�ɑJ�ڂ��鏈��
//04/15�@�����@Enter�L�[�ł��V�[���J�ڂł���悤�ɂ��鏈���ǉ�

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

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

        //Enter�L�[�������ꂽ���ǂ������`�F�b�N
        if (Keyboard.current!=null&&Keyboard.current.enterKey.wasPressedThisFrame)
        {
            //TestGameScene�Ɉړ�
            SceneManager.LoadScene("TestGameScene");
        }
    }
}
