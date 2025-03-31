//====================================================
// �X�N���v�g���FMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̎����O�i�ړ�
// �ŏI�X�V���F03/31
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� Update���Ɉړ������ǉ�
// 03/31 ���� �X�N���v�g���ύX AutoMovePlayer��MovePlayer
//====================================================
using UnityEngine;
using UnityEngine.UIElements;

public class MovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
   
    private Vector3 MoveDirection;    // ���݂̐i�s����

    // ���̃X�N���v�g����i�s�������擾���邽�߂̃v���p�e�B                                  
    public Vector3 GetMoveDirection => MoveDirection;

    // ���̃X�N���v�g����i�s������ݒ肷�邽�߂̃Z�b�^�[
    public void SetMoveDirection(Vector3 NewDirection)
    {
        MoveDirection = NewDirection;
    }

    void Start()
    {
        MoveDirection = transform.forward;

        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("AutoMovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // ���x���擾
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // �i�s�������擾���A���̕����ֈړ�
        transform.position += MoveDirection * speed * Time.deltaTime;

        // ������i�s�����ɍ��킹��
        transform.forward = MoveDirection;
    }
}