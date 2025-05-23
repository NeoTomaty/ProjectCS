using UnityEngine;


public class ArrowGuide: MonoBehaviour
{
    [Header("参照")]
    public Transform player;          // プレイヤーのTransform
    public Transform target;          // ターゲットのTransform
    public Camera mainCamera;         // メインカメラ
    public Transform arrowObject;     // 矢印の3Dオブジェクト

    [Header("調整用パラメータ")]
    public float arrowDistance = 2f;          // プレイヤーから矢印までの距離
    public float arrowHeightOffset = 1f;      // 矢印の高さ（Y軸オフセット）
    public float rotationSmoothSpeed = 5f;    // 回転補間速度
    public float moveSmoothSpeed = 5f;        // 移動補間速度
    public float showThresholdDot = 0f;       // 正面とみなすDot閾値

    Vector3 previousDirection = Vector3.right;

    void Update()
    {
        if (!player || !target || !mainCamera || !arrowObject)
            return;

        Vector3 toTarget = target.position - player.position;
        Vector3 flatToTarget = Vector3.ProjectOnPlane(toTarget, Vector3.up).normalized;

        if (flatToTarget == Vector3.zero)
        {
            arrowObject.gameObject.SetActive(false);
            return;
        }

        Vector3 camForward = Vector3.ProjectOnPlane(mainCamera.transform.forward, Vector3.up).normalized;
        Vector3 camRight = Vector3.Cross(Vector3.up, camForward).normalized;

        float dotForward = Vector3.Dot(camForward, flatToTarget);

        Vector3 displayDir;
        if (dotForward >= showThresholdDot)
        {
            displayDir = flatToTarget;
            previousDirection = Vector3.Dot(camRight, flatToTarget) >= 0 ? camRight : -camRight;
        }
        else
        {
            float dotRight = Vector3.Dot(camRight, flatToTarget);
            displayDir = dotRight >= 0 ? camRight : -camRight;
            previousDirection = displayDir;
        }

        // ★ 高さ調整付きの目標位置を計算
        Vector3 targetPosition = player.position + displayDir * arrowDistance;
        targetPosition.y = player.position.y + arrowHeightOffset;

        // 移動と回転の補間
        arrowObject.position = Vector3.Lerp(arrowObject.position, targetPosition, Time.deltaTime * moveSmoothSpeed);
        Quaternion targetRot = Quaternion.LookRotation(flatToTarget) * Quaternion.Euler(0, 90f, 0);
        arrowObject.rotation = Quaternion.Slerp(arrowObject.rotation, targetRot, Time.deltaTime * rotationSmoothSpeed);

        arrowObject.gameObject.SetActive(true);
    }
}
