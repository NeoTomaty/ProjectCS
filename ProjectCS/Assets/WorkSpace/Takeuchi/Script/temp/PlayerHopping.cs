//====================================================
// スクリプト名：PlayerHopping
// 作成者：竹内
// 内容：プレイヤーがスーパーボールのように反発しながら跳ねる動作
// 　　　タイミングよくキー入力することでより高く跳ねる
// 　　　プレイヤーにアタッチする
// 最終更新日：04/17
// [Log]
// 04/17 竹内 スクリプト作成
// 04/17 ChatGPT スペースキー増幅ロジック追加
//====================================================

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerHopping : MonoBehaviour
{
    public float initialHopForce = 5f;        // 初期跳ね力
    public float minHopForce = 0.5f;          // 最小跳ね力
    public float maxHopForce = 10f;           // 最大跳ね力

    public float hopAddAmount = 1.0f;         // 増幅時に加算される力
    public float hopDecayAmount = 0.5f;       // 着地時に減衰される力

    public float raycastDistance = 1.2f;      // 地面までの判定距離
    public Vector3 GravityScale = new Vector3(0.0f, -9.8f, 0.0f); // 重力

    public string TagName = "Ground";        // 床のタグ

    private Rigidbody rb;
    private float currentHopForce;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentHopForce = initialHopForce;
    }

    void Update()
    {
        // カスタム重力
        rb.AddForce(GravityScale, ForceMode.Acceleration);

        // ジャンプの入力チェック
        bool jumpInputDetected = false;

        // ゲームパッドが接続されている場合、Aボタンを優先
        if (Gamepad.current != null)
        {
            jumpInputDetected = Input.GetKeyDown(KeyCode.JoystickButton0);
        }
        // ゲームパッドが接続されていない場合、スペースキーを使用
        else
        {
            jumpInputDetected = Input.GetKeyDown(KeyCode.Space);
        }

        // スペースキーによる増幅（地面近くで押したときのみ）
        if (IsNearGround() && jumpInputDetected)
        {
            currentHopForce = Mathf.Min(currentHopForce + hopAddAmount, maxHopForce);
            Debug.Log("ジャンプタイミングOK");
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagName))
        {
            // Y方向の速度をリセットし、跳ねる
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * currentHopForce, ForceMode.Impulse);

            // 着地時に減衰（min以下にはしない）
            currentHopForce = Mathf.Max(currentHopForce - hopDecayAmount, minHopForce);
        }
    }

    private bool IsNearGround()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        bool hit = Physics.Raycast(ray, raycastDistance);

        // 可視化（緑＝地面検出中、赤＝空中）
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, hit ? Color.green : Color.red);

        return hit;
    }
}

