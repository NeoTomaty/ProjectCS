//======================================================
// [GameClearSequence_Ver2]
// 作成者：荒井修
// 最終更新日：06/13
// 
// [Log]
// 06/13　荒井　スナックの中身が飛び散るエフェクトの再生処理を実装
//======================================================
using UnityEngine;

// 新しいクリア演出
public class GameClearSequence_Ver2 : MonoBehaviour
{
    // エフェクト
    [Header("エフェクトの設定")]
    [SerializeField] GameObject SnackEffect;
    [SerializeField] float EffectSize = 1.0f;

    // パーティクルのメッシュ
    [Header("パーティクルのメッシュ")]
    [SerializeField] Mesh ParticleMesh;
    [SerializeField] Material ParticleMaterial;

    // パーティクルの制御
    [Header("パーティクルのパラメータ")]
    [SerializeField] float Size = 1.0f;
    [SerializeField] float Speed = 1.0f;
    [SerializeField] float RotateSpeedMIN = 30.0f;
    [SerializeField] float RotateSpeedMAX = 200.0f;

    // クリア演出開始
    public void Play()
    {
        // エフェクトを再生
        if (SnackEffect == null) return;

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
        PSMain.startSize = Size;    // サイズ
        PSMain.startSpeed = Speed;  // 射出速度
        var Rotation = PS.rotationOverLifetime; // 回転速度
        float min = RotateSpeedMIN * Mathf.Deg2Rad;
        float max = RotateSpeedMAX * Mathf.Deg2Rad;
        Rotation.x = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.y = new ParticleSystem.MinMaxCurve(min, max);
        Rotation.z = new ParticleSystem.MinMaxCurve(min, max);


        PS.Play();

        // カメラ制御
    }

    // クリア演出終了
    public void End()
    {
        // クリア演出を終了する処理をここに記述
        // 例えば、UIの非表示や次のシーンへの遷移など
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
