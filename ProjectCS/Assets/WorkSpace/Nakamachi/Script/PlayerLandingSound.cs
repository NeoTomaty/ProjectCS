//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/16
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

    //���n�������ǂ����𔻒肷�邽�߂̃t���O
    private bool HasLanded = false;

    void Start()
    {
        //AudioSource�R���|�[�l���g���擾
        AudioSource = GetComponent<AudioSource>();
    }

    //�Փ˂����������Ƃ��̏���
    void OnCollisionEnter(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g��Ground�^�O�������Ă��āA�܂����n���Ă��Ȃ��Ƃ�
        if (collision.gameObject.CompareTag("Ground") && !HasLanded)
        {
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
}
