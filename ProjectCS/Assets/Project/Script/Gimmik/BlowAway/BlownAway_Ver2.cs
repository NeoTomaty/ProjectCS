//====================================================
// スクリプト名：BlownAway_Ver2
// 作成者：藤本
// 
// [Log]
// 04/13 高下 OnCollisionEnter内で飛ばし方を修正
// 04/21 高下 重力関連を別スクリプト(ObjectGravity)に移動させました
// 04/24 藤本 ポイント計算を開始するためのコードを追記しました
// 04/28 竹内 SweetSizeUpが関与する箇所を抹消
// 04/29 荒井 吹っ飛ばしにジャンプのパワーによる補正を追加
//====================================================
using UnityEngine;

public class BlownAway_Ver2 : MonoBehaviour
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
    private float MinForwardForce = 0.0f;// 前方方向の最小力
    [SerializeField]
    private float MaxForwardForce = 0.0f;// 前方方向の最大力
    [SerializeField]
    private float MinRandomXYRange = 0.0f; // ランダムに加えるXY軸の範囲（最小）
    [SerializeField]
    private float MaxRandomXYRange = 0.0f; // ランダムに加えるXY軸の範囲（最大）

    [SerializeField]
    private Transform RespawnArea;         // 移動させる範囲オブジェト

    [SerializeField]
    private LiftingJump LiftingJump; // リフティングジャンプのスクリプト

    private FallPointCalculator FallPoint; // 落下地点を計算するスクリプト

    private Rigidbody Rb;



    void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();
    }

    private void Update()
    {
        if (transform.position.y > 70f)
        {
            MoveToRandomXZInRespawnArea();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        // 飛ぶ先の位置を決定
        //MoveToRandomXZInRespawnArea();

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
        float ScaleRatio = transform.localScale.x;

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
        //Vector3 ForceDirection = UpwardDirection + RandomDirection + ForwardForce;

        // snackの座標のログ
        Debug.Log($"snackの座標: {transform.position}");

        Vector3 ForceDirection = UpwardDirection;

        // Rigidbodyに力を加える
        Rb.AddForce(ForceDirection, ForceMode.Impulse);

        // ゲージによる補正
        if (LiftingJump != null)
        {
            ForceDirection *= LiftingJump.GetForce * LiftingJump.GetJumpPower;
            // プレイヤーのリフティングパートを終了する
            LiftingJump.FinishLiftingJump();
        }

        // ポイント計算開始
        FlyingPoint flyingPoint = GetComponent<FlyingPoint>();
        if (flyingPoint != null)
        {
            flyingPoint.Launch(collision.transform); // プレイヤーを渡す
        }

        // ヒットストップを開始する
        StartCoroutine(HitStop(Mathf.Lerp(MinHitStopTime, MaxHitStopTime, SpeedRatio)));
    }

    // ヒットストップ関数
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

    }

    // リスポーン位置
    private void MoveToRandomXZInRespawnArea()
    {
        if (RespawnArea == null)
        {
            Debug.LogWarning("RespawnAreaが設定されていません");
            return;
        }

        // RespawnAreaの範囲（中心＋スケール）を使ってXZ座標をランダムに決定
        Vector3 center = RespawnArea.position;
        Vector3 size = RespawnArea.localScale;

        float randomX = Random.Range(center.x - size.x / 2, center.x + size.x / 2);
        float randomZ = Random.Range(center.z - size.z / 2, center.z + size.z / 2);

        // Y座標はRespawnAreaの高さまたは任意の高さに設定
        float y = center.y;

        transform.position = new Vector3(randomX, y, randomZ);
        Rb.linearVelocity = Vector3.zero; // 速度もリセット

        Debug.LogWarning("高さ10f");

        // 落下地点を計算
        FallPoint.CalculateGroundPoint();

    }
}