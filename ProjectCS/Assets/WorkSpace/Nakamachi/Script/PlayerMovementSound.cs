//PlayerMovementSound.cs
//作成者:中町雷我
//最終更新日:2025/04/17
//[Log]
//04/17　中町　Playerが動いているときの効果音処理

using UnityEngine;

public class PlayerMovementSound : MonoBehaviour
{
    //走る音のオーディオクリップ
    public AudioClip RunningSound;

    //オーディオソースコンポーネント
    private AudioSource audioSource;

    //プレイヤーが地面に触れているかどうかを示すフラグ
    private bool IsGrounded;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //オーディオソースコンポーネントを取得
        audioSource = GetComponent<AudioSource>();

        //オーディオソースに走る音のクリップを設定
        audioSource.clip = RunningSound;

        //ループ再生を有効にする
        audioSource.loop = true;
    }

    void OnTriggerEnter(Collider other)
    {
        //触れたオブジェクトがGroundタグを持っているとき
        if (other.CompareTag("Ground"))
        {
            //地面に触れているフラグをtrueに設定
            IsGrounded = true;

            //走る音を再生
            audioSource.Play();
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
            audioSource.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //地面に触れていて、かつ音が再生されていないとき
        if (IsGrounded&&!audioSource.isPlaying)
        {
            //走る音を再生
            audioSource.Play();
        }
    }
}
