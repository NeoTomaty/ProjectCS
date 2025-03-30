//====================================================
// �X�N���v�g���FLRMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̍��E�ړ�����
// �ŏI�X�V���F03/31
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� ���E�ړ������ǉ�
//====================================================
using UnityEngine;

public class LRMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X

    [SerializeField]
    private float TurnSpeed = 100.0f; // �J�[�u�̉�]���x

    private Vector3 MoveDirection;    // ���݂̐i�s����

    // ���̃X�N���v�g����i�s�������擾���邽�߂̃v���p�e�B                                  
    public Vector3 GetMoveDirection => MoveDirection;

    void Start()
    {
        MoveDirection = transform.forward; // �����̐i�s����

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

        // ���x�������قǃJ�[�u���ɂ�������
        float rotationAmount = (TurnSpeed / Mathf.Max(speed, 1)) * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            // ���J�[�u
            MoveDirection = Quaternion.Euler(0, -rotationAmount, 0) * MoveDirection;
        }
        if (Input.GetKey(KeyCode.D))
        {
            // �E�J�[�u
            MoveDirection = Quaternion.Euler(0, rotationAmount, 0) * MoveDirection;
        }
    }
}
