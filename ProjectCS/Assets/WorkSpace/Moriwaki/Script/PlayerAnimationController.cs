//======================================================
// [PlayerAnimationController]
// �쐬�ҁF�X�e
// �ŏI�X�V���F05/22
//
// [Log]
// 05/22�@�X�e �A�j���[�^�[�̊Ǘ�
//======================================================

using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator animator; // ���f����Animator�iMixamo���f���j

    [SerializeField]
    private PlayerSpeedManager speedManager; // PlayerSpeedManager�ւ̎Q��

    [SerializeField]
    private float speedToAnimatorSpeed = 0.01f; // ���x �� �A�j���[�V�����Đ����x�ւ̕ϊ��{��

    [SerializeField]
    private ChargeJumpPlayer chargeJumpPlayer;

    private void Update()
    {
        if (speedManager == null || animator == null)
            return;

        float currentSpeed = speedManager.GetPlayerSpeed;

        // Animator.speed ��ϊ��{���ɉ����Đݒ�
        float animSpeed = currentSpeed * speedToAnimatorSpeed;

        // �Đ����x�̉����Ə����ݒ�i�K�v�ɉ����Ē����j
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