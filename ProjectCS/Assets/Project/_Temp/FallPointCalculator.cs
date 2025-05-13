//====================================================
// �X�N���v�g���FFallPointCalculator
// �쐬�ҁF����
// ���e�F��΂��Ώۂ̗����n�_���v�Z����N���X
// �ŏI�X�V���F05/13
// 
// [Log]
// 04/27 ���� �X�N���v�g�쐬
// 05/13 ���� �n�ʂƂ̓����蔻����^�O���烌�C���[�ɕύX
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. ���̃X�N���v�g�͔�΂��Ώۂ̃I�u�W�F�N�g�ɃA�^�b�`
// 2. LAManager�ɂ�LiftingArea�I�u�W�F�N�g�ɂ��Ă���LiftingAreaManager��ݒ�
// 3. �I�u�W�F�N�g���΂�����̃��[�v��̍��W�����肵���Ƃ��ɁACalculateGroundPoint���Ăяo���i���X�N���v�g����Q�Ɓj
// 4. BaseGroundLayerMask�Ƀx�[�X�ƂȂ�n�ʁi��ԉ��̒n�ʁj�̃��C���[��ݒ肷��
using UnityEngine;

public class FallPointCalculator : MonoBehaviour
{
    private string GroundTag = "Ground"; // �n�ʃI�u�W�F�N�g�̃^�O
    private Vector3 FallPoint; // �����n�_
   
    [SerializeField] private LiftingAreaManager LAManager; // LiftingAreaManager���Q��

    [Tooltip("BaseGround�̃��C���[��ݒ�")]
    [SerializeField] private LayerMask BaseGroundLayerMask; // �x�[�X�ƂȂ�n�ʂ̃��C���[

    void Start()
    {
        if(!LAManager) Debug.LogError("LiftingAreaManager���ݒ肳��Ă��܂���");

        CalculateGroundPoint(); // test
    }

    // �X�i�b�N���n�ʂɐڂ��Ă��邩���t�e�B���O�G���A�O�̂Ƃ���
    // ���t�e�B���O�G���A�̈ʒu���Ē�������
    private void OnCollisionStay(Collision collision)
    {
        if (!collision.gameObject.CompareTag(GroundTag)) return;

        if(!LAManager.GetIsTargetContacting())
        {
            CalculateGroundPoint(); // ���t�e�B���O�G���A�̃|�C���g���Čv�Z
            Debug.Log("���t�e�B���O�G���A�Čv�Z���s");
        }
    }

    public void CalculateGroundPoint()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        // BaseGroundLayer�̂ݔ���
        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity, BaseGroundLayerMask)) // �ꉞ�������̃��C�͖����ɐݒ�
        {
            Debug.Log("�����n�_�F" + hit.point);
            FallPoint = hit.point;

            // ���t�e�B���O�G���A�I�u�W�F�N�g���ړ�������
            LAManager.SetFallPoint(FallPoint);
        }
    }

    // FallPoint���擾�i�g�p���邩������Ȃ����ꉞ����Ă܂��j
    public Vector3 GetFallPoint()
    {
        return FallPoint; 
    }
}
