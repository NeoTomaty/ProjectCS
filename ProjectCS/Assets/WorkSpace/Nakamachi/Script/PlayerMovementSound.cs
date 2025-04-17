//PlayerMovementSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/17
//[Log]
//04/17�@�����@Player�������Ă���Ƃ��̌��ʉ�����

using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    //���鉹�̃I�[�f�B�I�N���b�v
    public AudioClip RunningSound;

    //�I�[�f�B�I�\�[�X�R���|�[�l���g
    private AudioSource audioSource;

    //�v���C���[���n�ʂɐG��Ă��邩�ǂ����������t���O
    private bool IsGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�I�[�f�B�I�\�[�X�R���|�[�l���g���擾
        audioSource = GetComponent<AudioSource>();

        //�I�[�f�B�I�\�[�X�ɑ��鉹�̃N���b�v��ݒ�
        audioSource.clip = RunningSound;

        //���[�v�Đ���L���ɂ���
        audioSource.loop = true;
    }

    void OnTriggerEnter(Collider other)
    {
        //�G�ꂽ�I�u�W�F�N�g��Ground�^�O�������Ă���Ƃ�
        if (other.CompareTag("Ground"))
        {
            //�n�ʂɐG��Ă���t���O��true�ɐݒ�
            IsGrounded = true;

            //���鉹���Đ�
            audioSource.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        //���ꂽ�I�u�W�F�N�g��Ground�^�O�������Ă���Ƃ�
        if (other.CompareTag("Ground"))
        {
            //�n�ʂɐG��Ă���t���O��false�ɐݒ�
            IsGrounded = false;

            //���鉹���~
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //�n�ʂɐG��Ă��āA�������Đ�����Ă��Ȃ��Ƃ�
        if (IsGrounded&&!audioSource.isPlaying)
        {
            //���鉹���Đ�
            audioSource.Play();
        }
    }
}
