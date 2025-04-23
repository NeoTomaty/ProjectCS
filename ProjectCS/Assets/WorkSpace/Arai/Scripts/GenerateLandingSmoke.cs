//======================================================
// [GenerateLandingSmoke]
// 作成者：荒井修
// 最終更新日：04/18
// 
// [Log]
// 04/17　荒井　着地時にエフェクトを生成するように実装
// 04/18　荒井　自然な見え方になるようにエフェクトの生成位置を補正するように実装
//======================================================
using UnityEngine;

public class GenerateLandingSmoke : MonoBehaviour
{
    [SerializeField] private GameObject LandingSmokePrefab;
    [SerializeField] private MovePlayer MovePlayer; // プレイヤーの移動スクリプト
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager; // プレイヤーの速度管理スクリプト

    [SerializeField] private float EffectSize = 1.0f; // エフェクトのサイズ

    [Tooltip("エフェクト生成位置の補正倍率")]
    [SerializeField] private float EffectPositionCorrection = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision)
    {
        if (LandingSmokePrefab == null) return;

        // 衝突地点の座標と法線を取得
        ContactPoint ContactPoint = collision.contacts[0];
        Vector3 HitPosition = ContactPoint.point;
        Vector3 HitNormal = ContactPoint.normal;

        // 衝突地点の法線が上向きなら着地扱いにする
        if (HitNormal == Vector3.up)
        {
            // 自然な見え方になるように生成地点を補正
            float CorrectionAmount = PlayerSpeedManager.GetPlayerSpeed * EffectPositionCorrection; // プレイヤーの速度に基づく補正量
            Vector3 SpawnPosition = HitPosition + (MovePlayer.GetMoveDirection * CorrectionAmount);

            // エフェクトを生成
            GameObject LandingSmokeEffect = Instantiate(LandingSmokePrefab, SpawnPosition, Quaternion.LookRotation(HitNormal));

            // エフェクトのサイズを設定
            LandingSmokeEffect.transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

            Destroy(LandingSmokeEffect, 2.0f); // 一定時間後に消す
        }
    }
}
