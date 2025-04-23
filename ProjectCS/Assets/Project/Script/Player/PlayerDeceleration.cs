//======================================================
// [�X�N���v�g��]PlayerDeceleration
// �쐬�ҁF�{�ѕ��P
// �ŏI�X�V���F4/1
// 
// [Log]
// 3/31 �{�с@�X�N���v�g�쐬
// 3/31 �{�с@��������������
// 4/1  �{��  �R���g���[���[����ǉ�
// 4/1  �{�с@������������
// 4/23 �����@���͂Ɋւ���d�l�ύX(PlayerInput(InputActionAsset))
//======================================================
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerDeceleration : MonoBehaviour
{
    //�v���C���[�̈ړ����x��ݒ肷��ϐ�
    public float Deceleration = 5.0f;//������

    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X

    private bool isHoldingKey = false;
    private float HoldTime = 0.0f;
    public float DecelerationsInterval = 1.0f; // �����Ԋu

    private PlayerInput PlayerInput;         // �v���C���[�̓��͂��Ǘ�����component
    private InputAction DecelerationAction;  // �����p��InputAction

    void Start()
    {
        // �����ɃA�^�b�`����Ă���PlayerInput���擾
        PlayerInput = GetComponent<PlayerInput>();

        // �Ή�����InputAction���擾
        DecelerationAction = PlayerInput.actions["Deceleration"];
    }

    void Update()
    {
        //�����Ō��݂̑��x���󂯎��

        if (DecelerationAction.ReadValue<float>() < -0.5f)
        {
            // �L�[���������u�Ԃ̏���
            if ( PlayerSpeedManager.GetPlayerSpeed> Deceleration)
            {
                //����
               PlayerSpeedManager.SetDecelerationValue(Deceleration);
            }
        }

        if (DecelerationAction.ReadValue<float>() < -0.5f)
        {
            if (!isHoldingKey)
            {
                // �������J�n���̏���
                isHoldingKey = true;
                HoldTime = 0.0f; // ������
            }

            HoldTime += Time.deltaTime;

            if (HoldTime >= DecelerationsInterval)
            {
                if (PlayerSpeedManager.GetPlayerSpeed > Deceleration)
                {
                    //����
                    PlayerSpeedManager.SetDecelerationValue(Deceleration);
                }
                HoldTime = 0.0f; // ������Ƀ��Z�b�g
            }
        }

        if (DecelerationAction.ReadValue<float>() < -0.5f)
        {
            // �L�[�𗣂������̏���
            isHoldingKey = false;
        }
    }
}
