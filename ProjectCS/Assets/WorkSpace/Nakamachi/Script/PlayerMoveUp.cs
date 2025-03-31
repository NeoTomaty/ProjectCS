//�X�N���v�g��:PlayerMoveUp.cs
//�쐬��:��������
//�ŏI�X�V��:2025/03/24
//[Log]
//2025/03/24�@��������@�v���C���[��WASD�œ������A�ǂɓ�����Ɖ������Ă����@�\������
//2025/03/31�@��������@�ǂɓ�����Ɖ����ʂ�+5��������

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveUp : MonoBehaviour
{
    //�v���C���[�̈ړ����x��ݒ肷��ϐ�
    public float Speed = 5.0f;

    //�������x��ۑ�����ϐ�
    private float InitialSpeed;

    //�Փˏ�Ԃ��Ǘ�����ϐ�
    private bool Collided = false;

    void Start()
    {
        //�������x��ۑ�
        InitialSpeed = Speed;

        //�������x���f�o�b�O���O�ɏo��
        Debug.Log("�������x: " + Speed);
    }

    void Update()
    {
        //�L�[�{�[�h�ƃR���g���[���[�̐��������̓��͂��擾
        float MoveHorizontal = Input.GetAxis("Horizontal");

        //�L�[�{�[�h�ƃR���g���[���[�̐��������̓��͂��擾
        float MoveVertical = Input.GetAxis("Vertical");

        //�ړ��x�N�g�����쐬
        Vector3 Movement = new Vector3(MoveHorizontal, 0.0f, MoveVertical);

        //�v���C���[���ړ�������
        transform.Translate(Movement * Speed * Time.deltaTime);
    }

    //�Փ˂����������Ƃ��ɌĂяo�����
    void OnCollisionEnter(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g�̃^�O��Wall�̂Ƃ�
        if (collision.gameObject.tag == "Wall")
        {
            //�Փˏ�Ԃ�true�ɐݒ�
            Collided = true;

            //�ǂɏՓ˂������Ƃ��f�o�b�O���O�ɏo��
            Debug.Log("�ǂɏՓ�");
        }
    }

    //�Փ˂��I�������Ƃ��ɌĂяo�����
    void OnCollisionExit(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g�̃^�O��Wall�ŁA�Փˏ�Ԃ�true�̂Ƃ�
        if (collision.gameObject.tag == "Wall" && Collided)
        {
            //�ړ����x��5.0f����������
            Speed += 5.0f;

            //�����ʂ��f�o�b�O���O�ɏo��
            Debug.Log("������: " + Speed);

            //�Փˏ�Ԃ�false�ɐݒ�
            Collided = false;
        }
    }
}