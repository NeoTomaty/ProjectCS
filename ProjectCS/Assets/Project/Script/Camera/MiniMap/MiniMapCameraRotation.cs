//====================================================
// スクリプト名：MiniMapCameraRotationRelative
// 作成者：竹内
// 内容：プレイヤーの回転に応じてミニマップカメラが
// 　　　滑らかに相対回転
// 最終更新日：04/16
// [Log]
// 04/16 竹内 スクリプト作成
//====================================================
using UnityEngine;

public class MiniMapCameraRotationRelative : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float rotationSmoothSpeed = 5f;

    private Vector3 initialPlayerForward;
    private float currentYRotation;

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("プレイヤーが未設定です");
            enabled = false;
            return;
        }

        // 初期の前ベクトルを記録（地面平面に投影）
        Vector3 forward = player.forward;
        forward.y = 0;
        initialPlayerForward = forward.normalized;

        // 初期Y回転角度を保存（現在のカメラのY角度）
        currentYRotation = transform.eulerAngles.y;
    }

    void LateUpdate()
    {
        // 現在のプレイヤーの前ベクトルを取得し、水平に制限
        Vector3 currentForward = player.forward;
        currentForward.y = 0;
        currentForward.Normalize();

        // 初期ベクトルから現在の前ベクトルまでの角度差分を取得
        float angle = Vector3.SignedAngle(initialPlayerForward, currentForward, Vector3.up);

        // 滑らかにカメラのY軸を補間
        currentYRotation = Mathf.LerpAngle(currentYRotation, angle, Time.deltaTime * rotationSmoothSpeed);

        // カメラ回転適用（X軸は俯瞰の90度、Y軸だけ動的、Zは0固定）
        transform.rotation = Quaternion.Euler(90f, currentYRotation, 0f);
    }
}