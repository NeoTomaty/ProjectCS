//====================================================
// スクリプト名：PlayerSpeedManager
// 作成者：高下
// 内容：プレイヤーの速度管理
// 最終更新日：06/27
// 
// [Log]
// 03/31 高下 スクリプト作成 
// 04/01 荒井 SetAccelerationValue関数をprivateからpublicに変更
// 04/01 竹内 SetDecelerationValue関数を追加
// 04/13 高下 GetSpeedRatio関数追加
// 04/27 荒井 SetOverAccelerationValue関数を追加
// 05/03 荒井 SetSpeed関数とSetOverSpeed関数を追加
// 05/03 荒井 SetOverAccelerationValue関数を削除
// 05/08 高下 GetMaxSpeedとGetMinSpeed関数追加
// 05/22 中町 加速音SE実装
// 06/27 中町 加速音SE音量調整実装
//====================================================
using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 100.0f;    // プレイヤーの速度値
    [SerializeField]
    private float MaxPlayerSpeed = 500.0f; // プレイヤーの速度最大値
    [SerializeField]
    private float MinPlayerSpeed = 120.0f; // プレイヤーの速度最小値

    // PlayerSpeed取得
    public float GetPlayerSpeed => PlayerSpeed;

    //効果音を再生するためのAudioSource(インスペクターで設定)
    [SerializeField] private AudioSource audioSource;

    //加速時に再生する効果音(インスペクターで設定)
    [SerializeField] private AudioClip AccelerationSE;

    //効果音の音量(0.0〜1.0)
    [Range(0.0f, 1.0f)]
    [SerializeField] private float SEVolume = 1.0f;

    // プレイヤーの速度の加算関数
    public void SetAccelerationValue(float AccelerationValue)
    {
        //加速前の速度を保存
        float PreviousSpeed = PlayerSpeed;

        PlayerSpeed += AccelerationValue;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // 速度を制限

        //実際に加速したときのみ効果音を再生
        if(PlayerSpeed > PreviousSpeed && AccelerationSE != null && audioSource != null)
        {
            //音量を設定
            audioSource.PlayOneShot(AccelerationSE, SEVolume);
        }
    }

    // プレイヤーの速度の減算関数
    public void SetDecelerationValue(float DecelerationValue)
    {
        PlayerSpeed -= DecelerationValue;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // 速度を制限
    }

    // プレイヤーの速度を設定する関数
    public void SetSpeed(float Speed)
    {
        PlayerSpeed = Speed;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // 速度を制限
    }

    // プレイヤーの速度に限界を超えた値を設定する関数
    public void SetOverSpeed(float Speed)
    {
        PlayerSpeed = Speed;
    }

    // プレイヤーの速度割合を取得
    public float GetSpeedRatio()
    {
        return Mathf.Clamp01((PlayerSpeed - MinPlayerSpeed) / (MaxPlayerSpeed - MinPlayerSpeed));
    }

    // プレイヤーの最大速度取得
    public float GetMaxSpeed()
    {
        return MaxPlayerSpeed;
    }

    // プレイヤーの最小速度取得
    public float GetMinSpeed()
    {
        return MinPlayerSpeed;
    }
}
