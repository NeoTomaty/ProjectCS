//======================================================
// SceneTransition�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/22
// 
// [Log]4/22 �{�с@�R���g���[���[�̃{�^�����������Ƃ��ɑJ�ڂ���
//
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneTransiton : MonoBehaviour
{

    // �C���X�y�N�^�r���[����ݒ肷��V�[����
    public string sceneName;
    // Update is called once per frame
    void Update()
    {

        //Xbox�R���g���[���[��A�{�^���������ꂽ���ǂ������`�F�b�N
        if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }

        //Enter�L�[�������ꂽ���ǂ������`�F�b�N
        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
