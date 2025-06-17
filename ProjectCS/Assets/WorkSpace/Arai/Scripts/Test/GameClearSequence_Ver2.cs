//======================================================
// [GameClearSequence_Ver2]
// 作成者：荒井修
// 最終更新日：06/13
// 
// [Log]
// 06/13　荒井　スナックの中身が飛び散るエフェクトの再生処理を実装
// 06/13　荒井　パーティクルの射出速度と回転速度をランダムに設定する処理を追加
//======================================================
using UnityEngine;

// 新しいクリア演出
public class GameClearSequence_Ver2 : MonoBehaviour
{
    [Header("エフェクトの設定")]
    [SerializeField] GameObject SnackEffect;
    [SerializeField] float EffectSize = 1.0f;

    [Header("パーティクルのメッシュ")]
    [SerializeField] Mesh ParticleMesh;
    [SerializeField] Material ParticleMaterial;

    [Header("パーティクルのパラメータ")]
    [SerializeField] float Size = 1.0f;
    [SerializeField] float SpeedMIN = 0.5f;
    [SerializeField] float SpeedMAX = 1.5f;
    [SerializeField] float RotateSpeedMIN = 30.0f;
    [SerializeField] float RotateSpeedMAX = 200.0f;

    // クリア演出開始
    public void Play()
    {
        if (SnackEffect == null || ParticleMesh == null || ParticleMaterial == null) return;

        // エフェクト生成
        GameObject Effect = Instantiate(SnackEffect, new Vector3(0f, 0f, 0f), Quaternion.identity);

        // エフェクトサイズ設定
        Effect.transform.localScale = new Vector3(EffectSize, EffectSize, EffectSize);

        // パーティクルメッシュ設定
        ParticleSystem PS = Effect.GetComponent<ParticleSystem>();
        var PSRenderer = PS.GetComponent<ParticleSystemRenderer>();
        PSRenderer.mesh = ParticleMesh;
        PSRenderer.material = ParticleMaterial;

        // パーティクルパラメータ設定
        var PSMain = PS.main;
        // サイズ
        PSMain.startSize = Size;

        // 射出速度
        float min = SpeedMIN;
        float max = SpeedMAX;
        PSMain.startSpeed = new ParticleSystem.MinMaxCurve(min, max);

        // 回転速度
        var Rotation = PS.rotationOverLifetime;
        min = RotateSpeedMIN * Mathf.Deg2Rad;
        max = RotateSpeedMAX * Mathf.Deg2Rad;
        Rotation.x = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.y = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.z = new ParticleSystem.MinMaxCurve(min, max);

        // エフェクト再生
        PS.Play();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
