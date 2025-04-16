//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/04/15
//[Log]
//04/15　中町　Playerが着地したときの効果音処理
//04/16　中町　着地したら一回だけ鳴らす処理に修正

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
