//SceneChanger.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/11
//[Log]
//04/11�@�����@TestResultScene����TestGameScene�ɑJ�ڂ��鏈��

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
