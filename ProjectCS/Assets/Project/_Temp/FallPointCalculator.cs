//====================================================
// �X�N���v�g���FFallPointCalculator
// �쐬�ҁF����
// ���e�F��΂��Ώۂ̗����n�_���v�Z����N���X
// �ŏI�X�V���F04/27
// 
// [Log]
// 04/27 ���� �X�N���v�g�쐬
//====================================================

// ******* ���̃X�N���v�g�̎g���� ******* //
// 1. ���̃X�N���v�g�͔�΂��Ώۂ̃I�u�W�F�N�g�ɃA�^�b�`
// 2. LAManager�ɂ�LiftingArea�I�u�W�F�N�g�ɂ��Ă���LiftingAreaManager��ݒ�
// 3. �I�u�W�F�N�g���΂�����̃��[�v��̍��W�����肵���Ƃ��ɁACalculateGroundPoint���Ăяo���i���X�N���v�g����Q�Ɓj
using UnityEngine;

public class FallPointCalculator : MonoBehaviour
{
    private string GroundTag = "Ground"; // �n�ʂƂ��ĔF������^�O��

    private Vector3 FallPoint; // �����n�_

    [SerializeField] private LiftingAreaManager LAManager; // LiftingAreaManager���Q��

    void Start()
    {
        if(!LAManager) Debug.LogError("LiftingAreaManager���ݒ肳��Ă��܂���");

        CalculateGroundPoint(); // test
    }

    public void CalculateGroundPoint()
    {
        RaycastHit hit;
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;

        if (Physics.Raycast(origin, direction, out hit, Mathf.Infinity)) // �ꉞ�������̃��C�͖����ɐݒ�
        {
            // �q�b�g�����n�ʃI�u�W�F�N�g�̃^�O���m�F����
            if (hit.collider.CompareTag(GroundTag))
            {
                Debug.Log("�����n�_�F" + hit.point);
                FallPoint = hit.point;

                // ���t�e�B���O�G���A�I�u�W�F�N�g���ړ�������
                LAManager.SetFallPoint(FallPoint);
            }
        }
    }

    // FallPoint���擾�i�g�p���邩������Ȃ����ꉞ����Ă܂��j
    public Vector3 GetFallPoint()
    {
        return FallPoint; 
    }
}
