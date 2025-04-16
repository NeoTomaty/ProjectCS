//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/16
//[Log]
//04/15�@�����@Player���W�����v�����Ƃ��̌��ʉ�����
//04/16�@�����@�W�����v�������񂾂��炷�����ɏC��

using UnityEngine;

public class PlayerJumpSound : MonoBehaviour
{
    public AudioClip JumpSound;
    private AudioSource AudioSource;
    private bool IsGrounded = true;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && IsGrounded)
        {
            IsGrounded = false;
            PlayJumpSound();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }
    }

    void PlayJumpSound()
    {
        AudioSource.PlayOneShot(JumpSound);
    }
}
