//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/04/16
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

    //着地したかどうかを判定するためのフラグ
    private bool HasLanded = false;

    void Start()
    {
        //AudioSourceコンポーネントを取得
        AudioSource = GetComponent<AudioSource>();
    }

    //衝突が発生したときの処理
    void OnCollisionEnter(Collision collision)
    {
        //衝突したオブジェクトがGroundタグを持っていて、まだ着地していないとき
        if (collision.gameObject.CompareTag("Ground") && !HasLanded)
        {
            //このオブジェクトがPlayerタグを持っている場合
            if (gameObject.CompareTag("Player"))
            {
                //着地音を再生
                AudioSource.PlayOneShot(LandingSound);

                //着地したとき
                HasLanded = true;
            }
        }
    }

    //衝突が終了したときの処理
    void OnCollisionExit(Collision collision)
    {
        //衝突が終了したオブジェクトがGroundタグを持っているとき
        if (collision.gameObject.CompareTag("Ground"))
        {
            //着地フラグをリセット
            HasLanded = false;
        }
    }
}
