//======================================================
// [GenerateSpark]
// 作成者：荒井修
// 最終更新日：04/18
// 
// [Log]
// 04/13　荒井　衝突時に火花を生成するように実装
// 04/13　荒井　プレイヤーの速度が閾値を跨いだ時に色が変わるように実装
// 04/16　荒井　パーティクルのパラメータを設定できるように変更
// 04/17　荒井　パーティクルの継続時間の指定を廃止
// 04/18　荒井　着地との区別方法を変更
//======================================================

using UnityEngine;

// プレイヤーにアタッチ
public class GenerateSpark : MonoBehaviour
{

    [SerializeField] private GameObject SparkPrefab;
    [SerializeField] private PlayerSpeedManager PlayerSpeedManager;

    // パーティクルの設定
    [SerializeField] private float ParticleSpeed = 30.0f;       // パーティクルの速度
    [SerializeField] private float ParticleSize = 0.2f;         // パーティクルのサイズ

    // 色の設定
    [SerializeField] private Color LowSpeedColor = Color.blue;      // 低速時
    [SerializeField] private Color MiddleSpeedColor = Color.yellow; // 中速時
    [SerializeField] private Color HighSpeedColor = Color.red;      // 高速時

    // 速度の閾値
    [SerializeField] private float LowToMiddleSpeedThreshold = 200.0f;  // 低速から中速
    [SerializeField] private float MiddleToHighSpeedThreshold = 400.0f; // 中速から高速

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (SparkPrefab == null) return;

        // 衝突地点の座標と法線を取得
        ContactPoint ContactPoint = collision.contacts[0];
        Vector3 HitPosition = ContactPoint.point;
        Vector3 HitNormal = ContactPoint.normal;

        // 衝突地点の法線が上向きなら着地扱いにする
        if (HitNormal == Vector3.up) return;

        // 火花を生成
        GameObject SparkEffect = Instantiate(SparkPrefab, HitPosition, Quaternion.LookRotation(HitNormal));

        // プレイヤーの速度を取得
        float PlayerSpeed = PlayerSpeedManager.GetPlayerSpeed;

        // 色の設定
        Color SparkColor;
        if (PlayerSpeed < LowToMiddleSpeedThreshold)
        {
            SparkColor = LowSpeedColor; // 低速時の色
        }
        else if (PlayerSpeed < MiddleToHighSpeedThreshold)
        {
            SparkColor = MiddleSpeedColor; // 中速時の色
        }
        else
        {
            SparkColor = HighSpeedColor; // 高速時の色
        }

        // 色を適用
        ParticleSystem.MainModule EffectMainModule = SparkEffect.GetComponent<ParticleSystem>().main;
        EffectMainModule.startSpeed = ParticleSpeed;
        EffectMainModule.startSize = ParticleSize;
        EffectMainModule.startColor = SparkColor;

        // 一定時間後に火花を消す
        Destroy(SparkEffect, 1.0f);
    }
}
