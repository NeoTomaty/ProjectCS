//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/15
//[Log]
//04/15�@�����@Player�����n�����Ƃ��̌��ʉ�����
//04/16�@�����@���n�������񂾂��炷�����ɏC��

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{

    //���n����AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip LandingSound;

    //AudioSource�R���|�[�l���g��ێ����邽�߂̕ϐ�
    private AudioSource AudioSource;

    //�v���C���[�����n�������ǂ����𔻒肷�邽�߂̕ϐ�
    private bool HasLanded = false;

    void Start()
    {
        //AudioSource�R���|�[�l���g���擾
        AudioSource = GetComponent<AudioSource>();
    }

    //�v���C���[���n�ʂɒ��n�����Ƃ��ɌĂ΂�郁�\�b�h
    void OnCollisionEnter(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g��Ground�^�O�������Ă��āA�܂����n���Ă��Ȃ��Ƃ�
        if (collision.gameObject.CompareTag("Ground") && !HasLanded)
        {
            //�v���C���[�����n�����Ƃ�
            HasLanded = true;

            //���n�����Đ�
            PlayLandingSound();
        }
    }

    //�v���C���[���n�ʂ��痣�ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    void OnCollisionExit(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g��Ground�^�O�������Ă���Ƃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            //�v���C���[���n�ʂ��痣�ꂽ�Ƃ�
            HasLanded = false;
        }
    }

    //���n�����Đ����郁�\�b�h
    void PlayLandingSound()
    {
        //AudioSource�R���|�[�l���g���g�p���Ē��n�����Đ�
        AudioSource.PlayOneShot(LandingSound);
    }
}
