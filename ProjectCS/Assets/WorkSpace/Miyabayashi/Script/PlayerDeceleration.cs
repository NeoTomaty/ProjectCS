//======================================================
// [�X�N���v�g��]PlayerDeceleration
// �쐬�ҁF�{�ѕ��P
// �ŏI�X�V���F3/31
// 
// [Log]
//======================================================
using UnityEngine;


public class PlayerDeceleration : MonoBehaviour
{
    //�v���C���[�̈ړ����x��ݒ肷��ϐ�
    public float Deceleration = 5.0f;//������

    float CurrentSpeed;//���݂̑��x

    private bool isHoldingKey = false;
    private float HoldTime = 0.0f;
    public float DecelerationsInterval = 1.0f; // �����Ԋu


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //�����Ńv���C���[�̏������x���󂯎��

    }

    // Update is called once per frame
    void Update()
    {
        //�����Ō��݂̑��x���󂯎��


        if (Input.GetKeyDown(KeyCode.S))
        {
            // �L�[���������u�Ԃ̏���
            if (CurrentSpeed > Deceleration)
            {
                CurrentSpeed -= Deceleration;//����
            }
        }

        if (Input.GetKey(KeyCode.S))
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
                if (CurrentSpeed > Deceleration)
                {
                    CurrentSpeed -= Deceleration;
                }
                HoldTime = 0.0f; // ������Ƀ��Z�b�g
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            // �L�[�𗣂������̏���
            isHoldingKey = false;
        }

    }
}
