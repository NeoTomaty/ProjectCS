//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/06/05
//[Log]
//04/15�@�����@Player���W�����v�����Ƃ��̌��ʉ�����
//04/16�@�����@�W�����v�������񂾂��炷�����ɏC��
//06/05�@�r��@�Đ������𑼂̃X�N���v�g�ōs����悤�ɕύX
//06/27�@�����@�W�����v��SE���ʒ�������

using UnityEngine;

public class PlayerJumpSound : MonoBehaviour
{
    //�W�����v����AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip JumpSound;

    //AudioSource�R���|�[�l���g��ێ����邽�߂̕ϐ�
    private AudioSource AudioSource;

    [Range(0.0f, 1.0f)]
    public float JumpVolume = 1.0f;

    void Start()
    {
        //AudioSource�R���|�[�l���g���擾
        AudioSource = GetComponent<AudioSource>();
    }

    //�W�����v�����Đ����郁�\�b�h
    public void PlayJumpSound()
    {
        //�w�肳�ꂽ���ʂŃW�����v�����Đ�
        AudioSource.PlayOneShot(JumpSound,JumpVolume);
    }
}
