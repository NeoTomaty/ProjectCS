//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/16
//[Log]
//04/15�@�����@Player�����n�����Ƃ��̌��ʉ�����
//04/16�@�����@���n�������񂾂��炷�����ɏC��

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{
<<<<<<< HEAD

=======
>>>>>>> origin/Nakamachi
    //���n����AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip LandingSound;

    //AudioSource�R���|�[�l���g��ێ����邽�߂̕ϐ�
    private AudioSource AudioSource;

<<<<<<< HEAD
    //�v���C���[�����n�������ǂ����𔻒肷�邽�߂̕ϐ�
=======
    //���n�������ǂ����𔻒肷�邽�߂̃t���O
>>>>>>> origin/Nakamachi
    private bool HasLanded = false;

    void Start()
    {
        //AudioSource�R���|�[�l���g���擾
        AudioSource = GetComponent<AudioSource>();
    }

<<<<<<< HEAD
    //�v���C���[���n�ʂɒ��n�����Ƃ��ɌĂ΂�郁�\�b�h
=======
    //�Փ˂����������Ƃ��̏���
>>>>>>> origin/Nakamachi
    void OnCollisionEnter(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g��Ground�^�O�������Ă��āA�܂����n���Ă��Ȃ��Ƃ�
        if (collision.gameObject.CompareTag("Ground") && !HasLanded)
        {
<<<<<<< HEAD
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
=======
            //���̃I�u�W�F�N�g��Player�^�O�������Ă���ꍇ
            if (gameObject.CompareTag("Player"))
            {
                //���n�����Đ�
                AudioSource.PlayOneShot(LandingSound);

                //���n�����Ƃ�
                HasLanded = true;
            }
        }
    }

    //�Փ˂��I�������Ƃ��̏���
    void OnCollisionExit(Collision collision)
    {
        //�Փ˂��I�������I�u�W�F�N�g��Ground�^�O�������Ă���Ƃ�
        if (collision.gameObject.CompareTag("Ground"))
        {
            //���n�t���O�����Z�b�g
            HasLanded = false;
        }
    }
>>>>>>> origin/Nakamachi
}
