//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/04/16
//[Log]
//04/15　中町　Playerがジャンプしたときの効果音処理
//04/16　中町　ジャンプしたら一回だけ鳴らす処理に修正

using UnityEngine;

public class PlayerJumpSound : MonoBehaviour
{
    //ジャンプ音のAudioClipを設定するための変数
    public AudioClip JumpSound;

    //AudioSourceコンポーネントを保持するための変数
    private AudioSource AudioSource;

    //プレイヤーが地面にいるかどうかを判定するための変数
    private bool IsGrounded = true;

    void Start()
    {
        //AudioSourceコンポーネントを取得
        AudioSource = GetComponent<AudioSource>();
    }

    //プレイヤーが地面から離れたときに呼ばれるメソッド
    void OnCollisionExit(Collision collision)
    {
        //衝突したオブジェクトがGroundタグを持っていて、プレイヤーが地面にいたとき
        if (collision.gameObject.CompareTag("Ground") && IsGrounded)
        {
            //プレイヤーが地面から離れたとき
            IsGrounded = false;

            //ジャンプ音を再生
            PlayJumpSound();
        }
    }

    //プレイヤーが地面に着地したときに呼ばれるメソッド
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトがGroundタグを持っているとき
        if (collision.gameObject.CompareTag("Ground"))
        {
            //プレイヤーが地面にいるとき
            IsGrounded = true;
        }
    }

    //ジャンプ音を再生するメソッド
    void PlayJumpSound()
    {
        //AudioSourceコンポーネントを使用してジャンプ音を再生
        AudioSource.PlayOneShot(JumpSound);
    }
}
