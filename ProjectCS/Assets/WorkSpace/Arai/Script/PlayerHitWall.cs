//======================================================
// [PlayerHitWall]
// �쐬�ҁF�r��C
// �ŏI�X�V���F03/31
// 
// [Log]
// 3/31�@�r��@�v���C���[���ǂɏՓ˂����ۂ̋������쐬
// 3/31�@�r��@�ړ��̉��X�N���v�g�����삵������m�F
// 4/01�@�r��@Player�I�u�W�F�N�g�̖{�X�N���v�g�ɑΉ�
//======================================================

using UnityEngine;

public class PlayerHitWall : MonoBehaviour
{
    [Tooltip("�ǂɏՓ˂����ۂ̉�����")]
    [SerializeField] private float Acceleration = 1.0f;

    // �v���C���[�̈ړ������Ƒ��x�ɃA�N�Z�X���邽�߂̕ϐ�
    // �����I�u�W�F�N�g�ɃA�^�b�`����Ă���X�N���v�g�ł���Ƃ����z��ł̎���
    MovePlayer MovePlayerScript;    //���ۂ̃X�N���v�g

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MovePlayerScript = GetComponent<MovePlayer>();

        if (MovePlayerScript == null)
        {
            Debug.LogError("�v���C���[ >> MovePlayer�X�N���v�g��������܂���");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (MovePlayerScript == null) return;

        // �Փ˂����I�u�W�F�N�g�̃^�O���`�F�b�N
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BrokenWall")
        {
            Debug.Log("�v���C���[ >> �ǂɓ�����܂���");

            // �v���C���[�̈ړ��x�N�g�����擾
            Vector3 PlayerMoveDirection = MovePlayerScript.GetMoveDirection;

            // �ǂ̐ڐG�ʂ̖@���x�N�g�����擾
            Vector3 Normal = collision.contacts[0].normal;

            // ���˃x�N�g�����v�Z
            Vector3 Reflect = Vector3.Reflect(PlayerMoveDirection, Normal);

            // ���˃x�N�g�����v���C���[�ɓK�p
            MovePlayerScript.SetMoveDirection(Reflect);

            // �v���C���[������
            MovePlayerScript.PlayerSpeedManager.SetAccelerationValue(Acceleration);
        }
    }
}
