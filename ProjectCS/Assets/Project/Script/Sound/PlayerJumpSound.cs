//PlayerLandingSound.cs
//作成者:中町雷我
//最終更新日:2025/06/05
//[Log]
//04/15　中町　Playerがジャンプしたときの効果音処理
//04/16　中町　ジャンプしたら一回だけ鳴らす処理に修正
//06/05　荒井　再生処理を他のスクリプトで行えるように変更

using UnityEngine;

public class PlayerJumpSound : MonoBehaviour
{
    //ジャンプ音のAudioClipを設定するための変数
    public AudioClip JumpSound;

    //AudioSourceコンポーネントを保持するための変数
    private AudioSource AudioSource;

    void Start()
    {
        //AudioSourceコンポーネントを取得
        AudioSource = GetComponent<AudioSource>();
    }

    //ジャンプ音を再生するメソッド
    public void PlayJumpSound()
    {
        //AudioSourceコンポーネントを使用してジャンプ音を再生
        AudioSource.PlayOneShot(JumpSound);
    }
}
