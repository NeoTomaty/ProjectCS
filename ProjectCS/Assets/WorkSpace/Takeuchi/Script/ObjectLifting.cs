//====================================================
// スクリプト名：ObjectLifting
// 作成者：竹内
// 内容：アタッチしたオブジェクトが対象オブジェクトの頭上を
// 　　　常にバウンドし続ける処理
// 　　　リフティングするオブジェクトにアタッチする
// 最終更新日：04/17
// [Log]
// 04/17 竹内 スクリプト作成
//====================================================
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectLifting : MonoBehaviour
{
    public GameObject Target;             // 対象オブジェクト（プレイヤー）
    private Rigidbody rb;                 // Rigidbody

    public Vector3 Gravity = new Vector3(0.0f, -9.8f, 0.0f);  // 重力加速度
    public Vector3 force = new Vector3(0.0f, 1.0f, 0.0f);     // Y軸に与える力

    public float maxBounceHeight = 5.0f;   // バウンドの最大
    public float minBounceHeight = 1.0f;   // バウンドの最小
    public float bounceDamping = 0.98f;    // バウンドの減衰率（1未満で徐々に低くなる）

    public float fixedBounceSpeed = 5.0f;  // 最小バウンドの固定上向き速度
    public string TagName = "Player";      // 衝突対象のタグ

    private float currentBounceSpeed;      // 現在のバウンド速度
    private bool isLift = true;            // 接地判定

    public bool isLink = true;             // プレイヤーとリンクして動作するか（ON:頭上に固定）


    // 開始時
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 最大バウンド高さに達するのに必要な初速を計算して代入
        currentBounceSpeed = Mathf.Sqrt(2 * Mathf.Abs(Gravity.y) * maxBounceHeight);
    }


    // 更新処理
    void Update()
    {
        // Lキーで切り替える
        if (Input.GetKeyDown(KeyCode.L))
        {
            isLink = !isLink;
        }
    }

    // 固定フレームの更新処理
    void FixedUpdate()
    {
        // 重力を常に与える
        rb.AddForce(Gravity, ForceMode.Acceleration);

        // プレイヤーの頭上をリフティングする場合のみ位置追従（XZのみ）
        if (isLink)
        {
            // ボールの位置をプレイヤーの頭上に維持（XZだけ追従、Yはバウンド用に物理で）
            Vector3 playerPos = Target.transform.position;
            transform.position = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        }


        // 地面などと接地中ならバウンド処理
        if (isLift)
        {
            // バウンド速度を減衰
            currentBounceSpeed *= bounceDamping;

            // 減衰後のバウンド速度が最小高さを下回った場合、最低バウンド速度に固定
            float minSpeed = Mathf.Sqrt(2 * Mathf.Abs(Gravity.y) * minBounceHeight);
            if (currentBounceSpeed < minSpeed)
            {
                currentBounceSpeed = fixedBounceSpeed;
            }

            //// 上向きにバウンドさせる
            //rb.linearVelocity = new Vector3(rb.linearVelocity.x, currentBounceSpeed, rb.linearVelocity.z);

            // 上方向に力を与える
            rb.AddForce(force, ForceMode.Impulse);

            // 一度空中に出たのでフラグを下げる
            isLift = false;

            Debug.Log("リフトアップ");
        }
    }

    // 接触したとき
    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーとリンクしている場合
        if(isLink)
        {
            // プレイヤーとぶつかったときの処理
            if (collision.gameObject.CompareTag(TagName))
            {
                // プレイヤーとの相対速度（主にプレイヤーのジャンプによる衝突）を取得
                float impactSpeed = collision.relativeVelocity.y;

                if (impactSpeed > 0)
                {
                    // 衝突の勢いに応じて追加のバウンド速度を計算
                    float boostHeight = impactSpeed * 0.3f; // ブースト倍率（調整可能）
                    float boostedSpeed = Mathf.Sqrt(2 * Mathf.Abs(Gravity.y) * boostHeight);

                    // 最大高さを超えないようにクランプ
                    float maxSpeed = Mathf.Sqrt(2 * Mathf.Abs(Gravity.y) * maxBounceHeight);
                    currentBounceSpeed = Mathf.Clamp(currentBounceSpeed + boostedSpeed, 0, maxSpeed);
                }

                // プレイヤーに触れたらフラグを立てる（次のFixedUpdateでバウンド）
                isLift = true;
            }
        }
        // プレイヤーとリンクしていない場合
        else
        {
            // プレイヤーとぶつかったときの処理
            if (collision.gameObject.CompareTag(TagName))
            {
                isLink = true;
            }
        }
    }


    // リンクフラグを変更（外部スクリプト用）
    public void SetLinkState(bool link)
    {
        isLink = link;
    }
}
