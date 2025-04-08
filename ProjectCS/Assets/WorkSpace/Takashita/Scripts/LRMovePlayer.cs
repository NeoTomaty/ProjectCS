//====================================================
// �X�N���v�g���FLRMovePlayer
// �쐬�ҁF����
// ���e�F�v���C���[�̍��E�ړ�����
// �ŏI�X�V���F04/01
// 
// [Log]
// 03/27 ���� �X�N���v�g�쐬 
// 03/31 ���� ���E�ړ������ǉ�
// 04/01 �R���g���[���[����ǉ�
// 04/08 ���E�ړ��𑬓x�Ɉˑ�������
//====================================================
using UnityEngine;
using UnityEngine.InputSystem;

public class LRMovePlayer : MonoBehaviour
{
    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X
    public MovePlayer MovePlayer; // �v���C���[�ړ��N���X



    [SerializeField] private float TurnSpeed = 100.0f;  // �J�[�u�̉�]���x
    [SerializeField] private float TurnResponse = 1.0f; // �J�[�u�̂��₷��

    void Start()
    {
        if (PlayerSpeedManager == null)
        {
            Debug.LogWarning("MovePlayer�X�N���v�g���A�^�b�`����Ă��܂���B");
        }
    }

    void Update()
    {
        if (PlayerSpeedManager == null) return;

        // ���x���擾
        float speed = PlayerSpeedManager.GetPlayerSpeed;

        // ���x�������قǃJ�[�u���ɂ�������
        //float rotationAmount = (TurnSpeed / Mathf.Max(speed, 1)) * Time.deltaTime;

        // ���݂̑��x�ɉ����ċȂ���₷������
        // �J�[�u�� = ��]���x �~ ���x �~ deltaTime
        float rotationAmount = TurnSpeed * (speed * TurnResponse) * Time.deltaTime;

        // �v���C���[�̍��E�ړ������������ϐ�
        float moveX = 0.0f;

        // �Q�[���p�b�h���ڑ�����Ă��邩�m�F
        if (Gamepad.current != null)
        {
            // �Q�[���p�b�h�̍��X�e�B�b�N���͂�D��
            moveX = Gamepad.current.leftStick.ReadValue().x;
        }
        else
        {
            // �Q�[���p�b�h���ڑ�����Ă��Ȃ��ꍇ�A�L�[�{�[�h�̓��͂��g�p
            if (Input.GetKey(KeyCode.A)) moveX = -1.0f;
            if (Input.GetKey(KeyCode.D)) moveX = 1.0f;
        }



        // ��]�����i���E�j
        if (Mathf.Abs(moveX) > 0.1f)    // ��Βl���傫�Ȓl�̏ꍇ
        {
            float angle = rotationAmount * Mathf.Sign(moveX);   // �������擾
            MovePlayer.SetMoveDirection(Quaternion.Euler(0, angle, 0) * MovePlayer.GetMoveDirection);
        }

        //// ���J�[�u
        //if (moveX < -0.1f)
        //{
        //    MovePlayer.SetMoveDirection(Quaternion.Euler(0, -rotationAmount, 0) * MovePlayer.GetMoveDirection);
        //}
        //// �E�J�[�u
        //if (moveX > 0.1f)
        //{
        //    MovePlayer.SetMoveDirection(Quaternion.Euler(0, rotationAmount, 0) * MovePlayer.GetMoveDirection);
        //}
    }
}
