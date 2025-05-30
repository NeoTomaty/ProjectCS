//====================================================
// スクリプト名：BlownAway_Ver3
// 作成者：藤本
// 内容：リフティング回数に応じて飛ぶ力が段階的に上がる
// [Log]
// 05/13 藤本 リフティング回数に応じて飛ぶ力が段階的に上がる
// 05/30 荒井 スコアのコンボボーナスのリセットを実装
//====================================================
using UnityEngine;
using System.Collections;

public class BlownAway_Ver3 : MonoBehaviour
{
    [Header("ヒットストップ時間")]
    [SerializeField] private float hitStopTime = 0.5f;            // ヒットストップの時間（秒）

    [Header("吹っ飛びの基本設定")]
    [SerializeField] private float baseForce = 100f;      // 初期飛ぶ力
    [SerializeField] private float forcePerLift = 100f;    // リフティング1回ごとに追加する力


    [SerializeField]
    private float MinUpwardForce = 50.0f;  // 真上への力（最小）
    [SerializeField]
    private float MaxUpwardForce = 200.0f; // 真上への力（最大）
   
    [SerializeField]
    private float MinRandomXYRange = 0.0f; // ランダムに加えるXY軸の範囲（最小）
    [SerializeField]
    private float MaxRandomXYRange = 0.0f; // ランダムに加えるXY軸の範囲（最大）

    [SerializeField]
    private float MinFallSpeed = 0.0f; // 落下時のスピート（最小）
    [SerializeField]
    private float MaxFallSpeed = 30.0f; // 落下時のスピート（最大）

    [SerializeField]
    private Transform RespawnArea;         // 移動させる範囲オブジェト

    [SerializeField]
    private LiftingJump LiftingJump; // リフティングジャンプのスクリプト

    [SerializeField]
    private Transform GroundArea;   // ステージの範囲を示すオブジェクト

    [SerializeField]
    private CameraFunction CameraFunction;

    [SerializeField]
    private FlyingPoint flyingPoint;　// スコア計算用スクリプト

    private FallPointCalculator FallPoint; // 落下地点を計算するスクリプト

    private float previousVerticalVelocity = 0f;  // リスポーン前のY方向速度を保存

    private bool HitNextFallArea = true;    // リスポーンエリアに連続で当たらないようにする

    private bool HitSnack = true; // snackに多段ヒットしないようにする

    private Rigidbody Rb;

    private int liftingCount = 1; // すっ飛ぶ力で使用するリフティング回数

    [SerializeField]
    [Header("クリア条件を管理しているオブジェクト")]
    private ClearConditions ClearConditionsScript; // リフティング回数管理のスクリプト

    private bool IsRespawn = true;

    void Start()
    {
        Rb = GetComponent<Rigidbody>();

        FallPoint = GetComponent<FallPointCalculator>();

        if (!CameraFunction) Debug.LogError("CameraFunctionが設定されていません");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"接触: {other.name}");

        if (other.CompareTag("Respawn") && HitNextFallArea == true)
        {
            HitNextFallArea = false;

            // 現在のY方向速度を保存
            previousVerticalVelocity = Rb.linearVelocity.y;

            MoveToRandomXZInRespawnArea();
        }
    }

    // snackがRespawnオブジェクトをスレ抜けてもリスポーンする
    void Update()
    {
        // Respawnオブジェクトのより高い位置にいたらリスポーン
        if (RespawnArea && transform.position.y > RespawnArea.position.y && HitNextFallArea == true)
        {
            HitNextFallArea = false;
            // 現在のY方向速度を保存
            previousVerticalVelocity = Rb.linearVelocity.y;

            // リスポーン無効でなければ
            if (IsRespawn)
            {
                Debug.Log($"Respawnオブジェクトの高さを超えたためリスポーン");
                MoveToRandomXZInRespawnArea();
            }
        }
    }

    // 落下スピードを制限する
    void FixedUpdate()
    {
        // 落下中かつ速度が上限を超えていたら制限
        if (Rb.linearVelocity.y < -MaxFallSpeed)
        {
            Vector3 clampedVelocity = Rb.linearVelocity;
            clampedVelocity.y = -MaxFallSpeed;
            Rb.linearVelocity = clampedVelocity;

            Debug.Log($"落下速度を制限しました: {Rb.linearVelocity.y}");
        }
    }
   

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            flyingPoint.ResetComboBonus();
        }
        else if (collision.gameObject.CompareTag("Player"))
        {
            // 多段ヒット防止
            if (!HitSnack) return;

            // 多段ヒット防止フラグfalse
            HitSnack = false;

            ClearConditionsScript.CheckLiftingCount();

            //ClearConditionsScript.
            // Snackに触れたらHitNextFallAreaをtrueに戻す
            HitNextFallArea = true;

            // リフティング回数を加算
            liftingCount++;

            // 力を計算：基本 + 回数 × 増加量
            float force = baseForce + (liftingCount * forcePerLift);

            // 上方向のベクトルに力を加える
            Vector3 forceDir = Vector3.up * force;
            Rb.AddForce(forceDir, ForceMode.Impulse);

            Debug.Log(liftingCount);


            // snackの座標のログ
            Debug.Log($"snackの座標: {transform.position}");

            // ゲージによる補正
            if (LiftingJump != null)
            {
                if (LiftingJump.IsLiftingPart)
                {
                    //ForceDirection *= LiftingJump.GetForce * LiftingJump.GetJumpPower;

                    // プレイヤーのリフティングパートを終了する
                    LiftingJump.FinishLiftingJump();    // AddForceの前に呼び出さないとスナックが飛ばない

                    if (flyingPoint != null)
                    {
                        flyingPoint.CalculateScore();
                        Debug.LogWarning("スコア計算開始");
                    }
                }
            }

            flyingPoint.CalculateScore();

            // ヒットストップを開始する
            StartCoroutine(HitStop());

            // カメラの強制ロックオン開始
            CameraFunction.StartLockOn(true);
        }
    }

    // ヒットストップ関数
    System.Collections.IEnumerator HitStop()
    {
        Time.timeScale = 0f; // 時間を止める（スローモーション）
        float timer = 0f;

        // リアルタイムで一定時間待つ
        while (timer < hitStopTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }

        Time.timeScale = 1f; // 時間を再開する
    }

    // リスポーン位置
    private void MoveToRandomXZInRespawnArea()
    {
        if (RespawnArea == null || GroundArea == null)
        {
            Debug.LogWarning("RespawnAreaが設定されていません");
            return;
        }

        // プレイヤーが離れたら多段ヒット防止フラグをtrue
        HitSnack = true;

        // 範囲取得
        Vector3 respawnCenter = RespawnArea.position;
        Vector3 respawnSize = RespawnArea.localScale;

        Vector3 groundCenter = GroundArea.position;
        Vector3 groundSize = GroundArea.localScale;

        // XとZの最小・最大を両方の範囲で共通する部分に制限
        float minX = Mathf.Max(respawnCenter.x - respawnSize.x / 2, groundCenter.x - groundSize.x / 2);
        float maxX = Mathf.Min(respawnCenter.x + respawnSize.x / 2, groundCenter.x + groundSize.x / 2);

        float minZ = Mathf.Max(respawnCenter.z - respawnSize.z / 2, groundCenter.z - groundSize.z / 2);
        float maxZ = Mathf.Min(respawnCenter.z + respawnSize.z / 2, groundCenter.z + groundSize.z / 2);

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);

        // Y座標はRespawnAreaの高さに設定
        float y = respawnCenter.y;

        // 保存した上方向の力を代入
        Vector3 newPos = new Vector3(randomX, y, randomZ);
        transform.position = newPos;
        Rb.linearVelocity = new Vector3(0f, previousVerticalVelocity, 0f);

        Debug.Log($"上昇速度: {Rb.linearVelocity.y}");
        Debug.Log($"リスポーン座標（グラウンド内）: {newPos}");

        FallPoint?.CalculateGroundPoint();

    }

    // クリア時処理
    public void OnClear()
    {
        IsRespawn = false;
    }
}