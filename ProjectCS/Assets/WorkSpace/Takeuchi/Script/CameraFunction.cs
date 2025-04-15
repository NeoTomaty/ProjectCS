//======================================================
// CameraFunctionスクリプト
// 作成者：竹内
// 最終更新日：3/31
// 
// [Log]
//======================================================
using UnityEngine;


public class CameraFunction : MonoBehaviour
{
    public Transform Player;         // プレイヤー
    public Transform Target;         // 対象

    public float Distance = 5.0f;               // カメラとプレイヤーの距離
    public float RotationSpeed = 50.0f;         // 回転速度
    public float VerticalMin = -80f;            // 縦方向移動の最小値
    public float VerticalMax = 80f;             // 縦方向移動の最大値
    public float AutoCorrectSpeed = 1.5f;       // 自動補正速度
    public float LookSmoothSpeed = 5.0f;        // 自動視線修正値
    public float PositionSmoothSpeed = 5.0f;    // カメラ位置の補正速度

    private float yaw = 0f;     // 横方向
    private float pitch = 20f;  // 縦方向

    void LateUpdate()
    {
        if (Player == null) return;

        bool isManual = false;

        // キー操作でカメラの回転
        if (Input.GetKey(KeyCode.J)) { yaw -= RotationSpeed * Time.deltaTime; isManual = true; }
        if (Input.GetKey(KeyCode.L)) { yaw += RotationSpeed * Time.deltaTime; isManual = true; }
        if (Input.GetKey(KeyCode.I)) { pitch += RotationSpeed * Time.deltaTime; isManual = true; }
        if (Input.GetKey(KeyCode.K)) { pitch -= RotationSpeed * Time.deltaTime; isManual = true; }

        // 範囲内に収める
        pitch = Mathf.Clamp(pitch, VerticalMin, VerticalMax);

        // カメラ操作なし＆Rキーでもない場合は自動補正
        if (!isManual && !Input.GetKey(KeyCode.R))
        {
            float targetYaw = Player.eulerAngles.y;
            float targetPitch = 20f;
            yaw = Mathf.LerpAngle(yaw, targetYaw, AutoCorrectSpeed * Time.deltaTime);
            pitch = Mathf.Lerp(pitch, targetPitch, AutoCorrectSpeed * Time.deltaTime);
        }

        // Rキー押下中は敵を自然に見る視点へ移動 + 回転
        if (Input.GetKey(KeyCode.R) && Target != null)
        {
            Vector3 viewDir = (Target.position - Player.position).normalized;

            // プレイヤーの背後（敵方向）に回り込む目標位置
            Vector3 desiredPos = Player.position - viewDir * Distance + Vector3.up * 2.0f;

            // カメラをスムーズにその位置へ
            transform.position = Vector3.Lerp(transform.position, desiredPos, PositionSmoothSpeed * Time.deltaTime);

            // 敵の頭あたりを注視（滑らかに補正）
            Vector3 lookTarget = Target.position + Vector3.up * 1.5f;
            Quaternion desiredRot = Quaternion.LookRotation(lookTarget - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, LookSmoothSpeed * Time.deltaTime);
        }
        else
        {
            // クォータニオンによるカメラ操作
            Vector3 offset = Quaternion.Euler(pitch, yaw, 0) * new Vector3(0, 0, -Distance);
            transform.position = Player.position + offset;

            if (Input.GetMouseButton(1) && Target != null)
            {
                transform.LookAt(Target.position + Vector3.up * 1.5f);
            }
            else
            {
                transform.LookAt(Player.position + Vector3.up * 1.5f);
            }
        }
    }
}



