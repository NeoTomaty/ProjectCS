//====================================================
// �X�N���v�g���FIsHitEnemy
// �쐬�ҁF�|��
// ���e�F�G�ƐڐG�����ۂɔ������鏈�����܂Ƃ߂�����
// �@�@�@�v���C���[�ƂȂ�I�u�W�F�N�g�ɃA�^�b�`����
// �ŏI�X�V���F04/17
// [Log]
// 04/17 �|�� �X�N���v�g�쐬
//====================================================
using UnityEngine;

public class IsHitEnemy : MonoBehaviour
{
    public GameObject liftingObject; // �o�E���h���Ă���{�[���iObjectLifting���t���Ă���j

    private ObjectLifting liftingScript;
    private Rigidbody liftingRb;

    public float enemyHitForce = 10.0f; // �G�ڐG���ɉ����������̗�

    void Start()
    {
        // �X�N���v�g��Rigidbody���擾
        if (liftingObject != null)
        {
            liftingScript = liftingObject.GetComponent<ObjectLifting>();
            liftingRb = liftingObject.GetComponent<Rigidbody>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Enemy�^�O�ƐڐG�����ꍇ
        if (collision.gameObject.CompareTag("Enemy") && liftingScript.isLink)
        {
            if (liftingScript != null)
            {
                // isLink��false�ɂ��ăv���C���[�̒Ǐ]������
                liftingScript.isLink = false;
            }

            if (liftingRb != null)
            {
                // ������ɗ͂�������
                liftingRb.AddForce(Vector3.up * enemyHitForce, ForceMode.Impulse);
                Debug.Log("Enemy�ɓ��������̂Ń����N���� + ������ɗ͂��������I");
            }
        }
    }
}
