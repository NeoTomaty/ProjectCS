//======================================================
// [TestMovePlayer]
// �쐬�ҁF�r��C
// �ŏI�X�V���F03/31
// 
// [Log]
// 3/31�@�r��@���͂��󂯕t���Ȃ��ړ��e�X�g���쐬
//======================================================

using UnityEngine;

public class TestMovePlayer : MonoBehaviour
{
    // �v���C���[�̈ړ����x
    public float speed = 10.0f;

    // �v���C���[�̈ړ�����
    public Vector3 moveDirection = new Vector3(0, 0, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ���܂��������Ɉړ���������
        transform.position += moveDirection * speed * Time.deltaTime;
    }
}
