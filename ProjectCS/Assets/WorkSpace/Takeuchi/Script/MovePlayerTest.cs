//======================================================
// MovePlayerスクリプト（テスト）
// 作成者：竹内
// 最終更新日：3/31
// 
// [Log]
//======================================================
using UnityEngine;

public class MovePlayerTest : MonoBehaviour
{
    public float MaxSpeed = 5.0f;  // 最大移動速度
    public float TurnSpeed = 300.0f; // 回転速度
    public Transform CameraTransform; // 追従カメラのTransform
    public float Acceleration = 1.0f; // 加速度
    public float Deceleration = 1.0f; // 減速

    private float CurrentSpeed = 0.0f; // 現在の速度

    void Update()
    {
        // カメラが設定されていない場合は処理しない
        if (CameraTransform == null) return;

        // 入力取得（WASD）
        float Horizontal = Input.GetAxisRaw("Horizontal");  // A(-1) / D(+1)
        float Vertical = Input.GetAxisRaw("Vertical");      // W(+1) / S(-1)

        // カメラの向きを基準に移動方向を決定
        Vector3 CameraForward = CameraTransform.forward;
        Vector3 CameraRight = CameraTransform.right;

        // 水平方向の影響をなくす（XZ 平面のみ考慮）
        CameraForward.y = 0;
        CameraRight.y = 0;
        CameraForward.Normalize();
        CameraRight.Normalize();

        // カメラの方向に基づいた移動ベクトルを計算
        Vector3 MoveDirection = CameraForward * Vertical + CameraRight * Horizontal;

        // 斜め移動時の速度を一定にする
        MoveDirection.Normalize();

        // 移動入力がある場合、加速
        if (MoveDirection.magnitude > 0.1f)
        {
            // プレイヤーの回転（移動方向に向く）
            Quaternion TargetRotation = Quaternion.LookRotation(MoveDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, TurnSpeed * Time.deltaTime);

            // 加速処理
            CurrentSpeed = Mathf.Min(CurrentSpeed + Acceleration * Time.deltaTime, MaxSpeed);  // 最大速度を設定
        }
        else
        {
            // 減速処理
            CurrentSpeed = Mathf.Max(CurrentSpeed - Deceleration * Time.deltaTime, 0); // 最小速度0
        }

        // 移動処理（加速度を反映）
        transform.position += MoveDirection * CurrentSpeed * Time.deltaTime;
    }
}
