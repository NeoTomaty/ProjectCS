//======================================================
// SelectPlayerAnimationController
// 作成者：森脇
// 最終更新日：6/5
//
// [Log]6/5 森脇 スクリプト作成
//======================================================

using UnityEngine;

public class SelectPlayerAnimationController : MonoBehaviour
{
    [Header("モデル切り替え")]
    [SerializeField] private GameObject rotationModel;  // 移動中の回転モデル

    [SerializeField] private GameObject model;          // 静止中のモデル

    [Header("移動のしきい値")]
    [SerializeField] private float moveThreshold = 0.1f;

    [Header("モデルの回転速度倍率")]
    [SerializeField] private float baseRotationSpeed = 5f;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // ← ここをRawでなく通常に変更
        bool isMoving = Mathf.Abs(horizontal) > moveThreshold;

        // モデル切り替え
        rotationModel.SetActive(!isMoving);
        model.SetActive(isMoving);

        if (isMoving)
        {
            // 回転したいY角度（右：180度、左：-180度）
            float targetYRotation = horizontal > 0 ? 180f : -180f;

            // ターゲット回転
            Quaternion targetRotation = Quaternion.Euler(0f, targetYRotation, 0f);

            // 回転速度は移動速度に応じてスケーリング
            float speedFactor = Mathf.Abs(horizontal);
            float dynamicRotationSpeed = baseRotationSpeed * speedFactor;

            // rotationModel を回転させる
            rotationModel.transform.rotation = Quaternion.Lerp(
                rotationModel.transform.rotation,
                targetRotation,
                Time.deltaTime * dynamicRotationSpeed
            );
        }
    }
}