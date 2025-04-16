//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/04/15
//[Log]
//04/15　中町　Playerが着地したときの効果音処理
//04/16　中町　着地したら一回だけ鳴らす処理に修正

using UnityEngine;

public class PlayerLandingSound : MonoBehaviour
{

    //着地音のAudioClipを設定するための変数
    public AudioClip LandingSound;

    //AudioSourceコンポーネントを保持するための変数
    private AudioSource AudioSource;

    //プレイヤーが着地したかどうかを判定するための変数
    private bool HasLanded = false;

    void Start()
    {
        //AudioSourceコンポーネントを取得
        AudioSource = GetComponent<AudioSource>();
    }

    //プレイヤーが地面に着地したときに呼ばれるメソッド
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトがGroundタグを持っていて、まだ着地していないとき
        if (collision.gameObject.CompareTag("Ground") && !HasLanded)
        {
            //プレイヤーが着地したとき
            HasLanded = true;

            //着地音を再生
            PlayLandingSound();
        }
    }

    //プレイヤーが地面から離れたときに呼ばれるメソッド
    void OnCollisionExit(Collision collision)
    {
        //衝突したオブジェクトがGroundタグを持っているとき
        if (collision.gameObject.CompareTag("Ground"))
        {
            //プレイヤーが地面から離れたとき
            HasLanded = false;
        }
    }

    //着地音を再生するメソッド
    void PlayLandingSound()
    {
        //AudioSourceコンポーネントを使用して着地音を再生
        AudioSource.PlayOneShot(LandingSound);
    }
}
