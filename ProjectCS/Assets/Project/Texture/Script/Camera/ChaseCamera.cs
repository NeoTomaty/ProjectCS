//======================================================
// ChaseCameraスクリプト
// 作成者：竹内
// 最終更新日：3/31
// 
// [Log]
//======================================================
using UnityEngine;

public class ChaseCamera : MonoBehaviour
{
    public Transform Target;                        // 追従する対象（プレイヤー）
    public Vector3 Offset = new Vector3(0, 2, -5);  // カメラの位置オフセット
    public float SmoothSpeed = 5.0f;                // カメラの追従速度

    void LateUpdate()
    {
        if (Target == null) return;

        // 目標の位置（プレイヤーの後ろ）を計算
        Vector3 desiredPosition = Target.position + Target.rotation * Offset;

        // スムーズにカメラを移動
        transform.position = Vector3.Lerp(transform.position, desiredPosition, SmoothSpeed * Time.deltaTime);

        // プレイヤーの向きを追従
        transform.LookAt(Target.position + Vector3.up * 1.5f); // 目線を少し上に調整
    }
}
