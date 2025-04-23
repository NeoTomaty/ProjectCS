//======================================================
// resultChange�X�N���v�g
// �쐬�ҁF�{��
// �ŏI�X�V���F4/22
// 
// [Log]4/22 �{�с@���U���g��ʂ�A�{�^���iEnter�j���������Ƃ���
//                 �{�^�����\�������
//
//======================================================

using UnityEngine;

public class ResultChange : MonoBehaviour
{
    [Header("���j���[UI���[�g")]
    public GameObject menuOverlay;

    private bool menuOpen = false;

    void Update()
    {
        // A�{�^�� or �G���^�[�L�[�ŊJ��
        if (!menuOpen && (Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Return)))
        {
            OpenMenu();
        }
    }


    void OpenMenu()
    {
        menuOverlay.SetActive(true);
        menuOpen = true;
    }
}
