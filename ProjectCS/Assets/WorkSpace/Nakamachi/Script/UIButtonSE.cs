//SEPlayer.cs
//�쐬��:��������
//�ŏI�X�V��:2025/05/29
//�A�^�b�`:PauseCanvas��TittleButton,StageSelect,Retry,Option�{�^���ɃA�^�b�`
//[Log]
//05/29�@�����@���j���[����SE

using UnityEngine;
using UnityEngine.UI;

//���̃X�N���v�g���A�^�b�`����I�u�W�F�N�g�ɂ͕K��Button�R���|�[�l���g���K�v
[RequireComponent(typeof(Button))]

public class UIButtonSE : MonoBehaviour
{
    //�X�N���v�g�̊J�n���ɌĂяo����郁�\�b�h
    private void Start()
    {
        //���̃I�u�W�F�N�g�ɃA�^�b�`����Ă���Button�R���|�[�l���g���擾���A�N���b�N�C�x���g�Ɍ��ʉ��Đ�������ǉ�
        //SEPlayer�̃C���X�^���X�����݂���΁A�I�����ʉ����Đ�
        GetComponent<Button>().onClick.AddListener(() => { SEPlayer.Instance?.PlaySelectSE(); });
    }
}