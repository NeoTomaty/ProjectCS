//====================================================
// �X�N���v�g���FAutoMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̎����O�i�ړ�
// �ŏI�X�V���F03/31
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� Update���Ɉړ������ǉ�
//====================================================
using UnityEngine;

public class AutoMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
    public LRMovePlayer LRMovePlayer;             // �J�[�u�������Ǘ�����N���X

    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("AutoMovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }

        if (LRMovePlayer == null)
        {
            Debug.LogWarning("LRMovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null || LRMovePlayer == null) return;

        // ���x���擾
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // �i�s�������擾���A���̕����ֈړ�
        transform.position += LRMovePlayer.GetMoveDirection * speed * Time.deltaTime;

        // ������i�s�����ɍ��킹��
        transform.forward = LRMovePlayer.GetMoveDirection;
    }
}