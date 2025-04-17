//PlayerMovementSound.cs
//作成者:中町雷我
//最終更新日:2025/04/17
//[Log]
//04/17　中町　Playerが走っているときの効果音処理
//04/17　中町　Playerのスピードの効果音処理

using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    //走る音のオーディオクリップ
    public AudioClip RunningSound;

    //スピード音のオーディオクリップ
    public AudioClip SpeedSound;

    //走る音用のオーディオソース
    private AudioSource RunningAudioSource;

    //スピード音用のオーディオソース
    private AudioSource SpeedAudioSource;

    //プレイヤーが地面に触れているかどうかを示すフラグ
    private bool IsGrounded;

    //プレイヤーの速度を取得するためのリジッドボディ
    private Rigidbody PlayerRigidbody;


    void Start()
    {
        //走る音用のオーディオソースコンポーネントを追加
        RunningAudioSource = gameObject.AddComponent<AudioSource>();
        RunningAudioSource.clip = RunningSound;
        RunningAudioSource.loop = true;

        //スピード音用のオーディオソースコンポーネントを追加
        SpeedAudioSource = gameObject.AddComponent<AudioSource>();
        SpeedAudioSource.clip = SpeedSound;
        SpeedAudioSource.loop = true;

        //プレイヤーのリジッドボディを取得
        PlayerRigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        //触れたオブジェクトがGroundタグを持っているとき
        if (other.CompareTag("Ground"))
        {
            //地面に触れているフラグをtrueに設定
            IsGrounded = true;

            //走る音を再生
            RunningAudioSource.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        //離れたオブジェクトがGroundタグを持っているとき
        if (other.CompareTag("Ground"))
        {
            //地面に触れているフラグをfalseに設定
            IsGrounded = false;

            //走る音を停止
            RunningAudioSource.Stop();

            //スピード音を停止
            SpeedAudioSource.Stop();
        }
    }

    void Update()
    {
        //地面に触れていて、かつ走る音が再生されていないとき
        if (IsGrounded && !RunningAudioSource.isPlaying)
        {
            //走る音を再生
            RunningAudioSource.Play();
        }

        //地面に触れていて、かつプレイヤーの速度が一定の値を超えているとき
        if (IsGrounded && PlayerRigidbody.linearVelocity.magnitude > 5.0f)
        {
            //スピード音が再生されていないときに再生
            if (!SpeedAudioSource.isPlaying)
            {
                SpeedAudioSource.Play();
            }
        }
        else
        {
            //プレイヤーの速度が一定の値以下のとき、スピード音を停止
            if (SpeedAudioSource.isPlaying)
            {
                SpeedAudioSource.Stop();
            }
        }
    }
}
