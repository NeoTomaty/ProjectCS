//====================================================
// スクリプト名：BlownAway
// 作成者：藤本
// 最終更新日：04/13
// 
// [Log]
// 04/13 高下 OnCollisionEnter内で飛ばし方を修正
// 
// 
//====================================================
using UnityEngine;

public class BlownAway : MonoBehaviour
{
    [SerializeField]
    private float MinHitStopTime = 0.5f;    // ヒットストップ時間（最小）
    [SerializeField]
    private float MaxHitStopTime = 1.0f;    // ヒットストップ時間（最大）
    [SerializeField]
    private float MinUpwardForce = 50.0f;  // 真上への力（最小）
    [SerializeField]
    private float MaxUpwardForce = 200.0f; // 真上への力（最大）
    [SerializeField]
    private float MinForwardForce = 100.0f;// 前方方向の最小力
    [SerializeField]
    private float MaxForwardForce = 300.0f;// 前方方向の最大力
    [SerializeField]
    private float MinRandomXYRange = 0.2f; // ランダムに加えるXY軸の範囲（最小）
    [SerializeField]
    private float MaxRandomXYRange = 1.0f; // ランダムに加えるXY軸の範囲（最大）
    [SerializeField]
    private Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f); // 重力の大きさ

    private Rigidbody Rb;
    private SweetSizeUp SweetSizeUpScript;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();

        // SweetSizeUpスクリプトを取得
        SweetSizeUpScript = GetComponent<SweetSizeUp>();
        if (!SweetSizeUpScript)
        {
            Debug.LogError("SweetSizeUpがアタッチされていません");
        }
    }

    void Update()
    {
        // 重力方向に加速させる
        Rb.AddForce(GravityScale, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
      
        // PlayerSpeedManagerスクリプトを取得
        PlayerSpeedManager PlayerSpeedManager = collision.gameObject.GetComponent<PlayerSpeedManager>();
        if (!PlayerSpeedManager)
        {
            Debug.LogWarning("PlayerSpeedManager取得失敗");
            return;
        }
        // プレイヤーの現在の速度割合を取得
        float SpeedRatio = PlayerSpeedManager.GetSpeedRatio();
        // 現在のお菓子の大きさの割合を取得する
        float ScaleRatio = SweetSizeUpScript.GetScaleRatio();

        // 真上方向に力を加える
        Vector3 UpwardDirection = Vector3.up * Mathf.Lerp(MinUpwardForce, MaxUpwardForce, SpeedRatio);

        // プレイヤーの前方方向を取得（向いている方向）
        Vector3 PlayerForward = collision.transform.forward;
        Vector3 ForwardForce = PlayerForward * Mathf.Lerp(MinForwardForce, MaxForwardForce, ScaleRatio);

        // ランダムなXY方向のベクトルを生成
        float RandomAngle = Random.Range(0.0f, 2.0f * Mathf.PI); // ランダムな角度
        Vector3 RandomDirection = new Vector3(Mathf.Cos(RandomAngle), 0.0f, Mathf.Sin(RandomAngle));

        // ランダム方向ベクトルを指定した距離になるようにスケーリング
        RandomDirection = RandomDirection.normalized * Mathf.Lerp(MinRandomXYRange, MaxRandomXYRange, ScaleRatio);

        // 最終的な力の方向
        Vector3 ForceDirection = UpwardDirection + RandomDirection + ForwardForce;

        // Rigidbodyに力を加える
        Rb.AddForce(ForceDirection, ForceMode.Impulse);

        // ヒットストップを開始する
        StartCoroutine(HitStop(Mathf.Lerp(MinHitStopTime, MaxHitStopTime, SpeedRatio)));
    }

    System.Collections.IEnumerator HitStop(float speed)
    {
        Time.timeScale = 0f;

        float normalizedSpeed = Mathf.InverseLerp(1f, 20f, speed);
        float duration = Mathf.Lerp(0.5f, MaxHitStopTime, normalizedSpeed);

        Vector3 originalPosition = transform.localPosition;
        float timer = 0f;

        while (timer < duration)
        {
            float shakeStrength = 0.05f;
            float offsetX = Random.Range(-shakeStrength, shakeStrength);
            float offsetY = Random.Range(-shakeStrength, shakeStrength);

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        Time.timeScale = 1f;

        SweetSizeUpScript.ScaleUpSweet(); // お菓子のスケールアップ
    }
}
