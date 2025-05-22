//======================================================
// [PlayerAnimationController]
// 作成者：森脇
// 最終更新日：05/22
//
// [Log]
// 05/22　森脇 アニメーターの管理
//======================================================

using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator; // モデルのAnimator（Mixamoモデル）

    [SerializeField]
    private PlayerSpeedManager speedManager; // PlayerSpeedManagerへの参照

    [SerializeField]
    private float speedToAnimatorSpeed = 0.01f; // 速度 → アニメーション再生速度への変換倍率

    [SerializeField]
    private ChargeJumpPlayer chargeJumpPlayer;

    private void Update()
    {
        if (speedManager == null || animator == null)
            return;

        float currentSpeed = speedManager.GetPlayerSpeed;

        // Animator.speed を変換倍率に応じて設定
        float animSpeed = currentSpeed * speedToAnimatorSpeed;

        // 再生速度の下限と上限を設定（必要に応じて調整）
        animSpeed = Mathf.Clamp(animSpeed, 0.1f, 3.0f);

        animator.speed = animSpeed;

        if (chargeJumpPlayer.IsJumping)
        {
            animator.SetTrigger("Jump");
        }
        else
        {
            animator.SetTrigger("Run");
        }
    }
}