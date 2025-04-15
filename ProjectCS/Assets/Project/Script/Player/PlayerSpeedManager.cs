//====================================================
// スクリプト名：PlayerSpeedManager
// 作成者：高下
// 内容：プレイヤーの速度管理
// 最終更新日：04/13
// 
// [Log]
// 03/31 高下 スクリプト作成 
// 04/01 荒井 SetAccelerationValue関数をprivateからpublicに変更
// 04/01 竹内 SetDecelerationValue関数を追加
// 04/13 高下 GetSpeedRatio関数追加
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

    // プレイヤーの速度の加算関数
    public void SetAccelerationValue(float AccelerationValue)
    {
        PlayerSpeed += AccelerationValue;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // 速度を制限
    }

    // プレイヤーの速度の減算関数
    public void SetDecelerationValue(float DecelerationValue)
    {
        PlayerSpeed -= DecelerationValue;
        PlayerSpeed = Mathf.Clamp(PlayerSpeed, MinPlayerSpeed, MaxPlayerSpeed); // 速度を制限
    }

    // プレイヤーの速度割合を取得
    public float GetSpeedRatio()
    {
        return Mathf.Clamp01((PlayerSpeed - MinPlayerSpeed) / (MaxPlayerSpeed - MinPlayerSpeed));
    }
}
