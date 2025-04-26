//====================================================
// �X�N���v�g���FLiftingAreaManager
// �쐬�ҁF����
// ���e�F�v���C���[�ƃ^�[�Q�b�g�Ƃ̃G���A���Ǘ�����N���X
// �ŏI�X�V���F04/26
// 
// [Log]
// 04/26 ���� �X�N���v�g�쐬
//
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. ���̃X�N���v�g�̓��t�e�B���O�G���A�I�u�W�F�N�g�ɃA�^�b�`����
// 2. Player�Ƀv���C���[�I�u�W�F�N�g��ݒ�
// 3. Target�Ƀ��t�e�B���O�ΏۃI�u�W�F�N�g��ݒ�

using UnityEngine;

public class LiftingAreaManager : MonoBehaviour
{

    [SerializeField] private GameObject Player; // �v���C���[�I�u�W�F�N�g
    [SerializeField] private GameObject Target; // �^�[�Q�b�g�I�u�W�F�N�g

    private bool IsPlayerContacting = false; // Player���G���A���ɓ��������ǂ���
    private bool IsTargetContacting = false; // Target���G���A���ɓ��������ǂ���

    PlayerStateManager PlayerState = null; // �v���C���[�̏�ԊǗ��R���|�[�l���g

    void Start()
    {
        if (!Player) Debug.LogError("�v���C���[�I�u�W�F�N�g���ݒ肳��Ă��܂���");
        if (!Target) Debug.LogError("�^�[�Q�b�g�I�u�W�F�N�g���ݒ肳��Ă��܂���");

        PlayerState = Player.GetComponent<PlayerStateManager>();
        if(!PlayerState) Debug.LogError("�v���C���[�I�u�W�F�N�g��PlayerStateManager���A�^�b�`����Ă��܂���");
    }

    private void OnTriggerEnter(Collider other)
    {
        // �v���C���[���G���A���ɓ��������ǂ�������
        if (other.gameObject == Player)
        {
            IsPlayerContacting = true;
        }

        // �^�[�Q�b�g���G���A���ɓ��������ǂ�������
        if (other.gameObject == Target)
        {
            IsTargetContacting = true;
        }

        // �����̃I�u�W�F�N�g�������Ă���ꍇ�A���t�e�B���O�p�[�g�Ɉڍs����
        if(IsPlayerContacting && IsTargetContacting)
        {
            PlayerState.SetLiftingState(PlayerStateManager.LiftingState.LiftingPart);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // �v���C���[���G���A�O�ɏo�����ǂ�������
        if (other.gameObject == Player)
        {
            IsPlayerContacting = false;
        }

        // �v���C���[���G���A�O�ɏo�����ǂ�������
        if (other.gameObject == Target)
        {
            IsTargetContacting = false;
        }

        // �ǂ��炩�Е��ł��G���A�O�ɏo����A�ʏ��Ԃɐ؂�ւ���
        if(!IsPlayerContacting || !IsTargetContacting)
        {
            PlayerState.SetLiftingState(PlayerStateManager.LiftingState.Normal);
        }
    }
}
