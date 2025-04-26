//====================================================
// �X�N���v�g���FPlayerStateManager
// �쐬�ҁF����
// ���e�F�v���C���[�̏�Ԃ��Ǘ�����N���X
// �ŏI�X�V���F04/26
// 
// [Log]
// 04/26 ���� �X�N���v�g�쐬
//
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. ���̃X�N���v�g�̓v���C���[�ɃA�^�b�`����

using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public enum LiftingState
    {
        Normal,      // �ʏ���
        LiftingPart, // ���t�e�B���O�p�[�g
    }

    private LiftingState State = LiftingState.Normal;

   
    public void SetLiftingState(LiftingState state)
    {
        State = state;
    }

    public LiftingState GetLiftingState()
    {
        return State; 
    }
}
