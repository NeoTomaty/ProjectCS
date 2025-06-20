//======================================================
// [PlayerModeResetter]
// �쐬�ҁF�X�e
// �쐬���F6/20
//
// [����]
// �v���C���[�̏�Ԃ��Ď����A����̏����Ń��f���������I��
// �ʏ��ԁirotationModel�j�ɖ߂�
// - LiftingJump.IsJumping��true�ɂȂ�����
// - �A�j���[�V�����C�x���g����̌Ăяo��
//======================================================

using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(LiftingJump))]
public class PlayerModeResetter : MonoBehaviour
{
    // ����Ώۂ̃R���|�[�l���g
    private PlayerAnimationController playerAnimationController;

    private LiftingJump liftingJump;

    // �O�̃t���[���ł̃W�����v��Ԃ��L�����邽�߂̃t���O
    private bool wasJumpingLastFrame = false;

    private void Awake()
    {
        // ����GameObject�ɃA�^�b�`����Ă���R���|�[�l���g�������Ŏ擾���܂��B
        playerAnimationController = GetComponent<PlayerAnimationController>();
        liftingJump = GetComponent<LiftingJump>();
    }

    private void Update()
    {
        // �W�����v��Ԃ��Ď����A�K�v�ł���΃��Z�b�g�������Ăяo��
        CheckJumpAndReset();
    }

    private void CheckJumpAndReset()
    {
        // liftingJump�R���|�[�l���g���Ȃ���Ή������Ȃ�
        if (liftingJump == null)
        {
            return;
        }

        // ���݃W�����v��(IsJumping == true) ���A�O�̃t���[���ł̓W�����v���Ă��Ȃ������ꍇ
        if (liftingJump.IsLiftingPart && !wasJumpingLastFrame)
        {
            Debug.Log("�W�����v�����m���܂����B���f����rotationModel�ɖ߂��܂��B");
            ForceReturnToRotationModel();
        }

        // ���݂̃W�����v��Ԃ��u�O�̃t���[���̏�ԁv�Ƃ��ĕۑ����A���̃t���[���̔���ɔ�����
        wasJumpingLastFrame = liftingJump.IsLiftingPart;
    }

    public void ForceReturnToRotationModel()
    {
        if (playerAnimationController != null)
        {
            playerAnimationController.ReturnToRotationModel();
        }
        else
        {
            Debug.LogError("PlayerAnimationController��������܂���I", this);
        }
    }
}