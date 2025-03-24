//�t�@�C����:PlayerMoveUp.cs
//�쐬��:��������
//�쐬��:2025/03/24
//�X�V����:
//2025/03/24:���ō쐬
//�T�v:�v���C���[��WASD�œ������A�ǂɓ�����Ɖ������Ă����@�\������

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveUp : MonoBehaviour
{
    //�v���C���[�̈ړ����x��ݒ肷��ϐ�
    public float speed = 5.0f;

    //�������x��ۑ�����ϐ�
    private float initialSpeed;

    //�Փˏ�Ԃ��Ǘ�����ϐ�
    private bool collided = false;

    void Start()
    {
        //�������x��ۑ�
        initialSpeed = speed;
    }

    void Update()
    {
        //�L�[�{�[�h�ƃR���g���[���[�̐��������̓��͂��擾
        float moveHorizontal = Input.GetAxis("Horizontal");

        //�L�[�{�[�h�ƃR���g���[���[�̐��������̓��͂��擾
        float moveVertical = Input.GetAxis("Vertical");

        //�ړ��x�N�g�����쐬
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        //�v���C���[���ړ�������
        transform.Translate(movement * speed * Time.deltaTime);
    }

    //�Փ˂����������Ƃ��ɌĂяo�����
    void OnCollisionEnter(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g�̃^�O��Wall�̂Ƃ�
        if (collision.gameObject.tag == "Wall")
        {
            //�Փˏ�Ԃ�true�ɐݒ�
            collided = true;
        }
    }

    //�Փ˂��I�������Ƃ��ɌĂяo�����
    void OnCollisionExit(Collision collision)
    {
        //�Փ˂����I�u�W�F�N�g�̃^�O��Wall�ŁA�Փˏ�Ԃ�true�̂Ƃ�
        if (collision.gameObject.tag == "Wall" && collided)
        {
            //�ړ����x��10.0f����������
            speed += 10.0f;

            //�Փˏ�Ԃ�false�ɐݒ�
            collided = false;
        }
    }
}