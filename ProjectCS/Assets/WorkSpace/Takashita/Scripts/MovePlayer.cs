//====================================================
// �X�N���v�g���FMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̎����O�i�ړ�
// �ŏI�X�V���F04/08
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� Update���Ɉړ������ǉ�
// 03/31 ���� �X�N���v�g���ύX AutoMovePlayer��MovePlayer
// 04/08 ���� �������̕Ǌђʖh�~����������
//====================================================
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
   
    private Vector3 MoveDirection;    // ���݂̐i�s����

    [SerializeField]
    float RayDistance = 10.0f;

    // ���̃X�N���v�g����i�s�������擾���邽�߂̃v���p�e�B                                  
    public Vector3 GetMoveDirection => MoveDirection;

    // ���̃X�N���v�g����i�s������ݒ肷�邽�߂̃Z�b�^�[
    public void SetMoveDirection(Vector3 NewDirection)
    {
        MoveDirection = NewDirection;
    }

    private string WallTag = "Wall";   // �ǂ̃^�O

    void Start()
    {
        MoveDirection = transform.forward;

        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // ���C�L���X�g�ŕǂ����o
        RaycastHit hit;
        if (Physics.Raycast(transform.position, MoveDirection, out hit, RayDistance))
        {
            // �Փ˂����I�u�W�F�N�g�̃^�O���m�F
            if (hit.collider.CompareTag(WallTag))  // �ǃ^�O�Ɉ�v���邩�m�F
            {
                // ���C���ǂɏՓ˂����ꍇ�A���̒n�_�Ɉړ�
                // �߂荞�񂾕��������߂����߁A�Փ˓_����ړ������̔��Ε����ɏ������炷
                Vector3 PushBackDirection = -MoveDirection.normalized;  // ���Ε���
                transform.position = hit.point + PushBackDirection * 0.1f;  // 0.1f�͉����߂��̋���
            }
            else
            {
                // �i�s�������擾���A���̕����ֈړ�
                transform.position += MoveDirection * PlayerSpeedManager.GetPlayerSpeed * Time.deltaTime;
            }
        }
        else
        {
            // �i�s�������擾���A���̕����ֈړ�
            transform.position += MoveDirection * PlayerSpeedManager.GetPlayerSpeed * Time.deltaTime;
        }

        // ������i�s�����ɍ��킹��
        transform.forward = MoveDirection;
    }
}