// �쐬��:�r��C
// �쐬��:2025/03/28
// �T�v:�j��\�ȕǂ̃X�N���v�g
// �X�V����:
// 2025//:

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    // ������
    [SerializeField] private float CharacterAcceleration = 10.0f;
    private bool Breakable = false;

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
            Debug.Log("�j��\�ȕ� >> �v���C���[���ǂɓ�����܂���");

            // �v���C���[�𒵂˕Ԃ�

            // �v���C���[��f�ʂ肳����

            // �j�󂳂��

            // �v���C���[������������
        }
    }
}
