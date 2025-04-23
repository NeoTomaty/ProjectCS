//PlayerMovementSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/17
//[Log]
//04/17�@�����@Player�������Ă���Ƃ��̌��ʉ�����
//04/17�@�����@Player�̃X�s�[�h�̌��ʉ�����

using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    //���鉹�̃I�[�f�B�I�N���b�v
    public AudioClip RunningSound;

    //�X�s�[�h���̃I�[�f�B�I�N���b�v
    public AudioClip SpeedSound;

    //���鉹�p�̃I�[�f�B�I�\�[�X
    private AudioSource RunningAudioSource;

    //�X�s�[�h���p�̃I�[�f�B�I�\�[�X
    private AudioSource SpeedAudioSource;

    //�v���C���[���n�ʂɐG��Ă��邩�ǂ����������t���O
    private bool IsGrounded;

    //�v���C���[�̑��x���擾���邽�߂̃��W�b�h�{�f�B
    private Rigidbody PlayerRigidbody;


    void Start()
    {
        //���鉹�p�̃I�[�f�B�I�\�[�X�R���|�[�l���g��ǉ�
        RunningAudioSource = gameObject.AddComponent<AudioSource>();
        RunningAudioSource.clip = RunningSound;
        RunningAudioSource.loop = true;

        //�X�s�[�h���p�̃I�[�f�B�I�\�[�X�R���|�[�l���g��ǉ�
        SpeedAudioSource = gameObject.AddComponent<AudioSource>();
        SpeedAudioSource.clip = SpeedSound;
        SpeedAudioSource.loop = true;

        //�v���C���[�̃��W�b�h�{�f�B���擾
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        //�G�ꂽ�I�u�W�F�N�g��Ground�^�O�������Ă���Ƃ�
        if (other.CompareTag("Ground"))
        {
            //�n�ʂɐG��Ă���t���O��true�ɐݒ�
            IsGrounded = true;

            //���鉹���Đ�
            RunningAudioSource.Play();
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
            RunningAudioSource.Stop();

            //�X�s�[�h�����~
            SpeedAudioSource.Stop();
        }
    }

    void Update()
    {
        //�n�ʂɐG��Ă��āA�����鉹���Đ�����Ă��Ȃ��Ƃ�
        if (IsGrounded && !RunningAudioSource.isPlaying)
        {
            //���鉹���Đ�
            RunningAudioSource.Play();
        }

        //�n�ʂɐG��Ă��āA���v���C���[�̑��x�����̒l�𒴂��Ă���Ƃ�
        if (IsGrounded && PlayerRigidbody.linearVelocity.magnitude > 5.0f)
        {
            //�X�s�[�h�����Đ�����Ă��Ȃ��Ƃ��ɍĐ�
            if (!SpeedAudioSource.isPlaying)
            {
                SpeedAudioSource.Play();
            }
        }
        else
        {
            //�v���C���[�̑��x�����̒l�ȉ��̂Ƃ��A�X�s�[�h�����~
            if (SpeedAudioSource.isPlaying)
            {
                SpeedAudioSource.Stop();
            }
        }
    }
}
