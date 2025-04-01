//====================================================
// スクリプト名：PlayerSpeedManager
// 作成者：高下
// 内容：プレイヤーの速度管理
// 最終更新日：03/31
// 
// [Log]
// 03/31 高下 スクリプト作成 
// 04/01 宮林　SetAccelerationValueをpublicに変更
// 04/01 宮林  SetDecelerationValueを追加
//====================================================
using UnityEngine;

public class PlayerSpeedManager : MonoBehaviour
{
    [SerializeField]
    private float PlayerSpeed = 100.0f; // プレイヤーの速度値

    // PlayerSpeed取得
    public float GetPlayerSpeed => PlayerSpeed;

    // プレイヤーの速度の加算関数
    public void SetAccelerationValue(float AccelerationValue)
    {
        PlayerSpeed += AccelerationValue;
        Debug.Log("PlayerSpeed速度加算値：" + AccelerationValue);
    }
    public void SetDecelerationValue(float DecelerationValue)
    {
        PlayerSpeed -= DecelerationValue;
        Debug.Log("PlayerSpeed速度減算値：" + DecelerationValue);
    }
}
