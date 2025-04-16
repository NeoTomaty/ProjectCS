//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/04/16
//[Log]
//04/15　中町　Playerがジャンプしたときの効果音処理
//04/16　中町　ジャンプしたら一回だけ鳴らす処理に修正

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
