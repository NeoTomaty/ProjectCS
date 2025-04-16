//PlayerLandingSound.cs
//�쐬��:��������
//�ŏI�X�V��:2025/04/15
//[Log]
//04/15�@�����@Player�����n�����Ƃ��̌��ʉ�����
//04/16�@�����@���n�������񂾂��炷�����ɏC��

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{
    public AudioClip LandingSound;
    private AudioSource AudioSource;
    private bool HasLanded = false;

    void Start()
    {
        AudioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && !HasLanded)
        {
            HasLanded = true;
            PlayLandingSound();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            HasLanded = false;
        }
    }

    void PlayLandingSound()
    {
        AudioSource.PlayOneShot(LandingSound);
    }
}
