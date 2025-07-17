//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/06/05
//[Log]
//04/15�@�����@Player�����n�����Ƃ��̌��ʉ�����
//04/16�@�����@���n�������񂾂��炷�����ɏC��
//06/05�@�r��@�Đ������𑼂̃X�N���v�g�ōs����悤�ɕύX

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{
    //���n����AudioClip��ݒ肷�邽�߂̕ϐ�
    public AudioClip LandingSound;
    [SerializeField] private float SeVolume = 0.5f;

    //AudioSource�R���|�[�l���g��ێ����邽�߂̕ϐ�
    private AudioSource AudioSource;

    void Start()
    {
        //AudioSource�R���|�[�l���g���擾
        AudioSource = GetComponent<AudioSource>();
    }

    //���n�����Đ����郁�\�b�h
    public void PlayLandingSound()
    {
        //���n�����Đ�
        AudioSource.PlayOneShot(LandingSound, SeVolume);
    }
}
