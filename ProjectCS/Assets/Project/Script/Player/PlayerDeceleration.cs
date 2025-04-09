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
//======================================================
using UnityEngine;


public class PlayerDeceleration : MonoBehaviour
{
    //�v���C���[�̈ړ����x��ݒ肷��ϐ�
    public float Deceleration = 5.0f;//������

    public PlayerSpeedManager PlayerSpeedManager; // ���x�Ǘ��N���X

    private bool isHoldingKey = false;
    private float HoldTime = 0.0f;
    public float DecelerationsInterval = 1.0f; // �����Ԋu


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        //�����Ō��݂̑��x���󂯎��


        if (Input.GetKeyDown(KeyCode.S)||Input.GetAxis("Vertical") < -0.1f)
        {
            // �L�[���������u�Ԃ̏���
            if ( PlayerSpeedManager.GetPlayerSpeed> Deceleration)
            {
                //����
               PlayerSpeedManager.SetDecelerationValue(Deceleration);
            }
        }

        if (Input.GetKey(KeyCode.S) || Input.GetAxis("Vertical") < -0.1f)
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

        if (Input.GetKeyUp(KeyCode.S) || Input.GetAxis("Vertical") < -0.1f)
        {
            // �L�[�𗣂������̏���
            isHoldingKey = false;
        }

    }
}
