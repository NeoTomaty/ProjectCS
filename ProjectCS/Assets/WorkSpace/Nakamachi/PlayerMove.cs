using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //�ړ����x��ݒ肷��ϐ�
    public float speed = 5.0f;

    void Update()
    {
        //W�L�[��������Ă���Ƃ��A�O���Ɉړ�
        if (Input.GetKey(KeyCode.W))
        {
            //�v���C���[�̈ʒu��O�������Ɉړ�
            transform.position += speed * transform.forward * Time.deltaTime;
        }

        //S�L�[��������Ă���Ƃ��A����Ɉړ�
        if (Input.GetKey(KeyCode.S))
        {
            //�v���C���[�̈ʒu����������Ɉړ�
            transform.position -= speed * transform.forward * Time.deltaTime;
        }

        //D�L�[��������Ă���Ƃ��A�E�����Ɉړ�
        if (Input.GetKey(KeyCode.D))
        {
            //�v���C���[�̈ʒu���E�����Ɉړ�
            transform.position += speed * transform.right * Time.deltaTime;
        }

        //A�L�[��������Ă���Ƃ��A�������Ɉړ�
        if (Input.GetKey(KeyCode.A))
        {
            //�v���C���[�̈ʒu���������Ɉړ�
            transform.position -= speed * transform.right * Time.deltaTime;
        }
    }
}