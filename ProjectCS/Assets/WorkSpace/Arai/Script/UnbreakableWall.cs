// �쐬��:�r��C
// �쐬��:2025/03/28
// �T�v:�j��s�\�ȕǂ̃X�N���v�g
// �X�V����:
// 2025/3/31 �|���@�ϐ��R�����g�A�E�g

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableWall : MonoBehaviour
{
    // ������
    //[SerializeField] private float CharacterAcceleration = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //�v���C���[���ǂɓ��������Ƃ��̏���
            Debug.Log("�j��s�\�ȕ� >> �v���C���[���ǂɓ�����܂���");

            // �v���C���[�𒵂˕Ԃ�

            // �v���C���[������������
        }
    }
}
